using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class DestinosRadio : Destinos
    {

        public struct MetodosBssDelRecurso
        {
            public int idMetodo;
            public string nombreMetodo;

            public MetodosBssDelRecurso(int id, string n)
            {
                idMetodo = id;
                nombreMetodo = n;
            }
        }

        #region Propiedades de DestinosRadio

        // Clave a la tabla de metodosbss_destinosradio
        private int _Fk_Metodosbss;
        public int Fk_Metodosbss
        {
            get { return _Fk_Metodosbss; }
            set { _Fk_Metodosbss = value; }
        }

        // Clave a la tabla de zonas. Zona Tx por defecto
        private int _Zonas_IdZonas;
        public int Zonas_IdZonas
        {
            get { return _Zonas_IdZonas; }
            set { _Zonas_IdZonas = value; }
        }

        private uint _TipoFrec;
        public uint TipoFrec
        {
            get { return _TipoFrec; }
            set { _TipoFrec = value; }
        }

        private bool _ExclusividadTXRX;
        public bool ExclusividadTXRX
        {
            get { return _ExclusividadTXRX; }
            set { _ExclusividadTXRX = value; }
        }

        private int _Frecuencia;
        public int Frecuencia
        {
            get { return _Frecuencia; }
            set { _Frecuencia = value; }
        }

        private int _MetodoCalculoClimax;
        public int MetodoCalculoClimax
        {
            get { return _MetodoCalculoClimax; }
            set { _MetodoCalculoClimax = value; }
        }

        private int _VentanaSeleccionBss;
        public int VentanaSeleccionBss
        {
            get { return _VentanaSeleccionBss; }
            set { _VentanaSeleccionBss = value; }
        }

        private bool _SincronizaGrupoClimax;
        public bool SincronizaGrupoClimax
        {
            get { return _SincronizaGrupoClimax; }
            set { _SincronizaGrupoClimax = value; }
        }

        private bool _AudioPrimerSqBss;
        public bool AudioPrimerSqBss
        {
            get { return _AudioPrimerSqBss; }
            set { _AudioPrimerSqBss = value; }
        }

        private List<MetodosBssDelRecurso> _metodosBss;
        public List<MetodosBssDelRecurso> MetodosBss
        {
            get { return _metodosBss; }
            set { _metodosBss = value; }
        }
        public MetodosBssDelRecurso this[int i]
        {
            get { return _metodosBss.ToArray()[i]; }
        }

        private bool _FrecuenciaNoDesasignable;
        public bool FrecuenciaNoDesasignable
        {
            get { return _FrecuenciaNoDesasignable; }
            set { _FrecuenciaNoDesasignable = value; }
        }

        private int _VentanaReposoZonaTxDefecto;
        public int VentanaReposoZonaTxDefecto
        {
            get { return _VentanaReposoZonaTxDefecto; }
            set { _VentanaReposoZonaTxDefecto = value; }
        }

        private int _PrioridadSesionSip;
        public int PrioridadSesionSip
        {
            get { return _PrioridadSesionSip; }
            set { _PrioridadSesionSip = value; }
        }

        private int _MetodosBssOfrecidos;
        public int MetodosBssOfrecidos
        {
            get { return _MetodosBssOfrecidos; }
            set { _MetodosBssOfrecidos = value; }
        }
        private int _CldSupervisionTime;
        public int CldSupervisionTime
        {
            get { return _CldSupervisionTime; }
            set { _CldSupervisionTime = value; }
        }

        // Nombre de la Zona Tx por defecto (No pertenece al modelo)
        private string _NombreZonaTxDefecto;
        public string NombreZonaTxDefecto
        {
            get { return _NombreZonaTxDefecto; }
            set { _NombreZonaTxDefecto = value; }
        }

        private string _CnfModoDestino;
        public string CnfModoDestino
        {
            get { return _CnfModoDestino; }
            set { _CnfModoDestino = value; }
        }

        private string _CnfTipoFrecuencia;
        public string CnfTipoFrecuencia
        {
            get { return _CnfTipoFrecuencia; }
            set { _CnfTipoFrecuencia = value; }
        }

        private string _ModoTransmision;
        public string ModoTransmision
        {
            get { return _ModoTransmision; }
            set { _ModoTransmision = value; }
        }
        //VMG 18/02/2019
        private string _EmplazamientoDefecto;
        public string EmplazamientoDefecto
        {
            get { return _EmplazamientoDefecto; }
            set { _EmplazamientoDefecto = value; }
        }

        // VMG 29/11/2018 
        // Esta variable 'XXXSpecified' la ponemos para indicar al XmlSerializer
        // que la siguiente variable con el nombre 'XXX' va a aparecer con el valor minOccur=0
        // https://docs.microsoft.com/en-us/previous-versions/dotnet/netframework-2.0/zds0b35c(v=vs.80)
        
        //public bool TiempoVueltaADefectoSpecified = false;
        private string _TiempoVueltaADefecto;
        //[System.Xml.Serialization.XmlIgnoreAttribute()] VMG Esto lo dejo para otra vez... pero no nos sirve
        //Solo funciona el string para ponerlo como nullable
        public string TiempoVueltaADefecto
        {
            get { return _TiempoVueltaADefecto; }
            set { _TiempoVueltaADefecto = value; }
        }
        #endregion

        public DestinosRadio()
            : base()
        {
            _metodosBss = new List<MetodosBssDelRecurso>();
        }

        public override string DataSetSelectSQL()
        {
            StringBuilder strConsulta = new StringBuilder();

            if (IdSistema != null && IdDestino != null)
                strConsulta.Append("SELECT dr.*,mb.*,z.idZonas, z.Nombre as NombreZona FROM DestinosRadio dr " + 
                                    "LEFT JOIN zonas z ON z.idZonas=dr.zonas_idZonas " +
                                    "LEFT JOIN metodosbss_destinosradio mbdr ON mbdr.metodosbss_destinosradio_fk = dr.fk_metodosbss " +
                                    "LEFT JOIN metodos_bss mb ON mb.idmetodos_bss = mbdr.metodos_bss_idmetodos_bss " +
                                    "WHERE dr.IdSistema='" + IdSistema + "' AND dr.IdDestino='" + IdDestino + "'");
			else if (IdSistema != null)
                strConsulta.Append("SELECT dr.*,mb.*,z.idZonas, z.Nombre as NombreZona FROM DestinosRadio dr " +
                                    "LEFT JOIN zonas z ON z.idZonas=dr.zonas_idZonas " +
                                    "LEFT JOIN metodosbss_destinosradio mbdr ON mbdr.metodosbss_destinosradio_fk = dr.fk_metodosbss " +
                                    "LEFT JOIN metodos_bss mb ON mb.idmetodos_bss = mbdr.metodos_bss_idmetodos_bss " +
                                    "WHERE dr.IdSistema='" + IdSistema + "'");
			else
                strConsulta.Append("SELECT dr.*,mb.*,z.idZonas, z.Nombre as NombreZona FROM DestinosRadio dr " +
                                    "LEFT JOIN zonas z ON z.idZonas=dr.zonas_idZonas " +
                                    "LEFT JOIN metodosbss_destinosradio mbdr ON mbdr.metodosbss_destinosradio_fk = dr.fk_metodosbss " +
                                    "LEFT JOIN metodos_bss mb ON mb.idmetodos_bss = mbdr.metodos_bss_idmetodos_bss");

            return strConsulta.ToString();
        }

        public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds)
        {
            ListaResultado.Clear();

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    DestinosRadio r = new DestinosRadio();

                    if (dr["fk_metodosbss"] != System.DBNull.Value)
                        r.Fk_Metodosbss = (int)dr["fk_metodosbss"];
                    if (dr["IdSistema"] != System.DBNull.Value)
                        r.IdSistema = (string)dr["IdSistema"];
                    if (dr["IdDestino"] != System.DBNull.Value)
                        r.IdDestino = (string)dr["IdDestino"];
                    if (dr["TipoDestino"] != System.DBNull.Value)
                        r.TipoDestino = (uint)dr["TipoDestino"];
                    if (dr["idZonas"] != System.DBNull.Value)
                        r.Zonas_IdZonas = (int)dr["idZonas"];
                    if (dr["TipoFrec"] != System.DBNull.Value)
                        r.TipoFrec = (uint)dr["TipoFrec"];
                    if (dr["ExclusividadTXRX"] != System.DBNull.Value)
                        r.ExclusividadTXRX = (bool)dr["ExclusividadTXRX"];  
                    if (dr["Frecuencia"] != System.DBNull.Value)
                        r.Frecuencia = (int)dr["Frecuencia"];
                    if (dr["MetodoCalculoClimax"] != System.DBNull.Value)
                        r.MetodoCalculoClimax = (int)dr["MetodoCalculoClimax"];
                    if (dr["VentanaSeleccionBss"] != System.DBNull.Value)
                        r.VentanaSeleccionBss = (int)dr["VentanaSeleccionBss"];
                    if (dr["SincronizaGrupoClimax"] != System.DBNull.Value)
                        r.SincronizaGrupoClimax = (bool)dr["SincronizaGrupoClimax"];
                    if (dr["AudioPrimerSqBss"] != System.DBNull.Value)
                        r.AudioPrimerSqBss = (bool)dr["AudioPrimerSqBss"];
                    if (dr["FrecuenciaNoDesasignable"] != System.DBNull.Value)
                        r.FrecuenciaNoDesasignable = (bool)dr["FrecuenciaNoDesasignable"];
                    if (dr["VentanaReposoZonaTxDefecto"] != System.DBNull.Value)
                        r.VentanaReposoZonaTxDefecto = (int)dr["VentanaReposoZonaTxDefecto"];
                    if (dr["PrioridadSesionSip"] != System.DBNull.Value)
                        r.PrioridadSesionSip = (int)dr["PrioridadSesionSip"];
                    if (dr["MetodosBssOfrecidos"] != System.DBNull.Value)
                        r.MetodosBssOfrecidos = (int)dr["MetodosBssOfrecidos"];
                    if (dr["CldSupervisionTime"] != System.DBNull.Value)
                        r.CldSupervisionTime = (int)dr["CldSupervisionTime"];

                    if (dr["name"] != System.DBNull.Value)
                    {
                        MetodosBssDelRecurso s= new MetodosBssDelRecurso((int)dr["idmetodos_bss"],(string)dr["name"]);
                        r._metodosBss.Add(s);
                    }
                    if (dr["NombreZona"] != System.DBNull.Value)
                        r.NombreZonaTxDefecto = (string)dr["NombreZona"];

                    if (null != dr["CnfModoDestino"] && dr["CnfModoDestino"] != System.DBNull.Value)
                        r.CnfModoDestino = (string)dr["CnfModoDestino"];

                    if (null != dr["CnfTipoFrecuencia"] && dr["CnfTipoFrecuencia"] != System.DBNull.Value)
                        r.CnfTipoFrecuencia = (string)dr["CnfTipoFrecuencia"];

                    if (null != dr["ModoTransmision"] && dr["ModoTransmision"] != System.DBNull.Value)
                        r.ModoTransmision = (string)dr["ModoTransmision"];

                    //VMG 18/02/2019
                    if (null != dr["EmplazamientoDefecto"] && dr["EmplazamientoDefecto"] != System.DBNull.Value)
                        r.EmplazamientoDefecto = (string)dr["EmplazamientoDefecto"];

                    if (null != dr["TiempoVueltaADefecto"] && dr["TiempoVueltaADefecto"] != System.DBNull.Value)
                        r.TiempoVueltaADefecto = (dr["TiempoVueltaADefecto"]).ToString();

                    ListaResultado.Add(r);
                }
            }
            return ListaResultado;
        }

        // Al insertar DestinoRadio==>Trigger AltaDestinoRadio (da de alta un destino)
        public override string[] InsertSQL()
        {
            string[] consulta = new string[2];
            StringBuilder strConsulta = new StringBuilder();

            strConsulta.Append("INSERT INTO DestinosRadio (IdSistema,IdDestino,TipoDestino,TipoFrec,ExclusividadTXRX,Frecuencia,MetodoCalculoClimax,VentanaSeleccionBss,SincronizaGrupoClimax,AudioPrimerSqBss,");
            strConsulta.Append("FrecuenciaNoDesasignable,VentanaReposoZonaTxDefecto,PrioridadSesionSip,MetodosBssOfrecidos,CldSupervisionTime,CnfModoDestino,CnfTipoFrecuencia,ModoTransmision,EmplazamientoDefecto,TiempoVueltaADefecto) ");
            strConsulta.AppendFormat(" VALUES ('{0}','{1}',{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},'{15}','{16}',{17},", IdSistema, IdDestino, TipoDestino, TipoFrec, ExclusividadTXRX,
                Frecuencia, MetodoCalculoClimax, VentanaSeleccionBss, SincronizaGrupoClimax, AudioPrimerSqBss,
                FrecuenciaNoDesasignable, VentanaReposoZonaTxDefecto, PrioridadSesionSip, MetodosBssOfrecidos, CldSupervisionTime,
                CnfModoDestino, CnfTipoFrecuencia,((ModoTransmision == null) ? "null" : ("'" + ModoTransmision + "'")));

            if (!string.IsNullOrEmpty(EmplazamientoDefecto) && string.Compare(EmplazamientoDefecto, "0") != 0)
                strConsulta.AppendFormat("'{0}',{1})", EmplazamientoDefecto, (TiempoVueltaADefecto!=null) ? Convert.ToInt16(TiempoVueltaADefecto): 0);
            else
                strConsulta.AppendFormat("NULL,NULL)");
            
            consulta[0] = strConsulta.ToString();
            consulta[1] = ReplaceSQL(IdSistema, "DestinosRadio");

            // Insertar los metodos bss asociados al destino
            foreach (MetodosBssDelRecurso m in MetodosBss)
            {
                CD40.BD.Entidades.MetodosbssDestinosradio mBss = new CD40.BD.Entidades.MetodosbssDestinosradio();
                mBss.Metodos_bss_Idmetodos_bss = m.idMetodo;
                mBss.Metodosbss_Destinosradio_fk = Fk_Metodosbss;

                string[] listaQueries = mBss.InsertSQL();

                Array.Resize<string>(ref consulta, consulta.Length + listaQueries.Length);
                Array.Copy(listaQueries,0,consulta,consulta.Length - listaQueries.Length,listaQueries.Length);
            }

            strConsulta.Clear();
            return consulta;
        }

        public override string[] UpdateSQL()
        {
            string[] consulta = new string[2];
            StringBuilder strConsulta = new StringBuilder();


            strConsulta.AppendFormat("UPDATE DestinosRadio SET IdSistema='{0}',IdDestino='{1}',TipoDestino={2},TipoFrec={3},ExclusividadTXRX={4},Frecuencia={5},MetodoCalculoClimax={6}",
                                   IdSistema,IdDestino,TipoDestino,TipoFrec,ExclusividadTXRX,Frecuencia,MetodoCalculoClimax);
            strConsulta.AppendFormat(",VentanaSeleccionBss={0},SincronizaGrupoClimax={1},AudioPrimerSqBss={2},FrecuenciaNoDesasignable={3} ,VentanaReposoZonaTxDefecto={4},PrioridadSesionSip={5},MetodosBssOfrecidos={6},CldSupervisionTime={7}",
                                    VentanaSeleccionBss,SincronizaGrupoClimax,AudioPrimerSqBss, FrecuenciaNoDesasignable, VentanaReposoZonaTxDefecto,PrioridadSesionSip,MetodosBssOfrecidos,CldSupervisionTime);
            strConsulta.AppendFormat(",CnfModoDestino='{0}',CnfTipoFrecuencia='{1}', ModoTransmision={2},  ", CnfModoDestino, CnfTipoFrecuencia, ((ModoTransmision == null) ? "null" : ("'" + ModoTransmision + "'")));

            if (!string.IsNullOrEmpty(EmplazamientoDefecto) && string.Compare(EmplazamientoDefecto,"0")!=0)
                strConsulta.AppendFormat("EmplazamientoDefecto='{0}', TiempoVueltaADefecto={1} ", EmplazamientoDefecto, Convert.ToInt16(TiempoVueltaADefecto));

            else
                strConsulta.AppendFormat("EmplazamientoDefecto=NULL, TiempoVueltaADefecto=NULL ");
            
            strConsulta.AppendFormat("WHERE IdDestino='{0}' AND IdSistema='{1}'", IdDestino, IdSistema);

            consulta[0] = strConsulta.ToString();
            consulta[1] = ReplaceSQL(IdSistema, "DestinosRadio");

            // Eliminar los metodos asociados al destino
            CD40.BD.Entidades.MetodosbssDestinosradio mBss = new MetodosbssDestinosradio();
            mBss.Idmetodosbss_Destinosradio = this.Fk_Metodosbss;

            string[] listaQueries = mBss.DeleteSQL();
            Array.Resize<string>(ref consulta, consulta.Length + listaQueries.Length);
            Array.Copy(listaQueries, 0, consulta, consulta.Length - listaQueries.Length, listaQueries.Length);

            // Insertar los metodos bss asociados al destino
            foreach (MetodosBssDelRecurso m in MetodosBss)
            {
                mBss.Metodos_bss_Idmetodos_bss = m.idMetodo;
                mBss.Metodosbss_Destinosradio_fk = Fk_Metodosbss;

                listaQueries = mBss.InsertSQL();

                Array.Resize<string>(ref consulta, listaQueries.Length);
                Array.Copy(listaQueries, 0, consulta, consulta.Length - listaQueries.Length, listaQueries.Length);
            }

            strConsulta.Clear();

            return consulta;
        }

        public override string[] DeleteSQL()
        {
            string[] consulta = new string[2];

            StringBuilder strConsulta = new StringBuilder();

            if (IdDestino != null)
                if (IdSistema != null)
                    strConsulta.AppendFormat("DELETE FROM DestinosRadio WHERE IdDestino='{0}' AND IdSistema='{1}'", IdDestino, IdSistema);
                else 
                    strConsulta.AppendFormat("DELETE FROM DestinosRadio WHERE IdDestino='{0}' ", IdDestino);
            else
                strConsulta.Append("DELETE FROM DestinosRadio");

            consulta[0] = strConsulta.ToString();
            consulta[1] = ReplaceSQL(IdSistema, "DestinosRadio");

            strConsulta.Clear();

            return consulta;
        }
    }
}
