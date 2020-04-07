using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
	public class RecursosRadio : ParametrosRecursosRadioKASiccip
    {
        #region Propiedades de RecursosRadio
        // Identificador del recurso
        protected string _IdRecursos;
        public string IdRecurso
        {
            get { return _IdRecursos; }
            set { _IdRecursos = value; _IdRecursoSCV = value; }
        }
        // Identificador del sistema al que pertenece el recurso
        protected string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; _IdSistemaSCV = value; }
        }
        // Tipo de Destino al que está asociado.
        private uint _TipoDestino;
        public uint TipoDestino
        {
            get { return _TipoDestino; }
            set { _TipoDestino = value; }
        }
        // Identificador del Destino al que está asociado.
        private string _IdDestino;
        public string IdDestino
        {
            get { return _IdDestino; }
            set { _IdDestino = value; }
        }
        // Identificador del emplazamineto al que está asociado.
        private string _IdEmplazamiento;
        public string IdEmplazamiento
        {
            get { return _IdEmplazamiento; }
            set { _IdEmplazamiento = value; }
        }

        // Nombre de la tabla de calificación de audio. (No pertenece al modelo).
        private string _NameTablaBss;
        public string NameTablaBss
        {
            get { return _NameTablaBss; }
            set { _NameTablaBss = value; }
        }

        // Tipo de recurso radio
        private uint _Tipo;
        public uint Tipo
        {
            get { return _Tipo; }
            set { _Tipo = value; }
        }

        // Método BSS. RSSI; Nucleo
        private string _MetodoBSS;
        public string MetodoBSS
        {
            get { return _MetodoBSS; }
            set { _MetodoBSS = value; }
        }

        // Clave a la tabla de zonas
        private int _Zonas_IdZonas;
        public int Zonas_IdZonas
        {
            get { return _Zonas_IdZonas; }
            set { _Zonas_IdZonas = value; }
        }

        // Clave a la tabla de radio_param
        private int _Radio_param_idradio_param;
        public int Radio_param_idradio_param
        {
            get { return _Radio_param_idradio_param; }
            set { _Radio_param_idradio_param = value; }
        }

        // Clave a la tabla tabla_bss_recurso
        private int _IdTablaBss;
        public int IdTablaBss
        {
            get { return _IdTablaBss; }
            set { _IdTablaBss = value; }
        }

        // Nombre de la zona (No pertenece al modelo)
        private string _NombreZona;
        public string NombreZona
        {
            get { return _NombreZona; }
            set { _NombreZona = value; }
        }

        private int _GrsDelay;
        public int GrsDelay
        {
            get { return _GrsDelay; }
            set { _GrsDelay = value; }
        }

        private int _OffSetFrequency;
        public int OffSetFrequency
        {
            get { return _OffSetFrequency; }
            set { _OffSetFrequency = value; }
        }

        private bool _EnableEventPttSq;
        public bool EnableEventPttSq
        {
            get { return _EnableEventPttSq; }
            set { _EnableEventPttSq = value; }
        }

        private int _Metodos_bss_idmetodos_bss;
        public int Metodos_bss_idmetodos_bss
        {
            get { return _Metodos_bss_idmetodos_bss; }
            set { _Metodos_bss_idmetodos_bss = value; }
        }

        private string _RedundanciaRol;
        public string RedundanciaRol
        {
            get { return _RedundanciaRol; }
            set { _RedundanciaRol = value; }
        }

        private string _RedundanciaIdPareja;
        public string RedundanciaIdPareja
        {
            get { return _RedundanciaIdPareja; }
            set { _RedundanciaIdPareja = value; }
        }

        //MVO: se añade la recuperación de la pasarela asociado al recurso radio
        //idTIFX asociado al recurso radio
        private string _idTIFX;

        public string idTIFX
        {
            get { return _idTIFX; }
            set { _idTIFX = value; }
        }

        //MVO: se añade la recuperación del tipo frecuencia del destino radio asociado al recurso
        private uint _DestRad_tipoFrec;
        public uint DestRad_tipoFrec
        {
            get { return _DestRad_tipoFrec; }
            set { _DestRad_tipoFrec = value; }
        }

        #endregion

        public RecursosRadio()
            : base()
        {
            TipoRecurso = (uint)Tipos.Tipo_Recurso.TR_RADIO;
        }

		//public new string DataSetSelectSQL()
        public override string DataSetSelectSQL()
		{
            //base.DataSetSelectSQL();

            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdRecurso != null)
                Consulta.Append("SELECT r.*,z.Nombre as NameZona,rec.Tipo, pr.*, tb.name, rp.*,mb.name as NameBss,rec.idTIFX rec_idTIFX,destRad.TipoFrec DestRad_tipoFrec FROM RecursosRadio r " +
                                "LEFT JOIN zonas z ON z.idZonas=r.zonas_idZonas " +
                                "LEFT JOIN tabla_bss tb ON tb.idtabla_bss = r.tabla_bss_idtabla_bss " +
                                "LEFT JOIN radio_param rp ON rp.idradio_param = r.radio_param_idradio_param " +
                                "LEFT JOIN metodos_bss mb ON mb.idmetodos_bss = rp.metodos_bss_idmetodos_bss " +
                                "LEFT JOIN destinosradio destrad ON destrad.IdSistema=r.IdSistema AND destrad.IdDestino=r.IdDestino AND destrad.TipoDestino=r.TipoDestino " +
                                "INNER JOIN ParametrosRecurso pr on pr.IdSistema=r.IdSistema AND pr.IdRecurso=r.IdRecurso AND pr.TipoRecurso=r.TipoRecurso " +
                                "INNER JOIN Recursos rec on rec.IdSistema=r.IdSistema AND rec.IdRecurso=r.IdRecurso AND rec.TipoRecurso=r.TipoRecurso " +
                                    "WHERE r.IdSistema='" + IdSistema + "' AND r.IdRecurso='" + IdRecurso + "'");
            else if (IdSistema != null && IdDestino != null)
                Consulta.Append("SELECT r.*,z.Nombre as NameZona,rec.Tipo, pr.*, tb.name, rp.*,mb.name as NameBss,rec.idTIFX rec_idTIFX,destRad.TipoFrec DestRad_tipoFrec FROM RecursosRadio r " +
                                "LEFT JOIN zonas z ON z.idZonas=r.zonas_idZonas " +
                                "LEFT JOIN tabla_bss tb ON tb.idtabla_bss = r.tabla_bss_idtabla_bss " +
                                "LEFT JOIN radio_param rp ON rp.idradio_param = r.radio_param_idradio_param " +
                                "LEFT JOIN metodos_bss mb ON mb.idmetodos_bss = rp.metodos_bss_idmetodos_bss " +
                                "LEFT JOIN destinosradio destrad ON destrad.IdSistema=r.IdSistema AND destrad.IdDestino=r.IdDestino AND destrad.TipoDestino=r.TipoDestino " +
                                "INNER JOIN ParametrosRecurso pr on pr.IdSistema=r.IdSistema AND pr.IdRecurso=r.IdRecurso AND pr.TipoRecurso=r.TipoRecurso " +
                                "INNER JOIN Recursos rec on rec.IdSistema=r.IdSistema AND rec.IdRecurso=r.IdRecurso AND rec.TipoRecurso=r.TipoRecurso " +
                                    "WHERE r.IdSistema='" + IdSistema + "' AND r.IdDestino='" + IdDestino + "'");
            else if (IdSistema != null && IdEmplazamiento != null)
                Consulta.Append("SELECT r.*,z.Nombre as NameZona,rec.Tipo, pr.*, tb.name, rp.*,mb.name as NameBss,rec.idTIFX rec_idTIFX,destRad.TipoFrec DestRad_tipoFrec FROM RecursosRadio r " +
                                "LEFT JOIN zonas z ON z.idZonas=r.zonas_idZonas " +
                                "LEFT JOIN tabla_bss tb ON tb.idtabla_bss = r.tabla_bss_idtabla_bss " +
                                "LEFT JOIN radio_param rp ON rp.idradio_param = r.radio_param_idradio_param " +
                                "LEFT JOIN metodos_bss mb ON mb.idmetodos_bss = rp.metodos_bss_idmetodos_bss " +
                                "LEFT JOIN destinosradio destrad ON destrad.IdSistema=r.IdSistema AND destrad.IdDestino=r.IdDestino AND destrad.TipoDestino=r.TipoDestino " +
                                "INNER JOIN ParametrosRecurso pr on pr.IdSistema=r.IdSistema AND pr.IdRecurso=r.IdRecurso AND pr.TipoRecurso=r.TipoRecurso " +
                                "INNER JOIN Recursos rec on rec.IdSistema=r.IdSistema AND rec.IdRecurso=r.IdRecurso AND rec.TipoRecurso=r.TipoRecurso " +
                                    "WHERE r.IdSistema='" + IdSistema + "' AND r.IdEmplazamiento='" + IdEmplazamiento + "'");
            else if (IdSistema != null && Zonas_IdZonas != 0)
                Consulta.Append("SELECT r.*,z.Nombre as NameZona,rec.Tipo, pr.*, tb.name, rp.*,mb.name as NameBss,rec.idTIFX rec_idTIFX,destRad.TipoFrec DestRad_tipoFrec FROM RecursosRadio r " +
                                "LEFT JOIN zonas z ON z.idZonas=r.zonas_idZonas " +
                                "LEFT JOIN tabla_bss tb ON tb.idtabla_bss = r.tabla_bss_idtabla_bss " +
                                "LEFT JOIN radio_param rp ON rp.idradio_param = r.radio_param_idradio_param " +
                                "LEFT JOIN metodos_bss mb ON mb.idmetodos_bss = rp.metodos_bss_idmetodos_bss " +
                                "LEFT JOIN destinosradio destrad ON destrad.IdSistema=r.IdSistema AND destrad.IdDestino=r.IdDestino AND destrad.TipoDestino=r.TipoDestino " +
                                "INNER JOIN ParametrosRecurso pr on pr.IdSistema=r.IdSistema AND pr.IdRecurso=r.IdRecurso AND pr.TipoRecurso=r.TipoRecurso " +
                                "INNER JOIN Recursos rec on rec.IdSistema=r.IdSistema AND rec.IdRecurso=r.IdRecurso AND rec.TipoRecurso=r.TipoRecurso " +
                                    "WHERE r.IdSistema='" + IdSistema + "' AND r.zonas_idZonas=" + Zonas_IdZonas);
            else if (IdSistema != null)
                Consulta.Append("SELECT r.*,z.Nombre as NameZona,rec.Tipo, pr.*, tb.name, rp.*,mb.name as NameBss,rec.idTIFX rec_idTIFX,destRad.TipoFrec DestRad_tipoFrec FROM RecursosRadio r " +
                                "LEFT JOIN zonas z ON z.idZonas=r.zonas_idZonas " +
                                "LEFT JOIN tabla_bss tb ON tb.idtabla_bss = r.tabla_bss_idtabla_bss " +
                                "LEFT JOIN radio_param rp ON rp.idradio_param = r.radio_param_idradio_param " +
                                "LEFT JOIN metodos_bss mb ON mb.idmetodos_bss = rp.metodos_bss_idmetodos_bss " +
                                "LEFT JOIN destinosradio destrad ON destrad.IdSistema=r.IdSistema AND destrad.IdDestino=r.IdDestino AND destrad.TipoDestino=r.TipoDestino " +
                                "INNER JOIN ParametrosRecurso pr on pr.IdSistema=r.IdSistema AND pr.IdRecurso=r.IdRecurso AND pr.TipoRecurso=r.TipoRecurso " +
                                "INNER JOIN Recursos rec on rec.IdSistema=r.IdSistema AND rec.IdRecurso=r.IdRecurso AND rec.TipoRecurso=r.TipoRecurso " +
                                "WHERE r.IdSistema='" + IdSistema + "' ORDER BY r.IdRecurso");
            else
                Consulta.Append("SELECT r.*,z.Nombre as NameZona,rec.Tipo, pr.*, tb.name, rp.*,mb.name as NameBss,rec.idTIFX rec_idTIFX,destRad.TipoFrec DestRad_tipoFrec FROM RecursosRadio r " +
                                "LEFT JOIN zonas z ON z.idZonas=r.zonas_idZonas " +
                                "LEFT JOIN tabla_bss tb ON tb.idtabla_bss = r.tabla_bss_idtabla_bss " +
                                "LEFT JOIN radio_param rp ON rp.idradio_param = r.radio_param_idradio_param " +
                                "LEFT JOIN metodos_bss mb ON mb.idmetodos_bss = rp.metodos_bss_idmetodos_bss " +
                                "LEFT JOIN destinosradio destrad ON destrad.IdSistema=r.IdSistema AND destrad.IdDestino=r.IdDestino AND destrad.TipoDestino=r.TipoDestino " +
                                "INNER JOIN ParametrosRecurso pr on pr.IdSistema=r.IdSistema AND pr.IdRecurso=r.IdRecurso AND pr.TipoRecurso=r.TipoRecurso " +
                                "INNER JOIN Recursos rec on rec.IdSistema=r.IdSistema AND rec.IdRecurso=r.IdRecurso AND rec.TipoRecurso=r.TipoRecurso");

            return Consulta.ToString();
        }

        public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds)
        {
            //base.ListSelectSQL();

            ListaResultado.Clear();

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    RecursosRadio pr = new RecursosRadio();

                    IdRecurso = pr.IdRecurso = (string)dr["IdRecurso"];
                    IdSistema = pr.IdSistema = (string)dr["IdSistema"];
                    TipoRecurso = pr.TipoRecurso = (uint)dr["TipoRecurso"];

                    if (dr["TipoDestino"] != System.DBNull.Value)
                        pr.TipoDestino = (uint)dr["TipoDestino"];
                    if (dr["IdDestino"] != System.DBNull.Value)
                        pr.IdDestino = (string)dr["IdDestino"];
                    if (dr["IdEmplazamiento"] != System.DBNull.Value)
                        pr.IdEmplazamiento = (string)dr["IdEmplazamiento"];

                    if (dr["tabla_bss_idtabla_bss"] != System.DBNull.Value)
                        pr.IdTablaBss = (int)dr["tabla_bss_idtabla_bss"];
                    if (dr["zonas_idzonas"] != System.DBNull.Value)
                        pr.Zonas_IdZonas = (int)dr["zonas_idzonas"];
                    
                    if (dr["NameZona"] != System.DBNull.Value)
                        pr.NombreZona = (string)dr["NameZona"];
                    if (dr["radio_param_idradio_param"] != System.DBNull.Value)
                        pr.Radio_param_idradio_param = (int)dr["radio_param_idradio_param"];
                    if (dr["radio_param_idradio_param"] != System.DBNull.Value)
                        pr.Radio_param_idradio_param = (int)dr["radio_param_idradio_param"];
                    if (dr["metodos_bss_idmetodos_bss"] != System.DBNull.Value)
                        pr.Metodos_bss_idmetodos_bss = (int)dr["metodos_bss_idmetodos_bss"];

                    if (dr["name"] != System.DBNull.Value)
                        pr.NameTablaBss = (string)dr["name"];

                    if (dr["EM"] != System.DBNull.Value)
                        pr.EM = (bool)dr["GrabacionEd137"]; // Provisionalmente, en EM se deja el estado de la grabación del recurso
                                                            // Para no tener que cambiar la interfaz SOAP en la pasarela (bool)dr["EM"]; 
                    if (dr["Tipo"] != System.DBNull.Value)
                        pr.Tipo = (uint)dr["Tipo"];
                    if (dr["SQ"] != System.DBNull.Value)
                        pr.SQ = (string)dr["SQ"];
                    if (dr["PTT"] != System.DBNull.Value)
                        pr.PTT = (string)dr["PTT"];
                    if (dr["FrqTonoE"] != System.DBNull.Value)
                        pr.FrqTonoE = (uint)dr["FrqTonoE"];
                    if (dr["UmbralTonoE"] != System.DBNull.Value)
                        pr.UmbralTonoE = (int)dr["UmbralTonoE"];
                    if (dr["FrqTonoM"] != System.DBNull.Value)
                        pr.FrqTonoM = (uint)dr["FrqTonoM"];
                    if (dr["UmbralTonoM"] != System.DBNull.Value)
                        pr.UmbralTonoM = (int)dr["UmbralTonoM"];
                    if (dr["FrqTonoSQ"] != System.DBNull.Value)
                        pr.FrqTonoSQ = (uint)dr["FrqTonoSQ"];
                    if (dr["UmbralTonoSQ"] != System.DBNull.Value)
                        pr.UmbralTonoSQ = (int)dr["UmbralTonoSQ"];
                    if (dr["FrqTonoPTT"] != System.DBNull.Value)
                        pr.FrqTonoPTT = (uint)dr["FrqTonoPTT"];
                    if (dr["UmbralTonoPTT"] != System.DBNull.Value)
                        pr.UmbralTonoPTT = (int)dr["UmbralTonoPTT"];
                    if (dr["BSS"] != System.DBNull.Value)
                        pr.BSS = (bool)dr["BSS"];   //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0;
                    if (dr["NTZ"] != System.DBNull.Value)
                        pr.NTZ = (bool)dr["NTZ"];   //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0;
                    if (dr["TipoNTZ"] != System.DBNull.Value)
                        pr.TipoNTZ = (uint)dr["TipoNTZ"];
                    if (dr["Cifrado"] != System.DBNull.Value)
                        pr.Cifrado = (bool)dr["Cifrado"];   //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0;
                    if (dr["SupervPortadoraTX"] != System.DBNull.Value)
                        pr.SupervPortadoraTx = (bool)dr["SupervPortadoraTX"];   //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0;
                    if (dr["SupervModuladoraTX"] != System.DBNull.Value)
                        pr.SupervModuladoraTx = (bool)dr["SupervModuladoraTX"]; //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0;
                    if (dr["ModoConfPTT"] != System.DBNull.Value)
                        pr.ModoConfPTT = (uint)dr["ModoConfPTT"];
                    if (dr["RepSQyBSS"] != System.DBNull.Value)
                        pr.RepSQyBSS = (uint)dr["RepSQyBSS"];
                    if (dr["DesactivacionSQ"] != System.DBNull.Value)
                        pr.DesactivacionSQ = (uint)dr["DesactivacionSQ"];
                    if (dr["TimeoutPTT"] != System.DBNull.Value)
                        pr.TimeoutPTT = (uint)dr["TimeoutPTT"];
                    if (dr["NameBss"] != System.DBNull.Value)
                        pr.MetodoBSS = (string)dr["NameBss"];
                    if (dr["UmbralVAD"] != System.DBNull.Value)
                        pr.UmbralVAD = (int)dr["UmbralVAD"];
					if (dr["TiempoPTT"] != System.DBNull.Value)
						pr.TiempoPTT = (uint)dr["TiempoPTT"];
					if (dr["NumFlujosAudio"] != System.DBNull.Value)
						pr.NumFlujosAudio = (uint)dr["NumFlujosAudio"];
					if (dr["KeepAlivePeriod"] != System.DBNull.Value)
						pr.KeepAlivePeriod = (uint)dr["KeepAlivePeriod"];
					if (dr["KeepAliveMultiplier"] != System.DBNull.Value)
						pr.KeepAliveMultiplier = (uint)dr["KeepAliveMultiplier"];
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
                    if (dr["GrabacionEd137"] != System.DBNull.Value)
                        pr.GrabacionEd137 = (bool)dr["GrabacionEd137"];   //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0;
                    if (dr["GrsDelay"] != System.DBNull.Value)
                        pr.GrsDelay = (int)dr["GrsDelay"];
                    if (dr["OffSetFrequency"] != System.DBNull.Value)
                        pr.OffSetFrequency = (int)dr["OffSetFrequency"];
                    if (dr["EnableEventPttSq"] != System.DBNull.Value)
                        pr.EnableEventPttSq = (bool)dr["EnableEventPttSq"];

                    //MVO: se recupera la pasarela asociada al recurso radio si procede
                    if (dr["rec_idTIFX"] != System.DBNull.Value)
                        pr.idTIFX = (string)dr["rec_idTIFX"];

                    //MVO: se recupera el tipo frecuencia del destino radio asociado al recurso
                    if (dr["DestRad_tipoFrec"] != System.DBNull.Value)
                        pr.DestRad_tipoFrec = (uint) dr["DestRad_tipoFrec"];

                    if (dr["RedundanciaRol"] != System.DBNull.Value)
                        pr.RedundanciaRol = (string)dr["RedundanciaRol"];

                    if (dr["RedundanciaIdPareja"] != System.DBNull.Value)
                        pr.RedundanciaIdPareja = (string)dr["RedundanciaIdPareja"];

                    ListaResultado.Add(pr);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
			string[] consulta = new string[6];

			Array.Copy(base.InsertSQL(), consulta, 2);

            consulta[2] = "INSERT INTO radio_param (GrsDelay,OffSetFrequency,EnableEventPttSq,metodos_bss_idmetodos_bss) VALUES (" +
                GrsDelay + "," +
                OffSetFrequency + "," +
                EnableEventPttSq + "," +
                Metodos_bss_idmetodos_bss + ")";

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO RecursosRadio (IdSistema,IdRecurso,TipoRecurso,TipoDestino,zonas_idZonas,radio_param_idradio_param,IdDestino,IdEmplazamiento,tabla_bss_idtabla_bss,EM,SQ,PTT,FrqTonoE,UmbralTonoE," +
                                                        "FrqTonoM,UmbralTonoM,FrqTonoSQ,UmbralTonoSQ,FrqTonoPTT,UmbralTonoPTT,BSS,NTZ,TipoNTZ,Cifrado," +
                                                        "SupervPortadoraTX,SupervModuladoraTX,ModoConfPTT,RepSQyBSS," +
														"DesactivacionSQ,TimeoutPTT,UmbralVAD,TiempoPTT,NumFlujosAudio,KeepAlivePeriod,KeepAliveMultiplier)" +
                            " VALUES ('" + IdSistema + "','" +
                                         IdRecurso + "'," +
                                         TipoRecurso + "," +
                                         TipoDestino + "," +
                                         Zonas_IdZonas + "," +
                                         //Radio_param_idradio_param + "," +
                                         "<LastInsertedValue>," +   // En GestorBaseDatos.InsertSQL() se reemplaza la cadena <LastInsertedValue> por el InsertId del INSERT INTO radio_param anterior.
                                         ((IdDestino == null) ? "null,'" : ("'" + IdDestino + "','")) +
                                         IdEmplazamiento + "'," +
                                         ((IdTablaBss > 0) ? IdTablaBss.ToString() : "null") + "," +
                                         EM + ",'" +
                                         SQ + "','" +
                                         PTT + "'," +
                                         FrqTonoE + "," +
                                         UmbralTonoE + "," +
                                         FrqTonoM + "," +
                                         UmbralTonoM + "," +
                                         FrqTonoSQ + "," +
                                         UmbralTonoSQ + "," +
                                         FrqTonoPTT + "," +
                                         UmbralTonoPTT + "," +
                                         BSS + "," +
                                         NTZ + "," +
                                         TipoNTZ + "," +
                                         Cifrado + "," +
                                         SupervPortadoraTx + "," +
                                         SupervModuladoraTx + "," +
                                         ModoConfPTT + "," +
                                         RepSQyBSS + "," +
                                         DesactivacionSQ + "," +
                                         TimeoutPTT + "," +
                                         UmbralVAD + "," +
										 TiempoPTT + "," +
										 NumFlujosAudio + "," +
										 KeepAlivePeriod + "," +
										 KeepAliveMultiplier +
												")");

			consulta[3] = Consulta.ToString();
            consulta[4] = ReplaceSQL(IdSistema, "RecursosRadio");
            consulta[5] = ReplaceSQL(IdSistema, "radio_param");

			return consulta;
		}

        public override string[] UpdateSQL()
        {
			string[] consulta = new string[6];

			Array.Copy(base.UpdateSQL(), consulta, 2);

            consulta[2] = "UPDATE radio_param set GrsDelay=" + GrsDelay + "," +
                                                "OffSetFrequency=" + OffSetFrequency + "," +
                                                "EnableEventPttSq=" + EnableEventPttSq + "," +
                                                "metodos_bss_idmetodos_bss=" + Metodos_bss_idmetodos_bss +
                                                " WHERE idradio_param=" + Radio_param_idradio_param;

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE RecursosRadio SET IdRecurso='" + IdRecurso + "'," +
                                            "IdSistema='" + IdSistema + "'," +
                                            "TipoRecurso=" + TipoRecurso + "," +
                                            "zonas_idZonas=" + Zonas_IdZonas + "," +
                                            "radio_param_idradio_param=" + Radio_param_idradio_param + "," +
                                            "IdDestino=" + ((IdDestino == null) ? "null, " : ("'" + IdDestino + "', ")) +
                                            "TipoDestino=" + TipoDestino + "," +
                                            "IdEmplazamiento='" + IdEmplazamiento + "'," +
                                            "tabla_bss_idtabla_bss=" + ((IdTablaBss > 0) ? IdTablaBss.ToString() : "null") + "," +
                                            "EM=" + EM + "," +
                                            "SQ='" + SQ + "'," +
                                            "PTT='" + PTT + "'," +
                                            "FrqTonoE=" + FrqTonoE + "," +
                                            "UmbralTonoE=" + UmbralTonoE + "," +
                                            "FrqTonoM=" + FrqTonoM + "," +
                                            "UmbralTonoM=" + UmbralTonoM + "," +
                                            "FrqTonoSQ=" + FrqTonoSQ + "," +
                                            "UmbralTonoSQ=" + UmbralTonoSQ + "," +
                                            "FrqTonoPTT=" + FrqTonoPTT + "," +
                                            "UmbralTonoPTT=" + UmbralTonoPTT + "," +
                                            "BSS=" + BSS + "," +
                                            "NTZ=" + NTZ + "," +
                                            "TipoNTZ=" + TipoNTZ + "," +
                                            "Cifrado=" + Cifrado + "," +
                                            "SupervPortadoraTx=" + SupervPortadoraTx + "," +
                                            "SupervModuladoraTx=" + SupervModuladoraTx + "," +
                                            "ModoConfPTT=" + ModoConfPTT + "," +
                                            "RepSQyBSS=" + RepSQyBSS + "," +
                                            "DesactivacionSQ=" + DesactivacionSQ + "," +
                                            "TimeoutPTT=" + TimeoutPTT + "," +
                                            "UmbralVAD=" + UmbralVAD + "," +
                                            "TiempoPTT=" + TiempoPTT + "," +
											"NumFlujosAudio=" + NumFlujosAudio + "," +
											"KeepAlivePeriod=" + KeepAlivePeriod + "," +
											"KeepAliveMultiplier=" + KeepAliveMultiplier + " " +
											"WHERE IdRecurso='" + IdRecurso + "' AND " + "IdSistema='" + IdSistema + "'"
                                            );

			consulta[3] = Consulta.ToString();
			consulta[4] = ReplaceSQL(IdSistema, "RecursosRadio");
            consulta[5] = ReplaceSQL(IdSistema, "radio_param");

			return consulta;
		}

        public override string[] UpdateDestinoSQL()
        {
			string[] consulta = new string[2];
            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE RecursosRadio SET IdDestino='" + IdDestino + "'," +
                                            "TipoDestino=" + TipoDestino + " " +
                                            "WHERE IdRecurso='" + IdRecurso + "' AND " + "IdSistema='" + IdSistema + "'"
                                            );

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "RecursosRadio");

            return consulta;
        }

        public override string[] LiberaDestinoSQL()
        {
			string[] consulta = new string[2];
			Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE RecursosRadio SET IdDestino=null, TipoDestino=null " +
                                            "WHERE IdDestino='" + IdDestino + "' AND " + "IdSistema='" + IdSistema + "'"
                                            );

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "RecursosRadio");

			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[6];

			Array.Copy(base.DeleteSQL(), consulta, 2);
			
            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdRecurso != null)
                Consulta.Append("DELETE FROM RecursosRadio WHERE IdSistema='" + IdSistema + "' AND IdRecurso='" + IdRecurso + "'");
            else if (IdSistema != null)
                Consulta.Append("DELETE FROM RecursosRadio WHERE IdSistema='" + IdSistema + "'");
            else
                Consulta.Append("DELETE FROM RecursosRadio");

			consulta[2] = Consulta.ToString();
			consulta[3] = ReplaceSQL(IdSistema, "RecursosRadio");
            consulta[4] = "DELETE FROM radio_param WHERE idradio_param=" + Radio_param_idradio_param;
            consulta[5] = ReplaceSQL(IdSistema, "radio_param");

			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM RecursosRadio WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
    }

    public class RecursosRadioForGateway : RecursosRadio
    {
        private int[] _ValuesTablaBss = new int[6];
        public int[] ValuesTablaBss
        {
            get { return _ValuesTablaBss; }
            set { _ValuesTablaBss = value; }
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdRecurso != null)
                Consulta.Append("SELECT r.*,z.Nombre as NameZona,rec.Tipo, pr.*, tb.name,vt.valor_prop,vt.valor_rssi, rp.*,mb.name as NameBss FROM RecursosRadio r " +
                                "LEFT JOIN zonas z ON z.idZonas=r.zonas_idZonas " +
                                "LEFT JOIN tabla_bss tb ON tb.idtabla_bss = r.tabla_bss_idtabla_bss " +
                                "LEFT JOIN valores_tabla vt ON vt.tabla_bss_idtabla_bss = tb.idtabla_bss " +
                                "LEFT JOIN radio_param rp ON rp.idradio_param = r.radio_param_idradio_param " +
                                "LEFT JOIN metodos_bss mb ON mb.idmetodos_bss = rp.metodos_bss_idmetodos_bss " +
                                "INNER JOIN ParametrosRecurso pr on pr.IdSistema=r.IdSistema AND pr.IdRecurso=r.IdRecurso AND pr.TipoRecurso=r.TipoRecurso " +
                                "INNER JOIN Recursos rec on rec.IdSistema=r.IdSistema AND rec.IdRecurso=r.IdRecurso AND rec.TipoRecurso=r.TipoRecurso " +
                                    "WHERE r.IdSistema='" + IdSistema + "' AND r.IdRecurso='" + IdRecurso + "'");
            else if (IdSistema != null && IdDestino != null)
                Consulta.Append("SELECT r.*,z.Nombre as NameZona,rec.Tipo, pr.*, tb.name,vt.valor_prop,vt.valor_rssi, rp.*,mb.name as NameBss FROM RecursosRadio r " +
                                "LEFT JOIN zonas z ON z.idZonas=r.zonas_idZonas " +
                                "LEFT JOIN tabla_bss tb ON tb.idtabla_bss = r.tabla_bss_idtabla_bss " +
                                "LEFT JOIN valores_tabla vt ON vt.tabla_bss_idtabla_bss = tb.idtabla_bss " +
                                "LEFT JOIN radio_param rp ON rp.idradio_param = r.radio_param_idradio_param " +
                                "LEFT JOIN metodos_bss mb ON mb.idmetodos_bss = rp.metodos_bss_idmetodos_bss " +
                                "INNER JOIN ParametrosRecurso pr on pr.IdSistema=r.IdSistema AND pr.IdRecurso=r.IdRecurso AND pr.TipoRecurso=r.TipoRecurso " +
                                "INNER JOIN Recursos rec on rec.IdSistema=r.IdSistema AND rec.IdRecurso=r.IdRecurso AND rec.TipoRecurso=r.TipoRecurso " +
                                    "WHERE r.IdSistema='" + IdSistema + "' AND r.IdDestino='" + IdDestino + "'");
            else if (IdSistema != null && IdEmplazamiento != null)
                Consulta.Append("SELECT r.*,z.Nombre as NameZona,rec.Tipo, pr.*, tb.name,vt.valor_prop,vt.valor_rssi, rp.*,mb.name as NameBss FROM RecursosRadio r " +
                                "LEFT JOIN zonas z ON z.idZonas=r.zonas_idZonas " +
                                "LEFT JOIN tabla_bss tb ON tb.idtabla_bss = r.tabla_bss_idtabla_bss " +
                                "LEFT JOIN valores_tabla vt ON vt.tabla_bss_idtabla_bss = tb.idtabla_bss " +
                                "LEFT JOIN radio_param rp ON rp.idradio_param = r.radio_param_idradio_param " +
                                "LEFT JOIN metodos_bss mb ON mb.idmetodos_bss = rp.metodos_bss_idmetodos_bss " +
                                "INNER JOIN ParametrosRecurso pr on pr.IdSistema=r.IdSistema AND pr.IdRecurso=r.IdRecurso AND pr.TipoRecurso=r.TipoRecurso " +
                                "INNER JOIN Recursos rec on rec.IdSistema=r.IdSistema AND rec.IdRecurso=r.IdRecurso AND rec.TipoRecurso=r.TipoRecurso " +
                                    "WHERE r.IdSistema='" + IdSistema + "' AND r.IdEmplazamiento='" + IdEmplazamiento + "'");
            else if (IdSistema != null && Zonas_IdZonas != 0)
                Consulta.Append("SELECT r.*,z.Nombre as NameZona,rec.Tipo, pr.*, tb.name,vt.valor_prop,vt.valor_rssi, rp.*,mb.name as NameBss FROM RecursosRadio r " +
                                "LEFT JOIN zonas z ON z.idZonas=r.zonas_idZonas " +
                                "LEFT JOIN tabla_bss tb ON tb.idtabla_bss = r.tabla_bss_idtabla_bss " +
                                "LEFT JOIN valores_tabla vt ON vt.tabla_bss_idtabla_bss = tb.idtabla_bss " +
                                "LEFT JOIN radio_param rp ON rp.idradio_param = r.radio_param_idradio_param " +
                                "LEFT JOIN metodos_bss mb ON mb.idmetodos_bss = rp.metodos_bss_idmetodos_bss " +
                                "INNER JOIN ParametrosRecurso pr on pr.IdSistema=r.IdSistema AND pr.IdRecurso=r.IdRecurso AND pr.TipoRecurso=r.TipoRecurso " +
                                "INNER JOIN Recursos rec on rec.IdSistema=r.IdSistema AND rec.IdRecurso=r.IdRecurso AND rec.TipoRecurso=r.TipoRecurso " +
                                    "WHERE r.IdSistema='" + IdSistema + "' AND r.zonas_idZonas=" + Zonas_IdZonas);
            else if (IdSistema != null)
                Consulta.Append("SELECT r.*,z.Nombre as NameZona,rec.Tipo, pr.*, tb.name,vt.valor_prop,vt.valor_rssi, rp.*,mb.name as NameBss FROM RecursosRadio r " +
                                "LEFT JOIN zonas z ON z.idZonas=r.zonas_idZonas " +
                                "LEFT JOIN tabla_bss tb ON tb.idtabla_bss = r.tabla_bss_idtabla_bss " +
                                "LEFT JOIN valores_tabla vt ON vt.tabla_bss_idtabla_bss = tb.idtabla_bss " +
                                "LEFT JOIN radio_param rp ON rp.idradio_param = r.radio_param_idradio_param " +
                                "LEFT JOIN metodos_bss mb ON mb.idmetodos_bss = rp.metodos_bss_idmetodos_bss " +
                                "INNER JOIN ParametrosRecurso pr on pr.IdSistema=r.IdSistema AND pr.IdRecurso=r.IdRecurso AND pr.TipoRecurso=r.TipoRecurso " +
                                "INNER JOIN Recursos rec on rec.IdSistema=r.IdSistema AND rec.IdRecurso=r.IdRecurso AND rec.TipoRecurso=r.TipoRecurso " +
                                "WHERE r.IdSistema='" + IdSistema + "'");
            else
                Consulta.Append("SELECT r.*,z.Nombre as NameZona,rec.Tipo, pr.*, tb.name,vt.valor_prop,vt.valor_rssi, rp.*,mb.name as NameBss FROM RecursosRadio r " +
                                "LEFT JOIN zonas z ON z.idZonas=r.zonas_idZonas " +
                                "LEFT JOIN tabla_bss tb ON tb.idtabla_bss = r.tabla_bss_idtabla_bss " +
                                "LEFT JOIN radio_param rp ON rp.idradio_param = r.radio_param_idradio_param " +
                                "LEFT JOIN metodos_bss mb ON mb.idmetodos_bss = rp.metodos_bss_idmetodos_bss " +
                                "INNER JOIN ParametrosRecurso pr on pr.IdSistema=r.IdSistema AND pr.IdRecurso=r.IdRecurso AND pr.TipoRecurso=r.TipoRecurso " +
                                "INNER JOIN Recursos rec on rec.IdSistema=r.IdSistema AND rec.IdRecurso=r.IdRecurso AND rec.TipoRecurso=r.TipoRecurso");

            return Consulta.ToString();
        }

        public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds)
        {
            //base.ListSelectSQL();

            ListaResultado.Clear();

            if (ds != null)
            {
                string idRec = string.Empty;

                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    RecursosRadioForGateway pr = new RecursosRadioForGateway();

                    if (idRec != (string)dr["IdRecurso"])
                    {
                        idRec = IdRecurso = pr.IdRecurso = (string)dr["IdRecurso"];
                        IdSistema = pr.IdSistema = (string)dr["IdSistema"];
                        TipoRecurso = pr.TipoRecurso = (uint)dr["TipoRecurso"];

                        if (dr["TipoDestino"] != System.DBNull.Value)
                            pr.TipoDestino = (uint)dr["TipoDestino"];
                        if (dr["IdDestino"] != System.DBNull.Value)
                            pr.IdDestino = (string)dr["IdDestino"];
                        if (dr["IdEmplazamiento"] != System.DBNull.Value)
                            pr.IdEmplazamiento = (string)dr["IdEmplazamiento"];

                        if (dr["tabla_bss_idtabla_bss"] != System.DBNull.Value)
                            pr.IdTablaBss = (int)dr["tabla_bss_idtabla_bss"];
                        if (dr["zonas_idzonas"] != System.DBNull.Value)
                            pr.Zonas_IdZonas = (int)dr["zonas_idzonas"];

                        if (dr["NameZona"] != System.DBNull.Value)
                            pr.NombreZona = (string)dr["NameZona"];
                        if (dr["radio_param_idradio_param"] != System.DBNull.Value)
                            pr.Radio_param_idradio_param = (int)dr["radio_param_idradio_param"];
                        if (dr["radio_param_idradio_param"] != System.DBNull.Value)
                            pr.Radio_param_idradio_param = (int)dr["radio_param_idradio_param"];
                        if (dr["metodos_bss_idmetodos_bss"] != System.DBNull.Value)
                            pr.Metodos_bss_idmetodos_bss = (int)dr["metodos_bss_idmetodos_bss"];

                        if (dr["name"] != System.DBNull.Value)
                        {
                            pr.NameTablaBss = (string)dr["name"];
                            if (dr["valor_prop"] != System.DBNull.Value && dr["valor_rssi"] != System.DBNull.Value)
                            {
                                pr.ValuesTablaBss[(int)dr["valor_prop"]] = (int)dr["valor_rssi"];
                            }
                        }

                        if (dr["EM"] != System.DBNull.Value)
                            pr.EM = (bool)dr["GrabacionEd137"]; // Provisionalmente, en EM se deja el estado de la grabación del recurso
                        // Para no tener que cambiar la interfaz SOAP en la pasarela (bool)dr["EM"]; 
                        if (dr["Tipo"] != System.DBNull.Value)
                            pr.Tipo = (uint)dr["Tipo"];
                        if (dr["SQ"] != System.DBNull.Value)
                            pr.SQ = (string)dr["SQ"];
                        if (dr["PTT"] != System.DBNull.Value)
                            pr.PTT = (string)dr["PTT"];
                        if (dr["FrqTonoE"] != System.DBNull.Value)
                            pr.FrqTonoE = (uint)dr["FrqTonoE"];
                        if (dr["UmbralTonoE"] != System.DBNull.Value)
                            pr.UmbralTonoE = (int)dr["UmbralTonoE"];
                        if (dr["FrqTonoM"] != System.DBNull.Value)
                            pr.FrqTonoM = (uint)dr["FrqTonoM"];
                        if (dr["UmbralTonoM"] != System.DBNull.Value)
                            pr.UmbralTonoM = (int)dr["UmbralTonoM"];
                        if (dr["FrqTonoSQ"] != System.DBNull.Value)
                            pr.FrqTonoSQ = (uint)dr["FrqTonoSQ"];
                        if (dr["UmbralTonoSQ"] != System.DBNull.Value)
                            pr.UmbralTonoSQ = (int)dr["UmbralTonoSQ"];
                        if (dr["FrqTonoPTT"] != System.DBNull.Value)
                            pr.FrqTonoPTT = (uint)dr["FrqTonoPTT"];
                        if (dr["UmbralTonoPTT"] != System.DBNull.Value)
                            pr.UmbralTonoPTT = (int)dr["UmbralTonoPTT"];
                        if (dr["BSS"] != System.DBNull.Value)
                            pr.BSS = (bool)dr["BSS"];   //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0;
                        if (dr["NTZ"] != System.DBNull.Value)
                            pr.NTZ = (bool)dr["NTZ"];   //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0;
                        if (dr["TipoNTZ"] != System.DBNull.Value)
                            pr.TipoNTZ = (uint)dr["TipoNTZ"];
                        if (dr["Cifrado"] != System.DBNull.Value)
                            pr.Cifrado = (bool)dr["Cifrado"];   //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0;
                        if (dr["SupervPortadoraTX"] != System.DBNull.Value)
                            pr.SupervPortadoraTx = (bool)dr["SupervPortadoraTX"];   //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0;
                        if (dr["SupervModuladoraTX"] != System.DBNull.Value)
                            pr.SupervModuladoraTx = (bool)dr["SupervModuladoraTX"]; //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0;
                        if (dr["ModoConfPTT"] != System.DBNull.Value)
                            pr.ModoConfPTT = (uint)dr["ModoConfPTT"];
                        if (dr["RepSQyBSS"] != System.DBNull.Value)
                            pr.RepSQyBSS = (uint)dr["RepSQyBSS"];
                        if (dr["DesactivacionSQ"] != System.DBNull.Value)
                            pr.DesactivacionSQ = (uint)dr["DesactivacionSQ"];
                        if (dr["TimeoutPTT"] != System.DBNull.Value)
                            pr.TimeoutPTT = (uint)dr["TimeoutPTT"];
                        if (dr["NameBss"] != System.DBNull.Value)
                            pr.MetodoBSS = (string)dr["NameBss"];
                        if (dr["UmbralVAD"] != System.DBNull.Value)
                            pr.UmbralVAD = (int)dr["UmbralVAD"];
                        if (dr["TiempoPTT"] != System.DBNull.Value)
                            pr.TiempoPTT = (uint)dr["TiempoPTT"];
                        if (dr["NumFlujosAudio"] != System.DBNull.Value)
                            pr.NumFlujosAudio = (uint)dr["NumFlujosAudio"];
                        if (dr["KeepAlivePeriod"] != System.DBNull.Value)
                            pr.KeepAlivePeriod = (uint)dr["KeepAlivePeriod"];
                        if (dr["KeepAliveMultiplier"] != System.DBNull.Value)
                            pr.KeepAliveMultiplier = (uint)dr["KeepAliveMultiplier"];
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
                        if (dr["GrabacionEd137"] != System.DBNull.Value)
                            pr.GrabacionEd137 = (bool)dr["GrabacionEd137"];   //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0;
                        if (dr["GrsDelay"] != System.DBNull.Value)
                            pr.GrsDelay = (int)dr["GrsDelay"];
                        if (dr["OffSetFrequency"] != System.DBNull.Value)
                            pr.OffSetFrequency = (int)dr["OffSetFrequency"];
                        if (dr["EnableEventPttSq"] != System.DBNull.Value)
                            pr.EnableEventPttSq = (bool)dr["EnableEventPttSq"];

                        if (dr["RedundanciaRol"] != System.DBNull.Value)
                            pr.RedundanciaRol = (string)dr["RedundanciaRol"];

                        if (dr["RedundanciaIdPareja"] != System.DBNull.Value)
                            pr.RedundanciaIdPareja = (string)dr["RedundanciaIdPareja"];


                        ListaResultado.Add(pr);
                    }
                    else
                    {
                        int indexItem = ListaResultado.FindIndex(r => (((RecursosRadioForGateway)r).IdRecurso == IdRecurso));
                        if (indexItem >= 0 && dr["name"] != System.DBNull.Value)
                        {
                            if (dr["valor_prop"] != System.DBNull.Value && dr["valor_rssi"] != System.DBNull.Value)
                            {
                                ((RecursosRadioForGateway)ListaResultado[indexItem]).ValuesTablaBss[(int)dr["valor_prop"]] = (int)dr["valor_rssi"];
                            }
                        }
                    }

                }
            }

            return ListaResultado;
        }

    }
}
