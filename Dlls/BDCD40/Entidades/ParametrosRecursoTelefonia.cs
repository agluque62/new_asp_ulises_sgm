using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class ParametrosRecursoTelefonia : ParametrosTFSCV
    {
        #region Propiedades
        // Identificador del recurso
        protected string _IdRecursoSCV;
        protected string IdRecursoSCV
        {
            get { return _IdRecursoSCV; }
            set { _IdRecursoSCV = value; }
        }
        // Identificador del sistema al que pertenece el recurso
        protected string _IdSistemaSCV;
        protected string IdSistemaSCV
        {
            get { return _IdSistemaSCV; }
            set { _IdSistemaSCV = value; }
        }
        // Tipo del Recurso
        protected uint _TipoRecurso;
        protected uint TipoRecurso
        {
            get { return _TipoRecurso; }
            set { _TipoRecurso = value; }
        }
        // Selección ganancia/AGC TX. 0: Ganancia; 1: AGC
        private uint _GananciaAGCTX;
        public uint GananciaAGCTX
        {
            get { return _GananciaAGCTX; }
            set { _GananciaAGCTX = value; }
        }
        // Ganancia o nivel salida AGC TX. En dBm
        private float _GananciaAGCTXdBm;
        public float GananciaAGCTXdBm
        {
            get { return _GananciaAGCTXdBm; }
            set { _GananciaAGCTXdBm = value; }
        }
        // Selección ganancia/AGC RX. 0: Ganancia; 1: AGC
        private uint _GananciaAGCRX;
        public uint GananciaAGCRX
        {
            get { return _GananciaAGCRX; }
            set { _GananciaAGCRX = value; }
        }
        // Ganancia o nivel salida AGC RX. En dBm
        private float _GananciaAGCRXdBm;
        public float GananciaAGCRXdBm
        {
            get { return _GananciaAGCRXdBm; }
            set { _GananciaAGCRXdBm = value; }
        }
        // Usar supersión silencio
        private bool _SupresionSilencio;
        public bool SupresionSilencio
        {
            get { return _SupresionSilencio; }
            set { _SupresionSilencio = value; }
        }
        // Tamaño por defecto trama RTP. En ms. Por defecto 20msg
        private uint _TamRTP;
        public uint TamRTP
        {
            get { return _TamRTP; }
            set { _TamRTP = value; }
        }
        //Codec preferido. 0:G711 ley A; 1:G711 ley µ
        private uint _Codec;
        public uint Codec
        {
            get { return _Codec; }
            set { _Codec = value; }
        }

        //VMG 31/10/2018
        //Tiempo maximo llamada entrante en lineas tlf
        private uint _iPrTmLlamEntrante;
        public uint iPrTmLlamEntrante
        {
            get { return _iPrTmLlamEntrante; }
            set { _iPrTmLlamEntrante = value; }
        }
        //Deteccion dtmf para líneas BC
        private byte _iPrDetDtmf;
        public byte iPrDetDtmf
        {
            get { return _iPrDetDtmf; }
            set { _iPrDetDtmf = value; }
        }
        //Tiempo teniendo información acústica antes de pasar el interfaz a reposo 
        private uint _TPrReleaseBL;
        public uint TPrReleaseBL
        {
            get { return _TPrReleaseBL; }
            set { _TPrReleaseBL = value; }
        }
        //Periodo para supervisar señal ring en llamadas entrantes
        private uint _iPrPeriodoSpvRing;
        public uint iPrPeriodoSpvRing
        {
            get { return _iPrPeriodoSpvRing; }
            set { _iPrPeriodoSpvRing = value; }
        }
        //Número de veces que hay que supervisar para dar por válido un valor
        private uint _iPrFiltroSpvRing;
        public uint iPrFiltroSpvRing
        {
            get { return _iPrFiltroSpvRing; }
            set { _iPrFiltroSpvRing = value; }
        }
        //Detección de Caller-Id en llamadas entrantes
        private byte _iPrDetCallerId;
        public byte iPrDetCallerId
        {
            get { return _iPrDetCallerId; }
            set { _iPrDetCallerId = value; }
        }
        //Tiempo para detectar CallerId en unidades de 100 ms
        private uint _iPrTmCallerId;
        public uint iPrTmCallerId
        {
            get { return _iPrTmCallerId; }
            set { _iPrTmCallerId = value; }
        }
        //Tiene en cuenta la inversión de polaridad en el comportamiento de la línea de AB
        private byte _iPrDetInversionPol;
        public byte iPrDetInversionPol
        {
            get { return _iPrDetInversionPol; }
            set { _iPrDetInversionPol = value; }
        }
        //Tiempo para detectar fin de llamada desde que llega ringoff para lineas de AB
        private uint _iPrTmDetFinLlamada;
        public uint iPrTmDetFinLlamada
        {
            get { return _iPrTmDetFinLlamada; }
            set { _iPrTmDetFinLlamada = value; }
        }
        //Deteccion del tipo de interface de telefonia
        private Tipos.TipoInterface _tipoInterface;
        public Tipos.TipoInterface tipoInterface
        {
            get { return _tipoInterface; }
            set { _tipoInterface = value; }
        }
        #endregion

        public ParametrosRecursoTelefonia()
        {
            TipoRecurso = (uint)Tipos.Tipo_Recurso.TR_TELEFONIA;
        }

        public override /* sealed */ string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
            if (IdSistemaSCV != null && IdRecursoSCV != null)
                Consulta.Append("SELECT * FROM ParametrosRecurso WHERE IdSistema='" + IdSistemaSCV + "' AND IdRecurso='" + IdRecursoSCV + "' AND TipoRecurso='" + TipoRecurso + "'");
            else if (IdSistemaSCV != null)
                Consulta.Append("SELECT * FROM ParametrosRecurso WHERE IdSistema='" + IdSistemaSCV + "' AND TipoRecurso=" + TipoRecurso);
            else
                Consulta.Append("SELECT * FROM ParametrosRecurso");

            return Consulta.ToString();
        }

        public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds)
        {
            System.Collections.Generic.List<Tablas> listaParametros = new System.Collections.Generic.List<Tablas>();

            //DataSetResultado = DataSetSelectSQL();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    ParametrosRecursoTelefonia pr = new ParametrosRecursoTelefonia();

                    pr.IdRecursoSCV = (string)dr["IdRecurso"];
                    pr.IdSistemaSCV = (string)dr["IdSistema"];
                    pr.TipoRecurso = (uint)dr["TipoRecurso"];
                    if (dr["GananciaAGCTX"] != System.DBNull.Value)
                        pr.GananciaAGCTX = (uint)dr["GananciaAGCTX"];
                    if (dr["GananciaAGCTXdBm"] != System.DBNull.Value)
                        pr.GananciaAGCTXdBm = (float)dr["GananciaAGCTXdBm"];
                    if (dr["GananciaAGCRX"] != System.DBNull.Value)
                        pr.GananciaAGCRX = (uint)dr["GananciaAGCRX"];
                    if (dr["GananciaAGCRXdBm"] != System.DBNull.Value)
                        pr.GananciaAGCRXdBm = (float)dr["GananciaAGCRXdBm"];
                    if (dr["SupresionSilencio"] != System.DBNull.Value)
                        pr.SupresionSilencio = (bool)dr["SupresionSilencio"];   //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0;
                    if (dr["TamRTP"] != System.DBNull.Value)
                        pr.TamRTP = (uint)dr["TamRTP"];
                    if (dr["Codec"] != System.DBNull.Value)
                        pr.Codec = (uint)dr["Codec"];

                    listaParametros.Add(pr);
                }
            }
            return listaParametros;
        }

        public override string[] InsertSQL()
        {
            string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO ParametrosRecurso (IdSistema,IdRecurso,TipoRecurso,GananciaAGCTX,GananciaAGCTXdBm,GananciaAGCRX,GananciaAGCRXdBm," +
                            "SupresionSilencio,TamRTP,Codec)" +
                            " VALUES ('" + IdSistemaSCV + "','" +
                                         IdRecursoSCV + "'," +
                                         TipoRecurso + "," +
                                         GananciaAGCTX + "," +
                                         GananciaAGCTXdBm + "," +
                                         GananciaAGCRX + "," +
                                         GananciaAGCRXdBm + "," +
                                         SupresionSilencio + "," +
                                         TamRTP + "," +
                                         Codec + ")");

            consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(IdSistemaSCV, "ParametrosRecurso");
            return consulta;
        }

        public override string[] UpdateSQL()
        {
            string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE ParametrosRecurso SET IdRecurso='" + IdRecursoSCV + "'," +
                                            "IdSistema='" + IdSistemaSCV + "'," +
                                            "TipoRecurso=" + TipoRecurso + "," +
                                            "GananciaAGCTX=" + GananciaAGCTX + "," +
                                            "GananciaAGCTXdBm=" + GananciaAGCTXdBm + "," +
                                            "GananciaAGCRX=" + GananciaAGCRX + "," +
                                            "GananciaAGCRXdBm=" + GananciaAGCRXdBm + "," +
                                            "SupresionSilencio=" + SupresionSilencio + "," +
                                            "TamRTP=" + TamRTP + "," +
                                            "Codec=" + Codec + " " +
                                            "WHERE IdRecurso='" + IdRecursoSCV + "' AND " + "IdSistema='" + IdSistemaSCV + "'"
                                            );

            consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(IdSistemaSCV, "ParametrosRecurso");
            return consulta;
        }

        public override string[] DeleteSQL()
        {
            string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            if (IdSistemaSCV != null && IdRecursoSCV != null)
                Consulta.Append("DELETE FROM ParametrosRecurso WHERE IdSistema='" + IdSistemaSCV + "' AND IdRecurso='" + IdRecursoSCV + "'" + "' AND TipoRecurso='" + TipoRecurso + "'");
            else if (IdSistemaSCV != null)
                Consulta.Append("DELETE FROM ParametrosRecurso WHERE IdSistema='" + IdSistemaSCV + "'");
            else
                Consulta.Append("DELETE FROM ParametrosRecurso");


            consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(IdSistemaSCV, "ParametrosRecurso");
            return consulta;
        }

        //public override int SelectCountSQL(string where)
        //{
        //    Consulta.Remove(0, Consulta.Length);
        //    Consulta.Append("SELECT COUNT(*) FROM ParametrosRecurso WHERE " + where);

        //    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
        //}

        public override string[] UpdateDestinoSQL() { return null; }

        public override string[] LiberaDestinoSQL() { return null; }
    }

    public class ParametrosEM : ParametrosRecursoTelefonia
    {
        public const string _LADO_EYM = "EyM";
        public const string _LADO_PLR = "PLR";

        public const string _MODO_2W = "2W";
        public const string _MODO_4W = "4W";

        public const int _TYPE_I = 0;
        public const int _TYPE_II = 1;
        public const int _TYPE_III = 2;
        public const int _TYPE_IV = 3;
        public const int _TYPE_V = 4;

        #region Propiedades
        private Tipos.Tipo_EM _TipoEM;
        public Tipos.Tipo_EM TipoEM
        {
            get { return _TipoEM; }
            set { _TipoEM = value; }
        }
        #endregion

        public ParametrosEM()
            : base()
        {
            _TipoEM = Tipos.Tipo_EM.Type_I;
            Modo = _MODO_2W;
            Lado = _LADO_EYM;
        }

        public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds)
        {
            System.Collections.Generic.List<Tablas> listaParametros = new System.Collections.Generic.List<Tablas>();

            //DataSetResultado = DataSetSelectSQL();
            if (ds != null)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    ParametrosEM pr = new ParametrosEM();

                    pr.IdRecursoSCV = (string)dr["IdRecurso"];
                    pr.IdSistemaSCV = (string)dr["IdSistema"];
                    pr.TipoRecurso = (uint)dr["TipoRecurso"];
                    if (dr["GananciaAGCTX"] != System.DBNull.Value)
                        pr.GananciaAGCTX = (uint)dr["GananciaAGCTX"];
                    if (dr["GananciaAGCTXdBm"] != System.DBNull.Value)
                        pr.GananciaAGCTXdBm = (int)dr["GananciaAGCTXdBm"];
                    if (dr["GananciaAGCRX"] != System.DBNull.Value)
                        pr.GananciaAGCRX = (uint)dr["GananciaAGCRX"];
                    if (dr["GananciaAGCRXdBm"] != System.DBNull.Value)
                        pr.GananciaAGCRXdBm = (int)dr["GananciaAGCRXdBm"];
                    if (dr["SupresionSilencio"] != System.DBNull.Value)
                        pr.SupresionSilencio = (bool)dr["SupresionSilencio"];   //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0;
                    if (dr["TamRTP"] != System.DBNull.Value)
                        pr.TamRTP = (uint)dr["TamRTP"];
                    if (dr["Codec"] != System.DBNull.Value)
                        pr.Codec = (uint)dr["Codec"];
                    if (dr["Lado"] != System.DBNull.Value)
                        pr.Lado = (string)dr["Lado"];
                    if (dr["Modo"] != System.DBNull.Value)
                        pr.Modo = (string)dr["Modo"];
                    if (dr["TipoEM"] != DBNull.Value)
                        pr.TipoEM = (Tipos.Tipo_EM)(int)dr["TipoEM"];

                    listaParametros.Add(pr);
                }
            }
            return listaParametros;
        }

        public override string[] InsertSQL()
        {
            string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO ParametrosRecurso (IdSistema,IdRecurso,TipoRecurso,GananciaAGCTX,GananciaAGCTXdBm,GananciaAGCRX,GananciaAGCRXdBm," +
                            "SupresionSilencio,TamRTP,Codec,TipoEM)" +
                            " VALUES ('" + IdSistemaSCV + "','" +
                                         IdRecursoSCV + "'," +
                                         TipoRecurso + "," +
                                         GananciaAGCTX + "," +
                                         GananciaAGCTXdBm + "," +
                                         GananciaAGCRX + "," +
                                         GananciaAGCRXdBm + "," +
                                         SupresionSilencio + "," +
                                         TamRTP + "," +
                                         Codec + "," +
                                         (int)TipoEM + ")");

            consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(IdSistemaSCV, "ParametrosRecurso");
            return consulta;
        }

        public override string[] UpdateSQL()
        {
            string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE ParametrosRecurso SET IdRecurso='" + IdRecursoSCV + "'," +
                                            "IdSistema='" + IdSistemaSCV + "'," +
                                            "TipoRecurso=" + TipoRecurso + "," +
                                            "GananciaAGCTX=" + GananciaAGCTX + "," +
                                            "GananciaAGCTXdBm=" + GananciaAGCTXdBm + "," +
                                            "GananciaAGCRX=" + GananciaAGCRX + "," +
                                            "GananciaAGCRXdBm=" + GananciaAGCRXdBm + "," +
                                            "SupresionSilencio=" + SupresionSilencio + "," +
                                            "TamRTP=" + TamRTP + "," +
                                            "Codec=" + Codec + "," +
                                            "TipoEM=" + (int)TipoEM + " " +
                                            "WHERE IdRecurso='" + IdRecursoSCV + "' AND " + "IdSistema='" + IdSistemaSCV + "'"
                                            );

            consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(IdSistemaSCV, "ParametrosRecurso");
            return consulta;
        }
    }

    public class ParametrosGrabacionTf : ParametrosEM
    {
        #region Propiedades
        private bool _GrabacionEd137;
        public bool GrabacionEd137
        {
            get { return _GrabacionEd137; }
            set { _GrabacionEd137 = value; }
        }
        #endregion

        public ParametrosGrabacionTf()
            : base()
        {
            _GrabacionEd137 = true;
        }

        public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds)
        {
            System.Collections.Generic.List<Tablas> listaParametros = new System.Collections.Generic.List<Tablas>();

            //DataSetResultado = DataSetSelectSQL();
            if (ds != null)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    ParametrosGrabacionTf pr = new ParametrosGrabacionTf();

                    pr.IdRecursoSCV = (string)dr["IdRecurso"];
                    pr.IdSistemaSCV = (string)dr["IdSistema"];
                    pr.TipoRecurso = (uint)dr["TipoRecurso"];
                    if (dr["GananciaAGCTX"] != System.DBNull.Value)
                        pr.GananciaAGCTX = (uint)dr["GananciaAGCTX"];
                    if (dr["GananciaAGCTXdBm"] != System.DBNull.Value)
                        pr.GananciaAGCTXdBm = (float)dr["GananciaAGCTXdBm"];
                    if (dr["GananciaAGCRX"] != System.DBNull.Value)
                        pr.GananciaAGCRX = (uint)dr["GananciaAGCRX"];
                    if (dr["GananciaAGCRXdBm"] != System.DBNull.Value)
                        pr.GananciaAGCRXdBm = (float)dr["GananciaAGCRXdBm"];
                    if (dr["SupresionSilencio"] != System.DBNull.Value)
                        pr.SupresionSilencio = (bool)dr["SupresionSilencio"];   //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0;
                    if (dr["TamRTP"] != System.DBNull.Value)
                        pr.TamRTP = (uint)dr["TamRTP"];
                    if (dr["Codec"] != System.DBNull.Value)
                        pr.Codec = (uint)dr["Codec"];
                    if (dr["Lado"] != System.DBNull.Value)
                        pr.Lado = (string)dr["Lado"];
                    if (dr["Modo"] != System.DBNull.Value)
                        pr.Modo = (string)dr["Modo"];
                    if (dr["TipoEM"] != DBNull.Value)
                        pr.TipoEM = (Tipos.Tipo_EM)(int)dr["TipoEM"];
                    if (dr["GrabacionEd137"] != System.DBNull.Value)
                        pr.GrabacionEd137 = (bool)dr["GrabacionEd137"];   //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0;

                    listaParametros.Add(pr);
                }
            }
            return listaParametros;
        }

        public override string[] InsertSQL()
        {
            string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            switch (tipoInterface)
            {
                case Tipos.TipoInterface.TI_BC:
                    Consulta.AppendFormat(System.Globalization.CultureInfo.GetCultureInfo("en-US"),
                        "INSERT INTO ParametrosRecurso (IdSistema,IdRecurso,TipoRecurso,GananciaAGCTX,GananciaAGCTXdBm," +
                        "GananciaAGCRX,GananciaAGCRXdBm,SupresionSilencio,TamRTP,Codec,TipoEM,GrabacionEd137,iTmLlamEntrante,iDetDtmf) " +
                        "VALUES ('{0}','{1}',{2},{3},{4:0.00},{5},{6:0.00},{7},{8},{9},{10},{11},{12},{13})",
                        IdSistemaSCV, IdRecursoSCV, TipoRecurso, GananciaAGCTX, GananciaAGCTXdBm, GananciaAGCRX, GananciaAGCRXdBm,
                        SupresionSilencio, TamRTP, Codec, (int)TipoEM, GrabacionEd137, iPrTmLlamEntrante, iPrDetDtmf);
                    break;
                case Tipos.TipoInterface.TI_BL:
                    Consulta.AppendFormat(System.Globalization.CultureInfo.GetCultureInfo("en-US"),
                        "INSERT INTO ParametrosRecurso (IdSistema,IdRecurso,TipoRecurso,GananciaAGCTX,GananciaAGCTXdBm," +
                        "GananciaAGCRX,GananciaAGCRXdBm,SupresionSilencio,TamRTP,Codec,TipoEM,GrabacionEd137,iTmLlamEntrante,TReleaseBL,iPeriodoSpvRing,iFiltroSpvRing) " +
                        "VALUES ('{0}','{1}',{2},{3},{4:0.00},{5},{6:0.00},{7},{8},{9},{10},{11},{12},{13},{14},{15})",
                        IdSistemaSCV, IdRecursoSCV, TipoRecurso, GananciaAGCTX, GananciaAGCTXdBm, GananciaAGCRX, GananciaAGCRXdBm,
                        SupresionSilencio, TamRTP, Codec, (int)TipoEM, GrabacionEd137, iPrTmLlamEntrante, TPrReleaseBL, iPrPeriodoSpvRing, iPrFiltroSpvRing);
                    break;
                case Tipos.TipoInterface.TI_AB:
                    Consulta.AppendFormat(System.Globalization.CultureInfo.GetCultureInfo("en-US"),
                        "INSERT INTO ParametrosRecurso (IdSistema,IdRecurso,TipoRecurso,GananciaAGCTX,GananciaAGCTXdBm," +
                        "GananciaAGCRX,GananciaAGCRXdBm,SupresionSilencio,TamRTP,Codec,TipoEM,GrabacionEd137,iTmLlamEntrante,iTmDetFinLlamada," +
                        "iPeriodoSpvRing,iFiltroSpvRing,iDetInversionPol,iDetCallerId,iTmCallerId) " +
                        "VALUES ('{0}','{1}',{2},{3},{4:0.00},{5},{6:0.00},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18})",
                        IdSistemaSCV, IdRecursoSCV, TipoRecurso, GananciaAGCTX, GananciaAGCTXdBm, GananciaAGCRX, GananciaAGCRXdBm,
                        SupresionSilencio, TamRTP, Codec, (int)TipoEM, GrabacionEd137, iPrTmLlamEntrante, iPrTmDetFinLlamada, iPrPeriodoSpvRing,
                        iPrFiltroSpvRing, iPrDetInversionPol, iPrDetCallerId, iPrTmCallerId);
                    break;
                default:
                    Consulta.AppendFormat(System.Globalization.CultureInfo.GetCultureInfo("en-US"),
                        "INSERT INTO ParametrosRecurso (IdSistema,IdRecurso,TipoRecurso,GananciaAGCTX,GananciaAGCTXdBm,GananciaAGCRX,GananciaAGCRXdBm," +
                        "SupresionSilencio,TamRTP,Codec,TipoEM,GrabacionEd137) VALUES ('{0}','{1}',{2},{3},{4:0.00},{5},{6:0.00},{7},{8},{9},{10},{11})",
                        IdSistemaSCV, IdRecursoSCV, TipoRecurso, GananciaAGCTX, GananciaAGCTXdBm, GananciaAGCRX, GananciaAGCRXdBm, SupresionSilencio,
                        TamRTP, Codec, (int)TipoEM, GrabacionEd137);
                    break;
            }
            consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(IdSistemaSCV, "ParametrosRecurso");
            return consulta;
        }

        public override string[] UpdateSQL()
        {
            string[] consulta = new string[3];

            Consulta.Remove(0, Consulta.Length);
            Consulta.AppendFormat(System.Globalization.CultureInfo.GetCultureInfo("en-US"), "UPDATE Recursos SET TipoRecurso={0} WHERE IdRecurso='{1}' AND IdSistema='{2}'", TipoRecurso, IdRecursoSCV, IdSistemaSCV);
            consulta[0] = Consulta.ToString();

            Consulta.Remove(0, Consulta.Length);
            //Salvo para LC metemos aquí los updates
            switch (tipoInterface)
            {
                case Tipos.TipoInterface.TI_BC:
                    Consulta.AppendFormat(System.Globalization.CultureInfo.GetCultureInfo("en-US"),
                        "REPLACE INTO ParametrosRecurso SET IdRecurso='{0}',IdSistema='{1}',TipoRecurso={2}," +
                        "GananciaAGCTX={3},GananciaAGCTXdBm={4:0.00},GananciaAGCRX={5},GananciaAGCRXdBm={6:0.00}," +
                        "SupresionSilencio={7},TamRTP={8},Codec={9},TipoEM={10},GrabacionEd137={11},iTmLlamEntrante={12},iDetDtmf={13}",
                        IdRecursoSCV, IdSistemaSCV, TipoRecurso, GananciaAGCTX, GananciaAGCTXdBm,
                        GananciaAGCRX, GananciaAGCRXdBm, SupresionSilencio, TamRTP, Codec, (int)TipoEM,
                        GrabacionEd137, iPrTmLlamEntrante, iPrDetDtmf);
                    break;
                case Tipos.TipoInterface.TI_BL:
                    Consulta.AppendFormat(System.Globalization.CultureInfo.GetCultureInfo("en-US"),
                        "REPLACE INTO ParametrosRecurso SET IdRecurso='{0}',IdSistema='{1}',TipoRecurso={2}," +
                        "GananciaAGCTX={3},GananciaAGCTXdBm={4:0.00},GananciaAGCRX={5},GananciaAGCRXdBm={6:0.00}," +
                        "SupresionSilencio={7},TamRTP={8},Codec={9},TipoEM={10},GrabacionEd137={11},iTmLlamEntrante={12},TReleaseBL={13}," +
                        "iPeriodoSpvRing={14},iFiltroSpvRing={15}",
                        IdRecursoSCV, IdSistemaSCV, TipoRecurso, GananciaAGCTX, GananciaAGCTXdBm,
                        GananciaAGCRX, GananciaAGCRXdBm, SupresionSilencio, TamRTP, Codec, (int)TipoEM,
                        GrabacionEd137, iPrTmLlamEntrante, TPrReleaseBL, iPrPeriodoSpvRing, iPrFiltroSpvRing);
                    break;
                case Tipos.TipoInterface.TI_AB:
                    Consulta.AppendFormat(System.Globalization.CultureInfo.GetCultureInfo("en-US"),
                        "REPLACE INTO ParametrosRecurso SET IdRecurso='{0}',IdSistema='{1}',TipoRecurso={2}," +
                        "GananciaAGCTX={3},GananciaAGCTXdBm={4:0.00},GananciaAGCRX={5},GananciaAGCRXdBm={6:0.00}," +
                        "SupresionSilencio={7},TamRTP={8},Codec={9},TipoEM={10},GrabacionEd137={11},iTmLlamEntrante={12}," +
                        "iTmDetFinLlamada={13},iPeriodoSpvRing={14},iFiltroSpvRing={15},iDetInversionPol={16}," +
                        "iDetCallerId={17},iTmCallerId={18}",
                        IdRecursoSCV, IdSistemaSCV, TipoRecurso, GananciaAGCTX, GananciaAGCTXdBm,
                        GananciaAGCRX, GananciaAGCRXdBm, SupresionSilencio, TamRTP, Codec, (int)TipoEM,
                        GrabacionEd137, iPrTmLlamEntrante, iPrTmDetFinLlamada, iPrPeriodoSpvRing,
                        iPrFiltroSpvRing, iPrDetInversionPol, iPrDetCallerId, iPrTmCallerId);
                    break;
                case Tipos.TipoInterface.TI_ATS_R2:
                case Tipos.TipoInterface.TI_ATS_N5:
                    Consulta.AppendFormat(System.Globalization.CultureInfo.GetCultureInfo("en-US"),
                        "REPLACE INTO ParametrosRecurso SET IdRecurso='{0}',IdSistema='{1}',TipoRecurso={2}," +
                        "GananciaAGCTX={3},GananciaAGCTXdBm={4:0.00},GananciaAGCRX={5},GananciaAGCRXdBm={6:0.00}," +
                        "SupresionSilencio={7},TamRTP={8},Codec={9},TipoEM={10},GrabacionEd137={11}",
                        IdRecursoSCV, IdSistemaSCV, TipoRecurso, GananciaAGCTX, GananciaAGCTXdBm,
                        GananciaAGCRX, GananciaAGCRXdBm, SupresionSilencio, TamRTP, Codec, (int)TipoEM,
                        GrabacionEd137);
                    break;
            }

            consulta[1] = Consulta.ToString();
            consulta[2] = ReplaceSQL(IdSistemaSCV, "ParametrosRecurso");
            return consulta;
        }
    }

}
