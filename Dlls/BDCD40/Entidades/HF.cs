using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CD40.BD.Entidades
{

    public class HFParams : Tablas
    {
        public enum TipoRecursoRadio
        {
            Audio_RX,
            Audio_TX,
            Audio_RX_TX,
            Audio_HF_TX,
            Audio_NM,
            Datos_RX,
            Datos_TX,
            Datos_RX_TX
        };


        #region Propiedades de HFParams
        // Identificador del sistema al que pertenece el recurso
        protected string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }

        // Identificador del recurso
        protected string _IdRecursos;
        public string IdRecurso
        {
            get { return _IdRecursos; }
            set { _IdRecursos = value; }
        }
        // Tipo de Destino al que está asociado.
        private string _SipUri;
        public string SipUri
        {
            get { return _SipUri; }
            set { _SipUri = value; }
        }
        // Identificador del Destino al que está asociado.
        private string _IpGestor;
        public string IpGestor
        {
            get { return _IpGestor; }
            set { _IpGestor = value; }
        }
        // Identificador del emplazamineto al que está asociado.
        private string _Oid;
        public string Oid
        {
            get { return _Oid; }
            set { _Oid = value; }
        }

        // Frecuencia del equipo (Solo para principal).
        private string _Frecuencia;
        public string Frecuencia
        {
            get { return _Frecuencia; }
            set { _Frecuencia = value; }
        }

        /// <summary>
        /// Emplazamiento
        /// </summary>
        private string _IdEmplazamiento;
        public string IdEmplazamiento
        {
            get { return _IdEmplazamiento; }
            set { _IdEmplazamiento = value; }
        }

        /// <summary>
        /// Puerto
        /// </summary>
        private uint _Puerto;
        public uint Puerto
        {
            get { return _Puerto; }
            set { _Puerto = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _Offset;
        public int Offset
        {
            get { return _Offset; }
            set { _Offset = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _Canalizacion;
        public int Canalizacion
        {
            get { return _Canalizacion; }
            set { _Canalizacion = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _Modulacion;
        public int Modulacion
        {
            get { return _Modulacion; }
            set { _Modulacion = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _Potencia;
        public int Potencia
        {
            get { return _Potencia; }
            set { _Potencia = value; }
        }

        // 0: Receptor
        // 1: Transmisor
        // 2: Transceptor.
        private int _TipoEquipo;
        public int TipoEquipo
        {
            get { return _TipoEquipo; }
            set { _TipoEquipo = value; }
        }

        // 0: Monocanal
        // 1: Multicanal
        // 2: Otros.
        private int _TipoCanal;
        public int TipoCanal
        {
            get { return _TipoCanal; }
            set { _TipoCanal = value; }
        }

        // 0: HF
        // 1: VHF
        // 2: UHF
        private int _TipoFrecuencia;
        public int TipoFrecuencia
        {
            get { return _TipoFrecuencia; }
            set { _TipoFrecuencia = value; }
        }

        // 0: Principal
        // 1: Reserva
        // 2: Ambos.
        private int _TipoModo;
        public int TipoModo
        {
            get { return _TipoModo; }
            set { _TipoModo = value; }
        }

        /// <summary>
        ///  Valor de 0 a 100
        /// </summary>
        private int _PrioridadEquipo;
        public int PrioridadEquipo
        {
            get { return _PrioridadEquipo; }
            set { _PrioridadEquipo = value; }
        }

        /// <summary>
        /// 0: Mhz; 1: Hz
        /// </summary>
        private int _FormatoFrecuenciaPrincipal;
        public int FormatoFrecuenciaPrincipal
        {
            get { return _FormatoFrecuenciaPrincipal; }
            set { _FormatoFrecuenciaPrincipal = value; }
        }


        /// <summary>
        /// 1000: Rohde 4200; 1001: Jotron 7000
        /// </summary>
        private int _ModeloEquipo;
        public int ModeloEquipo
        {
            get { return _ModeloEquipo; }
            set { _ModeloEquipo = value; }
        }

        #endregion


        #region PRIVATED var
        const int ALL = -1;
        int _TipoRecurso;
        #endregion

        public HFParams()
            : base()
        {
            _TipoRecurso = ALL;
        }

        public HFParams(int tipoFrecuencia)
            : base()
        {
            _TipoRecurso = tipoFrecuencia;
        }

        //public new string DataSetSelectSQL()
        public override string DataSetSelectSQL()
        {
            //base.DataSetSelectSQL();
            StringBuilder strConsulta = new StringBuilder();

            if (_TipoRecurso == ALL)
            {
                if (IdSistema != null && IdRecurso != null)
                    strConsulta.Append("SELECT r.*,rr.IdDestino,rr.IdEmplazamiento FROM HFParams r  " +
                                        "INNER JOIN recursosradio rr ON rr.IdRecurso=r.IdRecurso " +
                                        "WHERE r.IdSistema='" + IdSistema + "' AND r.IdRecurso='" + IdRecurso + "'");
                else if (IdSistema != null)
                    strConsulta.Append("SELECT r.*,rr.IdDestino,rr.IdEmplazamiento FROM HFParams r  " +
                                        "INNER JOIN recursosradio rr ON rr.IdRecurso=r.IdRecurso " +
                                        "WHERE r.IdSistema='" + IdSistema + "'");
                else
                    strConsulta.Append("SELECT r.*,rr.IdDestino,rr.IdEmplazamiento FROM HFParams r " +
                                        "INNER JOIN recursosradio rr ON rr.IdRecurso=r.IdRecurso ");
            }
            else if (_TipoRecurso == (int)TipoRecursoRadio.Audio_HF_TX)
            {
                if (IdSistema != null && IdRecurso != null)
                {
                    strConsulta.Append("SELECT r.*,rr.IdDestino,rr.IdEmplazamiento, ");
                    strConsulta.Append("CASE WHEN rs.idEquipos IS NOT NULL AND eu.IpRed1 IS NOT NULL AND eu.SipPort  IS NOT NULL THEN CONCAT('<sip:',r.IdRecurso,'@',eu.IpRed1,':',eu.SipPort,'>') ");
                    strConsulta.Append("WHEN rs.idEquipos IS NOT NULL AND eu.IpRed1 IS NOT NULL AND eu.SipPort  IS NULL THEN  CONCAT('<sip:',r.IdRecurso,'@',eu.IpRed1) ");
                    strConsulta.Append("WHEN rs.idTifx IS NOT NULL AND GW.IpRed IS NOT NULL AND P.SipPortlocal IS NOT NULL THEN CONCAT('<sip:',r.IdRecurso,'@',GW.IpRed,':',P.SipPortlocal,'>') ");
                    strConsulta.Append("WHEN rs.idTifx IS NOT NULL AND GW.IpRed IS NOT NULL AND P.SipPortlocal IS NULL THEN CONCAT('<sip:',r.IdRecurso,'@',GW.IpRed,'>') ");
                    strConsulta.Append("ELSE NULL END as SipUriEquipo ");
                    strConsulta.Append("FROM HFParams r INNER JOIN recursos rs ON rs.IdRecurso=r.IdRecurso INNER JOIN recursosradio rr ON rr.IdRecurso=r.IdRecurso ");
                    strConsulta.Append("LEFT JOIN equiposeu eu ON  eu.IdSistema=rs.IdSistema AND eu.idequipos=rs.idequipos ");
                    strConsulta.Append("LEFT JOIN Tifx P ON  P.IdSistema=rs.IdSistema AND P.idTifx=rs.idTifx ");
                    strConsulta.Append("LEFT JOIN GwActivas GW ON  P.IdSistema=rs.IdSistema AND P.idTifx=rs.idTifx ");
                    strConsulta.AppendFormat("WHERE r.IdSistema='{0}' AND r.IdRecurso='{1}' AND rs.Tipo={2} ORDER BY r.idRecurso", IdSistema, IdRecurso, (int)TipoRecursoRadio.Audio_HF_TX);
                }
                else if (IdSistema != null)
                {
                    strConsulta.Append("SELECT r.*,rr.IdDestino,rr.IdEmplazamiento, ");
                    strConsulta.Append("CASE WHEN rs.idEquipos IS NOT NULL AND eu.IpRed1 IS NOT NULL AND eu.SipPort  IS NOT NULL THEN CONCAT('<sip:',r.IdRecurso,'@',eu.IpRed1,':',eu.SipPort,'>') ");
                    strConsulta.Append("WHEN rs.idEquipos IS NOT NULL AND eu.IpRed1 IS NOT NULL AND eu.SipPort  IS NULL THEN  CONCAT('<sip:',r.IdRecurso,'@',eu.IpRed1) ");
                    strConsulta.Append("WHEN rs.idTifx IS NOT NULL AND GW.IpRed IS NOT NULL AND P.SipPortlocal IS NOT NULL THEN CONCAT('<sip:',r.IdRecurso,'@',GW.IpRed,':',P.SipPortlocal,'>') ");
                    strConsulta.Append("WHEN rs.idTifx IS NOT NULL AND GW.IpRed IS NOT NULL AND P.SipPortlocal IS NULL THEN CONCAT('<sip:',r.IdRecurso,'@',GW.IpRed,'>') ");
                    strConsulta.Append("ELSE NULL END as SipUriEquipo ");
                    strConsulta.Append("FROM HFParams r INNER JOIN recursos rs ON rs.IdRecurso=r.IdRecurso INNER JOIN recursosradio rr ON rr.IdRecurso=r.IdRecurso ");
                    strConsulta.Append("LEFT JOIN equiposeu eu ON  eu.IdSistema=rs.IdSistema AND eu.idequipos=rs.idequipos ");
                    strConsulta.Append("LEFT JOIN Tifx P ON  P.IdSistema=rs.IdSistema AND P.idTifx=rs.idTifx ");
                    strConsulta.Append("LEFT JOIN GwActivas GW ON  GW.IdSistema=rs.IdSistema AND GW.idTifx=rs.idTifx ");
                    strConsulta.AppendFormat("WHERE r.IdSistema='{0}' AND rs.Tipo={1} ORDER BY r.idRecurso", IdSistema, (int)TipoRecursoRadio.Audio_HF_TX);
                }
                else
                {
                    strConsulta.Append("SELECT r.*,rr.IdDestino,rr.IdEmplazamiento, ");
                    strConsulta.Append("CASE WHEN rs.idEquipos IS NOT NULL AND eu.IpRed1 IS NOT NULL AND eu.SipPort  IS NOT NULL THEN CONCAT('<sip:',r.IdRecurso,'@',eu.IpRed1,':',eu.SipPort,'>') ");
                    strConsulta.Append("WHEN rs.idEquipos IS NOT NULL AND eu.IpRed1 IS NOT NULL AND eu.SipPort  IS NULL THEN  CONCAT('<sip:',r.IdRecurso,'@',eu.IpRed1) ");
                    strConsulta.Append("WHEN rs.idTifx IS NOT NULL AND GW.IpRed IS NOT NULL AND P.SipPortlocal IS NOT NULL THEN CONCAT('<sip:',r.IdRecurso,'@',GW.IpRed,':',P.SipPortlocal,'>') ");
                    strConsulta.Append("WHEN rs.idTifx IS NOT NULL AND GW.IpRed IS NOT NULL AND P.SipPortlocal IS NULL THEN CONCAT('<sip:',r.IdRecurso,'@',GW.IpRed,'>') ");
                    strConsulta.Append("ELSE NULL END as SipUriEquipo ");
                    strConsulta.Append("FROM HFParams r INNER JOIN recursos rs ON rs.IdRecurso=r.IdRecurso INNER JOIN recursosradio rr ON rr.IdRecurso=r.IdRecurso ");
                    strConsulta.Append("LEFT JOIN equiposeu eu ON  eu.IdSistema=rs.IdSistema AND eu.idequipos=rs.idequipos ");
                    strConsulta.Append("LEFT JOIN Tifx P ON  P.IdSistema=rs.IdSistema AND P.idTifx=rs.idTifx ");
                    strConsulta.Append("LEFT JOIN GwActivas GW ON  GW.IdSistema=rs.IdSistema AND GW.idTifx=rs.idTifx ");
                    strConsulta.AppendFormat("WHERE rs.Tipo={0} ", (int)TipoRecursoRadio.Audio_HF_TX);
                }
            }
            else
            {
                //Recursos M+N TipoRecursoRadio.Audio_NM
                if (IdSistema != null && IdRecurso != null)
                {
                    strConsulta.Append("SELECT r.*,rr.IdDestino,rr.IdEmplazamiento, ");
                    strConsulta.Append("CASE WHEN rs.idEquipos IS NOT NULL AND eu.IpRed1 IS NOT NULL AND eu.SipPort  IS NOT NULL THEN CONCAT('<sip:',r.IdRecurso,'@',eu.IpRed1,':',eu.SipPort,'>') ");
                    strConsulta.Append("WHEN rs.idEquipos IS NOT NULL AND eu.IpRed1 IS NOT NULL AND eu.SipPort  IS NULL THEN  CONCAT('<sip:',r.IdRecurso,'@',eu.IpRed1) ");
                    strConsulta.Append("WHEN rs.idTifx IS NOT NULL AND GW.IpRed IS NOT NULL AND P.SipPortlocal IS NOT NULL THEN CONCAT('<sip:',r.IdRecurso,'@',GW.IpRed,':',P.SipPortlocal,'>') ");
                    strConsulta.Append("WHEN rs.idTifx IS NOT NULL AND GW.IpRed IS NOT NULL AND P.SipPortlocal IS NULL THEN CONCAT('<sip:',r.IdRecurso,'@',GW.IpRed,'>') ");
                    strConsulta.Append("ELSE NULL END as SipUriEquipo ");
                    strConsulta.Append("FROM HFParams r INNER JOIN recursos rs ON rs.IdRecurso=r.IdRecurso ");
                    strConsulta.Append("INNER JOIN recursosradio rr ON rr.IdRecurso=r.IdRecurso ");
                    strConsulta.Append("LEFT JOIN equiposeu eu ON  eu.IdSistema=rs.IdSistema AND eu.idequipos=rs.idequipos ");
                    strConsulta.Append("LEFT JOIN Tifx P ON  P.IdSistema=rs.IdSistema AND P.idTifx=rs.idTifx ");
                    strConsulta.Append("LEFT JOIN GwActivas GW ON  GW.IdSistema=rs.IdSistema AND GW.idTifx=rs.idTifx ");
                    strConsulta.AppendFormat("WHERE r.IdSistema='{0}' AND r.IdRecurso='{1}' AND (rs.Tipo=4 OR rs.Tipo=5 OR rs.Tipo=6) ", IdSistema, IdRecurso);
                }
                else if (IdSistema != null)
                {
                    //Se descartan los recursos N+M Principales que no tienen destino asociado
                    //hfParam.TipoModo==Principal y IdDestino es null
                    strConsulta.Append("SELECT r.*,rr.IdDestino,rr.IdEmplazamiento, ");
                    strConsulta.Append("CASE WHEN rs.idEquipos IS NOT NULL AND eu.IpRed1 IS NOT NULL AND eu.SipPort  IS NOT NULL THEN CONCAT('<sip:',r.IdRecurso,'@',eu.IpRed1,':',eu.SipPort,'>') ");
                    strConsulta.Append("WHEN rs.idEquipos IS NOT NULL AND eu.IpRed1 IS NOT NULL AND eu.SipPort  IS NULL THEN  CONCAT('<sip:',r.IdRecurso,'@',eu.IpRed1) ");
                    strConsulta.Append("WHEN rs.idTifx IS NOT NULL AND GW.IpRed IS NOT NULL AND P.SipPortlocal IS NOT NULL THEN CONCAT('<sip:',r.IdRecurso,'@',GW.IpRed,':',P.SipPortlocal,'>') ");
                    strConsulta.Append("WHEN rs.idTifx IS NOT NULL AND GW.IpRed IS NOT NULL AND P.SipPortlocal IS NULL THEN CONCAT('<sip:',r.IdRecurso,'@',GW.IpRed,'>') ");
                    strConsulta.Append("ELSE NULL END as SipUriEquipo ");

                    strConsulta.Append("FROM HFParams r INNER JOIN recursos rs ON rs.IdRecurso=r.IdRecurso ");
                    strConsulta.Append("INNER JOIN recursosradio rr ON rr.IdRecurso=r.IdRecurso ");
                    strConsulta.Append("LEFT JOIN equiposeu eu ON  eu.IdSistema=rs.IdSistema AND eu.idequipos=rs.idequipos ");
                    strConsulta.Append("LEFT JOIN Tifx P ON  P.IdSistema=rs.IdSistema AND P.idTifx=rs.idTifx ");
                    strConsulta.Append("LEFT JOIN GwActivas GW ON  GW.IdSistema=rs.IdSistema AND GW.idTifx=rs.idTifx ");
                    strConsulta.AppendFormat("WHERE r.IdSistema='{0}' AND (rs.Tipo=4 OR rs.Tipo=5 OR rs.Tipo=6) ", IdSistema);
                    strConsulta.Append("AND (rs.Tipo=4 OR rs.Tipo=5 OR rs.Tipo=6) AND ((r.TipoModo=0 AND rr.IdDestino IS NOT NULL) OR (r.TipoModo<>0)) ORDER BY r.IdRecurso ");
                }
                else
                {
                    strConsulta.Append("SELECT r.*,rr.IdDestino,rr.IdEmplazamiento FROM HFParams r INNER JOIN recursos rs ON rs.IdRecurso=r.IdRecurso ");
                    strConsulta.AppendFormat("INNER JOIN recursosradio rr ON rr.IdRecurso=r.IdRecurso WHERE rs.Tipo={0} ", _TipoRecurso);
                }
            }

            return strConsulta.ToString();
        }

        public override System.Collections.Generic.List<Tablas> ListSelectSQL(System.Data.DataSet ds)
        {
            ListaResultado.Clear();

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    HFParams pr = new HFParams();

                    IdRecurso = pr.IdRecurso = (string)dr["IdRecurso"];
                    IdSistema = pr.IdSistema = (string)dr["IdSistema"];

                    if (dr.Table.Columns.Contains("SipUriEquipo"))
                        if (dr["SipUriEquipo"] != System.DBNull.Value)
                            pr.SipUri = (string)dr["SipUriEquipo"];

                    if (dr["IpGestor"] != System.DBNull.Value)
                        pr.IpGestor = (string)dr["IpGestor"];
                    if (dr["Oid"] != System.DBNull.Value)
                        pr.Oid = (string)dr["Oid"];
                    if (dr["TipoEquipo"] != System.DBNull.Value)
                        pr.TipoEquipo = (int)dr["TipoEquipo"];
                    if (dr["TipoCanal"] != System.DBNull.Value)
                        pr.TipoCanal = (int)dr["TipoCanal"];
                    if (dr["TipoFrecuencia"] != System.DBNull.Value)
                        pr.TipoFrecuencia = (int)dr["TipoFrecuencia"];
                    if (dr["IdEmplazamiento"] != System.DBNull.Value)
                        pr.IdEmplazamiento = (string)dr["IdEmplazamiento"];
                    if (dr["TipoModo"] != System.DBNull.Value)
                        pr.TipoModo = (int)dr["TipoModo"];
                    if (dr["PrioridadEquipo"] != System.DBNull.Value)
                        pr.PrioridadEquipo = (int)dr["PrioridadEquipo"];
                    if (dr["Frecuencia"] != System.DBNull.Value)
                        pr.Frecuencia = (string)dr["Frecuencia"];
                    if (dr["Puerto"] != System.DBNull.Value)
                        pr.Puerto = (uint)(int)dr["Puerto"];
                    if (dr["Offset"] != System.DBNull.Value)
                        pr.Offset = (int)dr["Offset"];
                    if (dr["Canalizacion"] != System.DBNull.Value)
                        pr.Canalizacion = (int)dr["Canalizacion"];
                    if (dr["Modulacion"] != System.DBNull.Value)
                        pr.Modulacion = (int)dr["Modulacion"];
                    if (dr["Potencia"] != System.DBNull.Value)
                        pr.Potencia = (int)dr["Potencia"];
                    if (dr["FormatoFrecuenciaPrincipal"] != System.DBNull.Value)
                        pr.FormatoFrecuenciaPrincipal = (int)dr["FormatoFrecuenciaPrincipal"];
                    if (dr["ModeloEquipo"] != System.DBNull.Value)
                        pr.ModeloEquipo = (int)dr["ModeloEquipo"];

                    ListaResultado.Add(pr);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
            string[] consulta = new string[2];
            StringBuilder strCadena = new StringBuilder();

            strCadena.Append("INSERT INTO HFParams (IdSistema,IdRecurso,IpGestor,Oid,Frecuencia,TipoEquipo,TipoCanal,TipoFrecuencia,TipoModo,PrioridadEquipo,Puerto,Offset,Canalizacion,Modulacion,Potencia,FormatoFrecuenciaPrincipal,ModeloEquipo)");
            strCadena.AppendFormat(" VALUES ('{0}','{1}','{2}','{3}','{4}',{5},{6},", IdSistema, IdRecurso, IpGestor, Oid, Frecuencia, TipoEquipo, TipoCanal);
            strCadena.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9})", TipoFrecuencia, TipoModo, PrioridadEquipo, Puerto, Offset, Canalizacion, Modulacion,
                                            Potencia, FormatoFrecuenciaPrincipal, ModeloEquipo);

            consulta[0] = strCadena.ToString();
            consulta[1] = ReplaceSQL(IdSistema, "HFParams");

            strCadena.Clear();

            return consulta;
        }

        public override string[] UpdateSQL()
        {
            string[] consulta = new string[2];

            StringBuilder strCadena = new StringBuilder();


            strCadena.AppendFormat("UPDATE HFParams SET IdRecurso='{0}',IpGestor='{1}',Oid='{2}',Frecuencia='{3}',", IdRecurso, IpGestor, Oid, Frecuencia);
            strCadena.AppendFormat("TipoEquipo={0},TipoCanal={1},TipoFrecuencia={2},TipoModo={3},PrioridadEquipo={4}", TipoEquipo, TipoCanal, TipoFrecuencia, TipoModo, PrioridadEquipo);
            strCadena.AppendFormat("Puerto={0},Offset={1},Canalizacion={2},Modulacion={3},Potencia={4},FormatoFrecuenciaPrincipal={5},ModeloEquipo={6} ",
                                    Puerto, Offset, Canalizacion, Modulacion, Potencia, FormatoFrecuenciaPrincipal, ModeloEquipo);
            strCadena.AppendFormat("WHERE IdSistema='{0}' AND IdRecurso='{1}'", IdSistema, IdRecurso);


            consulta[0] = strCadena.ToString();
            consulta[1] = ReplaceSQL(IdSistema, "HFParams");

            strCadena.Clear();

            return consulta;
        }

        public override string[] DeleteSQL()
        {
            string[] consulta = new string[2];

            StringBuilder strConsulta = new StringBuilder();

            if (IdSistema != null && IdRecurso != null)
                strConsulta.AppendFormat("DELETE FROM HFParams WHERE IdSistema='{0}' AND IdRecurso='{1}' ", IdSistema, IdRecurso);
            else if (IdSistema != null)
                strConsulta.AppendFormat("DELETE FROM HFParams WHERE IdSistema='{0}' ", IdSistema);
            else
                strConsulta.Append("DELETE FROM HFParams");

            consulta[0] = strConsulta.ToString();
            consulta[1] = ReplaceSQL(IdSistema, "HFParams");

            strConsulta.Clear();

            return consulta;
        }
    }

    public class HFRangoFrecuencias : Tablas
    {
        #region Propiedades de HFParams
        // Identificador del sistema al que pertenece el recurso
        protected string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }

        // Identificador del recurso
        protected string _IdRecursos;
        public string IdRecurso
        {
            get { return _IdRecursos; }
            set { _IdRecursos = value; }
        }
        // Tipo de Destino al que está asociado.
        private uint _Min;
        public uint Min
        {
            get { return _Min; }
            set { _Min = value; }
        }
        // Identificador del Destino al que está asociado.
        private uint _Max;
        public uint Max
        {
            get { return _Max; }
            set { _Max = value; }
        }
        #endregion

        public HFRangoFrecuencias()
            : base()
        { }

        //public new string DataSetSelectSQL()
        public override string DataSetSelectSQL()
        {
            //base.DataSetSelectSQL();

            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdRecurso != null)
                Consulta.Append("SELECT r.* FROM HFRangoFrecuencias r WHERE r.IdSistema='" + IdSistema + "' AND r.IdRecurso='" + IdRecurso + "'");
            else if (IdSistema != null)
                Consulta.Append("SELECT r.* FROM HFRangoFrecuencias r WHERE r.IdSistema='" + IdSistema + "'");
            else
                Consulta.Append("SELECT r.* FROM HFRangoFrecuencias r");

            return Consulta.ToString();
        }

        public override System.Collections.Generic.List<Tablas> ListSelectSQL(System.Data.DataSet ds)
        {
            ListaResultado.Clear();

            if (ds != null)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    HFRangoFrecuencias pr = new HFRangoFrecuencias();

                    IdRecurso = pr.IdRecurso = (string)dr["IdRecurso"];
                    IdSistema = pr.IdSistema = (string)dr["IdSistema"];

                    if (dr["Min"] != System.DBNull.Value)
                        pr.Min = Convert.ToUInt32(dr["Min"]);
                    if (dr["Max"] != System.DBNull.Value)
                        pr.Max = Convert.ToUInt32(dr["Max"]);

                    ListaResultado.Add(pr);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
            string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO HFRangoFrecuencias (IdSistema,IdRecurso,Min,Max)" +
                            " VALUES ('" + IdSistema + "','" +
                                            IdRecurso + "'," +
                                            Min + "," +
                                            Max + ")");

            consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(IdSistema, "HFRangoFrecuencias");

            return consulta;
        }

        public override string[] UpdateSQL()
        {
            string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE HFRangoFrecuencias SET IdRecurso='" + IdRecurso + "','" +
                                            "IdSistema='" + IdSistema + "'," +
                                            "Min=" + Min + "," +
                                            "Max=" + Max + " " +
                                            "WHERE IdRecurso='" + IdRecurso + "' AND " + "IdSistema='" + IdSistema + "'"
                                            );

            consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(IdSistema, "HFRangoFrecuencias");

            return consulta;
        }

        public override string[] DeleteSQL()
        {
            string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdRecurso != null)
                Consulta.Append("DELETE FROM HFRangoFrecuencias WHERE IdSistema='" + IdSistema + "' AND IdRecurso='" + IdRecurso + "'");
            else if (IdSistema != null)
                Consulta.Append("DELETE FROM HFRangoFrecuencias WHERE IdSistema='" + IdSistema + "'");
            else
                Consulta.Append("DELETE FROM HFRangoFrecuencias");

            consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(IdSistema, "HFRangoFrecuencias");

            return consulta;
        }
    }

}
