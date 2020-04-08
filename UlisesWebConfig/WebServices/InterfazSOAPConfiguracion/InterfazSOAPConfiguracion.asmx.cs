using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Runtime.Serialization;
//using System.Runtime.Serialization.Formatters.Soap;
using System.Xml.Serialization;

using CD40.BD;
using CD40.BD.Entidades;
//using SICCIP.Entidades;
using ConfiguracionElementosHw;

namespace UlisesWebConfig.WebServices.InterfazSOAPConfiguracion
{
    /// <summary>
    /// Descripción breve de InterfazSOAPConfiguracion
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    #region XML-Includes
    [XmlInclude(typeof(Tablas))]
    [XmlInclude(typeof(Agrupaciones))]
    [XmlInclude(typeof(SectoresAgrupacion))]
    [XmlInclude(typeof(GruposTelefonia))]
    [XmlInclude(typeof(TifX))]
    [XmlInclude(typeof(Top))]
    [XmlInclude(typeof(Sistema))]
    [XmlInclude(typeof(CD40.BD.Entidades.Operadores))]
    [XmlInclude(typeof(Emplazamientos))]
    [XmlInclude(typeof(Nucleos))]
    [XmlInclude(typeof(Prefijos))]
    [XmlInclude(typeof(Sectorizaciones))]
    [XmlInclude(typeof(Redes))]
    [XmlInclude(typeof(PermisosRedes))]
    [XmlInclude(typeof(Troncales))]
    [XmlInclude(typeof(SectoresSectorizacion))]
    [XmlInclude(typeof(Encaminamientos))]
    [XmlInclude(typeof(Sectores))]
    [XmlInclude(typeof(Rangos))]
    [XmlInclude(typeof(Rutas))]
    [XmlInclude(typeof(ParametrosRecursoRadio))]
    [XmlInclude(typeof(ParametrosRecursoTelefonia))]
    [XmlInclude(typeof(ParametrosRecursoLCEN))]
    [XmlInclude(typeof(Recursos))]
    [XmlInclude(typeof(RecursosLCEN))]
    [XmlInclude(typeof(RecursosTF))]
    [XmlInclude(typeof(RecursosRadio))]
    [XmlInclude(typeof(RecursosRadioForGateway))]
    [XmlInclude(typeof(UsuariosAbonados))]
    [XmlInclude(typeof(Destinos))]
    [XmlInclude(typeof(DestinosExternos))]
    [XmlInclude(typeof(Externos))]
    [XmlInclude(typeof(DestinosExternosSector))]
    [XmlInclude(typeof(DestinosInternos))]
    [XmlInclude(typeof(Internos))]
    [XmlInclude(typeof(DestinosInternosSector))]
    [XmlInclude(typeof(DestinosRadio))]
    [XmlInclude(typeof(DestinosRadioSector))]
    [XmlInclude(typeof(EstadosRecursos))]
    [XmlInclude(typeof(Radio))]
    [XmlInclude(typeof(DestinosTelefonia))]
    [XmlInclude(typeof(Niveles))]
    [XmlInclude(typeof(Altavoces))]
    [XmlInclude(typeof(GruposTelefonia))]
    [XmlInclude(typeof(SectoresSector))]
    [XmlInclude(typeof(TroncalesRuta))]
    [XmlInclude(typeof(TeclasSector))]
    [XmlInclude(typeof(ParametrosSector))]
    [XmlInclude(typeof(HFParams))]
    [XmlInclude(typeof(HFRangoFrecuencias))]


    [XmlInclude(typeof(RangosSCV))]
    [XmlInclude(typeof(RecursosSCV))]
    [XmlInclude(typeof(SectoresSCV))]
    [XmlInclude(typeof(ParametrosRDSCV))]
    [XmlInclude(typeof(ParametrosLCENSCV))]
    [XmlInclude(typeof(ParametrosTFSCV))]
    [XmlInclude(typeof(ParametrosSectorSCV))]
    [XmlInclude(typeof(PermisosRedesSCV))]
    [XmlInclude(typeof(TeclasSectorSCV))]
    [XmlInclude(typeof(NivelesSCV))]

    [XmlInclude(typeof(Tipos.ExportaTipoEnumerados))]
    #endregion
    public class InterfazSOAPConfiguracion : System.Web.Services.WebService
    {
        public struct EstadoNode
        {
            public string Name;
            public bool Presencia;  // 0: No presente; 1: Presente
            public int Estado;     // 0-1: Desconocido; 2: Principal; 3: Reserva
        }
        public struct EstadoCluster
        {
            public EstadoNode EstadoNode1, EstadoNode2;
        }
        enum TipoRecursoRadio
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

        private GestorBaseDatos GestorBDCD40;
        private MySql.Data.MySqlClient.MySqlConnection MySqlConnectionToCd40;
        ConfiguracionSistema CfgSistema;

        CfgUsuario ConfiguracionUsuario;
        CfgPasarela ConfiguracionPasarela;
        CfgEnlaceExterno[] ConfiguracionEnlaceExterno;
        CfgEnlaceInterno[] ConfiguracionEnlaceInterno;

        static object Sync = new object();

        public InterfazSOAPConfiguracion()
        {
            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
            string cadenaConexion;
            System.Configuration.Configuration webConfiguracion = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            if (webConfiguracion.ConnectionStrings.ConnectionStrings.Count > 0)
            {
                cadenaConexion = webConfiguracion.ConnectionStrings.ConnectionStrings["ConexionBaseDatosCD40"].ToString();

                MySqlConnectionToCd40 = new MySql.Data.MySqlClient.MySqlConnection(cadenaConexion);

                GestorBDCD40 = new GestorBaseDatos(MySqlConnectionToCd40);
            }

            ConfiguracionUsuario = new ConfiguracionElementosHw.CfgUsuario();
            ConfiguracionPasarela = new CfgPasarela();

            ConfiguracionUsuario.ListaAbonados = new List<NumerosAbonado>();
            //ConfiguracionUsuario.ListaNombreUsuario = new List<string>();

            CfgSistema = new ConfiguracionSistema();
            CfgSistema.ParametrosGenerales = new ParametrosGeneralesSistema();
        }

