using System;
using System.IO;
using System.Net;
using System.Timers;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

using NLog;

using ClusterLib.Properties;
using Utilities;

namespace ClusterLib
{
    public class Cluster : IDisposable
    {
        /** 20171019. Para complementar el LOG... */
        public static class LogHelper
        {
            static Logger nlog = LogManager.GetLogger("ClusterLib");
            public static string Log(LogLevel level, string message,
                [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0, [System.Runtime.CompilerServices.CallerMemberName] string caller = null)
            {
                String msg = String.Format("[{0},{1}]: {2}", caller, lineNumber, message);
                nlog.Log(level, msg);
                return msg;
            }
            public static void Trace([System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0, [System.Runtime.CompilerServices.CallerMemberName] string caller = null)
            {
                String msg = String.Format("LogHelper TRACE: {0} line {1}", caller, lineNumber);
                nlog.Trace(msg);
            }
            public static void LogTick(ClusterState cluster, [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
                [System.Runtime.CompilerServices.CallerMemberName] string caller = null)
            {
                String msg = String.Format("ClusterInfo: [ L: {0,-12} R: {1,-12} ]", cluster.LocalNode.State, cluster.RemoteNode.State);
                Log(LogLevel.Debug, msg, lineNumber, caller);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        enum ClusterIpState { Unknown, Finding, Found, NotFound }

        /// <summary>
        /// 
        /// </summary>
        public ClusterState State
        {
            get { return _State; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        public Cluster(ClusterSettings settings)
        {
            _Settings = settings;
            _State = new ClusterState(_Settings);

            _EndPoint = new IPEndPoint(IPAddress.Parse(_Settings.EpIp), _Settings.EpPort);

            _PeriodicTasks = new Timer(_PeriodicTasksInterval);
            _PeriodicTasks.AutoReset = false;
            _PeriodicTasks.Elapsed += PeriodicTasks;

            _ToStart = new Timer(_Settings.TimeToStart);
            _ToStart.AutoReset = false;
            _ToStart.Elapsed += ToStart;

            _Ping1 = new Ping();
            _Ping1.PingCompleted += OnPingCompleted;
            _Ping2 = new Ping();
            _Ping2.PingCompleted += OnPingCompleted;

            // 20171020. Para intentar sincronizar las recuperaciones de red....
            _FromInvalidInterval = _Settings.TimeToStart;
        }

        /// <summary>
        /// 
        /// </summary>
        ~Cluster()
        {
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Run()
        {
            _ToStart.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToStart(object sender, ElapsedEventArgs e)
        {
#if _VERSION_00_
            try
            {
                _Comm = new UdpSocket(_Settings.Ip, _Settings.Port);
                _Comm.NewDataEvent += OnNewData;
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                _Logger.DebugException(ex.Message, ex);
            }

            _State.LocalNode.ValidAdapters = GetAdaptersState();
            if (_State.LocalNode.ValidAdapters > 0)
            {
                if (!ExistClusterAddresses(false))
                {
                    _State.LocalNode.SetState(NodeState.Activating, Resources.LocalActivateAsk);
                }
                else
                {
                    _State.LocalNode.SetState(NodeState.NoActive, Resources.FoundClusterIps);
                }
            }
            else
            {
                _State.LocalNode.SetState(NodeState.NoActive, Resources.DeactivateByNotAdapters);
            }

            if (_Comm != null)
                _Comm.BeginReceive();

            _PeriodicTasks.Enabled = true;
#else
            // 20170925. No arranca hasta que no consigue que se active la LAN Interna.
            // Se considera que hay red si se completa favorablemente toda la inicializacion ...

            // 20171019. Este parte del timer se ejecutará periodicamente hasta que se inicie la red interna...
            LogHelper.Log(LogLevel.Info, "ToStart TICK");
            try
            {
                _Comm = new UdpSocket(_Settings.Ip, _Settings.Port);
                _Comm.NewDataEvent += OnNewData;
                _Comm.BeginReceive();
                LogHelper.Log(LogLevel.Info, "Red Interna Disponible...");
            }
            catch (Exception x)
            {
                LogHelper.Log(LogLevel.Error, x.Message);
                // Rearranco este timer...
                _ToStart.Enabled = true;
            }

            try
            {
                // 20171019. Esta parte del timer solo se ejecuta la primera vez...Si no hay errores internos...
                if (_State.LocalNode.State == NodeState.NoValid)
                {
                    // 20171019. Fuerzo el borrado de la IP virtual...
                    ForceDeleteVirtualAddress();
                    /////////////////////////////////////////////////
                    _State.LocalNode.ValidAdapters = GetAdaptersState();
                    if (_State.LocalNode.ValidAdapters > 0)
                    {
                        if (!ExistClusterAddresses(false))
                        {
                            _State.LocalNode.SetState(NodeState.Activating, Resources.LocalActivateAsk);
                        }
                        else
                        {
                            _State.LocalNode.SetState(NodeState.NoActive, Resources.FoundClusterIps);
                        }
                    }
                    else
                    {
                        _State.LocalNode.SetState(NodeState.NoActive, Resources.DeactivateByNotAdapters);
                    }

                    _PeriodicTasks.Enabled = true;
                }
            }
            catch (Exception x)
            {
                LogHelper.Log(LogLevel.Error, x.Message);

                // Rearranco este timer...
                _ToStart.Enabled = true;
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public void Activate()
        {
            LogHelper.Trace();
            Activate(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Deactivate()
        {
            lock (_Sync)
            {
                LogHelper.Trace();
                if (_State.LocalNode.State == NodeState.NoActive)
                {
                    LogHelper.Log(LogLevel.Info, String.Format(Resources.AlreadyDeactivate, _State.LocalNode.StateBegin));
                    return;
                }

                if (_State.RemoteNode.State == NodeState.NoValid)
                {
                    throw new InvalidOperationException(Resources.DeactivateValidRemoteNodeError);
                }

                if ((NumValidAdapters(_State.RemoteNode.ValidAdapters) < NumValidAdapters(_State.LocalNode.ValidAdapters)))
                {
                    throw new InvalidOperationException(Resources.DeactivateNumAdaptersError);
                }

                if (_State.LocalNode.State == NodeState.Active)
                {
                    DeleteVirtualAddresses();
                }

                _State.LocalNode.SetState(NodeState.NoActive, Resources.LocalDeactivateAsk);
            }
        }

        #region IDisposable Members

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            LogHelper.Trace();
            if (_State.LocalNode.State == NodeState.Active)
            {
                DeleteVirtualAddresses();
            }

            _State.LocalNode.SetState(NodeState.NoActive, Resources.LocalDeactivateAsk);
            _State.LocalNode.SetState(NodeState.NoValid, Resources.ExitApplication);

            Dispose(true);
        }

        #endregion

        #region Private Members

        const int _PeriodicTasksInterval = 1000;
        const int _ActivityTimeOut = 5000;
        int _FromInvalidInterval = 10000;

        // static Logger _Logger = LogManager.GetCurrentClassLogger();

        ClusterSettings _Settings;
        ClusterState _State;

        /** */
        int[] VirtualIpContext = new int[2] { -1, -1 };

        object _Sync = new object();
        UdpSocket _Comm = null;
        IPEndPoint _EndPoint = null;
        Timer _PeriodicTasks = null;
        Timer _ToStart = null;
        DateTime _RemoteStateTime;
        DateTime _PingReceivedTime;
        Ping _Ping1 = null;
        Ping _Ping2 = null;
        ClusterIpState _ClusterIp1State;
        ClusterIpState _ClusterIp2State;
        int _Adapter1Index = -1;
        int _Adapter2Index = -1;
        bool _Disposed = false;
        static bool esperar = true;


        /// <summary>
        /// 
        /// </summary>
        void DeleteArpTable()
        {
            // 20171020. Esta accion que dura tiempo y puede ser ejecutada de forma asincrona la saco el thread del automata..
            Task.Factory.StartNew(() =>
            {
                try
                {
                    LogHelper.Log(LogLevel.Info, "Borrando Tabla ARP");
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.WorkingDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                    startInfo.FileName = "delarp.bat";
                    process.StartInfo = startInfo;
                    process.Start();
                    process.WaitForExit();
                    System.Threading.Thread.Sleep(1000);
                    LogHelper.Log(LogLevel.Info, "Tabla ARP Borrada");
                }
                catch (Exception ex)
                {
                    LogHelper.Log(LogLevel.Error, String.Format("Excepcion Borrando Tabla ARP: {0}", ex.Message));
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        void ResetInternalLan()
        {
            try
            {
                LogHelper.Trace();
                if (_Comm != null)
                {
                    _Comm.Dispose();
                    _Comm = null;
                    Run();
                }
            }
            catch (Exception x)
            {
                LogHelper.Log(LogLevel.Error, x.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bDispose"></param>
        void Dispose(bool bDispose)
        {
            LogHelper.Trace();
            if (!_Disposed)
            {
                LogHelper.Trace();
                _Disposed = true;

                if (bDispose)
                {
                    _PeriodicTasks.Enabled = false;
                    _PeriodicTasks.Close();
                    _PeriodicTasks = null;

                    if (_Comm != null)
                    {
                        _Comm.Dispose();
                        _Comm = null;
                    }

                    lock (_Sync)
                    {
                        //if (_State.LocalNode.State == NodeState.Active)
                        {
                            /** 20170803. AGL. En este caso hay que forzar el borrado */
                            DeleteVirtualAddresses();
                        }
                    }

                    _Ping1.Dispose();
                    _Ping2.Dispose();
                    _Ping1 = null;
                    _Ping2 = null;

                    _Settings = null;
                    _State = null;
                    _EndPoint = null;
                    _Sync = null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adapters"></param>
        /// <returns></returns>
        int NumValidAdapters(int adapters)
        {
            switch (adapters)
            {
                case 0:
                    return 0;
                case 1:
                case 2:
                    return 1;
                case 3:
                    return 2;
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adapterIndex"></param>
        /// <returns></returns>
        bool IsValidAdapter(int adapterIndex)
        {
            return ((_State.LocalNode.ValidAdapters & adapterIndex) != 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
#if DEBUG
        // Simular la desconexion de las redes principales.
        public bool WorkingNetworkSimulOff { get; set; }
#endif
        int GetAdaptersState()
        {
#if DEBUG
            if (WorkingNetworkSimulOff)
                return 0;
#endif
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            int adapters = 0;

            foreach (NetworkInterface adapter in nics)
            {
                if ((adapter.OperationalStatus == OperationalStatus.Up) && (adapter.NetworkInterfaceType != NetworkInterfaceType.Loopback))
                {
                    UnicastIPAddressInformationCollection ips = adapter.GetIPProperties().UnicastAddresses;

                    foreach (UnicastIPAddressInformation ip in ips)
                    {
                        string strIp = ip.Address.ToString();

                        if (strIp == _Settings.AdapterIp1)
                        {
                            adapters |= 1;
                            _Adapter1Index = adapter.GetIPProperties().GetIPv4Properties().Index;
                        }
                        if (strIp == _Settings.AdapterIp2)
                        {
                            adapters |= 2;
                            _Adapter2Index = adapter.GetIPProperties().GetIPv4Properties().Index;
                        }
                        if (adapters == 3)
                        {
                            return adapters;
                        }
                    }
                }
            }

            return adapters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="async"></param>
        /// <returns></returns>
        bool ExistClusterAddresses_old(bool async)
        {
            LogHelper.Trace();

            if (((DateTime.Now - _PingReceivedTime).TotalMilliseconds > 2000) &&
               (_ClusterIp1State != ClusterIpState.Finding) && (_ClusterIp2State != ClusterIpState.Finding))
            {
                _ClusterIp1State = ClusterIpState.Finding;
                _ClusterIp2State = ClusterIpState.Finding;

                _Ping1.SendAsync(_Settings.ClusterIp1, 2000, null);
                _Ping2.SendAsync(_Settings.ClusterIp2, 2000, null);
            }

            if (!async)
            {
                while ((_ClusterIp1State == ClusterIpState.Finding) || (_ClusterIp2State == ClusterIpState.Finding))
                {
                    System.Threading.Thread.Sleep(500);
                }
            }

            //return ((_ClusterIp1State != ClusterIpState.NotFound) || (_ClusterIp2State != ClusterIpState.NotFound));
            return ((_ClusterIp1State == ClusterIpState.Found) || (_ClusterIp2State == ClusterIpState.Found));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="async"></param>
        /// <returns></returns>
        bool ExistClusterAddresses(bool async = false)
        {
            LogHelper.Trace();

            PingReply reply1 = null;
            PingReply reply2 = null;
            try
            {
                reply1 = (new Ping()).Send(_Settings.ClusterIp1, 500);
                LogHelper.Log(LogLevel.Debug, String.Format("PingReply {1,8} From {0,15}, {2,6} ms",
                    reply1.Address != null ? reply1.Address.ToString() : _Settings.ClusterIp1, reply1.Status.ToString(), reply1.RoundtripTime));

                reply2 = (new Ping()).Send(_Settings.ClusterIp2, 500);
                LogHelper.Log(LogLevel.Debug, String.Format("PingReply {1,8} From {0,15}, {2,6} ms",
                    reply2.Address != null ? reply2.Address.ToString() : _Settings.ClusterIp2, reply2.Status.ToString(), reply2.RoundtripTime));
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogLevel.Error, String.Format("Excepcion en PING: {0}", ex.Message));
            }
            return (reply1 != null && reply1.Status == IPStatus.Success) || (reply2 != null && reply2.Status == IPStatus.Success);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnPingCompleted(object sender, PingCompletedEventArgs e)
        {
            LogHelper.Trace();

            ClusterIpState state;

            switch (e.Reply.Status)
            {
                case IPStatus.TimedOut:
                case IPStatus.TimeExceeded:
                case IPStatus.TtlExpired:
                    state = ClusterIpState.NotFound;
                    break;
                default:
                    state = ClusterIpState.Found;
                    break;
            }

            lock (_Sync)
            {
                if (sender == _Ping1)
                {
                    _ClusterIp1State = state;
                }
                else
                {
                    _ClusterIp2State = state;
                }

                _PingReceivedTime = DateTime.Now;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool CreateVirtualAddresses()
        {
            LogHelper.Trace();
            // if (ExistClusterAddresses(true))               
            if (ExistClusterAddresses(false))
            {
                LogHelper.Log(LogLevel.Error, "Cluster IP Address exist...Creating Virtual Address...");
                return true;
            }

            bool error1 = false, error2 = false;

            if (IsValidAdapter(1))
            {
                Task.Factory.StartNew(() => CreateVirtualAddressesTask(0, out error1)).Wait();
            }
            if (IsValidAdapter(2))
            {
                Task.Factory.StartNew(() => CreateVirtualAddressesTask(1, out error2)).Wait();
            }

            if (error1 || error2)
            {
                _State.LocalNode.SetState(NodeState.NoActive, Resources.CreateIpError);
                ForceDeleteVirtualAddress();
            }
            else
            {
                _State.LocalNode.SetState(NodeState.Active, Resources.NodeActive);
            }

            return false;
        }
        void CreateVirtualAddressesTask(int vipIndex, out bool error)
        {
            vipIndex = vipIndex == 0 ? 0 : 1;
            string ip = vipIndex == 0 ? _Settings.ClusterIp1 : _Settings.ClusterIp2;
            string ms = vipIndex == 0 ? _Settings.ClusterMask1 : _Settings.ClusterMask2;
            int adapterIndex = vipIndex == 0 ? _Adapter1Index : _Adapter2Index;
            try
            {
                VirtualIpContext[vipIndex] = Native.IpHlpApi.AddIPAddress(ip, ms, adapterIndex);
                LogHelper.Log(LogLevel.Info, String.Format("Virtual IP {0} Creada...", ip));
                error = false;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogLevel.Error, String.Format("Excepcion en {0}, IP {1}: {2}", Resources.CreateIpError, ip, ex.Message));
                error = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void DeleteVirtualAddresses()
        {
            LogHelper.Trace();
            if (IsValidAdapter(1))
            {
                Task.Factory.StartNew(() => ForceDeleteVirtualAddressTask(0)).Wait();
            }
            if (IsValidAdapter(2))
            {
                Task.Factory.StartNew(() => ForceDeleteVirtualAddressTask(1)).Wait();
            }
        }

        /// <summary>
        /// 20170803. AGL. Cuando desaparecen los Adapadores, si se chequea la validez de los mismos antes de eliminar la Ip Virtual esta no se realiza.
        /// </summary>
        void ForceDeleteVirtualAddress()
        {
            Task.Factory.StartNew(() => ForceDeleteVirtualAddressTask(0)).Wait();
            Task.Factory.StartNew(() => ForceDeleteVirtualAddressTask(1)).Wait();
        }
        private void ForceDeleteVirtualAddressTask(int vipIndex)
        {
            string ip = vipIndex == 0 ? _Settings.ClusterIp1 : _Settings.ClusterIp2;
            //int context = VirtualIpContext[vipIndex];
            try
            {
                if (VirtualIpContext[vipIndex] >= 0)
                {
                    Native.IpHlpApi.DeleteIPAddressOnContext(VirtualIpContext[vipIndex]);
                    VirtualIpContext[vipIndex] = -1;
                }
                else
                {
                    Native.IpHlpApi.DeleteIPAddress(ip);
                }
                LogHelper.Log(LogLevel.Info, String.Format("Virtual IP {0} Eliminada...", ip));
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogLevel.Error, String.Format("Excepcion en {0}, IP {1}: {2}", Resources.DeleteIpError, ip, ex.Message));
            }
        }

        /// <summary>
        /// 20171020
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        bool IsLocalIp(string ip)
        {
            IPAddress IpChecking = null;
            if (IPAddress.TryParse(ip, out IpChecking))
            {
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
                foreach (IPAddress localip in localIPs)
                {
                    if (localip.Equals(IpChecking)) return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        void SendState()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, _State.LocalNode);

                try
                {
                    if (_Comm != null)
                        _Comm.Send(_EndPoint, ms.ToArray());
                }
                catch (Exception ex)
                {
                    LogHelper.Log(LogLevel.Error, String.Format("Excepcion en {0}: {1}", Resources.DeleteIpError, ex.Message));
                    /** 20171019. Si hay una excepcion aqui entiendo que se ha perdido la LAN interna... */
                    ResetInternalLan();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="async"></param>
        void Activate(bool async)
        {
            NodeState state;
            string changeCause;

            LogHelper.Trace();
            lock (_Sync)
            {
                if (_State.LocalNode.State == NodeState.Active)
                {
                    LogHelper.Log(LogLevel.Info, String.Format(Resources.AlreadyActivate, _State.LocalNode.StateBegin));
                    return;
                }

                if (_State.LocalNode.State == NodeState.NoActive)
                {
                    if ((_State.RemoteNode.State != NodeState.NoValid) &&
                       (NumValidAdapters(_State.LocalNode.ValidAdapters) < NumValidAdapters(_State.RemoteNode.ValidAdapters)))
                    {
                        throw new InvalidOperationException(Resources.ActivateNumAdaptersError);
                    }

                    _State.LocalNode.SetState(NodeState.Activating, Resources.LocalActivateAsk);
                }
            }

            if (!async)
            {
                do
                {
                    System.Threading.Thread.Sleep(500);

                    lock (_Sync)
                    {
                        state = _State.LocalNode.State;
                        changeCause = _State.LocalNode.ChangeCause;
                    }

                } while (state == NodeState.Activating);

                if (state != NodeState.Active)
                {
                    throw new InvalidOperationException(changeCause);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="dg"></param>
        void OnNewData(object sender, DataGram dg)
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream ms = new MemoryStream(dg.Data);
                object msg = bf.Deserialize(ms);

                lock (_Sync)
                {
                    if (msg is MsgType)
                    {
                        switch ((MsgType)msg)
                        {
                            case MsgType.Activate:
                                LogHelper.Log(LogLevel.Info, Resources.RemoteActivateAsk);
                                Activate(true);
                                break;
                            case MsgType.Deactivate:
                                LogHelper.Log(LogLevel.Info, Resources.RemoteDeactivateAsk);
                                Deactivate();
                                break;
                            case MsgType.GetState:
                                MemoryStream info = new MemoryStream();
                                bf.Serialize(info, _State);
                                if (_Comm != null)
                                    _Comm.Send(dg.Client, info.ToArray());
                                break;
                            default:
                                throw new Exception(Resources.UnknownMsgType);
                        }
                    }
                    else if (msg is NodeInfo)
                    {
                        //_Logger.Log((_State.RemoteNode.State == NodeState.NoValid) ? LogLevel.Info : LogLevel.Trace, 
                        //    Resources.ReceivedRemoteNodeState.Replace("\\n", Environment.NewLine), msg);

                        LogHelper.Log(_State.RemoteNode.State == NodeState.NoValid ? LogLevel.Info : LogLevel.Trace,
                            String.Format(Resources.ReceivedRemoteNodeState.Replace("\\n", Environment.NewLine), msg));
                        _State.RemoteNode.UpdateInfo((NodeInfo)msg);
                        _RemoteStateTime = DateTime.Now;
                    }
                    else
                    {
                        throw new Exception(Resources.UnknownMsgType);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogLevel.Error, String.Format("Excepcion en {0}: {1}", Resources.DeleteIpError, ex.Message));
                //if (!_Disposed)
                //{
                //    _Logger.ErrorException(Resources.NewDataError, ex);
                //}
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PeriodicTasks(object sender, ElapsedEventArgs e)
        {
            bool FromLocalInvalid = false;
            try
            {
                lock (_Sync)
                {
                    LogHelper.LogTick(_State);

                    if ((_State.RemoteNode.State != NodeState.NoValid) &&
                       ((DateTime.Now - _RemoteStateTime).TotalMilliseconds > _ActivityTimeOut))
                    {
                        LogHelper.Log(LogLevel.Warn, Resources.RemoteNodeNotOperational);
                        _State.RemoteNode.SetState(NodeState.NoValid, null);
                    }

                    _State.LocalNode.ValidAdapters = GetAdaptersState();
                    int numLocalValidAdapters = NumValidAdapters(_State.LocalNode.ValidAdapters);
                    int numRemoteValidAdapters = (_State.RemoteNode.State != NodeState.NoValid ? NumValidAdapters(_State.RemoteNode.ValidAdapters) : 0);

                    if (_State.LocalNode.ValidAdapters > 0)
                    {
                        if (_State.LocalNode.State == NodeState.NoValid)
                        {
                            /** Se han 'recuperado' los adaptadores. Se limpian las IP virtuales */
                            ForceDeleteVirtualAddress();
                            _State.LocalNode.SetState(NodeState.NoActive, /*Resources.AdapterDetected*/"");
                            FromLocalInvalid = true;
                        }
                        else if (_State.LocalNode.State == NodeState.NoActive)
                        {
                            if (_State.RemoteNode.State == NodeState.NoValid)
                            {
                                // 20171020. Si estoy NoActivo y Remoto en NoValido puede ser por desconexion de LAN INTERNA...
                                if (!ExistClusterAddresses())
                                {
                                    _State.LocalNode.SetState(NodeState.Activating, Resources.LocalActivateAsk);
                                }
                            }
                            else if (_State.RemoteNode.State == NodeState.NoActive)
                            {
                                if ((numLocalValidAdapters > numRemoteValidAdapters) ||
                                   ((numLocalValidAdapters == numRemoteValidAdapters) &&
                                   (_State.LocalNode.StateBegin.Ticks < _State.RemoteNode.StateBegin.Ticks)))
                                {
                                    _State.LocalNode.SetState(NodeState.Activating, Resources.LocalActivateAsk);
                                }
                            }
                            else if ((_State.RemoteNode.State == NodeState.Activating) || (_State.RemoteNode.State == NodeState.Active))
                            {
                                if (numLocalValidAdapters > numRemoteValidAdapters)
                                {
                                    _State.LocalNode.SetState(NodeState.Activating, Resources.LocalActivateAsk);
                                }
                            }
                        }
                        else if (_State.LocalNode.State == NodeState.Activating)
                        {
                            if ((DateTime.Now - _State.LocalNode.StateBegin).TotalMilliseconds > (2 * _ActivityTimeOut))
                            {
                                _State.LocalNode.SetState(NodeState.NoActive, Resources.ActivateTimeout);
                            }
                            else if (_State.RemoteNode.State == NodeState.NoValid)
                            {
                                //if ((DateTime.Now - _State.LocalNode.StateBegin).TotalMilliseconds > _ActivityTimeOut)
                                {
                                    //while (ExistClusterAddresses(false))
                                    //{
                                    //    System.Threading.Thread.Sleep(10000);
                                    //    _Logger.Info("Waiting for disabled Cluster IP.");
                                    //}

                                    //DeleteArpTable();
                                    if (CreateVirtualAddresses())   // Retorna true si la IP del cluster ya existe
                                    {
                                        // si la IP del cluster ya existe y el nodo remoto no contesta
                                        // La IP del cluster la tiene el nodo local
                                        //_State.LocalNode.SetState(NodeState.Active, Resources.NodeActive);
                                        ForceDeleteVirtualAddress();
                                        _State.LocalNode.SetState(NodeState.NoActive, Resources.LocalDeactivateAsk);

                                        //System.Threading.Thread.Sleep(5000);
                                        //Activate(true);
                                        //_State.LocalNode.SetState(NodeState.Activating, Resources.LocalActivateAsk);                                        
                                    }
                                    else
                                    {
                                        LogHelper.Log(LogLevel.Info, "IP Virtual asignada al nodo");
                                        DeleteArpTable();
                                    }
                                }
                            }
                            else if (_State.RemoteNode.State == NodeState.NoActive)
                            {
                                if (numLocalValidAdapters >= numRemoteValidAdapters)
                                {
                                    CreateVirtualAddresses();
                                }
                            }
                            else if (_State.RemoteNode.State == NodeState.Activating)
                            {
                                if ((numRemoteValidAdapters > numLocalValidAdapters) ||
                                   ((numRemoteValidAdapters == numLocalValidAdapters) &&
                                   (_State.RemoteNode.StateBegin.Ticks < _State.LocalNode.StateBegin.Ticks)))
                                {
                                    _State.LocalNode.SetState(NodeState.NoActive, Resources.RemoteNodeActivating);
                                }
                            }
                            else if (_State.RemoteNode.State == NodeState.Active)
                            {
                                if (numRemoteValidAdapters > numLocalValidAdapters)
                                {
                                    _State.LocalNode.SetState(NodeState.NoActive, Resources.ActivateNumAdaptersError);
                                }
                            }
                        }
                        else if (_State.LocalNode.State == NodeState.Active)
                        {
                            if (_State.RemoteNode.State == NodeState.Activating)
                            {
                                if (numRemoteValidAdapters >= numLocalValidAdapters)
                                {
                                    DeleteVirtualAddresses();
                                    _State.LocalNode.SetState(NodeState.NoActive, Resources.RemoteDeactivateAsk);
                                }
                            }
                            else if (_State.RemoteNode.State == NodeState.Active)
                            {
                                //if ((numRemoteValidAdapters > numLocalValidAdapters) ||
                                //   ((numRemoteValidAdapters == numLocalValidAdapters) &&
                                //   (_State.RemoteNode.StateBegin.Ticks < _State.LocalNode.StateBegin.Ticks)))
                                {
                                    LogHelper.Log(LogLevel.Warn, Resources.DetectedTwoActiveNodesError);
                                    DeleteVirtualAddresses();
                                    _State.LocalNode.SetState(NodeState.NoActive, Resources.LocalDeactivateAsk);
                                }
                            }
                        }

                        SendState();
                    }
                    else
                    {
                        if (_State.LocalNode.State != NodeState.NoValid)
                        {
                            /** 20170803. AGL, si no hay adaptadores hay que forzar el borrado de las IP virtuales. */
                            // DeleteVirtualAddresses();
                            ForceDeleteVirtualAddress();
                        }

                        _State.LocalNode.SetState(NodeState.NoValid, Resources.DeactivateByNotAdapters);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogLevel.Error, String.Format("Excepcion en {0}: {1}", Resources.PeriodicTaskError, ex.Message));
                //if (!_Disposed)
                //{
                //    _Logger.ErrorException(Resources.PeriodicTaskError, ex);
                //}
            }
            finally
            {
                if (!_Disposed)
                {
                    _PeriodicTasks.Interval = FromLocalInvalid ? _FromInvalidInterval : _PeriodicTasksInterval;
                    _PeriodicTasks.Enabled = true;
                }
            }
        }

        #endregion
    }
}
