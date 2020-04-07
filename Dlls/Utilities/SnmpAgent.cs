using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Linq;
using System.Text;


using NLog;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Objects;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Pipeline;
using Lextm.SharpSnmpLib.Security;

namespace UtilitiesCD40
{
    static class SnmpAgent
    {
        public const string OID_TRAP_GENERA_INCIDENCIA = "1.1.600.1.";

        public static event Action<string, ISnmpData, IPEndPoint> TrapReceived = delegate { };

        public static SynchronizationContext Context
        {
            get { return _context; }
            set { _context = value; }
        }

        public static ObjectStore Store
        {
            get { return SnmpAgent._store; }
        }

        public static void Init(string ip)
        {
            // Cambiar al puerto estándar SNMP 161
            Init(ip, null, 161, 262);
        }

        public static void Init(string ip, string trapMcastIp, int port, int trap)
        {
            SnmpLogger logger = new SnmpLogger();
            ObjectStore objectStore = new ObjectStore();

            OctetString getCommunity = new OctetString("public");
            OctetString setCommunity = new OctetString("public");
            IMembershipProvider[] membershipProviders = new IMembershipProvider[]
			{
				new Version1MembershipProvider(getCommunity, setCommunity),
				new Version2MembershipProvider(getCommunity, setCommunity),
				new Version3MembershipProvider()
			};
            IMembershipProvider composedMembershipProvider = new ComposedMembershipProvider(membershipProviders);

            TrapV1MessageHandler trapv1 = new TrapV1MessageHandler();
            TrapV2MessageHandler trapv2 = new TrapV2MessageHandler();
            InformRequestMessageHandler inform = new InformRequestMessageHandler();

            HandlerMapping[] handlerMappings = new HandlerMapping[]
			{
				new HandlerMapping("v1", "GET", new GetV1MessageHandler()),
				new HandlerMapping("v2,v3", "GET", new GetMessageHandler()),
				new HandlerMapping("v1", "SET", new SetV1MessageHandler()),
				new HandlerMapping("v2,v3", "SET", new SetMessageHandler()),
				new HandlerMapping("v1", "GETNEXT", new GetNextV1MessageHandler()),
				new HandlerMapping("v2,v3", "GETNEXT", new GetNextMessageHandler()),
				new HandlerMapping("v2,v3", "GETBULK", new GetBulkMessageHandler()),
				new HandlerMapping("v1", "TRAPV1", trapv1),
				new HandlerMapping("v2,v3", "TRAPV2", trapv2),
				new HandlerMapping("v2,v3", "INFORM", inform)/*,
				new HandlerMapping("*", "*", new NullMessageHandler())*/
			};
            MessageHandlerFactory messageHandlerFactory = new MessageHandlerFactory(handlerMappings);

            User[] users = new User[]
			{
				new User(new OctetString("neither"), DefaultPrivacyProvider.DefaultPair),
				new User(new OctetString("authen"), new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("authentication")))),
				new User(new OctetString("privacy"), new DESPrivacyProvider(new OctetString("privacyphrase"), new MD5AuthenticationProvider(new OctetString("authentication"))))
			};
            UserRegistry userRegistry = new UserRegistry(users);

            EngineGroup engineGroup = new EngineGroup();
            Listener listener = new Listener() { Users = userRegistry };
            SnmpApplicationFactory factory = new SnmpApplicationFactory(logger, objectStore, composedMembershipProvider, messageHandlerFactory);

            _engine = new SnmpEngine(factory, listener, engineGroup);
            _engine.Listener.AddBinding(new IPEndPoint(IPAddress.Parse(ip), port));
            _engine.Listener.AddBinding(new IPEndPoint(IPAddress.Parse(ip), trap));
            //			_engine.Listener.AddBinding(new IPEndPoint(IPAddress.Parse(ip), trap), trapMcastIp != null ? IPAddress.Parse(trapMcastIp) : null);
            _engine.ExceptionRaised += (sender, e) => _logger.Error(e.Exception, "ERROR Snmp");

            _closed = false;
            _store = objectStore;
            _context = SynchronizationContext.Current ?? new SynchronizationContext();

            trapv1.MessageReceived += TrapV1Received;
            trapv2.MessageReceived += TrapV2Received;
            inform.MessageReceived += InformRequestReceived;

            /*(new IPEndPoint(IPAddress.Parse(ip), 0)).SetAsDefault();*/
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Start()
        {
            /** AGL.START Controlado */
#if _NEWSTART_
            if (Settings.Default.SNMPEnabled == 1)
            {
                _engine.Start();

                SnmpIntObject.Get(Settings.Default.TopStOid).Value = 1;
                SnmpIntObject.Get(Settings.Default.TopOid).Value = 0;	// Tipo de elemento Hw: 0 => Top
            }
#else
            _engine.Start();
#endif
            /** */
        }

        public static void Close()
        {
            _closed = true;
            _engine.Stop();
            _engine.Dispose();
        }

        public static void GetValueAsync(IPEndPoint ep, string oid, Action<ISnmpData> handler)
        {
            GetValueAsync(ep, oid, handler, 2000);
        }

        public static void GetValueAsync(IPEndPoint ep, string oid, Action<ISnmpData> handler, int timeout)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                List<Variable> vList = new List<Variable> { new Variable(new ObjectIdentifier(oid)) };