        #region Métodos
        public DireccionamientoIP[] ListaPlanDireccionamientoIP(string id_sistema)
        {
            //Procedimientos pr = new Procedimientos();
            DataSet dsPlan = Procedimientos.PlanDireccionamientoIP(GestorBDCD40.ConexionMySql, id_sistema);
            DireccionamientoIP[] plan = new DireccionamientoIP[dsPlan.Tables[0].Rows.Count];
            int numFila = 0;

            if (dsPlan != null)
            {
                foreach (System.Data.DataRow dr in dsPlan.Tables[0].Rows)
                {
                    DireccionamientoIP p = new DireccionamientoIP();

                    p.IdHost = (string)dr["Id"];
                    p.TipoHost = (Tipos.Tipo_Elemento_HW)Convert.ToUInt32(dr["TipoEH"]);
                    if (dr["IpRed1"] != System.DBNull.Value)
                        p.IpRed1 = (string)dr["IpRed1"];
                    if (dr["IpRed2"] != System.DBNull.Value)
                        p.IpRed2 = (string)dr["IpRed2"];
                    if (dr["Interno"] != System.DBNull.Value)
                        p.Interno = ((long)dr["Interno"]) != 0;
                    if (dr["Min"] != System.DBNull.Value)
                        p.Min = Convert.ToInt64(dr["Min"]);
                    if (dr["Max"] != System.DBNull.Value)
                        p.Max = Convert.ToInt64(dr["Max"]);

                    if (dr["EsCentralIP"] != System.DBNull.Value)
                        p.EsCentralIP = ((long)dr["EsCentralIP"]) != 0;


                    // Se obtiene la dirección IP del servidor proxy 3
                    if (dr["IpRed3"] != System.DBNull.Value)
                        p.IpRed3 = (string)dr["IpRed3"];
                    else
                        p.IpRed3 = String.Empty;

                    // Se obtiene la dirección IP del servidor 1 de presencia
                    if (dr["SrvPresenciaIpRed1"] != System.DBNull.Value)
                        p.SrvPresenciaIpRed1 = (string)dr["SrvPresenciaIpRed1"];
                    else
                        p.SrvPresenciaIpRed1 = String.Empty;

                    // Se obtiene la dirección IP del servidor 2 de presencia
                    if (dr["SrvPresenciaIpRed2"] != System.DBNull.Value)
                        p.SrvPresenciaIpRed2 = (string)dr["SrvPresenciaIpRed2"];
                    else
                        p.SrvPresenciaIpRed2 = String.Empty;

                    // Se obtiene la dirección IP del servidor 3 de presencia
                    if (dr["SrvPresenciaIpRed3"] != System.DBNull.Value)
                        p.SrvPresenciaIpRed3 = (string)dr["SrvPresenciaIpRed3"];
                    else
                        p.SrvPresenciaIpRed3 = String.Empty;

                    plan[numFila++] = p;
                }
            }

            return plan;
        }
        private void DameParametrosGenerales(string id_sistema)
        {
            Sistema s = new Sistema();
            s.IdSistema = id_sistema;

            List<Tablas> listaSistemas = GestorBDCD40.ListSelectSQL(s, null);
            if (listaSistemas.Count > 0)
            {
                s = (Sistema)listaSistemas[0];

                CfgSistema.ParametrosGenerales.TamLiteralEnlAG = s.TamLiteralAG;
                CfgSistema.ParametrosGenerales.TamLiteralEnlDA = s.TamLiteralDA;
                CfgSistema.ParametrosGenerales.TamLiteralEmplazamiento = s.TamLiteralEmplazamiento;
                CfgSistema.ParametrosGenerales.TamLiteralEnlExt = s.TamLiteralEnlExt;
                CfgSistema.ParametrosGenerales.TamLiteralEnlIA = s.TamLiteralIA;
                CfgSistema.ParametrosGenerales.TiempoMaximoPTT = s.TiempoPtt;
                CfgSistema.ParametrosGenerales.TiempoSinJack1 = s.TiempoSinJack1;
                CfgSistema.ParametrosGenerales.TiempoSinJack2 = s.TiempoSinJack2;
            }
            /*
            IFormatter format = new SoapFormatter();
            Stream canal = new FileStream("Listado.xml", FileMode.Create, FileAccess.Write, FileShare.None);
            format.Serialize(canal, s);
            canal.Close();
            */
        }
        private void DamePlanNumeracionATS(string id_sistema)
        {
            //Procedimientos pr = new Procedimientos();
            var ut = new CD40.BD.Utilidades(GestorBDCD40.ConexionMySql);

            ushort j = 0;
            Encaminamientos e = new Encaminamientos();
            e.IdSistema = id_sistema;


            List<Tablas> listaEncaminamientos = GestorBDCD40.ListSelectSQL(e, null);
            int tamListaEncaminamientos = listaEncaminamientos.Count;

            CfgSistema.PlanNumeracionATS = new NumeracionATS[tamListaEncaminamientos];
            for (ushort i = 0; i < tamListaEncaminamientos; i++)
            {
                // Datos de la central
                CfgSistema.PlanNumeracionATS[i] = new NumeracionATS();
                CfgSistema.PlanNumeracionATS[i].CentralPropia = ((Encaminamientos)listaEncaminamientos[i]).CentralPropia;
                CfgSistema.PlanNumeracionATS[i].Throwswitching = ((Encaminamientos)listaEncaminamientos[i]).Throwswitching;
                CfgSistema.PlanNumeracionATS[i].NumTest = CfgSistema.PlanNumeracionATS[i].CentralPropia ? ((Encaminamientos)listaEncaminamientos[i]).NumTest : string.Empty;
                CfgSistema.PlanNumeracionATS[i].Central = ((Encaminamientos)listaEncaminamientos[i]).Central;

                // Datos de los rangos
                Rangos r = new Rangos();
                r.IdSistema = id_sistema;
                r.Central = ((Encaminamientos)listaEncaminamientos[i]).Central;
                // Rangos de operador
                r.Tipo = "O";
                List<Tablas> listaRangos = GestorBDCD40.ListSelectSQL(r, null);
                int tamListaRangos = listaRangos.Count;
                CfgSistema.PlanNumeracionATS[i].RangosOperador = new RangosSCV[tamListaRangos];
                for (ushort num = 0; num < tamListaRangos; num++)
                {
                    CfgSistema.PlanNumeracionATS[i].RangosOperador[num] = (RangosSCV)listaRangos[num];
                }
                // Rangos de privilegiado 
                r.Tipo = "P";
                listaRangos = GestorBDCD40.ListSelectSQL(r, null);
                tamListaRangos = listaRangos.Count;
                CfgSistema.PlanNumeracionATS[i].RangosPrivilegiados = new RangosSCV[tamListaRangos];
                for (ushort num = 0; num < tamListaRangos; num++)
                {
                    CfgSistema.PlanNumeracionATS[i].RangosPrivilegiados[num] = (RangosSCV)listaRangos[num];
                }

                // Datos de las rutas
                DataSet dsRutas = Procedimientos.ListaRutas(GestorBDCD40.ConexionMySql, id_sistema, ((Encaminamientos)listaEncaminamientos[i]).Central);

                CfgSistema.PlanNumeracionATS[i].ListaRutas = new PlanRutas[dsRutas.Tables[0].Rows.Count];
                j = 0;
                foreach (System.Data.DataRow drRutas in dsRutas.Tables[0].Rows)
                {
                    CfgSistema.PlanNumeracionATS[i].ListaRutas[j] = new PlanRutas();

                    CfgSistema.PlanNumeracionATS[i].ListaRutas[j].TipoRuta = (string)drRutas["Tipo"];
                    CfgSistema.PlanNumeracionATS[i].ListaRutas[j].ListaTroncales = ut.ListaDeTroncalesEnUnaRuta(id_sistema, ((Encaminamientos)listaEncaminamientos[i]).Central, (string)drRutas["IdRuta"]);
                    j++;
                }
            }
        }
        private void DamePlanDireccionamientoIP(string id_sistema)
        {
            CfgSistema.PlanDireccionamientoIP = ListaPlanDireccionamientoIP(id_sistema);
        }
        private void DamePlanTroncales(string id_sistema)
        {
            Troncales t = new Troncales();
            t.IdSistema = id_sistema;

            List<Tablas> listaTroncales = GestorBDCD40.ListSelectSQL(t, null);
            int tamListaTroncales = listaTroncales.Count;
            CfgSistema.PlanTroncales = new ListaTroncales[tamListaTroncales];
            for (ushort i = 0; i < tamListaTroncales; i++)
            {
                CfgSistema.PlanTroncales[i] = new ListaTroncales();
                CfgSistema.PlanTroncales[i].IdTroncal = ((Troncales)listaTroncales[i]).IdTroncal;
                CfgSistema.PlanTroncales[i].NumeroTest = ((Troncales)listaTroncales[i]).NumTest;

                RecursosTF r = new RecursosTF();
                r.IdSistema = id_sistema;
                r.IdTroncal = ((Troncales)listaTroncales[i]).IdTroncal;

                List<Tablas> listaRecursos = GestorBDCD40.ListSelectSQL(r, null);
                int tamListaRecursos = listaRecursos.Count;
                //int posA = 0;
                //int posB = tamListaRecursos - 1;

                CfgSistema.PlanTroncales[i].ListaRecursos = new PlanRecursos[tamListaRecursos];
                for (ushort j = 0; j < tamListaRecursos; j++)
                {
                    /*
                    int posGlobal;
                    if (((RecursosTF)listaRecursos[j]).Lado == "0")
                        posGlobal = posA++;
                    else
                        posGlobal = posB--;
                    */
                    CfgSistema.PlanTroncales[i].ListaRecursos[j] = new PlanRecursos();
                    CfgSistema.PlanTroncales[i].ListaRecursos[j].IdRecurso = ((RecursosTF)listaRecursos[j]).IdRecurso;

                    Recursos rTroncal = new Recursos();
                    rTroncal.IdSistema = r.IdSistema;
                    rTroncal.IdRecurso = CfgSistema.PlanTroncales[i].ListaRecursos[j].IdRecurso;
                    rTroncal.TipoRecurso = (uint)Tipos.Tipo_Recurso.TR_TELEFONIA;
                    List<Tablas> listaRecursosTroncal = GestorBDCD40.ListSelectSQL(rTroncal, null);
                    if (listaRecursosTroncal.Count > 0)
                        CfgSistema.PlanTroncales[i].ListaRecursos[j].Tipo = (Tipos.TipoInterface)((Recursos)listaRecursosTroncal[0]).Interface;
                }
            }
        }
        private void DamePlanRedes(string id_sistema)
        {
            Redes rd = new Redes();
            rd.IdSistema = id_sistema;

            List<Tablas> listaRedes = GestorBDCD40.ListSelectSQL(rd, null);
            int tamListaRedes = listaRedes.Count;
            CfgSistema.PlanRedes = new ListaRedes[tamListaRedes];
            for (ushort i = 0; i < tamListaRedes; i++)
            {
                CfgSistema.PlanRedes[i] = new ListaRedes();
                CfgSistema.PlanRedes[i].IdRed = ((Redes)listaRedes[i]).IdRed;
                CfgSistema.PlanRedes[i].Prefijo = Convert.ToUInt32(((Redes)listaRedes[i]).Prefijo);

                RecursosTF r = new RecursosTF();
                r.IdSistema = id_sistema;
                r.IdRed = ((Redes)listaRedes[i]).IdRed;

                List<Tablas> listaRecursos = GestorBDCD40.ListSelectSQL(r, null);
                int tamListaRecursos = listaRecursos.Count;
                CfgSistema.PlanRedes[i].ListaRecursos = new PlanRecursos[tamListaRecursos];
                for (ushort j = 0; j < tamListaRecursos; j++)
                {
                    CfgSistema.PlanRedes[i].ListaRecursos[j] = new PlanRecursos();
                    CfgSistema.PlanRedes[i].ListaRecursos[j].IdRecurso = ((RecursosTF)listaRecursos[j]).IdRecurso;

                    Recursos rTroncal = new Recursos();
                    rTroncal.IdSistema = r.IdSistema;
                    rTroncal.IdRecurso = ((RecursosTF)listaRecursos[j]).IdRecurso;
                    List<Tablas> listaRecursosTroncal = GestorBDCD40.ListSelectSQL(rTroncal, null);
                    if (listaRecursosTroncal.Count > 0)
                        CfgSistema.PlanRedes[i].ListaRecursos[j].Tipo = (Tipos.TipoInterface)((Recursos)listaRecursosTroncal[0]).Interface;
                }
            }
        }
        private void DamePlanAsignacionUsuarios(string id_sistema)
        {
            uint i = 0;
            //Procedimientos pr = new Procedimientos();
            DataSet ds = Procedimientos.AsignacionUsuariosATops(GestorBDCD40.ConexionMySql, id_sistema);

            CfgSistema.PlanAsignacionUsuarios = new AsignacionUsuariosTV[ds.Tables[0].Rows.Count];
            foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
            {
                CfgSistema.PlanAsignacionUsuarios[i] = new AsignacionUsuariosTV();
                CfgSistema.PlanAsignacionUsuarios[i].IdUsuario = (string)dr["IdSector"];
                CfgSistema.PlanAsignacionUsuarios[i].IpGrabador1 = dr["IpGrabador1"] != System.DBNull.Value ? (string)dr["IpGrabador1"] : string.Empty;
                CfgSistema.PlanAsignacionUsuarios[i].IpGrabador2 = dr["IpGrabador2"] != System.DBNull.Value ? (string)dr["IpGrabador2"] : string.Empty;
                CfgSistema.PlanAsignacionUsuarios[i].RtspPort = dr["RtspPort"] != System.DBNull.Value ? (int)dr["RtspPort"] : 0;
                CfgSistema.PlanAsignacionUsuarios[i++].IdHost = (string)dr["IdTop"];
            }
        }
        private void DamePlanAsignacionRecursos(string id_sistema)
        {
            const int PUERTO_SIP_DEFECTO = 5060;

            Recursos r = new Recursos();
            r.IdSistema = id_sistema;
            r.TipoRecurso = (uint)Tipos.Tipo_Recurso.TR_DONT_CARE;

            List<Tablas> listaRecursos = GestorBDCD40.ListSelectSQL(r, null);

            if (listaRecursos != null && listaRecursos.Count > 0)
            {
                int tamListaRecursos = listaRecursos.Count;
                CfgSistema.PlanAsignacionRecursos = new AsignacionRecursosGW[tamListaRecursos];

                for (ushort i = 0; i < tamListaRecursos; i++)
                {
                    CfgSistema.PlanAsignacionRecursos[i] = new AsignacionRecursosGW();
                    CfgSistema.PlanAsignacionRecursos[i].IdRecurso = ((Recursos)listaRecursos[i]).IdRecurso;
                    CfgSistema.PlanAsignacionRecursos[i].IdHost = ((Recursos)listaRecursos[i]).IdTifX != null ? ((Recursos)listaRecursos[i]).IdTifX : ((Recursos)listaRecursos[i]).IdEquipo;

                    //Modificacion para sacar el puerto sip de la ip del encaminamiento (equiposeu)
                    if (((Recursos)listaRecursos[i]).TipoRecurso == 1)
                    {
                        int position = (((Recursos)listaRecursos[i]).IpRed1).IndexOf(":");

                        if (position == -1)
                        {
                            //Si la IP no tiene puerto configurada, se toma el puerto SIP
                            if (((Recursos)listaRecursos[i]).PuertoSip == 0)//Si el puerto SIP es 0, se devuelve el puerto por defecto
                                CfgSistema.PlanAsignacionRecursos[i].SipPort = PUERTO_SIP_DEFECTO;
                            else
                                CfgSistema.PlanAsignacionRecursos[i].SipPort = ((Recursos)listaRecursos[i]).PuertoSip;
                        }
                        else
                        {
                            //Se toma el puerto de la dirección IP
                            CfgSistema.PlanAsignacionRecursos[i].SipPort = Convert.ToUInt16((((Recursos)listaRecursos[i]).IpRed1).Substring(position + 1));
                        }
                    }
                    else
                    {
                        CfgSistema.PlanAsignacionRecursos[i].SipPort = ((Recursos)listaRecursos[i]).PuertoSip;
                    }
                }
            }
        }
        private void DamePlanDireccionamientoSIP(string id_sistema)
        {
            //Procedimientos pr = new Procedimientos();
            DataSet dsPlan = Procedimientos.PlanDireccionamientoSIP(GestorBDCD40.ConexionMySql, id_sistema);
            List<DireccionamientoSIP> plan = new List<DireccionamientoSIP>();

            int numFila = 0;
            int numAbonado = 0;
            string ultimoUsuario = "";

            if (dsPlan != null)
            {
                foreach (System.Data.DataRow dr in dsPlan.Tables[0].Rows)
                {
                    if (ultimoUsuario != (string)dr["IdSector"])
                    {
                        ultimoUsuario = (string)dr["IdSector"];
                        numAbonado = 0;

                        DireccionamientoSIP unDir = new DireccionamientoSIP();
                        unDir.IdUsuario = (string)dr["IdSector"];
                        unDir.NumerosAbonadoQueAtiende = new List<DireccionamientoSIP.StrNumeroAbonado>();

                        plan.Add(unDir);
                        numFila++;
                    }

                    DireccionamientoSIP.StrNumeroAbonado abonado;   // = new DireccionamientoSIP.StrNumeroAbonado();

                    abonado.IdAgrupacion = (string)dr["IdAgrupacion"];
                    abonado.Prefijo = Convert.ToUInt32(dr["IdPrefijo"]);
                    abonado.NumeroAbonado = (string)dr["IdAbonado"];
                    plan[numFila - 1].NumerosAbonadoQueAtiende.Add(abonado);

                    numAbonado++;
                }
            }

            CfgSistema.PlanDireccionamientoSIP = plan;
        }
        private List<ParametrosRDSCV.RecursosExternos> GetListaEnlaces(ParametrosRecursosRadioKASiccip r)
        {
            //MVO: se comenta el contenido de la función para que siempre devuelva null, y así evitar
            //     que realice la consulta a la vista interface_sacim.viewgeturienlacelineasplanactivo  que no existe en la BD 

            /*  
                List<ParametrosRDSCV.RecursosExternos> listaEnlaces = new List<ParametrosRDSCV.RecursosExternos>();
                DataSet ds = GestorBDCD40.GetDataSet(r.GetUriRecursoDestino(), null);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if ((string)dr["IdRecurso2"] != ((RecursosRadio)r).IdRecurso)
                        {
                            ParametrosRDSCV.RecursosExternos rec = new ParametrosRDSCV.RecursosExternos();

                            rec.Nombre = (string)dr["IdRecurso2"];

                            listaEnlaces.Add(rec);
                        }
                    }
                }

                return listaEnlaces.Count == 0 ? null : listaEnlaces;

            */

            return null;
        }
        private bool GetGrupo(string idSistema, string idGrupo)
        {
            GruposTelefonia gr = new GruposTelefonia();
            gr.IdSistema = idSistema;
            gr.IdGrupo = idGrupo;

            return GestorBDCD40.ListSelectSQL(gr, null).Count > 0;
        }
        public List<Tablas> ListaRecursosPasarela(string id_sistema, string id_hw)
        {
            Recursos r = new Recursos();
            r.IdTifX = id_hw;
            r.IdSistema = id_sistema;

            List<Tablas> listaRecursos = GestorBDCD40.ListSelectSQL(r, null);
            return listaRecursos;
        }
        private string GetEstadoDelRecurso(string idSectorizacion, Recursos r, Radio rRadio)
        {
            EstadoRecursos eRecurso = new EstadoRecursos();
            eRecurso.IdSistema = rRadio.IdSistema;
            eRecurso.IdSector = rRadio.IdSector;
            eRecurso.IdNucleo = rRadio.IdNucleo;
            eRecurso.IdSectorizacion = idSectorizacion;
            eRecurso.IdDestino = rRadio.IdDestino;
            eRecurso.IdRecurso = r.IdRecurso;

            List<Tablas> estado = GestorBDCD40.ListSelectSQL(eRecurso, null);
            if (estado.Count > 0)
            {
                return ((EstadoRecursos)estado[0]).Estado;
            }

            return "S";
        }
        private int[] GetValueTablaBss(string tableName)
        {
            int[] valores = new int[6];
            ValoresTabla vTable = new ValoresTabla();

            List<Tablas> valoresTabla = GestorBDCD40.ListSelectSQL(vTable, tableName, null);
            if (valoresTabla != null)
            {
                for (int i = 0; i < valoresTabla.Count; i++)
                {
                    if (((ValoresTabla)valoresTabla[i]).Valor_rssi < 16)
                        valores[((ValoresTabla)valoresTabla[i]).Valor_Prop] = ((ValoresTabla)valoresTabla[i]).Valor_rssi;
                }
            }

            return valores;
        }

        #endregion Métodos

        #region WebServices

        [WebMethod(Description = "Pasándole el identificador de sistema, devuelve la versión de la configuración activa registrada en la base de datos")]
        public void GetVersionConfiguracion_XML(string id_sistema)
        {
            //Procedimientos pr = new Procedimientos();

            GetVersion v;
            v.IdSistema = id_sistema;
            v.Version = Procedimientos.VersionSectorizacion(GestorBDCD40.ConexionMySql, id_sistema);

            System.Xml.Serialization.XmlSerializer xmlWriter = new System.Xml.Serialization.XmlSerializer(typeof(GetVersion));

            StreamWriter xmlFile = new StreamWriter(Server.MapPath("XML_GetVersionConfiguracion.xml"));
            xmlWriter.Serialize(xmlFile, v);
            xmlFile.Close();
        }

        [WebMethod(Description = "Pasándole el identificador de sistema, devuelve la versión de la configuración activa registrada en la base de datos")]
        public string GetVersionConfiguracion(string id_sistema)
        {
            //Procedimientos pr = new Procedimientos();

            return Procedimientos.VersionSectorizacion(GestorBDCD40.ConexionMySql, id_sistema);

            //lock (Sync)
            //{
            //    object data;

            //    try
            //    {
            //        System.Xml.Serialization.XmlSerializer xmlReader = new System.Xml.Serialization.XmlSerializer(typeof(GetVersion));
            //        StreamReader xmlFile = new StreamReader(Server.MapPath("XML_GetVersionConfiguracion.xml"));
            //        data = xmlReader.Deserialize(xmlFile.BaseStream);
            //        xmlFile.Close();
            //    } 
            //    catch (System.IO.FileNotFoundException e)
            //    {
            //        System.Diagnostics.Debug.Assert(false, e.Message);
            //        return null;
            //    }

            //    return ((GetVersion)data).Version;
            //}
        }

        [WebMethod(Description = "Pasándole el identificador de sistema, devuelve el Scv [1|2] activo. 0 si error")]
        public string GetScvActivo(string idSistema)
        {
            lock (Sync)
            {
                Sistema s = new Sistema();
                s.IdSistema = idSistema;

                List<Tablas> sistema = GestorBDCD40.ListSelectSQL(s, null);
                if (sistema.Count > 0)
                {
                    if (((Sistema)sistema[0]).EstadoScv1 == 1 && ((Sistema)sistema[0]).EstadoScv2 != 1)
                        return "1";
                    else if (((Sistema)sistema[0]).EstadoScv1 != 1 && ((Sistema)sistema[0]).EstadoScv2 == 1)
                        return "2";
                    else
                        return "0";
                }
                return "0";
            }
        }

        [WebMethod(Description = "Envia el estado de los nodos que componen el cluster")]
        public EstadoCluster GetEstadoCluster()
        {
            EstadoCluster estado = new EstadoCluster();

            estado.EstadoNode1.Name = estado.EstadoNode2.Name = "";

            DataSet ds = Procedimientos.GetEstadoCluster(GestorBDCD40.ConexionMySql);

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    estado.EstadoNode1.Name = (string)ds.Tables[0].Rows[0]["Name"];
                    estado.EstadoNode1.Presencia = (bool)ds.Tables[0].Rows[0]["Presencia"];
                    estado.EstadoNode1.Estado = (int)ds.Tables[0].Rows[0]["Estado"];
                }

                if (ds.Tables[0].Rows.Count == 2)
                {
                    estado.EstadoNode2.Name = (string)ds.Tables[0].Rows[1]["Name"];
                    estado.EstadoNode2.Presencia = (bool)ds.Tables[0].Rows[1]["Presencia"];
                    estado.EstadoNode2.Estado = (int)ds.Tables[0].Rows[1]["Estado"];
                }
            }