                try
                {
                    IList<Variable> value = Messenger.Get(VersionCode.V2, ep, new OctetString("public"), vList, timeout);
                    if ((value.Count == 1) && (value[0].Data.TypeCode != SnmpType.NoSuchInstance))
                    {
                        _context.Post(delegate
                        {
                            if (!_closed)
                            {
                                handler(value[0].Data);
                            }
                        }, "SnmpAgent.ValueGetted");
                    }
                }
                catch (Exception)
                {

                }
            });
        }

        public static void SetValueAsync(IPEndPoint ep, string oid, ISnmpData data, Action<ISnmpData> handler)
        {
            SetValueAsync(ep, oid, data, handler, 4000);
        }

        public static void SetValueAsync(IPEndPoint ep, string oid, ISnmpData data, Action<ISnmpData> handler, int timeout)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                List<Variable> vList = new List<Variable> { new Variable(new ObjectIdentifier(oid), data) };

                try
                {
                    IList<Variable> value = Messenger.Set(VersionCode.V2, ep, new OctetString("private"), vList, timeout);
                    if ((value.Count == 1) && (value[0].Data.TypeCode != SnmpType.NoSuchInstance))
                    {
                        _context.Post(delegate
                        {
                            if (!_closed)
                            {
                                handler(value[0].Data);
                            }
                        }, "SnmpAgent.ValueSetted");
                    }
                }
                catch (Exception)
                {

                }
            });
        }

        public static void GetAsync(IPEndPoint ep, IList<Variable> vList, Action<IList<Variable>> handler)
        {
            GetAsync(ep, vList, handler, 4000);
        }

        public static void GetAsync(IPEndPoint ep, IList<Variable> vList, Action<IList<Variable>> handler, int timeout)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    IList<Variable> results = Messenger.Get(VersionCode.V2, ep, new OctetString("public"), vList, timeout);
                    _context.Post(delegate
                    {
                        if (!_closed)
                        {
                            handler(results);
                        }
                    }, "SnmpAgent.GetResult");
                }
                catch (Exception /*e*/)
                {

                }
            });
        }

        public static void Trap(string oid, ISnmpData data, params IPEndPoint[] eps)
        {
            Trap(new ObjectIdentifier(oid), data, eps);
        }

        public static void Trap(ObjectIdentifier oid, ISnmpData data, params IPEndPoint[] eps)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    List<Variable> vList = new List<Variable> { new Variable(oid, data) };

                    foreach (IPEndPoint ep in eps)
                    {
                        Messenger.SendTrapV2(0, VersionCode.V2, ep, new OctetString("private"), new ObjectIdentifier(OID_TRAP_GENERA_INCIDENCIA), 0, vList);
                    }
                }
                catch (Exception /*e*/)
                {
                }
            });
        }

        #region Private Members

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private static SnmpEngine _engine;
        private static ObjectStore _store;
        private static SynchronizationContext _context;
        private static bool _closed;

        private static void TrapV1Received(object sender, TrapV1MessageReceivedEventArgs e)
        {
            _context.Post(delegate
            {
                if (!_closed)
                {
                    var pdu = e.TrapV1Message.Pdu();
                    if (pdu.ErrorStatus.ToInt32() == 0)
                    {
                        foreach (var v in pdu.Variables)
                        {
                            TrapReceived(v.Id.ToString(), v.Data, e.Sender);
                        }
                    }
                }
            }, "SnmpAgent.TrapV1Received");
        }

        private static void TrapV2Received(object sender, TrapV2MessageReceivedEventArgs e)
        {
            _context.Post(delegate
            {
                if (!_closed)
                {
                    var pdu = e.TrapV2Message.Pdu();
                    //if (pdu.ErrorStatus.ToInt32() == 0)
                    {
                        foreach (var v in pdu.Variables)
                        {
                            TrapReceived(v.Id.ToString(), v.Data, e.Sender);
                        }
                    }
                }
            }, "SnmpAgent.TrapV2Received");
        }

        private static void InformRequestReceived(object sender, InformRequestMessageReceivedEventArgs e)
        {
            _context.Post(delegate
            {
                if (!_closed)
                {
                    var pdu = e.InformRequestMessage.Pdu();
                    if (pdu.ErrorStatus.ToInt32() == 0)
                    {
                        foreach (var v in pdu.Variables)
                        {
                            TrapReceived(v.Id.ToString(), v.Data, e.Sender);
                        }
                    }
                }
            }, "SnmpAgent.InformReceived");
        }

        #endregion
    }

    sealed class SnmpStringObject : ScalarObject
    {
        public string Value
        {
            get { return _data.ToString(); }
            set
            {
                if (Value != value)
                {
                    _data = new OctetString(value);
                }

                if ((_trapsEps != null) && (_trapsEps.Length > 0))
                {
                    SnmpAgent.Trap(Variable.Id, _data, _trapsEps);
                }
            }
        }
        public void SendTrap(string value)
        {
            OctetString data = new OctetString(value);

            if ((_trapsEps != null) && (_trapsEps.Length > 0))
                SnmpAgent.Trap(Variable.Id, data, _trapsEps);
        }

        public override ISnmpData Data
        {
            get { return _data; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                _data = (OctetString)value;
            }
        }

        public SnmpStringObject(string oid, string value, params IPEndPoint[] trapsEps)
            : base(oid)
        {
            _data = new OctetString(value);
            _trapsEps = trapsEps;
        }

        public static SnmpStringObject Get(string oid)
        {
            return SnmpAgent.Store.GetObject(new ObjectIdentifier(oid)) as SnmpStringObject;
        }

        #region Private Members

        private Lextm.SharpSnmpLib.OctetString _data;
        private IPEndPoint[] _trapsEps;

        #endregion
    }

}