            return estado;
        }

        [WebMethod(Description = "Pasándole el identificador de usuario y el sistema al que pertenece, devuelve la lista de enlaces externos del usuario.")]
        public CfgEnlaceExterno[] GetListaEnlacesExternos(string id_sistema, string id_usuario)
        {
            lock (Sync)
            {
                if (string.IsNullOrEmpty(id_sistema) && string.IsNullOrEmpty(id_usuario))
                    return null;

                var ut = new CD40.BD.Utilidades(GestorBDCD40.ConexionMySql);
                string strUltimoEnlace = string.Empty;
                string strUltimoAlias = string.Empty;
                int numEnlaceDistinto = 0;

                // Obtener identificador de la sectorización activa
                Sectorizaciones s = new Sectorizaciones();
                s.IdSistema = id_sistema;
                s.Activa = true;
                List<Tablas> sectorizacion = GestorBDCD40.ListSelectSQL(s, null);
                if (sectorizacion.Count <= 0)
                    return null;

                //DestinosRadioSector eExternoSector = new DestinosRadioSector();
                Radio eExternoSector = new Radio();
                eExternoSector.IdSistema = id_sistema;
                eExternoSector.IdSector = id_usuario;
                // El identificador de la sectorizacion donde se tienen que obtener los datos
                // coincide con la fecha de activación de la sectorización activa
                eExternoSector.IdSectorizacion = ((Sectorizaciones)sectorizacion[0]).FechaActivacion.ToString("dd/MM/yyyy HH:mm:ss");
                //eExternoSector.IdSectorizacion = ((Sectorizaciones)sectorizacion[0]).IdSectorizacion;

                // Cuenta los enlaces radio con identificador y literal distinto de un sector que tengan configurado un recurso
                int numEnlacesExternos = ut.SelectCountDistinctEnlaceSQL(eExternoSector, true);
                if (numEnlacesExternos > 0)
                {
                    //ConfiguracionEnlaceExterno = new CfgEnlaceExterno[numEnlacesExternos];
                    ConfiguracionEnlaceExterno = null;

                    //Creamos la lista dinámica donde se almacenarán la lista de enlaces radio que posteriormente se devolverá
                    //en un array, para evitar devolver elementos con valor null
                    List<Tablas> listaEnlacesExternos = GestorBDCD40.ListSelectSQL(eExternoSector, null);

                    int iNumTotalEnlaces = listaEnlacesExternos.Count;

                    List<CfgEnlaceExterno> ObjListaEnlaces = new List<CfgEnlaceExterno>();

                    for (int i = 0; i < iNumTotalEnlaces; i++)
                    {
                        if (strUltimoEnlace != ((Radio)listaEnlacesExternos[i]).IdDestino ||
                            (strUltimoEnlace == ((Radio)listaEnlacesExternos[i]).IdDestino && strUltimoAlias != ((Radio)listaEnlacesExternos[i]).Literal))
                        {
                            // Sólo si el destino externo tiene recursos, se considerará bueno el enlace
                            // en caso contrario, se ignora el enlace
                            RecursosRadio rRadio = new RecursosRadio();
                            rRadio.IdSistema = id_sistema;
                            rRadio.IdDestino = ((Radio)listaEnlacesExternos[i]).IdDestino;
                            List<Tablas> listaRecursosEnlace = GestorBDCD40.ListSelectSQL(rRadio, null);
                            if (listaRecursosEnlace.Count > 0)
                            {
                                CfgEnlaceExterno ObjEnlace = null;

                                // Nuevo enlace del sector
                                strUltimoEnlace = ((Radio)listaEnlacesExternos[i]).IdDestino;
                                strUltimoAlias = ((Radio)listaEnlacesExternos[i]).Literal;

                                ObjEnlace = new CfgEnlaceExterno();
                                ObjEnlace.ListaPosicionesEnHmi = new List<uint>();
                                ObjEnlace.ListaRecursos = new List<CfgRecursoEnlaceExterno>();
                                ObjEnlace.DestinoAudio = new List<string>();

                                //Como literal se pone el Id del Destino
                                ObjEnlace.Literal = ((Radio)listaEnlacesExternos[i]).IdDestino;
                                ObjEnlace.AliasEnlace = ((Radio)listaEnlacesExternos[i]).Literal;

                                ObjEnlace.Prioridad = ((Radio)listaEnlacesExternos[i]).PrioridadSIP;
                                ObjEnlace.EstadoAsignacion = ((Radio)listaEnlacesExternos[i]).ModoOperacion;
                                ObjEnlace.ListaPosicionesEnHmi.Add(((Radio)listaEnlacesExternos[i]).PosHMI);
                                ObjEnlace.DestinoAudio.Add(((Radio)listaEnlacesExternos[i]).Cascos);
                                ObjEnlace.SupervisionPortadora = ((Radio)listaEnlacesExternos[i]).SupervisionPortadora;

                                EstadoAltavoces altavoz = new EstadoAltavoces();
                                altavoz.IdSistema = id_sistema;
                                altavoz.IdSectorizacion = ((Sectorizaciones)sectorizacion[0]).IdSectorizacion;
                                altavoz.IdSector = id_usuario;
                                altavoz.IdNucleo = ((Radio)listaEnlacesExternos[i]).IdNucleo;
                                altavoz.IdDestino = ((Radio)listaEnlacesExternos[i]).IdDestino;
                                List<Tablas> listaAltavoces = GestorBDCD40.ListSelectSQL(altavoz, null);
                                for (int k = 0; k < listaAltavoces.Count; k++)
                                {
                                    ObjEnlace.DestinoAudio.Add(((EstadoAltavoces)listaAltavoces[k]).Estado);
                                }

                                DestinosRadio eExterno = new DestinosRadio();
                                eExterno.IdSistema = id_sistema;
                                eExterno.IdDestino = ((Radio)listaEnlacesExternos[i]).IdDestino;

                                List<Tablas> listaEnlaces = GestorBDCD40.ListSelectSQL(eExterno, null);
                                if (listaEnlaces.Count > 0)
                                {
                                    // Convertir los valores de tipo de frecuencia a los que espera encontrar Nbx:
                                    // HF: 1; VHF: 2; UHF: 3
                                    switch (((DestinosRadio)listaEnlaces[0]).TipoFrec)
                                    {
                                        case 0: // Destino tipo VHF => Recurso tipo VHF
                                            ObjEnlace.TipoFrecuencia = 2;
                                            break;
                                        case 1: // Destino tipo UHF => Recurso tipo UHF
                                            ObjEnlace.TipoFrecuencia = 3;
                                            break;
                                        case 2: // Destino tipo HF => Recurso tipo HF
                                            ObjEnlace.TipoFrecuencia = 1;
                                            break;
                                        default:
                                            ObjEnlace.TipoFrecuencia = ((DestinosRadio)listaEnlaces[0]).TipoFrec;
                                            break;
                                    }
                                    //ObjEnlace.TipoFrecuencia = ((DestinosRadio)listaEnlaces[0]).TipoFrec;
                                    ObjEnlace.ExclusividadTxRx = ((DestinosRadio)listaEnlaces[0]).ExclusividadTXRX;
                                    ObjEnlace.FrecuenciaSintonizada = ((DestinosRadio)listaEnlaces[0]).Frecuencia;

                                    ObjEnlace.MetodoCalculoClimax = ((DestinosRadio)listaEnlaces[0]).MetodoCalculoClimax;
                                    ObjEnlace.VentanaSeleccionBss = ((DestinosRadio)listaEnlaces[0]).VentanaSeleccionBss;
                                    ObjEnlace.SincronizaGrupoClimax = ((DestinosRadio)listaEnlaces[0]).SincronizaGrupoClimax;
                                    ObjEnlace.AudioPrimerSqBss = ((DestinosRadio)listaEnlaces[0]).AudioPrimerSqBss;
                                    ObjEnlace.FrecuenciaNoDesasignable = ((DestinosRadio)listaEnlaces[0]).FrecuenciaNoDesasignable;
                                    ObjEnlace.VentanaReposoZonaTxDefecto = ((DestinosRadio)listaEnlaces[0]).VentanaReposoZonaTxDefecto;
                                    ObjEnlace.NombreZonaTxDefecto = ((DestinosRadio)listaEnlaces[0]).NombreZonaTxDefecto;
                                    //ObjEnlace.MetodosBss = ((DestinosRadio)listaEnlaces[0]).MetodosBss;
                                    ObjEnlace.PrioridadSesionSIP = ((DestinosRadio)listaEnlaces[0]).PrioridadSesionSip;
                                    ObjEnlace.CldSupervisionTime = ((DestinosRadio)listaEnlaces[0]).CldSupervisionTime;
                                    ObjEnlace.MetodosBssOfrecidos = ((DestinosRadio)listaEnlaces[0]).MetodosBssOfrecidos;
                                    ObjEnlace.ModoTransmision = ((DestinosRadio)listaEnlaces[0]).ModoTransmision;
                                    //VMG 18/02/2019
                                    ObjEnlace.EmplazamientoDefecto = ((DestinosRadio)listaEnlaces[0]).EmplazamientoDefecto;
                                    ObjEnlace.TiempoVueltaADefecto = ((DestinosRadio)listaEnlaces[0]).TiempoVueltaADefecto;
                                    //Se pone un 
                                    ObjEnlace.PorcentajeRSSI = "0";
                                }

                                for (int j = 0; j < listaRecursosEnlace.Count; j++)
                                {
                                    CfgRecursoEnlaceExterno cfgRecurso = new CfgRecursoEnlaceExterno();
                                    cfgRecurso.IdRecurso = ((RecursosRadio)listaRecursosEnlace[j]).IdRecurso;
                                    Recursos r = new Recursos();
                                    r.IdSistema = id_sistema;
                                    r.IdRecurso = cfgRecurso.IdRecurso;
                                    List<Tablas> tRecurso = GestorBDCD40.ListSelectSQL(r, null);
                                    if (tRecurso.Count > 0)
                                        // Recurso M+N RX, M+N Tx o M+N RTx
                                        cfgRecurso.Tipo = (((Recursos)tRecurso[0]).Tipo > 3 && ((Recursos)tRecurso[0]).Tipo < 7) ? ((Recursos)tRecurso[0]).Tipo % 4 : ((Recursos)tRecurso[0]).Tipo;
                                    cfgRecurso.ModoConfPTT = ((RecursosRadio)listaRecursosEnlace[j]).ModoConfPTT;
                                    cfgRecurso.NumFlujosAudio = ((RecursosRadio)listaRecursosEnlace[j]).NumFlujosAudio;
                                    cfgRecurso.IdEmplazamiento = ((RecursosRadio)listaRecursosEnlace[j]).IdEmplazamiento;

                                    cfgRecurso.NombreZona = ((RecursosRadio)listaRecursosEnlace[j]).NombreZona;
                                    cfgRecurso.NameTablaBss = ((RecursosRadio)listaRecursosEnlace[j]).NameTablaBss;
                                    cfgRecurso.MetodoBss = ((RecursosRadio)listaRecursosEnlace[j]).MetodoBSS == "RSSI" ? 0 : 1;
                                    cfgRecurso.IdMetodoBss = ((RecursosRadio)listaRecursosEnlace[j]).MetodoBSS;

                                    cfgRecurso.GrsDelay = ((RecursosRadio)listaRecursosEnlace[j]).GrsDelay;
                                    //cfgRecurso.CldSupervisionTime = ((RecursosRadio)listaRecursosEnlace[j]).CldSupervisionTime;
                                    cfgRecurso.OffSetFrequency = ((RecursosRadio)listaRecursosEnlace[j]).OffSetFrequency;
                                    cfgRecurso.EnableEventPttSq = ((RecursosRadio)listaRecursosEnlace[j]).EnableEventPttSq;
                                    cfgRecurso.RedundanciaRol = ((RecursosRadio)listaRecursosEnlace[j]).RedundanciaRol;
                                    cfgRecurso.RedundanciaIdPareja = ((RecursosRadio)listaRecursosEnlace[j]).RedundanciaIdPareja;
                                    cfgRecurso.Estado = GetEstadoDelRecurso(((Sectorizaciones)sectorizacion[0]).IdSectorizacion, r, (Radio)listaEnlacesExternos[i]);// "S"; 

                                    // Obtener los valores de la tabla de calificación de audio
                                    cfgRecurso.ValuesTablaBss = GetValueTablaBss(cfgRecurso.NameTablaBss);

                                    ObjEnlace.ListaRecursos.Add(cfgRecurso);
                                }

                                //Añadimos el enlace radio a la lista
                                ObjListaEnlaces.Add(ObjEnlace);

                                numEnlaceDistinto++;
                            }
                        }
                        else
                        {
                            //El destino radio tiene el mismo identificador y literal que el destino anterior, por lo que se añade la posición a la lista de posiciones HMI del ultimo enlace de la lista
                            ObjListaEnlaces[numEnlaceDistinto - 1].ListaPosicionesEnHmi.Add(((Radio)listaEnlacesExternos[i]).PosHMI);
                            ObjListaEnlaces[numEnlaceDistinto - 1].SupervisionPortadora |= ((Radio)listaEnlacesExternos[i]).SupervisionPortadora;
                        }
                    }

                    //Se pasa la lista a un Array para devolver el valor
                    if (ObjListaEnlaces.Count > 0)
                        ConfiguracionEnlaceExterno = ObjListaEnlaces.ToArray();

                    //Se libera la memoria asignada a la lista de enlaces
                    ObjListaEnlaces.Clear();

                    return ConfiguracionEnlaceExterno;
                }


                return null;
            }
        }

        [WebMethod(Description = "Pasándole el identificador de usuario y el sistema al que pertenece, devuelve la lista de enlaces internos del usuario.")]
        public CfgEnlaceInterno[] GetListaEnlacesInternos(string id_sistema, string id_usuario)
        {
            /*
            EnlacesInternosSector eInterno = new EnlacesInternosSector();
            int numEnlacesInternos = GestorBDCD40.SelectCountSQL(eInterno, "IdSistema='" + id_sistema + "' AND IdSector='" + id_usuario + "'");
            EnlacesLCENSector eLcen = new EnlacesLCENSector();
            numEnlacesInternos += GestorBDCD40.SelectCountSQL(eLcen, "IdSistema='" + id_sistema + "' AND IdSector='" + id_usuario + "'");
            */
            // Obtener identificador de la sectorización activa
            lock (Sync)
            {
                Sectorizaciones s = new Sectorizaciones();
                s.IdSistema = id_sistema;
                s.Activa = true;
                List<Tablas> sectorizacion = GestorBDCD40.ListSelectSQL(s, null);

                if (sectorizacion.Count <= 0)
                    return null;

                // Enlaces a usuarios internos
                //DestinosInternosSector eInterno = new DestinosInternosSector();
                Internos eInterno = new Internos();
                eInterno.IdSistema = id_sistema;
                eInterno.IdSector = id_usuario;
                // El identificador de la sectorizacion donde se tienen que obtener los datos
                // coincide con la fecha de activación de la sectorización activa
                eInterno.IdSectorizacion = ((Sectorizaciones)sectorizacion[0]).FechaActivacion.ToString("dd/MM/yyyy HH:mm:ss");
                //eInterno.IdSectorizacion = ((Sectorizaciones)sectorizacion[0]).IdSectorizacion;

                List<Tablas> listaEnlacesInternos = GestorBDCD40.ListSelectSQL(eInterno, null);

                // Enlaces a usuarios externos
                //DestinosExternosSector eExterno = new DestinosExternosSector();
                Externos eExterno = new Externos();
                eExterno.IdSistema = id_sistema;
                eExterno.IdSector = id_usuario;
                // El identificador de la sectorizacion donde se tienen que obtener los datos
                // coincide con la fecha de activación de la sectorización activa
                eExterno.IdSectorizacion = ((Sectorizaciones)sectorizacion[0]).FechaActivacion.ToString("dd/MM/yyyy HH:mm:ss");
                //eExterno.IdSectorizacion = ((Sectorizaciones)sectorizacion[0]).IdSectorizacion;
                List<Tablas> listaEnlacesExternos = GestorBDCD40.ListSelectSQL(eExterno, null);

                int iNumEnlacesExternosSector = 0;
                int numEnlacesSector = 0;

                //Obtenemos el número de enlaces externos
                for (int i = 0; i < listaEnlacesExternos.Count; i++)
                {
                    if (((Externos)listaEnlacesExternos[i]).IdPrefijo == 1 && (((Externos)listaEnlacesExternos[i]).TipoAcceso == "IA"))
                    {
                        //En el panel de LC, sólo puede haber destinos LCEN asociados a Destinos ATS, por lo que no se muestran como teclas.
                        //Estos enlaces se obtendrán dentro de la lista de recursos del destino ATS que tenga el mismo grupo configurado en externos.
                        continue;
                    }
                    else
                        iNumEnlacesExternosSector++;

                }

                numEnlacesSector = listaEnlacesInternos.Count + iNumEnlacesExternosSector;


                if (numEnlacesSector > 0)
                {
                    ConfiguracionEnlaceInterno = new CfgEnlaceInterno[numEnlacesSector];

                    #region Enlaces a usuarios internos
                    for (int i = 0; i < listaEnlacesInternos.Count; i++)
                    {
                        ConfiguracionEnlaceInterno[i] = new CfgEnlaceInterno();
                        ConfiguracionEnlaceInterno[i].Literal = ((Internos)listaEnlacesInternos[i]).Literal;
                        ConfiguracionEnlaceInterno[i].PosicionHMI = ((Internos)listaEnlacesInternos[i]).PosHMI;
                        ConfiguracionEnlaceInterno[i].TipoEnlaceInterno = ((Internos)listaEnlacesInternos[i]).TipoAcceso;
                        ConfiguracionEnlaceInterno[i].Prioridad = ((Internos)listaEnlacesInternos[i]).PrioridadSIP;
                        ConfiguracionEnlaceInterno[i].OrigenR2 = ((Internos)listaEnlacesInternos[i]).OrigenR2;
                        //Por defecto, se inicializa a cadena vacía, para que aparezca en la interfaz
                        ConfiguracionEnlaceInterno[i].Dominio = "";

                        DestinosTelefonia interno = new DestinosTelefonia();
                        interno.IdSistema = id_sistema;
                        interno.IdDestino = ((Internos)listaEnlacesInternos[i]).IdDestino;
                        List<Tablas> listaInternos = GestorBDCD40.ListSelectSQL(interno, null);
                        if (listaInternos.Count > 0)
                        {
                            ConfiguracionEnlaceInterno[i].Dependencia = ((DestinosTelefonia)listaInternos[0]).IdGrupo;
                        }


                        #region Recurso para el destino
                        ConfiguracionEnlaceInterno[i].ListaRecursos = new List<CfgRecursoEnlaceInternoConInterface>();

                        CfgRecursoEnlaceInternoConInterface cfgRecurso = new CfgRecursoEnlaceInternoConInterface();

                        cfgRecurso.NombreRecurso = ((Internos)listaEnlacesInternos[i]).IdDestino;
                        cfgRecurso.Prefijo = ((Internos)listaEnlacesInternos[i]).IdPrefijo;

                        ConfiguracionEnlaceInterno[i].ListaRecursos.Add(cfgRecurso);

                        /*
                        DestinosInternos dInterno = new DestinosInternos();
                        dInterno.IdSistema = id_sistema;
                        dInterno.IdDestino = ((DestinosInternosSector)listaEnlacesInternos[i]).IdDestino;
                        dInterno.IdPrefijo = ((DestinosInternosSector)listaEnlacesInternos[i]).IdPrefijo;
                        Tablas[] destino = GestorBDCD40.ListSelectSQL(dInterno);
                        if (destino.Length > 0)
                        {
                            cfgRecurso.NombreRecurso = ((DestinosInternos)destino[0]).IdSector;
                            cfgRecurso.Prefijo = ((DestinosInternosSector)listaEnlacesInternos[i]).IdPrefijo;
                            ConfiguracionEnlaceInterno[i].ListaRecursos.Add(cfgRecurso);
                        }*/
                        #endregion
                    }
                    #endregion

                    #region Enlaces a usuarios externos

                    uint iPrefijo = 0;
                    int pos = listaEnlacesInternos.Count;

                    //Se recorre la lista de enlaces externos para añadirlos
                    for (int j = 0; j < listaEnlacesExternos.Count && pos < ConfiguracionEnlaceInterno.Length; j++)
                    {
                        //int pos = Array.FindIndex(ConfiguracionEnlaceInterno, 
                        //                delegate(CfgEnlaceInterno obj) { return ((obj != null) &&
                        //                                                         (obj.TipoEnlaceInterno == ((Externos)listaEnlacesExternos[j]).TipoAcceso) &&  
                        //                                                         (obj.PosicionHMI == ((Externos)listaEnlacesExternos[j]).PosHMI)); 
                        //                                                });

                        if (((Externos)listaEnlacesExternos[j]).IdPrefijo == 1 && (((Externos)listaEnlacesExternos[j]).TipoAcceso == "IA"))
                        {
                            //Si es un enlace Externo de tipo LCE del panel de Línea Caliente no se trata porque este enlace
                            //estará asociado a algún destino ATS del panel
                            continue;
                        }

                        iPrefijo = ((Externos)listaEnlacesExternos[j]).IdPrefijo;

                        ConfiguracionEnlaceInterno[pos] = new CfgEnlaceInterno();
                        ConfiguracionEnlaceInterno[pos].Literal = ((Externos)listaEnlacesExternos[j]).Literal;
                        ConfiguracionEnlaceInterno[pos].TipoEnlaceInterno = ((Externos)listaEnlacesExternos[j]).TipoAcceso;
                        ConfiguracionEnlaceInterno[pos].PosicionHMI = ((Externos)listaEnlacesExternos[j]).PosHMI;
                        ConfiguracionEnlaceInterno[pos].Prioridad = ((Externos)listaEnlacesExternos[j]).PrioridadSIP;
                        ConfiguracionEnlaceInterno[pos].OrigenR2 = ((Externos)listaEnlacesExternos[j]).OrigenR2;

                        //Si no se trata de un destino ATS del panel de línea caliente  (idPrefijo=3 TipoEnlaceInterno='IA')
                        if ((ConfiguracionEnlaceInterno[pos].TipoEnlaceInterno == "IA" && iPrefijo == 3) == false)
                        {
                            if (((Externos)listaEnlacesExternos[j]).Grupo != string.Empty && ((Externos)listaEnlacesExternos[j]).Literal == ((Externos)listaEnlacesExternos[j]).Grupo)
                            {
                                if (GetGrupo(id_sistema, ((Externos)listaEnlacesExternos[j]).Grupo))
                                    ConfiguracionEnlaceInterno[pos].Dependencia = ((Externos)listaEnlacesExternos[j]).Grupo;
                            }
                        }
                        /*
                        List<Tablas> listaInternos = GestorBDCD40.ListSelectSQL(interno, null);
                        if (listaInternos.Count > 0)
                        {
                            ConfiguracionEnlaceInterno[i].Dependencia = ((DestinosTelefonia)listaInternos[0]).IdGrupo;
                        }
                        */

                        #region Lista de recursos para el destino
                        ConfiguracionEnlaceInterno[pos].ListaRecursos = new List<CfgRecursoEnlaceInternoConInterface>();

                        if (iPrefijo == 32)  // PP
                        {
                            DataSet listaRecurso = Procedimientos.ListaRecursosDestino(GestorBDCD40.ConexionMySql,
                                                                                        id_sistema,
                                                                                        ((Externos)listaEnlacesExternos[j]).IdDestino,
                                                                                        (int)((Externos)listaEnlacesExternos[j]).IdPrefijo,
                                                                                        ConfiguracionEnlaceInterno[pos].Dependencia);
                            if (listaRecurso != null && listaRecurso.Tables.Count > 0)
                            {
                                for (int numRecurso = 0; numRecurso < listaRecurso.Tables[0].Rows.Count; numRecurso++)
                                {
                                    CfgRecursoEnlaceInternoConInterface cfgRecursoInterno = new CfgRecursoEnlaceInternoConInterface();

                                    cfgRecursoInterno.NombreRecurso = (string)listaRecurso.Tables[0].Rows[numRecurso]["IdRecurso"];

                                    //Por defecto, se inicializa a cadena vacía, para que aparezca en la interfaz
                                    cfgRecursoInterno.NombreMostrar = "";

                                    cfgRecursoInterno.Interface = Tipos.TipoInterface.TI_BC; //Pueder ser BL

                                    cfgRecursoInterno.Interface = (Tipos.TipoInterface)(uint)listaRecurso.Tables[0].Rows[numRecurso]["Interface"];
                                    if (cfgRecursoInterno.Interface == Tipos.TipoInterface.TI_EM_PP &&
                                        listaRecurso.Tables[0].Rows[numRecurso]["Interface"] != System.DBNull.Value &&
                                        (string)listaRecurso.Tables[0].Rows[numRecurso]["Modo"] == "4W")
                                        // Sólo los recursos EyM 4W van a deshabilitar el cancelador de eco en el puesto
                                        // los recursos EyM 2W se comportan como un PaP normal
                                        cfgRecursoInterno.Prefijo = (uint)Tipos.TipoInterface.TI_EM_PP;
                                    else
                                        cfgRecursoInterno.Prefijo = ((Externos)listaEnlacesExternos[j]).IdPrefijo;

                                    // En principio, los destinos PP o LCEN no deben tener número de abonado, pero... los PP Abonado de Namibia??
                                    DestinosExternos dExterno = new DestinosExternos();
                                    dExterno.IdSistema = id_sistema;
                                    dExterno.IdDestino = ((Externos)listaEnlacesExternos[j]).IdDestino;
                                    dExterno.IdPrefijo = ((Externos)listaEnlacesExternos[j]).IdPrefijo;

                                    List<Tablas> destino = GestorBDCD40.ListSelectSQL(dExterno, null);
                                    if (destino.Count > 0)
                                        cfgRecursoInterno.NumeroAbonado = ((DestinosExternos)destino[0]).IdAbonado;

                                    ConfiguracionEnlaceInterno[pos].ListaRecursos.Add(cfgRecursoInterno);
                                }
                            }
                        }
                        else if (iPrefijo == 3 && (((Externos)listaEnlacesExternos[j]).TipoAcceso == "IA"))
                        {
                            //Destinos ATS de telefonía del panel de línea Caliente (Destinos ATS (prefijo=3) y tipoAcceso=IA) 
                            // Se comprueba si el destino tiene un número de abonado asociado.
                            // Se comprueba si el destino ATS tiene asociados destinos LCEN y si los tiene se recuperan los recursos asociados

                            //Numero de abonado del destino ATS
                            CfgRecursoEnlaceInternoConInterface cfgRecursoInterno = new CfgRecursoEnlaceInternoConInterface();

                            cfgRecursoInterno.Prefijo = iPrefijo;

                            DestinosExternos dExterno = new DestinosExternos();
                            dExterno.IdSistema = id_sistema;
                            dExterno.IdDestino = ((Externos)listaEnlacesExternos[j]).IdDestino;
                            dExterno.IdPrefijo = iPrefijo;

                            List<Tablas> destino = GestorBDCD40.ListSelectSQL(dExterno, null);
                            if (null != destino && destino.Count > 0)
                                cfgRecursoInterno.NumeroAbonado = ((DestinosExternos)destino[0]).IdAbonado;

                            ConfiguracionEnlaceInterno[pos].ListaRecursos.Add(cfgRecursoInterno);

                            //El destino ATS puede tener asociados un conjunto de recursos LCEN, y la asociacion se realiza a través del grupo
                            //El nombre del grupo se crea dinámicamente en la sectorización y no existe en la tabla grupos GruposTelefonia
                            if (!string.IsNullOrEmpty((((Externos)listaEnlacesExternos[j]).Grupo)))
                            {
                                //Se obtiene la lista de Recursos correspondientes a los destinos de LC  asociados al Destino ATS
                                DataSet listaRecurso = Procedimientos.ListaRecursosLCEN_DestinoATS(GestorBDCD40.ConexionMySql,
                                                                                            id_sistema,
                                                                                            ((Externos)listaEnlacesExternos[j]).IdSectorizacion,
                                                                                            ((Externos)listaEnlacesExternos[j]).IdNucleo,
                                                                                            ((Externos)listaEnlacesExternos[j]).IdSector,
                                                                                            ((Externos)listaEnlacesExternos[j]).Grupo);

                                //No se informa el grupo en la dependencia para que el HMI lo trate correctamente.
                                //ConfiguracionEnlaceInterno[pos].Dependencia = ((Externos)listaEnlacesExternos[j]).Grupo;
                                ConfiguracionEnlaceInterno[pos].Dependencia = string.Empty;

                                if (listaRecurso != null && listaRecurso.Tables.Count > 0)
                                {
                                    for (int numRecurso = 0; numRecurso < listaRecurso.Tables[0].Rows.Count; numRecurso++)
                                    {
                                        cfgRecursoInterno = new CfgRecursoEnlaceInternoConInterface();

                                        cfgRecursoInterno.NombreRecurso = (string)listaRecurso.Tables[0].Rows[numRecurso]["IdRecurso"];
                                        //Por defecto, se inicializa a cadena vacía, para que aparezca en la interfaz
                                        cfgRecursoInterno.NombreMostrar = "";

                                        cfgRecursoInterno.Interface = (Tipos.TipoInterface)(uint)listaRecurso.Tables[0].Rows[numRecurso]["Interface"];
                                        if (cfgRecursoInterno.Interface == Tipos.TipoInterface.TI_EM_PP &&
                                            listaRecurso.Tables[0].Rows[numRecurso]["Interface"] != System.DBNull.Value &&
                                            (string)listaRecurso.Tables[0].Rows[numRecurso]["Modo"] == "4W")
                                            // Sólo los recursos EyM 4W van a deshabilitar el cancelador de eco en el puesto
                                            // los recursos EyM 2W se comportan como un PaP normal
                                            cfgRecursoInterno.Prefijo = (uint)Tipos.TipoInterface.TI_EM_PP;
                                        else
                                        {
                                            //Tipo recurso LCEN
                                            cfgRecursoInterno.Prefijo = 1;
                                        }

                                        // En principio, los destinos PP o LCEN no deben tener número de abonado, pero... los PP Abonado de Namibia??
                                        dExterno = new DestinosExternos();
                                        dExterno.IdSistema = id_sistema;
                                        dExterno.IdDestino = ((Externos)listaEnlacesExternos[j]).IdDestino;
                                        dExterno.IdPrefijo = ((Externos)listaEnlacesExternos[j]).IdPrefijo;

                                        if (null != destino)
                                            destino.Clear();

                                        destino = GestorBDCD40.ListSelectSQL(dExterno, null);

                                        if (null != destino && destino.Count > 0)
                                            cfgRecursoInterno.NumeroAbonado = ((DestinosExternos)destino[0]).IdAbonado;

                                        ConfiguracionEnlaceInterno[pos].ListaRecursos.Add(cfgRecursoInterno);
                                    }
                                }
                            }
                        }
                        else // RED
                        {
                            // Los destinos de redes, no llevan recursos, pero sí pueden llevar número de abonado
                            CfgRecursoEnlaceInternoConInterface cfgRecursoInterno = new CfgRecursoEnlaceInternoConInterface();

                            cfgRecursoInterno.Prefijo = ((Externos)listaEnlacesExternos[j]).IdPrefijo;

                            //Por defecto, se inicializa a cadena vacía, para que aparezca en la interfaz
                            cfgRecursoInterno.NombreMostrar = "";


                            if (((Externos)listaEnlacesExternos[j]).TipoAcceso == "DA")
                            {
                                switch (cfgRecursoInterno.Prefijo)
                                {
                                    case 2:
                                        cfgRecursoInterno.Interface = Tipos.TipoInterface.TI_Radio;
                                        //Calcular el OrigenR2
                                        break;
                                    case 3:
                                        //En teoria solo debe de haber uno 
                                        cfgRecursoInterno.Interface = Tipos.TipoInterface.TI_ATS_R2;
                                        break;
                                    case 4:
                                        cfgRecursoInterno.Interface = Tipos.TipoInterface.TI_AB;
                                        break;
                                    case 7:
                                        //En teoria solo debe de haber uno
                                        cfgRecursoInterno.Interface = Tipos.TipoInterface.TI_ATS_QSIG;
                                        break;

                                    default:
                                        cfgRecursoInterno.Interface = Tipos.TipoInterface.TI_Radio;
                                        break;
                                }

                                //Enlaces externos de telefonía (tipoAcceso=DA)
                                // Ver si el destino tiene un número de abonado asociado.
                                DestinosExternos dExterno = new DestinosExternos();
                                dExterno.IdSistema = id_sistema;
                                dExterno.IdDestino = ((Externos)listaEnlacesExternos[j]).IdDestino;
                                dExterno.IdPrefijo = ((Externos)listaEnlacesExternos[j]).IdPrefijo;

                                List<Tablas> destino = GestorBDCD40.ListSelectSQL(dExterno, null);
                                if (destino.Count > 0)
                                    cfgRecursoInterno.NumeroAbonado = ((DestinosExternos)destino[0]).IdAbonado;

                                ConfiguracionEnlaceInterno[pos].ListaRecursos.Add(cfgRecursoInterno);
                            }
                            else
                            {
                                // AG
                                Agenda ag = new Agenda();
                                ag.IdSistema = id_sistema;
                                ag.IdSector = ((Externos)listaEnlacesExternos[j]).OrigenR2;
                                ag.IdNucleo = ((Externos)listaEnlacesExternos[j]).IdNucleo;
                                ag.Prefijo = ((Externos)listaEnlacesExternos[j]).IdPrefijo;
                                ag.Nombre = ((Externos)listaEnlacesExternos[j]).IdDestino;
                                List<Tablas> destino = GestorBDCD40.ListSelectSQL(ag, null);
                                if (destino.Count > 0)
                                    cfgRecursoInterno.NumeroAbonado = ((Agenda)destino[0]).Numero;

                                ConfiguracionEnlaceInterno[pos].ListaRecursos.Add(cfgRecursoInterno);
                            }
                        }

                        pos++;

                        #endregion
                    }
                    #endregion

                    return ConfiguracionEnlaceInterno;
                }

                return null;
            }
        }

        [WebMethod(Description = "Pasándole el identificador hardware del terminal de voz y el sistema al que pertenece, " +
                                "devuelve el identificador de usuario alojado en el mismo y el modo de arranque ('A' o 'M').")]
        public LoginTerminalVoz LoginTop(string id_sistema, string id_hw)
        {
            lock (Sync)
            {
                LoginTerminalVoz loginTop = new LoginTerminalVoz();
                //Procedimientos pr = new Procedimientos();
                string[] parametros = Procedimientos.LoginTop(GestorBDCD40.ConexionMySql, id_sistema, id_hw);
                loginTop.IdUsuario = parametros[0];
                loginTop.ModoLogin = parametros[1];

                return loginTop;
            }
        }

        [WebMethod(Description = "Pasándole el identificador de usuario y el sistema al que pertenece, devuelve la configuración del usuario.")]
        public ConfiguracionElementosHw.CfgUsuario GetCfgUsuario(string id_sistema, string id_usuario)
        {
            lock (Sync)
            {
                var ut = new CD40.BD.Utilidades(GestorBDCD40.ConexionMySql);
                //Procedimientos pr = new Procedimientos();
                #region Lista Número de Abonados
                DataSet listaNumerosInternos = Procedimientos.NumerosAbonadoInternos(GestorBDCD40.ConexionMySql, id_sistema, id_usuario);
                foreach (System.Data.DataRow ds in listaNumerosInternos.Tables[0].Rows)
                {
                    NumerosAbonado abExternos = new NumerosAbonado();
                    abExternos.Numero = (string)ds["IdAbonado"];
                    abExternos.Prefijo = Convert.ToUInt32(ds["IdPrefijo"]);

                    ConfiguracionUsuario.ListaAbonados.Add(abExternos);
                }

                #endregion

                #region Numero enlaces
                // Obtener identificador de la sectorización activa
                Sectorizaciones sc = new Sectorizaciones();
                sc.IdSistema = id_sistema;
                sc.Activa = true;
                List<Tablas> sectorizacion = GestorBDCD40.ListSelectSQL(sc, null);
                if (sectorizacion.Count <= 0)
                    return null;

                Internos di = new Internos();
                Externos de = new Externos();
                Radio dr = new Radio();

                dr.IdSistema = de.IdSistema = di.IdSistema = id_sistema;
                dr.IdSector = de.IdSector = di.IdSector = id_usuario;
                dr.IdSectorizacion = de.IdSectorizacion = di.IdSectorizacion = ((Sectorizaciones)sectorizacion[0]).FechaActivacion.ToString("dd/MM/yyyy HH:mm:ss");
                //dr.IdSectorizacion = ((Sectorizaciones)sectorizacion[0]).IdSectorizacion;

                /*
                 * MODIFICIÓN HECHA A RAIZ DE CAMBIAR ACCESOABASEDEDATOS POR GESTORBASEDEDATOS *
                ConfiguracionUsuario.NumeroEnlacesInternos = GestorBDCD40.SelectCountSQL(di, "idSector='" + id_usuario + "' AND idSistema='" + id_sistema + "' AND IdSectorizacion='" + dr.IdSectorizacion + "'");
                ConfiguracionUsuario.NumeroEnlacesInternos += GestorBDCD40.SelectCountSQL(de, "idSector='" + id_usuario + "' AND idSistema='" + id_sistema + "' AND IdSectorizacion='" + dr.IdSectorizacion + "'");
                ConfiguracionUsuario.NumeroEnlacesExternos = ut.SelectCountDistinctEnlaceSQL(dr);

                 */
                ConfiguracionUsuario.NumeroEnlacesInternos = GestorBDCD40.DataSetSelectSQL(di, null).Tables[0].Rows.Count;  // (di, "idSector='" + id_usuario + "' AND idSistema='" + id_sistema + "' AND IdSectorizacion='" + dr.IdSectorizacion + "'");
                ConfiguracionUsuario.NumeroEnlacesInternos += GestorBDCD40.DataSetSelectSQL(de, null).Tables[0].Rows.Count; //SelectCountSQL(de, "idSector='" + id_usuario + "' AND idSistema='" + id_sistema + "' AND IdSectorizacion='" + dr.IdSectorizacion + "'");
                ConfiguracionUsuario.NumeroEnlacesExternos = ut.SelectCountDistinctEnlaceSQL(dr, false);

                #endregion

                #region Sector
                Sectores s = new Sectores();
                s.IdSistema = id_sistema;
                s.IdSector = id_usuario;
                s.SectorSimple = true;

                List<Tablas> listaSelect = GestorBDCD40.ListSelectSQL(s, null);
                if (listaSelect.Count > 0)
                    ConfiguracionUsuario.Sector = (SectoresSCV)(listaSelect[0]);
                #endregion

                #region ParametrosSector
                ParametrosSector pSector = new ParametrosSector();
                pSector.IdSistema = id_sistema;
                pSector.IdSector = id_usuario;
                listaSelect = GestorBDCD40.ListSelectSQL(pSector, null);

                if (listaSelect.Count > 0)
                {
                    ConfiguracionUsuario.ParametrosDelSector = (ParametrosSectorSCVKeepAlive)listaSelect[0];
                }

                #endregion

                #region PermisosRedes

                DataSet dsPermisos = Procedimientos.PermisosDeRedes(GestorBDCD40.ConexionMySql, id_sistema, id_usuario);
                if (dsPermisos.Tables.Count > 0)
                    ConfiguracionUsuario.PermisosRedDelSector = new PermisosRedesSCV[dsPermisos.Tables[0].Rows.Count];

                for (ushort num = 0; num < dsPermisos.Tables[0].Rows.Count; num++)
                {
                    ConfiguracionUsuario.PermisosRedDelSector[num] = new PermisosRedesSCV();
                    ConfiguracionUsuario.PermisosRedDelSector[num].IdRed = dsPermisos.Tables[0].Rows[num]["IdRed"].ToString();
                    ConfiguracionUsuario.PermisosRedDelSector[num].Llamar = dsPermisos.Tables[0].Rows[num]["Llamar"] != System.DBNull.Value ? ((string)dsPermisos.Tables[0].Rows[num]["Llamar"] == "true") : false;
                    ConfiguracionUsuario.PermisosRedDelSector[num].Recibir = dsPermisos.Tables[0].Rows[num]["Recibir"] != System.DBNull.Value ? ((string)dsPermisos.Tables[0].Rows[num]["Recibir"] == "true") : false;
                }
                #endregion

                #region TeclasSector
                TeclasSector teclasSector = new TeclasSector();

                DataSet teclas = Procedimientos.TeclasSector(GestorBDCD40.ConexionMySql, id_sistema, id_usuario);
                //listaSelect=GestorBDCD40.ListSelectSQL(tSector);
                if (teclas.Tables[0].Rows.Count > 0)
                {
                    DataRow teclaRow = teclas.Tables[0].Rows[0];

                    teclasSector.IdSector = (string)teclaRow["IdSector"];
                    teclasSector.IdSistema = (string)teclaRow["IdSistema"];
                    teclasSector.IdNucleo = (string)teclaRow["IdNucleo"];
                    if (teclaRow["TransConConsultaPrev"] != System.DBNull.Value)
                        teclasSector.TransConConsultaPrev = (bool)teclaRow["TransConConsultaPrev"];
                    if (teclaRow["TransDirecta"] != System.DBNull.Value)
                        teclasSector.TransDirecta = (bool)teclaRow["TransDirecta"];
                    if (teclaRow["Conferencia"] != System.DBNull.Value)
                        teclasSector.Conferencia = (bool)teclaRow["Conferencia"];
                    if (teclaRow["Escucha"] != System.DBNull.Value)
                        teclasSector.Escucha = (bool)teclaRow["Escucha"];
                    if (teclaRow["Retener"] != System.DBNull.Value)
                        teclasSector.Retener = (bool)teclaRow["Retener"];
                    if (teclaRow["Captura"] != System.DBNull.Value)
                        teclasSector.Captura = (bool)teclaRow["Captura"];
                    if (teclaRow["Redireccion"] != System.DBNull.Value)
                        teclasSector.Redireccion = (bool)teclaRow["Redireccion"];
                    if (teclaRow["RepeticionUltLlamada"] != System.DBNull.Value)
                        teclasSector.RepeticionUltLlamada = (bool)teclaRow["RepeticionUltLlamada"];
                    if (teclaRow["RellamadaAut"] != System.DBNull.Value)
                        teclasSector.RellamadaAut = (bool)teclaRow["RellamadaAut"];
                    if (teclaRow["TeclaPrioridad"] != System.DBNull.Value)
                        teclasSector.TeclaPrioridad = (bool)teclaRow["TeclaPrioridad"];
                    if (teclaRow["Tecla55mas1"] != System.DBNull.Value)
                        teclasSector.Tecla55mas1 = (bool)teclaRow["Tecla55mas1"];
                    if (teclaRow["Monitoring"] != System.DBNull.Value)
                        teclasSector.Monitoring = (bool)teclaRow["Monitoring"];
                    if (teclaRow["CoordinadorTF"] != System.DBNull.Value)
                        teclasSector.CoordinadorTF = (bool)teclaRow["CoordinadorTF"];
                    if (teclaRow["CoordinadorRD"] != System.DBNull.Value)
                        teclasSector.CoordinadorRD = (bool)teclaRow["CoordinadorRD"];
                    if (teclaRow["IntegracionRDTF"] != System.DBNull.Value)
                        teclasSector.IntegracionRDTF = (bool)teclaRow["IntegracionRDTF"];
                    if (teclaRow["LlamadaSelectiva"] != System.DBNull.Value)
                        teclasSector.LlamadaSelectiva = (bool)teclaRow["LlamadaSelectiva"];
                    if (teclaRow["GrupoBSS"] != System.DBNull.Value)
                        teclasSector.GrupoBSS = (bool)teclaRow["GrupoBSS"];
                    if (teclaRow["LTT"] != System.DBNull.Value)
                        teclasSector.LTT = (bool)teclaRow["LTT"];
                    if (teclaRow["SayAgain"] != System.DBNull.Value)
                        teclasSector.SayAgain = (bool)teclaRow["SayAgain"];
                    if (teclaRow["InhabilitacionRedirec"] != System.DBNull.Value)
                        teclasSector.InhabilitacionRedirec = (bool)teclaRow["InhabilitacionRedirec"];
                    if (teclaRow["Glp"] != System.DBNull.Value)
                        teclasSector.Glp = (bool)teclaRow["Glp"];
                }
                ConfiguracionUsuario.TeclasDelSector = teclasSector;
                #endregion

                #region Niveles
                Niveles n = new Niveles();

                DataSet dsNiveles = Procedimientos.NivelesIntrusion(GestorBDCD40.ConexionMySql, id_sistema, id_usuario);

                if (dsNiveles.Tables[0].Rows.Count > 0)
                {
                    DataRow rowNiveles = dsNiveles.Tables[0].Rows[0];
                    if (rowNiveles["IdSector"] != System.DBNull.Value)
                        n.IdSector = (string)rowNiveles["IdSector"];
                    if (rowNiveles["IdNucleo"] != System.DBNull.Value)
                        n.IdNucleo = (string)rowNiveles["IdNucleo"];
                    if (rowNiveles["IdSistema"] != System.DBNull.Value)
                        n.IdSistema = (string)rowNiveles["IdSistema"];
                    if (rowNiveles["CICL"] != System.DBNull.Value)
                        n.CICL = (uint)rowNiveles["CICL"];
                    if (rowNiveles["CIPL"] != System.DBNull.Value)
                        n.CIPL = (uint)rowNiveles["CIPL"];
                    if (rowNiveles["CPICL"] != System.DBNull.Value)
                        n.CPICL = (uint)rowNiveles["CPICL"];
                    if (rowNiveles["CPIPL"] != System.DBNull.Value)
                        n.CPIPL = (uint)rowNiveles["CPIPL"];

                }
                #endregion

                ConfiguracionUsuario.NivelesDelSector = n;

                // Nombre de la agruapacion
                ConfiguracionUsuario.Nombre = id_usuario;

                //if (s.SectorSimple)
                //    ConfiguracionUsuario.IdIdentificador = id_usuario;
                //else
                //{
                SectoresSector ss = new SectoresSector();
                ss.IdSistema = id_sistema;
                ss.IdSector = id_usuario;
                ss.EsDominante = true;

                List<Tablas> sDominante = GestorBDCD40.ListSelectSQL(ss, null);
                if (sDominante.Count > 0)
                    // Nombre del usuario dominante
                    ConfiguracionUsuario.IdIdentificador = ((SectoresSector)sDominante[0]).IdSectorOriginal;

                //}
                return ConfiguracionUsuario;
            }
        }

        [WebMethod(Description = "Pasándole el identificador hardware de la pasarela y el sistema al que pertenece, " +
                                "devuelve la configuración de la pasarela.")]
        public CfgPasarela GetCfgPasarela(string id_sistema, string id_hw)
        {
            lock (Sync)
            {
                Recursos r = new Recursos();
                r.IdTifX = id_hw;
                r.IdSistema = id_sistema;

                ConfiguracionPasarela.Nombre = id_hw;
                //ConfiguracionPasarela.NumRecursos = Convert.ToInt32(GestorBDCD40.SelectCountSQL(r, "idTIFX='" + id_hw + "' AND idSistema='" + id_sistema + "'"));
                ConfiguracionPasarela.NumRecursos = GestorBDCD40.DataSetSelectSQL(r, null).Tables[0].Rows.Count;

                TifX tifX = new TifX();
                tifX.IdSistema = id_sistema;
                tifX.IdTifx = id_hw;

                System.Data.DataSet ds = GestorBDCD40.DataSetSelectSQL(tifX, null);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ConfiguracionPasarela.ModoSincronizacion = Convert.ToInt32(ds.Tables[0].Rows[0]["ModoSincronizacion"]);
                    ConfiguracionPasarela.MasterSincronizacion = Convert.ToString(ds.Tables[0].Rows[0]["Master"]);
                    ConfiguracionPasarela.PuertoLocalSNMP = Convert.ToUInt32(ds.Tables[0].Rows[0]["SNMPPortLocal"]);
                    ConfiguracionPasarela.PuertoRemotoSNMP = Convert.ToUInt32(ds.Tables[0].Rows[0]["SNMPPortRemoto"]);
                    ConfiguracionPasarela.PuertoRemotoTrapsSNMP = Convert.ToUInt32(ds.Tables[0].Rows[0]["SNMPTraps"]);
                    ConfiguracionPasarela.PuertoLocalSIP = Convert.ToUInt32(ds.Tables[0].Rows[0]["SIPPortLocal"]);
                    ConfiguracionPasarela.PeriodoSupervisionSIP = Convert.ToUInt32(ds.Tables[0].Rows[0]["TimeSupervision"]);
                    ConfiguracionPasarela.IpGrabador1 = ds.Tables[0].Rows[0]["IpRecorder1"] != System.DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[0]["IpRecorder1"]) : "";
                    ConfiguracionPasarela.IpGrabador2 = ds.Tables[0].Rows[0]["IpRecorder2"] != System.DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[0]["IpRecorder2"]) : "";
                    //VMG 18/02/2019
                    ConfiguracionPasarela.iSupervLanGW = Convert.ToByte(ds.Tables[0].Rows[0]["iSupervLanGW"]);
                    ConfiguracionPasarela.itmmaxSupervLanGW = Convert.ToByte(ds.Tables[0].Rows[0]["itmmaxSupervLanGW"]);

                    List<Tablas> listaRecursos = ListaRecursosPasarela(id_sistema, id_hw);
                    int tamListaRecursos = listaRecursos.Count;
                    ConfiguracionPasarela.ListaRecursos = new RecursosSCV[tamListaRecursos];
                    for (ushort num = 0; num < tamListaRecursos; num++)
                    {
                        ConfiguracionPasarela.ListaRecursos[num] = (RecursosSCV)listaRecursos[num];
                    }
                }

                return ConfiguracionPasarela;
            }
        }

        [WebMethod(Description = "Pasándole un identificador de sistema, un identificador de recurso y" +
                                " el tipo de interfaz (TI_Radio=0, TI_LCEN=1, TI_BC=2, TI_BL=3, TI_AB=4, TI_ATS_R2=5, TI_ATS_N5=6, TI_ATS_QSIG=7, TI_ISDN_2BD=8, TI_ISDN_30BD=9, TI_I_O=10, TI_DATOS=11)" +
                                ", retorna los parámetros correspondientes al recurso.")]
        public Tablas GetParametrosRecursoById(string idSistema, string idRecurso, int tipo)
        {
            lock (Sync)
            {
                Recursos r = new Recursos();
                r.IdSistema = idSistema;
                r.IdRecurso = idRecurso;
                r.Interface = (Tipos.TipoInterface)tipo;

                return GetParametrosRecurso(r);
            }
        }

        [WebMethod(Description = "Pasándole un recurso, retorna los parámetros correspondientes.")]
        public Tablas GetParametrosRecurso(Recursos r)
        {
            lock (Sync)
            {
                //Procedimientos pr = new Procedimientos();
                switch ((Tipos.TipoInterface)r.Interface)
                {
                    case Tipos.TipoInterface.TI_DATOS:
                    case Tipos.TipoInterface.TI_Radio:
                        RecursosRadioForGateway pRadio = new RecursosRadioForGateway();
                        pRadio.IdSistema = r.IdSistema;
                        pRadio.IdRecurso = r.IdRecurso;

                        List<Tablas> tRadio = GestorBDCD40.ListSelectSQL(pRadio, null);
                        if (tRadio.Count > 0)
                        {
                            ((RecursosRadioForGateway)tRadio[0]).ListaEnlacesRecursosExternos = GetListaEnlaces((RecursosRadio)tRadio[0]);
                            return (RecursosRadioForGateway)tRadio[0];
                        }
                        break;
                    case Tipos.TipoInterface.TI_LCEN:
                        RecursosLCEN pLCEN = new RecursosLCEN();
                        pLCEN.IdSistema = r.IdSistema;
                        pLCEN.IdRecurso = r.IdRecurso;

                        List<Tablas> tLCEN = GestorBDCD40.ListSelectSQL(pLCEN, null);
                        if (tLCEN.Count > 0)
                        {
                            string idDestino = ((RecursosLCEN)tLCEN[0]).IdDestino;
                            ((ParametrosRecursoLCEN)tLCEN[0]).ListaEnlacesInternos = Procedimientos.ListaUsuariosImplicadosEnRecurso(GestorBDCD40.ConexionMySql, r.IdSistema, idDestino, 1);
                            return (ParametrosRecursoLCEN)tLCEN[0];
                        }
                        break;
                    case Tipos.TipoInterface.TI_BC:
                    case Tipos.TipoInterface.TI_BL:
                    case Tipos.TipoInterface.TI_AB:
                    case Tipos.TipoInterface.TI_ATS_R2:
                    case Tipos.TipoInterface.TI_ATS_N5:
                    case Tipos.TipoInterface.TI_ATS_QSIG:
                    case Tipos.TipoInterface.TI_ISDN_2BD:
                    case Tipos.TipoInterface.TI_ISDN_30BD:
                    case Tipos.TipoInterface.TI_EM_MARC:
                    case Tipos.TipoInterface.TI_EM_PP:
                        RecursosTF pTf = new RecursosTF();
                        pTf.IdSistema = r.IdSistema;
                        pTf.IdRecurso = r.IdRecurso;

                        List<Tablas> tTf = GestorBDCD40.ListSelectSQL(pTf, null);
                        if (tTf.Count > 0)
                        {
                            string idDestino = ((RecursosTF)tTf[0]).IdDestino;

                            ((ParametrosEM)tTf[0]).ListaEnlacesInternos = Procedimientos.ListaUsuariosImplicadosEnRecurso(GestorBDCD40.ConexionMySql, r.IdSistema, idDestino, null);
                            return (ParametrosEM)tTf[0];
                        }
                        break;
                    default:
                        return null;
                }

                return null;
            }
        }

        [WebMethod(Description = "Pasándole el identificador del sistema devuelve el conjunto de " +
                                "parámetros que conforman la configuración común de los distintos elementos del SCV.")]
        public ConfiguracionSistema GetConfigSistema(string id_sistema)
        {
            lock (Sync)
            {
                DameParametrosGenerales(id_sistema);
                DamePlanNumeracionATS(id_sistema);
                DamePlanDireccionamientoIP(id_sistema);
                DamePlanTroncales(id_sistema);
                DamePlanRedes(id_sistema);
                DamePlanAsignacionUsuarios(id_sistema);
                DamePlanAsignacionRecursos(id_sistema);
                DamePlanDireccionamientoSIP(id_sistema);

                return CfgSistema;
            }
        }

        [WebMethod(Description = "Pasándole el identificador del sistema devuelve los parámetros generales del sistema.")]
        public ParametrosGeneralesSistema GetParametrosGenerales(string id_sistema)
        {
            lock (Sync)
            {
                DameParametrosGenerales(id_sistema);

                return CfgSistema.ParametrosGenerales;
            }
        }

        [WebMethod(Description = "Pasándole el identificador del sistema devuelve el plan de numeración ATS.")]
        public NumeracionATS[] GetPlanNumeracionATS(string id_sistema)
        {
            lock (Sync)
            {
                DamePlanNumeracionATS(id_sistema);

                return CfgSistema.PlanNumeracionATS;
            }
        }

        [WebMethod(Description = "Pasándole el identificador del sistema devuelve el plan de direccionamiento IP.")]
        public DireccionamientoIP[] GetPlanDireccionamientoIP(string id_sistema)
        {
            lock (Sync)
            {
                DamePlanDireccionamientoIP(id_sistema);

                return CfgSistema.PlanDireccionamientoIP;
            }
        }

        [WebMethod(Description = "Pasándole el identificador del sistema devuelve el plan de troncales.")]
        public ListaTroncales[] GetPlanTroncales(string id_sistema)
        {
            lock (Sync)
            {
                DamePlanTroncales(id_sistema);

                return CfgSistema.PlanTroncales;
            }
        }

        [WebMethod(Description = "Pasándole el identificador del sistema devuelve el plan de redes.")]
        public ListaRedes[] GetPlanRedes(string id_sistema)
        {
            lock (Sync)
            {
                DamePlanRedes(id_sistema);

                return CfgSistema.PlanRedes;
            }
        }

        [WebMethod(Description = "Pasándole el identificador del sistema devuelve el plan de asignación de usuarios a terminales de voz.")]
        public AsignacionUsuariosTV[] GetPlanAsignacionUsuarios(string id_sistema)
        {
            lock (Sync)
            {
                DamePlanAsignacionUsuarios(id_sistema);

                return CfgSistema.PlanAsignacionUsuarios;
            }
        }

        [WebMethod(Description = "Pasándole el identificador del sistema devuelve el plan de asignación de recursos a pasarelas.")]
        public AsignacionRecursosGW[] GetPlanAsignacionRecursos(string id_sistema)
        {
            lock (Sync)
            {
                DamePlanAsignacionRecursos(id_sistema);

                return CfgSistema.PlanAsignacionRecursos;
            }
        }

        [WebMethod(Description = "Pasándole el identificador del sistema devuelve el plan de direccionamiento SIP.")]
        public System.Collections.Generic.List<DireccionamientoSIP> GetPlanDireccionamientoSIP(string id_sistema)
        {
            lock (Sync)
            {
                DamePlanDireccionamientoSIP(id_sistema);

                return CfgSistema.PlanDireccionamientoSIP;
            }
        }

        [WebMethod(Description = "Pasándole el identificador del sistema, proporciona la dirección multicast y el puerto " +
                                "para notificación de una activación de configuración.")]
        public ParametrosMulticast GetParametrosMulticast(string id_sistema)
        {
            lock (Sync)
            {
                ParametrosMulticast pMulticast = new ParametrosMulticast();
                Sistema sis = new Sistema();
                sis.IdSistema = id_sistema;

                List<Tablas> sSistema = GestorBDCD40.ListSelectSQL(sis, null);
                if (sSistema.Count > 0)
                {
                    pMulticast.GrupoMulticastConfiguracion = ((Sistema)sSistema[0]).GrupoMulticastConfiguracion;
                    pMulticast.PuertoMulticastConfiguracion = ((Sistema)sSistema[0]).PuertoMulticastConfiguracion;

                    return pMulticast;
                }

                return null;
            }
        }

        [WebMethod(Description = "Configuración relativa a frecuencias HF Ulises5Ki.")]
        public PoolHfElement[] GetPoolHfElement(string id_sistema)
        {
            lock (Sync)
            {
                int i = 0;
                HFParams hfFrequencies = new HFParams((int)TipoRecursoRadio.Audio_HF_TX);
                hfFrequencies.IdSistema = id_sistema;

                List<Tablas> listaFrecuenciasHf = GestorBDCD40.ListSelectSQL(hfFrequencies, null);

                if (listaFrecuenciasHf.Count > 0)
                {
                    PoolHfElement[] poolHf = new PoolHfElement[listaFrecuenciasHf.Count];

                    foreach (HFParams hfParam in listaFrecuenciasHf)
                    {
                        poolHf[i] = new PoolHfElement();

                        poolHf[i].Id = hfParam.IdRecurso;
                        poolHf[i].SipUri = hfParam.SipUri;
                        poolHf[i].Oid = hfParam.Oid;
                        poolHf[i].IpGestor = hfParam.IpGestor;

                        // Recuperar los rangos de frecuencia
                        CD40.BD.Entidades.HFRangoFrecuencias rangoFrec = new CD40.BD.Entidades.HFRangoFrecuencias();
                        rangoFrec.IdSistema = id_sistema;
                        rangoFrec.IdRecurso = hfParam.IdRecurso;

                        List<Tablas> rangoFrecuencias = GestorBDCD40.ListSelectSQL(rangoFrec, null);
                        if (rangoFrecuencias.Count > 0)
                        {
                            int r = 0;
                            poolHf[i].Frecs = new HfRangoFrecuencias[rangoFrecuencias.Count];
                            foreach (CD40.BD.Entidades.HFRangoFrecuencias rango in rangoFrecuencias)
                            {
                                poolHf[i].Frecs[r] = new HfRangoFrecuencias();

                                poolHf[i].Frecs[r].FMin = rango.Min;
                                poolHf[i].Frecs[r].FMax = rango.Max;
                                r++;
                            }
                        }

                        i++;
                    }

                    return poolHf;
                }
                return null;
            }
        }

        [WebMethod(Description = "Configuración relativa a los equipos N+M")]
        public Node[] GetPoolNMElements(string id_sistema)
        {
            lock (Sync)
            {
                int i = 0;
                HFParams hfFrequencies = new HFParams((int)TipoRecursoRadio.Audio_NM);
                hfFrequencies.IdSistema = id_sistema;

                List<Tablas> listaFrecuenciasHf = GestorBDCD40.ListSelectSQL(hfFrequencies, null);

                if (listaFrecuenciasHf.Count > 0)
                {
                    Node[] poolNM = new Node[listaFrecuenciasHf.Count];

                    foreach (HFParams hfParam in listaFrecuenciasHf)
                    {
                        poolNM[i] = new Node();

                        poolNM[i].Id = hfParam.IdRecurso;
                        poolNM[i].SipUri = hfParam.SipUri;
                        poolNM[i].Oid = hfParam.Oid;
                        poolNM[i].IpGestor = hfParam.IpGestor;
                        poolNM[i].EsReceptor = hfParam.TipoEquipo == 0 || hfParam.TipoEquipo == 2;
                        poolNM[i].EsTransmisor = hfParam.TipoEquipo == 1 || hfParam.TipoEquipo == 2;
                        poolNM[i].TipoDeFrecuencia = (Tipo_Frecuencia)(hfParam.TipoFrecuencia + 1); // El enum Tipo_Frecuencia contempla el tipo Basica=0 que no tiene caso para la configuración N+M
                        poolNM[i].TipoDeCanal = (Tipo_Canal)hfParam.TipoCanal;
                        poolNM[i].FormaDeTrabajo = (Tipo_Formato_Trabajo)hfParam.TipoModo;
                        poolNM[i].Prioridad = hfParam.PrioridadEquipo;
                        poolNM[i].FrecuenciaPrincipal = hfParam.Frecuencia;
                        poolNM[i].Puerto = hfParam.Puerto;
                        poolNM[i].Offset = (GearCarrierOffStatus)hfParam.Offset;
                        poolNM[i].Canalizacion = (GearChannelSpacings)hfParam.Canalizacion;
                        poolNM[i].Modulacion = (GearModulations)hfParam.Modulacion;
                        poolNM[i].NivelDePotencia = (GearPowerLevels)hfParam.Potencia;
                        poolNM[i].FormatoFrecuenciaPrincipal = (Tipo_Formato_Frecuencia)hfParam.FormatoFrecuenciaPrincipal;
                        poolNM[i].ModeloEquipo = hfParam.ModeloEquipo;
                        poolNM[i].IdEmplazamiento = hfParam.IdEmplazamiento;

                        // Recuperar los rangos de frecuencia
                        CD40.BD.Entidades.HFRangoFrecuencias rangoFrec = new CD40.BD.Entidades.HFRangoFrecuencias();
                        rangoFrec.IdSistema = id_sistema;
                        rangoFrec.IdRecurso = hfParam.IdRecurso;

                        List<Tablas> rangoFrecuencias = GestorBDCD40.ListSelectSQL(rangoFrec, null);
                        if (rangoFrecuencias.Count > 0)
                        {
                            int r = 0;
                            poolNM[i].Frecs = new HfRangoFrecuencias[rangoFrecuencias.Count];
                            foreach (CD40.BD.Entidades.HFRangoFrecuencias rango in rangoFrecuencias)
                            {
                                poolNM[i].Frecs[r] = new HfRangoFrecuencias();

                                poolNM[i].Frecs[r].FMin = rango.Min;
                                poolNM[i].Frecs[r].FMax = rango.Max;
                                r++;
                            }
                        }

                        i++;
                    }

                    return poolNM;
                }
                return null;
            }
        }

        #endregion WebServices

        //[WebMethod]
        //public string HelloWorld()
        //{
        //    return "Hola a todos";
        //}
    }
}
