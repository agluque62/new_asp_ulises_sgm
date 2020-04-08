using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

using CD40.BD.Entidades;
using CD40.BD;
using ClusterLib;

namespace UlisesWebConfig.WebServices.MainService
{
    public struct DatosControlBackup
    {
        public string NomRecursoHistoricos;
        public string NomRecursoIndicadores;

        public uint[] Profundidad;
    }

    /// <summary>
    /// Descripción breve de MainService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    #region XML-Includes
    [XmlInclude(typeof(Tablas))]
    [XmlInclude(typeof(Agrupaciones))]
    [XmlInclude(typeof(Alarmas))]
    [XmlInclude(typeof(SectoresAgrupacion))]
    [XmlInclude(typeof(GruposTelefonia))]
    [XmlInclude(typeof(TifX))]
    [XmlInclude(typeof(Top))]
    [XmlInclude(typeof(Sistema))]
    [XmlInclude(typeof(Operadores))]
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

    [XmlInclude(typeof(Zonas))]
    [XmlInclude(typeof(ValoresTabla))]
    [XmlInclude(typeof(Tabla_bss))]
    [XmlInclude(typeof(Tabla_bss_recurso))]
    [XmlInclude(typeof(RadioParam))]
    [XmlInclude(typeof(MetodosBss))]
    [XmlInclude(typeof(MetodosbssDestinosradio))]

    [XmlInclude(typeof(Agenda))]
    [XmlInclude(typeof(GwActivas))]
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
    [XmlInclude(typeof(Incidencias))]
    [XmlInclude(typeof(HistoricoIncidencias))]
    [XmlInclude(typeof(EquiposEU))]
    [XmlInclude(typeof(Tipos.ExportaTipoEnumerados))]
    #endregion
    public partial class MainService : System.Web.Services.WebService
    {
        #region Datos Privados

        object _Sync = new object();
        private static Ref_InterfazSacta.ServicioInterfazSactaSoapClient ServicioSacta = new Ref_InterfazSacta.ServicioInterfazSactaSoapClient();

        private GestorBaseDatos GestorBDCD40;
        private GestorBaseDatos GestorBDCD40ToMantto;
        private MySql.Data.MySqlClient.MySqlConnection MySqlConnectionToCd40;
        private MySql.Data.MySqlClient.MySqlConnection MySqlConnectionToCd40ToMantto;

        static System.Threading.TimerCallback TimerProcedure;
        static System.Threading.Timer TimerPresenciaCD30;

        private static bool EstadoCD30Activo = false;
        private static int Estado1 = 0;
        private static int Estado2 = 2;

        #endregion

        #region Metodos Privados

        int AsignaEnlaceARecurso(Tablas[] t, MySql.Data.MySqlClient.MySqlTransaction trans)
        {
            EstadosRecursos er = new EstadosRecursos();
            CD40.BD.Utilidades u = new CD40.BD.Utilidades(GestorBDCD40.ConexionMySql);
            int i = 0;

            try
            {
                // Inicio de una transacción
                //MySql.Data.MySqlClient.MySqlTransaction trans = GestorBDCD40.StartTransaction(true);

                for (; i < t.Length; i++)
                    u.UpdateDestinoSQL(((ParametrosRecursoGeneral)t[i]), trans);

                // Obtener los registros de EstadosRecursos asignado a los paneles de los distintos sectores
                DataSet listaEstados = u.GetEstadosRecursoDestino(((RecursosRadio)t[0]).IdSistema, ((RecursosRadio)t[0]).IdDestino, trans);

                er.IdSistema = ((RecursosRadio)t[0]).IdSistema;
                er.IdDestino = ((RecursosRadio)t[0]).IdDestino;
                er.TipoDestino = ((RecursosRadio)t[0]).TipoDestino;
                // Eliminar los registros de EstadosRecursos del destino modificado
                GestorBDCD40.DeleteSQL(er, trans);
                //DeleteSQL(er);

                // Insertar los nuevos registros de EstadosRecursos con el destino asignado a los recursos.
                foreach (DataRow posicion in listaEstados.Tables[0].Rows)
                {
                    uint num = 0;
                    foreach (RecursosRadio r in t)
                    {
                        er.IdSector = (string)posicion["IdSector"];
                        er.IdNucleo = (string)posicion["IdNucleo"];
                        er.PosHMI = (uint)posicion["PosHMI"];
                        er.IdRecurso = r.IdRecurso;
                        er.TipoRecurso = (uint)posicion["TipoRecurso"];
                        er.Estado = "S"; // Todos los recursos asignados a un destino estarán "S"eleccionados. Incidencia #781 num == 0 ? "S" : "A";
                        num++;
                        GestorBDCD40.InsertSQL(er, trans);
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
            }

            return i;
        }

        /// <summary>
        /// Description = "Actualiza los parámetros y números de abonado de los sectores que no forman parte de una agrupación"
        /// </summary>
        /// <param name="idSistema"></param>
        /// <param name="idNucleo"></param>
        /// <param name="idSector"></param>
        /// <param name="trans"></param>
        void RegeneraParametros(string idSistema, string idNucleo, string idSector, MySql.Data.MySqlClient.MySqlTransaction trans)
        {
            //CD40.BD.Procedimientos p = new Procedimientos();

            string[] listaSectores = Procedimientos.SectoresCompuestosPorSectorOriginal(GestorBDCD40.ConexionMySql, trans, idSistema, idSector);

            //MySql.Data.MySqlClient.MySqlTransaction trans = GestorBDCD40.StartTransaction(true);

            try
            {
                for (int j = 0; j < listaSectores.Length; j++)
                {
                    // Componer los ParametrosSector del sector agrupado
                    DataSet listaParametros = Procedimientos.ParametrosSectorNoAgrupado(GestorBDCD40.ConexionMySql, trans, idSistema, listaSectores[j]);

                    ParametrosSector ps = Utilidades.RegeneraParametrosSector(listaParametros);

                    ps.IdSistema = idSistema;
                    ps.IdNucleo = idNucleo;
                    ps.IdSector = listaSectores[j];

                    GestorBDCD40.DeleteSQL(ps, trans);
                    GestorBDCD40.InsertSQL(ps, trans);

                    // Componer los números de abonado del sector agrupado
                    DataSet listaAbonados = Procedimientos.NumerosAbonadosSectorNoAgrupado(GestorBDCD40.ConexionMySql, trans, idSistema, idNucleo, listaSectores[j]);

                    UsuariosAbonados ua = new UsuariosAbonados();
                    ua.IdSistema = idSistema;
                    ua.IdNucleo = idNucleo;
                    ua.IdSector = listaSectores[j];
                    GestorBDCD40.DeleteSQL(ua, trans);

                    foreach (DataRow la in listaAbonados.Tables[0].Rows)
                    {
                        ua.IdPrefijo = (uint)la["IdPrefijo"];
                        ua.IdAbonado = (string)la["IdAbonado"];

                        GestorBDCD40.InsertSQL(ua, trans);
                    }

                }

                //GestorBDCD40.Commit(trans);
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                GestorBDCD40.RollBack(trans);
            }
        }

        string GeneraAlgoritmo(string idSistema, string idNucleo, string listaSectores, MySql.Data.MySqlClient.MySqlTransaction trans)
        {
            System.Text.StringBuilder nombreAlgoritmo = new System.Text.StringBuilder();

            if (!string.IsNullOrWhiteSpace(listaSectores))
            {
                DataSet ds = SectoresNumSactaSorted(idSistema, idNucleo, listaSectores);

                string[] sectores = listaSectores.Split(new char[] { ',', '\'' }, StringSplitOptions.RemoveEmptyEntries);

                if (sectores.Length > 0)
                {
                    // Obtener los parámetros de longitud de identificadores del sistema
                    Sistema pSistema = new Sistema();
                    pSistema.IdSistema = idSistema;
                    List<Tablas> sistema = GestorBDCD40.ListSelectSQL(pSistema, trans);
                    if (null != sistema && sistema.Count > 0)
                        pSistema = (Sistema)sistema[0];

                    int numCaracteresPorSector = (int)pSistema.TamLiteralDA / sectores.Length;

                    /**
                     * AGL. ID.83/84 A veces daba un error porque ds.Tables.Count == 0
                     * */
                    if (null != ds && ds.Tables.Count > 0)
                    {
                        foreach (System.Data.DataRow d in ds.Tables[0].Rows)
                        {
                            nombreAlgoritmo.Append(((string)d[0]).Substring(0, System.Math.Min(((string)d[0]).Length, numCaracteresPorSector)));
                        }
                    }
                    /** Fin Modificacion */
                }
            }

            return nombreAlgoritmo.ToString();
        }

        void CreateEvent(int idIncidencia, string node, string idioma, string sistema, string ip)
        {
            string consulta;
            string desc = "";
            System.Data.DataSet ds = new System.Data.DataSet();


            lock (_Sync)
            {
                try
                {
                    switch (idioma)
                    {
                        case "en":
                            consulta = string.Format("SELECT incidencia FROM incidencias_ingles WHERE IdIncidencia={0}", idIncidencia);
                            break;
                        case "fr":
                            consulta = string.Format("SELECT incidencia FROM incidencias_frances WHERE IdIncidencia={0}", idIncidencia);
                            break;
                        default:
                            consulta = string.Format("SELECT incidencia FROM incidencias WHERE IdIncidencia={0}", idIncidencia);
                            break;
                    }
                    ds = GestorBDCD40ToMantto.GetDataSet(consulta, null);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        string[] incidencia = ds.Tables[0].Rows[0]["incidencia"].ToString().Split(new char[] { ' ' });

                        // Formatearlo de acuerdo a como lo espera el servidor de mantenimiento
                        desc = string.Format("{0},{1} {2} {3}", idIncidencia, incidencia[0], node, incidencia[1]);

                        System.Configuration.Configuration webConfiguracion = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                        string toServerMantto = webConfiguracion.AppSettings.Settings["ServerManttoIp"].Value;

                        UtilitiesCD40.GeneraIncidencias.SendTrap(toServerMantto, idIncidencia.ToString(), desc);
                    }
                }
                catch (MySql.Data.MySqlClient.MySqlException)
                {
                }
            }
        }

        void GuardarUsuariosAbonados(string sistema, string sector, string elnucleo, MySql.Data.MySqlClient.MySqlTransaction tran, DataSet dsNumeroAbonado)
        {
            UsuariosAbonados t = new UsuariosAbonados();
            t.IdSistema = sistema;
            t.IdSector = sector;
            t.IdNucleo = elnucleo;
            GestorBDCD40.DeleteSQL(t, tran);

            for (int i = 0; i < dsNumeroAbonado.Tables[0].Rows.Count; i++)
            {
                t.IdPrefijo = (uint)dsNumeroAbonado.Tables[0].Rows[i].ItemArray[1];
                t.IdAbonado = (string)dsNumeroAbonado.Tables[0].Rows[i].ItemArray[2];
                GestorBDCD40.InsertSQL(t, tran);
            }

            RegeneraParametros(sistema, elnucleo, sector, tran);
        }

        void GuardarAgenda(string sistema, string sector, string elnucleo, MySql.Data.MySqlClient.MySqlTransaction tran, DataSet dsAgenda)
        {
            Agenda ag = new Agenda();
            ag.IdSistema = sistema;
            ag.IdNucleo = elnucleo;
            ag.IdSector = sector;

            GestorBDCD40.DeleteSQL(ag, tran);

            for (int i = 0; i < dsAgenda.Tables[0].Rows.Count; i++)
            {
                ag.Nombre = (string)dsAgenda.Tables[0].Rows[i].ItemArray[5];
                ag.Prefijo = (uint)dsAgenda.Tables[0].Rows[i].ItemArray[3];
                ag.Numero = (string)dsAgenda.Tables[0].Rows[i].ItemArray[4];
                GestorBDCD40.InsertSQL(ag, tran);
            }
        }

        void CD30Off(Object opt)
        {
            TimerPresenciaCD30.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
            EstadoCD30Activo = false;
        }

        string CreaFicheroDump(bool commit)
        {
            string strUsuario = "root";
            string strPwd = "cd40";
            string strDatabaseOrigen = "cd40_trans";
            string strDatabaseDestino = "cd40";
            string strServer = "localhost";
            string s = string.Empty;
            string strTablas = string.Empty;
            int iCodSalida = -1;

            strTablas = Utilidades.GetTablasModificadas(GestorBDCD40);
            if (strTablas == null)  // No hay tablas que actualizar
                return "";

            try
            {
                Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Nucleo\\CD40", false);
                strUsuario = Convert.ToString(rk.GetValue("Usuario"));
                strPwd = Convert.ToString(rk.GetValue("Clave"));
                strDatabaseOrigen = Convert.ToString(rk.GetValue("Base Datos Origen"));
                strDatabaseDestino = Convert.ToString(rk.GetValue("Base Datos Destino"));
                strServer = Convert.ToString(rk.GetValue("Servidor"));
                rk.Close();
            }
            catch (Exception)
            {
                Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE", true);
                rk = rk.CreateSubKey("Nucleo");
                rk = rk.CreateSubKey("CD40");

                rk.SetValue("Usuario", strUsuario);
                rk.SetValue("Clave", strPwd);
                rk.SetValue("Base Datos Origen", strDatabaseOrigen);
                rk.SetValue("Base Datos Destino", strDatabaseDestino);
                rk.SetValue("Servidor", strServer);
                rk.Close();
            }
            finally
            {
                string proceso = HttpContext.Current.Server.MapPath("~/") + "trans.bat";
                string argumentos = strUsuario + " " +
                                    strPwd + " " +
                                    strServer + " " +
                                    (commit ? strDatabaseOrigen : strDatabaseDestino) + " " +
                                    (commit ? strDatabaseDestino : strDatabaseOrigen) + " " +
                                    (commit ? strTablas : "");
                //"root jucar localhost cd40_trans cd40 [tabla1 tabla 2 ... ]";


                System.Diagnostics.ProcessStartInfo p = new System.Diagnostics.ProcessStartInfo();
                p.FileName = proceso; p.Arguments = argumentos;


                //MVO. Se ejecuta el bat en la shell, para que se puedan ejecutar varios comandos
                p.UseShellExecute = true;
                p.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;


                //s = pFinal.StandardError.ReadToEnd();

                //MVO: Se registra en el fichero de log, el tipo de operación realizada: se implanta la sectorización 
                //    o se restaura la BD original a partir de la base de datos en explotación
                if (commit)
                    Utilidades.ErrorLog("CommitCD40_Log.txt", "Se implanta la sectorización en la BD de explotación.");
                else
                    Utilidades.ErrorLog("CommitCD40_Log.txt", "Se restaura la configuración de la BD de explotación");


                Utilidades.ErrorLog("CommitCD40_Log.txt", "  Se ejecuta: " + proceso + " " + p.Arguments);
                Utilidades.ErrorLog("CommitCD40_Log.txt", "  Tablas modificadas:" + strTablas);

                //Se ejecuta el proceso
                System.Diagnostics.Process pFinal = System.Diagnostics.Process.Start(p);

                try
                {
                    //Se espera a que finalice el proceso un máximo de 5 minutos 
                    if (pFinal.WaitForExit(300000))
                    {
                        //El proceso ha acabo correctamente
                        Utilidades.ErrorLog("CommitCD40_Log.txt", "Proceso realizado correctamente. ExitCode: " + pFinal.ExitCode);
                        iCodSalida = pFinal.ExitCode;
                    }
                    else
                    {
                        Utilidades.ErrorLog("CommitCD40_Log.txt", "Time Out excedido");
                    }
                }
                catch (Exception ex)
                {
                    Utilidades.ErrorLog("CommitCD40_Log.txt", "Excepcion al ejecutar trans.bat. Error:" + ex.Message.ToString());
                }
                finally
                {
                    if (pFinal != null)
                    {
                        pFinal.Close();
                    }
                }

            }

            return (iCodSalida == 0).ToString();
        }
        
        #endregion Metodos Privados

        public MainService()
        {
            string cadenaConexion;
            System.Configuration.Configuration webConfiguracion = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            if (webConfiguracion.ConnectionStrings.ConnectionStrings.Count > 0)
            {
                cadenaConexion = webConfiguracion.ConnectionStrings.ConnectionStrings["ConexionBaseDatosCD40_Trans"].ToString();
                MySqlConnectionToCd40 = new MySql.Data.MySqlClient.MySqlConnection(cadenaConexion);
                GestorBDCD40 = new GestorBaseDatos(MySqlConnectionToCd40);

                cadenaConexion = webConfiguracion.ConnectionStrings.ConnectionStrings["ConexionBaseDatosCD40"].ToString();
                MySqlConnectionToCd40ToMantto = new MySql.Data.MySqlClient.MySqlConnection(cadenaConexion);
                GestorBDCD40ToMantto = new GestorBaseDatos(MySqlConnectionToCd40ToMantto);
            }

            string toSincro = webConfiguracion.AppSettings.Settings["SincronizaCD30"].Value;
            if (toSincro == "1")
            {
                if (TimerProcedure == null)
                    TimerProcedure = new System.Threading.TimerCallback(CD30Off);
                if (TimerPresenciaCD30 == null)
                    TimerPresenciaCD30 = new System.Threading.Timer(TimerProcedure, null, 0, 10000);
            }
        }


        #region WEB METHODS

        [WebMethod(Description = "Restaura la copia de la base de datos sobre la base de datos original")]
        public string Commit()
        {
            string retorno = CreaFicheroDump(true /* commit */);
            GestorBDCD40.ExecuteNonQuery("TRUNCATE TABLE TablasModificadas", null);
            return retorno;
        }

        [WebMethod(Description = "Recupera la última activa como base de datos de gestíón")]
        public string Rollback()
        {
            string retorno = CreaFicheroDump(false /* rollback */);

            GestorBDCD40.ExecuteNonQuery("TRUNCATE TABLE TablasModificadas", null);

            return retorno;
        }

        [WebMethod]
        public bool HayModificacionesPendientes()
        {
            return Utilidades.GetTablasModificadas(GestorBDCD40) != null;
        }

        [WebMethod]
        public bool FindTableInTablasModificadas(string table)
        {
            return Utilidades.FindTableInTablasModificadas(GestorBDCD40, table);
        }

        [WebMethod(Description = "Retorna un DataSet con los registros que cumplan con la clave pasada en <t>")]
        public DataSet DataSetSelectSQL(Tablas t)
        {
            return GestorBDCD40.DataSetSelectSQL(t, null);
        }

        [WebMethod(Description = "Retorna una Tablas[] con los registros que cumplan con la clave pasada en <t>. Si la " +
                                "SELECT no devuelve ningún registro, el array estará vacío")]
        public List<Tablas> ListSelectSQL(Tablas t)
        {
            return GestorBDCD40.ListSelectSQL(t, null);
        }

        [WebMethod(Description = "Retorna el número de registros insertados o un número negativo cuyo valor" +
                                " absoluto indica el código de excepción en caso de error")]
        public int InsertSQL(Tablas t)
        {
            return GestorBDCD40.InsertSQL(t, null);
        }

        [WebMethod(Description = "Retorna el número de registros actualizados o un número negativo cuyo valor" +
                                " absoluto indica el código de excepción en caso de error. No actualiza las claves.")]
        public int UpdateSQL(Tablas t)
        {
            return GestorBDCD40.UpdateSQL(t, null);
        }

        [WebMethod(Description = "Genera registros en EstadosRecursos para una lista de nuevos recursos que dan servicio a un destino radio concreto.")]
        public void GeneraEstadosRecursosParaUnDestino(Tablas[] t)
        {
            EstadosRecursos er = new EstadosRecursos();
            CD40.BD.Utilidades u = new CD40.BD.Utilidades(GestorBDCD40.ConexionMySql);

            // Obtener los registros de EstadosRecursos asignado a los paneles de los distintos sectores
            DataSet listaEstados = u.GetEstadosRecursoDestino(((RecursosRadio)t[0]).IdSistema, ((RecursosRadio)t[0]).IdDestino, null);
            er.IdSistema = ((RecursosRadio)t[0]).IdSistema;

            if (listaEstados == null)
                return;

            // Insertar los nuevos registros de EstadosRecursos con el destino asignado a los recursos.
            foreach (DataRow posicion in listaEstados.Tables[0].Rows)
            {
                foreach (RecursosRadio r in t)
                {
                    er.IdSector = (string)posicion["IdSector"];
                    er.IdNucleo = (string)posicion["IdNucleo"];
                    er.PosHMI = (uint)posicion["PosHMI"];
                    er.IdRecurso = r.IdRecurso;
                    er.TipoRecurso = (uint)posicion["TipoRecurso"];
                    er.Estado = "A";
                    er.IdDestino = r.IdDestino;
                    er.TipoDestino = 0;

                    GestorBDCD40.InsertSQL(er, null);
                    //er.InsertSQL();
                }
            }
        }

        [WebMethod(Description = "Elimina registros en EstadosRecursos para una lista de recursos que daban servicio a un destino radio concreto " +
                                    "y ya no lo dan.")]
        public void EliminaEstadosRecursosParaUnDestino(Tablas[] t, string destino)
        {
            EstadosRecursos er = new EstadosRecursos();

            foreach (RecursosRadio r in t)
            {
                er.IdSistema = r.IdSistema;
                er.IdRecurso = r.IdRecurso;
                er.IdDestino = destino;

                GestorBDCD40.DeleteSQL(er, null);
                //er.DeleteSQL();
            }
        }

        [WebMethod(Description = "Retorna el número de registros eliminados o un número negativo cuyo valor" +
                                " absoluto indica el código de excepción en caso de error.")]
        public int DeleteSQL(Tablas t)
        {
            //AccesoABaseDeDatos a = new AccesoABaseDeDatos();
            return GestorBDCD40.DeleteSQL(t, null);
        }

        [WebMethod(Description = "Retorna un data set con las incidencias de un sistema y si genera o no alarma.")]
        public DataSet IncidenciasMasAlarma(string idSistema, string idioma)
        {
            if (idioma == "es")
                return GestorBDCD40.GetDataSet(Tablas.SelectView("ViewIncidenciasMasAlarma", idSistema), null);

            return GestorBDCD40.GetDataSet(Tablas.SelectView("ViewIncidenciasMasAlarma_ingles", idSistema), null);
            //return Tablas.SelectView("ViewIncidenciasMasAlarma", idSistema);
        }

        [WebMethod(Description = "Retorna un data set (IdTop,IdNucleo,IdSector) con la asignación de usuarios a tops de una sectorización y sistema dados." +
                                "Si la top no tiene usuario asignado, IdNucleo e IdSector son null.")]
        public DataSet AsignacionDeUsuariosATops(string id_sistema, string id_sectorizacion)
        {
            return GestorBDCD40.GetDataSet(Tablas.SelectView("ViewSectoresEnTops", id_sistema, id_sectorizacion), null);
            //return Tablas.SelectView("ViewSectoresEnTops", id_sistema, id_sectorizacion);
        }

        [WebMethod(Description = "Retorna un data set (IdSistema,IdDestino,TipoDestino,IdGrupo,IdPrefijo,IdAbonado) con los destinos de un sistema")]
        public DataSet DestinosDeTelefoniaEnElSistema(string id_sistema)
        {
            return GestorBDCD40.GetDataSet(Tablas.SelectView("ViewDestinosTelefonia", id_sistema), null);
            //return Tablas.SelectView("ViewDestinosTelefonia", id_sistema);
        }

        [WebMethod(Description = "Retorna un data set (IdSistema,IdDestino,IdTop,IdSector) con la asignación de usuarios a tops de una sectorización y sistema dados." +
                                "Si la top no tiene usuario asignado, dSector es null.")]
        public DataSet SectoresEnTopsParaInformeXML(string id_sistema, string id_sectorizacion)
        {
            return GestorBDCD40.GetDataSet(Tablas.SelectView("SectoresEnTopsParaInformeXML", id_sistema, id_sectorizacion), null);
            //return Tablas.SelectView("SectoresEnTopsParaInformeXML", id_sistema, id_sectorizacion);
        }

        [WebMethod(Description = "Retorna una lista de Recursos libres que pueden ser asignados")]
        public System.Data.DataSet RecursosSinAsignar(string id, string id_sistema)
        {
            //Procedimientos p = new Procedimientos();
            return Procedimientos.RecursosSinAsignar(GestorBDCD40.ConexionMySql, id, id_sistema);
        }

        [WebMethod(Description = "Retorna una lista de Troncales sin asignar a rutas directas que pueden ser asignados")]
        public System.Data.DataSet TroncalesSinAsignarARutas(string id_sistema, string idCentral, string idRuta)
        {
            //Procedimientos p = new Procedimientos();
            return Procedimientos.TroncalesSinAsignarARutas(GestorBDCD40.ConexionMySql, id_sistema, idCentral, idRuta);
        }

        [WebMethod(Description = "Retorna una lista de Prefijos sin asignar a Redes")]
        public System.Data.DataSet PrefijosSinAsignarARedes(string id_sistema)
        {
            //Procedimientos p = new Procedimientos();
            return Procedimientos.PrefijosSinAsignarARedes(GestorBDCD40.ConexionMySql, id_sistema);
        }

        [WebMethod(Description = "Pasándole el identificador del sistema y el tipo de enlace (Interno o Instantáneo), retorna una lista de destinos compatibles" +
                                " con el tipo del enlace y sin asignar a ningún enlace")]
        public Tablas[] DestinosSinAsignarAEnlaces(string id_sistema, int tipoEnlace)
        {
            Tablas[] listaEnlaces;
            DataSet dsEnlaces;
            //Procedimientos p = new Procedimientos();
            ushort i = 0;

            switch ((Tipos.Tipo_Enlace)tipoEnlace)
            {
                case Tipos.Tipo_Enlace.TE_INTERNO:
                    dsEnlaces = Procedimientos.DestinosInternosSinAsignarAEnlaces(GestorBDCD40.ConexionMySql, id_sistema);
                    break;
                case Tipos.Tipo_Enlace.TE_LCEN:
                    dsEnlaces = Procedimientos.DestinosInstantaneosSinAsignarAEnlaces(GestorBDCD40.ConexionMySql, id_sistema);
                    break;
                default:
                    return null;
            }

            listaEnlaces = new Tablas[dsEnlaces.Tables[0].Rows.Count];

            foreach (DataRow rowEnlace in dsEnlaces.Tables[0].Rows)
            {
                DestinosTelefonia d = new DestinosTelefonia();
                d.IdSistema = (string)rowEnlace["IdSistema"];
                d.IdDestino = (string)rowEnlace["IdDestino"];
                d.TipoDestino = (uint)rowEnlace["TipoDestino"];
                d.IdPrefijo = (uint)rowEnlace["IdPrefijo"];
                if (rowEnlace["IdGrupo"] != System.DBNull.Value)
                    d.IdGrupo = (string)rowEnlace["IdGrupo"];

                listaEnlaces[i++] = d;
            }

            return listaEnlaces;
        }

        [WebMethod(Description = "Pasándole el identificador del sistema y el tipo de enlace (Radio, Interno o Instantáneo), retorna una lista de recursos compatibles" +
                                " con el tipo del enlace y sin asignar a ningún enlace")]
        public DataSet RecursosSinAsignarAEnlaces(string id_sistema, int tipoEnlace)
        {
            DataSet dsRecursos;
            //Procedimientos p = new Procedimientos();
            switch ((Tipos.Tipo_Enlace)tipoEnlace)
            {
                case Tipos.Tipo_Enlace.TE_EXTERNO:
                    dsRecursos = Procedimientos.RecursosRadioSinAsignarAEnlaces(GestorBDCD40.ConexionMySql, id_sistema);
                    break;
                case Tipos.Tipo_Enlace.TE_INTERNO:
                    dsRecursos = Procedimientos.RecursosTFSinAsignarAEnlaces(GestorBDCD40.ConexionMySql, id_sistema);
                    break;
                case Tipos.Tipo_Enlace.TE_LCEN:
                    dsRecursos = Procedimientos.RecursosLCENSinAsignarAEnlaces(GestorBDCD40.ConexionMySql, id_sistema);
                    break;
                default:
                    return null;
            }

            return dsRecursos;
        }

        [WebMethod(Description = "Pasándole el identificador del sistema y el tipo de enlace (Radio, Interno o Instantáneo), retorna una lista de recursos compatibles" +
                            " con el tipo del enlace y sin asignar a ningún enlace")]
        public Tablas[] RecursosSinAsignarAEnlaces1(string id_sistema, int tipoEnlace, string site)
        {
            Tablas[] listaRecursos;
            DataSet dsRecursos;
            ushort i = 0;
            //Procedimientos p = new Procedimientos();

            switch ((Tipos.Tipo_Enlace)tipoEnlace)
            {
                case Tipos.Tipo_Enlace.TE_EXTERNO:
                    //dsRecursos = Procedimientos.RecursosRadioSinAsignarAEnlaces(GestorBDCD40.ConexionMySql, id_sistema, site);
                    //MVO1
                    CD40.BD.Utilidades objUtil = new CD40.BD.Utilidades(GestorBDCD40.ConexionMySql);
                    dsRecursos = objUtil.GetRecursosRadioSinAsignarAEnlaces(id_sistema, site, null);
                    break;
                case Tipos.Tipo_Enlace.TE_INTERNO:
                    dsRecursos = Procedimientos.RecursosTFSinAsignarAEnlaces(GestorBDCD40.ConexionMySql, id_sistema);
                    break;
                case Tipos.Tipo_Enlace.TE_LCEN:
                    dsRecursos = Procedimientos.RecursosLCENSinAsignarAEnlaces(GestorBDCD40.ConexionMySql, id_sistema);
                    break;
                default:
                    return null;
            }

            listaRecursos = new Tablas[dsRecursos.Tables[0].Rows.Count];
            foreach (DataRow rowRecurso in dsRecursos.Tables[0].Rows)
            {
                Recursos d = new Recursos();
                d.IdSistema = (string)rowRecurso["IdSistema"];
                d.IdRecurso = (string)rowRecurso["IdRecurso"];
                d.Tipo = (uint)rowRecurso["Tipo"];
                listaRecursos[i++] = d;
            }

            return listaRecursos;
        }

        [WebMethod(Description = "Pasándole el identificador del sistema, el identificador del sector y la página del enlace a configurar, retorna una lista de destinos radio" +
                                " sin asignar a ningún enlace del sector y página.")]
        public List<string> DestinosRadioSinAsignarALaPaginaDelSector(string idSistema, string idSector, uint pagina)
        {
            //AccesoABaseDeDatos a = new AccesoABaseDeDatos();
            //Procedimientos p = new Procedimientos();
            uint numFrecPorPagina = 0;
            ParametrosSector s = new ParametrosSector();
            s.IdSistema = idSistema;
            s.IdSector = idSector;

            //List<Tablas> elSector = a.ListSelectSQL(s);
            List<Tablas> elSector = this.ListSelectSQL(s);
            if (elSector.Count > 0)
                numFrecPorPagina = ((ParametrosSector)elSector[0]).NumFrecPagina;

            if (numFrecPorPagina > 0)
                return Procedimientos.DestinosRadioSinAsignarALaPaginaDelSector(GestorBDCD40.ConexionMySql, idSistema, idSector, pagina - 1, numFrecPorPagina);

            return null;
        }

        [WebMethod(Description = "Pasándole el identificador del sistema, el identificador del sector y la página del enlace a configurar, retorna una lista de destinos radio" +
                            " asignados al sector y página correspondiente.")]
        public Tablas[] DestinosRadioAsignadosAPaginaSector(string idSistema, string idSector, uint pagina)
        {
            //AccesoABaseDeDatos a = new AccesoABaseDeDatos();
            uint numFrecPorPagina = 0;
            ParametrosSector s = new ParametrosSector();
            s.IdSistema = idSistema;
            s.IdSector = idSector;

            //List<Tablas> elSector = a.ListSelectSQL(s);
            List<Tablas> elSector = this.ListSelectSQL(s);
            if (elSector.Count > 0)
                numFrecPorPagina = ((ParametrosSector)elSector[0]).NumFrecPagina;

            if (numFrecPorPagina > 0)
            {
                //Procedimientos p = new Procedimientos();
                int i = 0;
                DataSet dsResultado = Procedimientos.DestinosRadioAsignadosASector(GestorBDCD40.ConexionMySql, idSistema, idSector, pagina - 1, numFrecPorPagina);

                Tablas[] listaDestinos = new Tablas[dsResultado.Tables[0].Rows.Count];
                foreach (DataRow dr in dsResultado.Tables[0].Rows)
                {
                    DestinosRadioSector r = new DestinosRadioSector();
                    if (dr["IdDestino"] != System.DBNull.Value)
                        r.IdDestino = (string)dr["IdDestino"];
                    if (dr["IdSistema"] != System.DBNull.Value)
                        r.IdSistema = (string)dr["IdSistema"];
                    if (dr["IdSector"] != System.DBNull.Value)
                        r.IdSector = (string)dr["IdSector"];
                    if (dr["IdNucleo"] != System.DBNull.Value)
                        r.IdNucleo = (string)dr["IdNucleo"];
                    if (dr["TipoDestino"] != System.DBNull.Value)
                        r.TipoDestino = (uint)dr["TipoDestino"];
                    if (dr["PosHMI"] != System.DBNull.Value)
                        r.PosHMI = (uint)dr["PosHMI"];
                    if (dr["Prioridad"] != System.DBNull.Value)
                        r.Prioridad = (uint)dr["Prioridad"];
                    if (dr["PrioridadSIP"] != System.DBNull.Value)
                        r.PrioridadSIP = (uint)dr["PrioridadSIP"];
                    if (dr["ModoOperacion"] != System.DBNull.Value)
                        r.ModoOperacion = (string)dr["ModoOperacion"];
                    if (dr["Cascos"] != System.DBNull.Value)
                        r.Cascos = (string)dr["Cascos"];
                    if (dr["Literal"] != System.DBNull.Value)
                        r.Literal = (string)dr["Literal"];

                    listaDestinos[i++] = (Tablas)r;
                }
                return listaDestinos;
            }

            return null;
        }

        [WebMethod(Description = "Pasándole el identificador del sistema y el identificador del sector, retorna una lista de destinos radio" +
                            " asignados al sector.")]
        public Tablas[] DestinosRadioAsignadosAlSector(string idSistema, string idSector)
        {
            // AccesoABaseDeDatos a = new AccesoABaseDeDatos();
            uint numFrecPorPagina = 0;
            ParametrosSector s = new ParametrosSector();
            s.IdSistema = idSistema;
            s.IdSector = idSector;

            //List<Tablas> elSector = a.ListSelectSQL(s);
            List<Tablas> elSector = this.ListSelectSQL(s);
            if (elSector.Count > 0)
                numFrecPorPagina = ((ParametrosSector)elSector[0]).NumFrecPagina;

            if (numFrecPorPagina > 0)
            {
                int i = 0;
                DestinosRadioSector dRadioSector = new DestinosRadioSector();
                dRadioSector.IdSistema = idSistema;
                dRadioSector.IdSector = idSector;
                DataSet dsResultado = this.DataSetSelectSQL(dRadioSector);

                Tablas[] listaDestinos = new Tablas[dsResultado.Tables[0].Rows.Count];
                foreach (DataRow dr in dsResultado.Tables[0].Rows)
                {
                    DestinosRadioSector r = new DestinosRadioSector();
                    if (dr["IdDestino"] != System.DBNull.Value)
                        r.IdDestino = (string)dr["IdDestino"];
                    if (dr["IdSistema"] != System.DBNull.Value)
                        r.IdSistema = (string)dr["IdSistema"];
                    if (dr["IdSector"] != System.DBNull.Value)
                        r.IdSector = (string)dr["IdSector"];
                    if (dr["IdNucleo"] != System.DBNull.Value)
                        r.IdNucleo = (string)dr["IdNucleo"];
                    if (dr["TipoDestino"] != System.DBNull.Value)
                        r.TipoDestino = (uint)dr["TipoDestino"];
                    if (dr["PosHMI"] != System.DBNull.Value)
                        r.PosHMI = (uint)dr["PosHMI"];
                    if (dr["Prioridad"] != System.DBNull.Value)
                        r.Prioridad = (uint)dr["Prioridad"];
                    if (dr["PrioridadSIP"] != System.DBNull.Value)
                        r.PrioridadSIP = (uint)dr["PrioridadSIP"];
                    if (dr["ModoOperacion"] != System.DBNull.Value)
                        r.ModoOperacion = (string)dr["ModoOperacion"];
                    if (dr["Cascos"] != System.DBNull.Value)
                        r.Cascos = (string)dr["Cascos"];
                    if (dr["Literal"] != System.DBNull.Value)
                        r.Literal = (string)dr["Literal"];
                    if (dr["SupervisionPortadora"] != System.DBNull.Value)
                        r.SupervisionPortadora = (bool)dr["SupervisionPortadora"];

                    listaDestinos[i++] = (Tablas)r;
                }
                return listaDestinos;
            }
            return null;
        }

        [WebMethod(Description = "Pasándole el identificador del sistema y el identificador del sector, retorna una lista de destinos de línea caliente" +
                            " sin asignar a ningún enlace del sector.")]
        public List<Tablas> DestinosLineaCalienteSinAsignarAlSector(string idSistema, string idSector, string idNucleo)
        {
            //Procedimientos p = new Procedimientos();
            return Procedimientos.DestinosLineaCalienteSinAsignarAlSector(GestorBDCD40.ConexionMySql, idSistema, idSector, idNucleo);
        }

        [WebMethod(Description = "Pasándole el identificador del sistema, devuelve la lista de destinos de línea caliente " +
                            " sin asignar a ningún enlace del sector.")]
        public List<Tablas> DestinosLineaCalienteSinAsignar(string idSistema)
        {
            //Procedimientos p = new Procedimientos();
            return Procedimientos.DestinosLineaCalienteSinAsignar(GestorBDCD40.ConexionMySql, idSistema);
        }

        [WebMethod(Description = "Pasándole el identificador del sistema y el identificador del sector, retorna una lista de destinos de telefonía" +
                            " sin asignar a ningún enlace del sector.")]
        public List<Tablas> DestinosTelefoniaSinAsignarAlSector(string idSistema, string idSector, string idNucleo)
        {
            //Procedimientos p = new Procedimientos();
            return Procedimientos.DestinosTelefoniaSinAsignarAlSector(GestorBDCD40.ConexionMySql, idSistema, idSector, idNucleo);
        }

        [WebMethod(Description = "Pasándole el identificador del sistema y el identificador del sector, retorna una lista de destinos de telefonía" +
                                " sin asignar a ningún enlace del sector.")]
        public Tablas[] DestinosTelefoniaAsignadosAlSector(string idSistema, string idSector, bool telefonia, bool internos)
        {
            //Procedimientos p = new Procedimientos();
            DataSet dsResultado = Procedimientos.DestinosTelefoniaAsignadosAlSector(GestorBDCD40.ConexionMySql, idSistema, idSector, telefonia, internos);

            int i = 0;

            Tablas[] listaDestinos = new Tablas[dsResultado.Tables[0].Rows.Count];

            if (internos)
            {
                foreach (System.Data.DataRow dr in dsResultado.Tables[0].Rows)
                {
                    DestinosInternosSector r = new DestinosInternosSector();
                    if (dr["IdSistema"] != System.DBNull.Value)
                        r.IdSistema = (string)dr["IdSistema"];
                    if (dr["IdDestino"] != System.DBNull.Value)
                        r.IdDestino = (string)dr["IdDestino"];
                    if (dr["TipoDestino"] != System.DBNull.Value)
                        r.TipoDestino = (uint)dr["TipoDestino"];
                    if (dr["IdNucleo"] != System.DBNull.Value)
                        r.IdNucleo = (string)dr["IdNucleo"];
                    if (dr["IdSector"] != System.DBNull.Value)
                        r.IdSector = (string)dr["IdSector"];
                    if (dr["IdPrefijo"] != System.DBNull.Value)
                        r.IdPrefijo = (uint)dr["IdPrefijo"];
                    if (dr["PosHMI"] != System.DBNull.Value)
                        r.PosHMI = (uint)dr["PosHMI"];
                    if (dr["Prioridad"] != System.DBNull.Value)
                        r.Prioridad = (uint)dr["Prioridad"];
                    if (dr["OrigenR2"] != System.DBNull.Value)
                        r.OrigenR2 = (string)dr["OrigenR2"];
                    if (dr["PrioridadSIP"] != System.DBNull.Value)
                        r.PrioridadSIP = (uint)dr["PrioridadSIP"];
                    if (dr["TipoAcceso"] != System.DBNull.Value)
                        r.TipoAcceso = (string)dr["TipoAcceso"];
                    if (dr["Literal"] != System.DBNull.Value)
                        r.Literal = (string)dr["Literal"];

                    listaDestinos[i++] = (Tablas)r;
                }
            }
            else
            {
                foreach (System.Data.DataRow dr in dsResultado.Tables[0].Rows)
                {
                    DestinosExternosSector r = new DestinosExternosSector();

                    if (dr["IdSistema"] != System.DBNull.Value)
                        r.IdSistema = (string)dr["IdSistema"];
                    if (dr["IdDestino"] != System.DBNull.Value)
                        r.IdDestino = (string)dr["IdDestino"];
                    if (dr["TipoDestino"] != System.DBNull.Value)
                        r.TipoDestino = (uint)dr["TipoDestino"];
                    if (dr["IdNucleo"] != System.DBNull.Value)
                        r.IdNucleo = (string)dr["IdNucleo"];
                    if (dr["IdSector"] != System.DBNull.Value)
                        r.IdSector = (string)dr["IdSector"];
                    if (dr["IdPrefijo"] != System.DBNull.Value)
                        r.IdPrefijo = (uint)dr["IdPrefijo"];
                    if (dr["PosHMI"] != System.DBNull.Value)
                        r.PosHMI = (uint)dr["PosHMI"];
                    if (dr["Prioridad"] != System.DBNull.Value)
                        r.Prioridad = (uint)dr["Prioridad"];
                    if (dr["OrigenR2"] != System.DBNull.Value)
                        r.OrigenR2 = (string)dr["OrigenR2"];
                    if (dr["PrioridadSIP"] != System.DBNull.Value)
                        r.PrioridadSIP = (uint)dr["PrioridadSIP"];
                    if (dr["TipoAcceso"] != System.DBNull.Value)
                        r.TipoAcceso = (string)dr["TipoAcceso"];
                    if (dr["Literal"] != System.DBNull.Value)
                        r.Literal = (string)dr["Literal"];
                    if (dr["IdPrefijoDestinoLCEN"] != System.DBNull.Value)
                        r.IdPrefijoDestinoLCEN = (uint)dr["IdPrefijoDestinoLCEN"];
                    if (dr["IdDestinoLCEN"] != System.DBNull.Value)
                        r.IdDestinoLCEN = (string)dr["IdDestinoLCEN"];
                    listaDestinos[i++] = (Tablas)r;
                }
            }

            return listaDestinos;
        }

        [WebMethod(Description = "Pasándole el identificador del sistema y el identificador del sector y el identificador de la sectorizacion," +
                            " retorna una lista de destinos de telefonía o línea caliente, internos o externos asignados al sector.")]
        public Tablas[] DestinosTelefoniaSectorizados(string idSistema, string idUsuario, string idSectorizacion, bool internos)
        {
            int i = 0;
            Tablas[] listaDestinos = null;

            DataSet dsResultado = Procedimientos.DestinosTelefoniaSectorizados(GestorBDCD40.ConexionMySql, idSistema, idUsuario, idSectorizacion, internos);

            if (null != dsResultado && dsResultado.Tables.Count > 0)
            {
                listaDestinos = new Tablas[dsResultado.Tables[0].Rows.Count];

                if (internos)
                {
                    //Destinos de telefonía internos
                    foreach (System.Data.DataRow dr in dsResultado.Tables[0].Rows)
                    {
                        Internos r = new Internos();
                        if (dr["IdSistema"] != System.DBNull.Value)
                            r.IdSistema = (string)dr["IdSistema"];
                        if (dr["IdSectorizacion"] != System.DBNull.Value)
                            r.IdSectorizacion = (string)dr["IdSectorizacion"];
                        if (dr["IdNucleo"] != System.DBNull.Value)
                            r.IdNucleo = (string)dr["IdNucleo"];
                        if (dr["IdSector"] != System.DBNull.Value)
                            r.IdSector = (string)dr["IdSector"];
                        if (dr["PosHMI"] != System.DBNull.Value)
                            r.PosHMI = (uint)dr["PosHMI"];
                        if (dr["Prioridad"] != System.DBNull.Value)
                            r.Prioridad = (uint)dr["Prioridad"];
                        if (dr["OrigenR2"] != System.DBNull.Value)
                            r.OrigenR2 = (string)dr["OrigenR2"];
                        if (dr["PrioridadSIP"] != System.DBNull.Value)
                            r.PrioridadSIP = (uint)dr["PrioridadSIP"];
                        if (dr["TipoAcceso"] != System.DBNull.Value)
                            r.TipoAcceso = (string)dr["TipoAcceso"];
                        if (dr["Literal"] != System.DBNull.Value)
                            r.Literal = (string)dr["Literal"];

                        if (dr["IdPrefijo"] != System.DBNull.Value)
                            r.IdPrefijo = (uint)dr["IdPrefijo"];

                        if (dr["IdDestino"] != System.DBNull.Value)
                            r.IdDestino = (string)dr["IdDestino"];

                        listaDestinos[i++] = (Tablas)r;
                    }
                }
                else
                {
                    //Destinos de telefonía externos
                    foreach (System.Data.DataRow dr in dsResultado.Tables[0].Rows)
                    {
                        Externos r = new Externos();
                        if (dr["IdSistema"] != System.DBNull.Value)
                            r.IdSistema = (string)dr["IdSistema"];
                        if (dr["IdSectorizacion"] != System.DBNull.Value)
                            r.IdSectorizacion = (string)dr["IdSectorizacion"];
                        if (dr["IdNucleo"] != System.DBNull.Value)
                            r.IdNucleo = (string)dr["IdNucleo"];
                        if (dr["IdSector"] != System.DBNull.Value)
                            r.IdSector = (string)dr["IdSector"];
                        if (dr["PosHMI"] != System.DBNull.Value)
                            r.PosHMI = (uint)dr["PosHMI"];
                        if (dr["Prioridad"] != System.DBNull.Value)
                            r.Prioridad = (uint)dr["Prioridad"];
                        if (dr["OrigenR2"] != System.DBNull.Value)
                            r.OrigenR2 = (string)dr["OrigenR2"];
                        if (dr["PrioridadSIP"] != System.DBNull.Value)
                            r.PrioridadSIP = (uint)dr["PrioridadSIP"];
                        if (dr["TipoAcceso"] != System.DBNull.Value)
                            r.TipoAcceso = (string)dr["TipoAcceso"];
                        if (dr["Literal"] != System.DBNull.Value)
                            r.Literal = (string)dr["Literal"];

                        if (dr["IdPrefijo"] != System.DBNull.Value)
                            r.IdPrefijo = (uint)dr["IdPrefijo"];

                        if (dr["IdDestino"] != System.DBNull.Value)
                            r.IdDestino = (string)dr["IdDestino"];

                        listaDestinos[i++] = (Tablas)r;
                    }
                }
            }

            return listaDestinos;
        }

        [WebMethod(Description = "Pasándole el identificador del sistema y el identificador del sector y el identificador de la sectorizacion," +
                            " retorna una lista de destinos radio asignados al sector.")]
        public Tablas[] DestinosRadioSectorizados(string idSistema, string idUsuario, string idSectorizacion)
        {
            //Procedimientos p = new Procedimientos();
            DataSet dsResultado = Procedimientos.DestinosRadioSectorizados(GestorBDCD40.ConexionMySql, idSistema, idUsuario, idSectorizacion);
            int i = 0;

            Tablas[] listaDestinos = new Tablas[dsResultado.Tables[0].Rows.Count];
            foreach (System.Data.DataRow dr in dsResultado.Tables[0].Rows)
            {
                Radio r = new Radio();
                if (dr["IdSistema"] != System.DBNull.Value)
                    r.IdSistema = (string)dr["IdSistema"];
                if (dr["IdSectorizacion"] != System.DBNull.Value)
                    r.IdSectorizacion = (string)dr["IdSectorizacion"];
                if (dr["IdNucleo"] != System.DBNull.Value)
                    r.IdNucleo = (string)dr["IdNucleo"];
                if (dr["IdSector"] != System.DBNull.Value)
                    r.IdSector = (string)dr["IdSector"];
                if (dr["PosHMI"] != System.DBNull.Value)
                    r.PosHMI = (uint)dr["PosHMI"];
                if (dr["Prioridad"] != System.DBNull.Value)
                    r.Prioridad = (uint)dr["Prioridad"];
                if (dr["PrioridadSIP"] != System.DBNull.Value)
                    r.PrioridadSIP = (uint)dr["PrioridadSIP"];
                if (dr["ModoOperacion"] != System.DBNull.Value)
                    r.ModoOperacion = (string)dr["ModoOperacion"];
                if (dr["Cascos"] != System.DBNull.Value)
                    r.Cascos = (string)dr["Cascos"];
                if (dr["Literal"] != System.DBNull.Value)
                    r.Literal = (string)dr["Literal"];

                listaDestinos[i++] = (Tablas)r;
            }
            return listaDestinos;
        }

        [WebMethod(Description = "Desencadena proceso multicast para activación de una sectorización")]
        public bool ComunicaSectorizacionActiva(string listenIP, string id_sistema, string id_sectorizacion, ref DateTime fechaActivacion)
        {
            System.Configuration.Configuration webConfiguracion = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            string toSincro = webConfiguracion.AppSettings.Settings["SincronizaCD30"].Value;
            string toServerMantto = webConfiguracion.AppSettings.Settings["ServerManttoIp"].Value;

            CD40.BD.Utilidades util = new CD40.BD.Utilidades(GestorBDCD40ToMantto.ConexionMySql);
            //CD40.BD.Utilidades.StartSnmp(listenIP, toServerMantto);

            CD40.BD.GestorBaseDatos.logFile.Debug(string.Format("Inicio ejecución ServiciosCD40.ComunicaSectorizacionActiva: listenIP={0} id_sistema={1} id_sectorizacion={2} fechaActivacion={3}", listenIP, id_sistema, id_sectorizacion, fechaActivacion.ToString()));

            if (toSincro == "0" || EstadoCD30Activo)
            {
                byte iEstadoSacta = 0;
                bool sactaPresente = false;

                iEstadoSacta = GetEstadoSacta();

                if (iEstadoSacta == 0 || iEstadoSacta == 16)
                {
                    // Servicio no arrancado (iEstadoSacta==0) o servicio arrancado pero sin conexion con SACTA (iEstadoSacta == 16)
                    sactaPresente = false;
                }
                else
                    sactaPresente = true;

                if (!sactaPresente || id_sectorizacion == "SACTA")  // No hay enlace con SACTA o la sectorización es de SACTA
                {
                    if (id_sectorizacion != "SACTA")
                    {
                        SectoresSectorizacion sSectorizacion = new SectoresSectorizacion();
                        sSectorizacion.IdSistema = id_sistema;
                        sSectorizacion.IdSectorizacion = id_sectorizacion;
                        List<Tablas> sectores = GestorBDCD40.ListSelectSQL(sSectorizacion, null);
                        if (sectores.Count == 0)
                        {
                            string[] parametros = { id_sectorizacion };
                            util.CreaEventoConfiguracion(id_sistema, 108, parametros, toServerMantto);

                            fechaActivacion = DateTime.MinValue;

                            CD40.BD.GestorBaseDatos.logFile.Debug("ComunicaSectorizacionActiva: Sectores.count=0, return false");

                            //CD40.BD.Utilidades.EndSnmp();
                            return false;
                        }
                    }

                    CD40.BD.GestorBaseDatos.logFile.Debug("ComunicaSectorizacionActiva: Se comprueba si están todos los sectores reales");
                    if (!EstanTodosLosReales(id_sistema, id_sectorizacion))
                    {

                        string[] parametros = { id_sectorizacion };
                        util.CreaEventoConfiguracion(id_sistema, 108, parametros, toServerMantto);

                        fechaActivacion = DateTime.MinValue;

                        CD40.BD.GestorBaseDatos.logFile.Debug("ComunicaSectorizacionActiva: No están todos los sectores reales. Return false");
                        //CD40.BD.Utilidades.EndSnmp();
                        return false;
                    }

                    try
                    {
                        CD40.BD.GestorBaseDatos.logFile.Debug(string.Format("ComunicaSectorizacionActiva: se activa la sectorización en la BD. Se llama a Utilidades.ActualizaActiva(GestorBDCD40.ConexionMySql, id_sistema, id_sectorizacion, fechaActivacion, {0})", sactaPresente));

                        // Activa una sectorizacion en la base de datos.
                        Utilidades.ActualizaActiva(GestorBDCD40.ConexionMySql, id_sistema, id_sectorizacion, fechaActivacion, sactaPresente);
                    }
                    catch (System.Web.Services.Protocols.SoapException ex)
                    {
                        CD40.BD.GestorBaseDatos.logFile.Debug("ComunicaSectorizacionActiva: error (SoapException) al activar la sectorización en la BD", ex);
                        // resetear la conexion y volverlo a intentear
                        GestorBDCD40.ConexionMySql.Close();

                        CD40.BD.GestorBaseDatos.logFile.Debug("ComunicaSectorizacionActiva: conexión cerrada con la BD");

                        CD40.BD.GestorBaseDatos.logFile.Debug(string.Format("ComunicaSectorizacionActiva: se vuelve a invocar a Utilidades.ActualizaActiva(GestorBDCD40.ConexionMySql, id_sistema, id_sectorizacion, fechaActivacion, {0})", sactaPresente));
                        Utilidades.ActualizaActiva(GestorBDCD40.ConexionMySql, id_sistema, id_sectorizacion, fechaActivacion, sactaPresente);
                    }

                    CD40.BD.GestorBaseDatos.logFile.Debug("ComunicaSectorizacionActiva: se hace commit en la BD");
                    // Consolidar las modificaciones en la base de datos activa
                    Commit();

                    CD40.BD.GestorBaseDatos.logFile.Debug("ComunicaSectorizacionActiva: commit OK");
                }
                else            // La sectorización es generada desde configuración pero con enlace SACTA
                {
                    CD40.BD.GestorBaseDatos.logFile.Debug(string.Format("ComunicaSectorizacionActiva false==((!sactaPresente || id_sectorizacion == 'SACTA')): se llama a GenerarSectorizacion({0},{1},false)", id_sistema, id_sectorizacion));

                    GenerarSectorizacion(id_sistema, id_sectorizacion, false);

                    CD40.BD.GestorBaseDatos.logFile.Debug(string.Format("ComunicaSectorizacionActiva: se llama a Utilidades.ActualizaActiva(GestorBDCD40.ConexionMySql,{0},{1},{2})", id_sistema, id_sectorizacion, fechaActivacion.ToString()));

                    Utilidades.ActualizaActiva(GestorBDCD40.ConexionMySql, id_sistema, id_sectorizacion, fechaActivacion, true);

                    CD40.BD.GestorBaseDatos.logFile.Debug("ComunicaSectorizacionActiva: se hace commit en la BD");
                    // Consolidar las modificaciones en la base de datos activa
                    Commit();

                    CD40.BD.GestorBaseDatos.logFile.Debug("ComunicaSectorizacionActiva: commit OK");
                }

                CD40.BD.GestorBaseDatos.logFile.Debug("ComunicaSectorizacionActiva: Se notifica la sectorizacion");
                bool retorno = Utilidades.NotificaSectorizacion(GestorBDCD40, GestorBDCD40ToMantto, listenIP, id_sistema, id_sectorizacion, out fechaActivacion, toServerMantto);

                //CD40.BD.Utilidades.EndSnmp();
                //VMG Genera el JSON
                //Utilidades.GeneraFicheroCnfParaProxy(GestorBDCD40.ConexionMySql, id_sistema);

                CD40.BD.GestorBaseDatos.logFile.Debug("ServiciosCD40.ComunicaSectorizacionActiva: Fin ejecución. Valor de retorno=" + retorno);

                return retorno;
            }
            else if (toSincro == "1")
            {
                util.CreaEventoConfiguracion(id_sistema, 113);

                //CD40.BD.Utilidades.EndSnmp();
                return false;
            }

            CD40.BD.GestorBaseDatos.logFile.Debug("ServiciosCD40.ComunicaSectorizacionActiva: Fin ejecución. Valor de retorno=false");
            //CD40.BD.Utilidades.EndSnmp();
            return false;
        }

        [WebMethod(Description = "Comunica via multicast a los elementos hardware el cambio de sistema activo.")]
        // scvActivo: [1|2]
        public bool ComunicaSistemaActivo(string id_sistema, string scvActivo)
        {
            System.Configuration.Configuration webConfiguracion = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            string toSincro = webConfiguracion.AppSettings.Settings["ListenIP"].Value;

            CD40.BD.Utilidades util = new CD40.BD.Utilidades(GestorBDCD40ToMantto.ConexionMySql);
            string[] parametros = new string[1];
            //if (toSincro == "1")
            //    parametros[0] = scvActivo == "1" ? "CD40" : "CD30";
            //else
            //    parametros[0] = scvActivo == "1" ? "A" : "B";

            parametros[0] = scvActivo;
            util.CreaEventoConfiguracion(id_sistema, 101, parametros);

            return Utilidades.NotificaCambioSistemaActivo(toSincro, GestorBDCD40, id_sistema, scvActivo);
        }

        [WebMethod]
        public void GeneraHistoricoSectorAsignado(string id_sistema, bool asignado, string sector, string posicion)
        {
            System.Configuration.Configuration webConfiguracion = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            string toServerMantto = webConfiguracion.AppSettings.Settings["ServerManttoIp"].Value;

            string[] parametros = new string[2];
            parametros[0] = sector;
            parametros[1] = posicion;
            CD40.BD.Utilidades util = new CD40.BD.Utilidades(GestorBDCD40.ConexionMySql);

            util.CreaEventoConfiguracion(id_sistema, (uint)(asignado == true ? 111 : 112), parametros, toServerMantto);
        }

        [WebMethod(Description = "Comprobar si la sectorización pasada como parámetro contiene todos los sectores reales.")]
        public bool EstanTodosLosReales(string id_sistema, string id_sectorizacion)
        {
            /* JCAM*/
            return Procedimientos.TodosLosSectoresReales(GestorBDCD40.ConexionMySql, id_sistema, id_sectorizacion);
        }

        [WebMethod(Description = "Retorna una lista de identificadores de sector que no han sido añadidos a la sectorizacion.")]
        public DataSet SectoresSinAsignarASectorizacion(string id_sistema, string id_nucleo, string id_sectorizacion)
        {
            //Procedimientos p = new Procedimientos();
            return Procedimientos.SectoresSinAsignarASectorizacion(GestorBDCD40.ConexionMySql, id_sistema, id_nucleo, id_sectorizacion);
        }

        [WebMethod(Description = "Retorna una lista con las tifx posibles para asignar un recurso en función del tipo de interfaz del mismo.")]
        public string[] SlotsLibresEnTifX(string id_sistema, int cuantos)
        {
            // Procedimientos p = new Procedimientos();
            return Procedimientos.SlotsLibresEnTifX(GestorBDCD40.ConexionMySql, id_sistema, cuantos);
        }

        [WebMethod(Description = "Retorna una cadena con el nombre de la agrupacion compuesta por los sectores pasados o null si no" +
                                " existe ninguna agrupación.")]
        public string GetAgrupacion(string idSistema, string idNucleo, int cuantos, string listaUsuarios)
        {
            CD40.BD.Utilidades u = new CD40.BD.Utilidades(GestorBDCD40.ConexionMySql);
            string nombre = u.GetAgrupacion(cuantos, listaUsuarios);
            if (nombre == null)
            {
                nombre = GeneraAlgoritmo(idSistema, idNucleo, listaUsuarios, null);
            }

            return nombre;
        }

        [WebMethod(Description = "Retorna el valor mínimo de la PrioridadR2 (máxima prioridad) de los usuarios que componen un sector.")]
        public uint GetPrioridadSector(string id_sistema, string id_nucleo, string listaUsuarios)
        {
            CD40.BD.Utilidades u = new CD40.BD.Utilidades(GestorBDCD40.ConexionMySql);
            return u.GetPrioridadSector(id_sistema, id_nucleo, listaUsuarios);
        }

        [WebMethod(Description = "Servicio encargado de llamar a la elaboración de la sectorización")]
        public void GenerarSectorizacion(string idSistema, string idSectorizacion, bool comprobarSectoresReales)
        {
            // Generar la nueva sectorizacion
            CD40.BD.Utilidades ut = new CD40.BD.Utilidades(GestorBDCD40.ConexionMySql);
            // ut.GeneraSectorizacion(idSistema, idSectorizacion, GeneraSectorizacionDll.Tipo_Sectorizacion.Sectorizacion_Completa, comprobarSectoresReales, GetEstadoSacta() != 0);
            ut.GeneraSectorizacion(idSistema, idSectorizacion, GeneraSectorizacionDll.Tipo_Sectorizacion.Sectorizacion_Completa, comprobarSectoresReales, false);
        }

        [WebMethod(Description = "Servicio encargado de llamar a la elaboración de la sectorización para el 1+1")]
        public bool SectorizacionSincroCD30CD40(string listenIp, string idSistema, string idSectorizacion, string datos)
        {
            // Generar la nueva sectorizacion
            CD40.BD.Utilidades ut = new CD40.BD.Utilidades(GestorBDCD40.ConexionMySql);
            int s = (int)ut.GeneraSectorizacion(idSistema, idSectorizacion, datos);
            if (s == 0)
            {  //Activar la sectorizacion
                DateTime ahora = DateTime.Now;
                return ComunicaSectorizacionActiva(listenIp, idSistema, idSectorizacion, ref ahora);
            }

            return false;
        }

        [WebMethod(Description = "Actualiza las sectorizaciones existentes después de una modificación en los usuarios")]
        public int RegeneraSectorizaciones(string idSistema, bool completa, bool radio, bool telefonia)
        {
            DataSet dsSectorizaciones;
            Sectorizaciones s = new Sectorizaciones();
            CD40.BD.Utilidades ut = new CD40.BD.Utilidades(GestorBDCD40.ConexionMySql);
            int iNumSectorizaciones = 0;

            GeneraSectorizacionDll.Tipo_Sectorizacion tipoSectorizacion = GeneraSectorizacionDll.Tipo_Sectorizacion.Sectorizacion_Completa;

            CD40.BD.GestorBaseDatos.logFile.Debug(string.Format("Inicio RegeneraSectorizaciones: sistema={0} completa={1}  radio={2} telefonia={3}", idSistema, completa, radio, telefonia));

            if (completa)
                tipoSectorizacion = GeneraSectorizacionDll.Tipo_Sectorizacion.Sectorizacion_Completa;
            else
            {
                if (radio)
                    tipoSectorizacion = GeneraSectorizacionDll.Tipo_Sectorizacion.Sectorizacion_Radio;
                if (telefonia)
                    tipoSectorizacion = GeneraSectorizacionDll.Tipo_Sectorizacion.Sectorizacion_Telefonia;
            }

            lock (_Sync)
            {
                s.IdSistema = idSistema;
                dsSectorizaciones = DataSetSelectSQL(s);

                if (dsSectorizaciones != null && dsSectorizaciones.Tables.Count > 0)
                {
                    CD40.BD.GestorBaseDatos.logFile.Debug(string.Format("RegeneraSectorizaciones: Se regeneran todas las sectorizaciones excepto la activa en el SCV (IdSectorizacion==FechaActivacion).Tipo Sectorizacion={0} Numero={1}", tipoSectorizacion, dsSectorizaciones.Tables[0].Rows.Count));
                    for (int j = 0; j < dsSectorizaciones.Tables[0].Rows.Count; j++)
                    {
                        // Se regeneran todas las sectorizaciones excepto la activa en el SCV (IdSectorizacion==FechaActivacion)
                        if (((DateTime)dsSectorizaciones.Tables[0].Rows[j]["FechaActivacion"]).ToString("dd/MM/yyyy HH:mm:ss") != (string)dsSectorizaciones.Tables[0].Rows[j]["IdSectorizacion"])
                        {
                            CD40.BD.GestorBaseDatos.logFile.Debug(string.Format("RegeneraSectorizaciones j={3}: Se llama a utilidades.GeneraSectorizacion({0},{1},{2},false)", idSistema, (string)dsSectorizaciones.Tables[0].Rows[j]["IdSectorizacion"], tipoSectorizacion, j));

                            ut.GeneraSectorizacion(idSistema, (string)dsSectorizaciones.Tables[0].Rows[j]["IdSectorizacion"], tipoSectorizacion, false);
                        }
                    }

                    iNumSectorizaciones = dsSectorizaciones.Tables[0].Rows.Count;
                }
            }

            CD40.BD.GestorBaseDatos.logFile.Debug(string.Format("Fin ejecución RegeneraSectorizaciones: iNumSectorizaciones={0}", iNumSectorizaciones));
            return iNumSectorizaciones;
        }

        [WebMethod(Description = "Actualiza los Parámetros y los números de abonado de los sectores que componen una agrupación")]
        public void RegeneraParametrosAgrupacion(string idSistema, string idNucleo, string idAgrupacion)
        {
            // CD40.BD.Procedimientos p = new Procedimientos();

            MySql.Data.MySqlClient.MySqlTransaction trans = GestorBDCD40.StartTransaction(true);

            // Componer los ParametrosSector del sector agrupado
            DataSet listaParametros = Procedimientos.ParametrosSectorAgrupado(GestorBDCD40.ConexionMySql, trans, idSistema, idAgrupacion);

            ParametrosSector ps = Utilidades.RegeneraParametrosSector(listaParametros);

            ps.IdSistema = idSistema;
            ps.IdNucleo = idNucleo;
            ps.IdSector = idAgrupacion;

            /** AGL. ID.83/84 Como daba problemas elimino la transacción */
            /**  MySql.Data.MySqlClient.MySqlTransaction trans = GestorBDCD40.StartTransaction(true); */
            //MySql.Data.MySqlClient.MySqlTransaction trans = null;
            /** Fin de Modificacion */
            try
            {
                GestorBDCD40.DeleteSQL(ps, trans);
                GestorBDCD40.InsertSQL(ps, trans);

                // Componer los números de abonado del sector agrupado
                DataSet listaAbonados = Procedimientos.NumerosAbonadosSectorAgrupado(GestorBDCD40.ConexionMySql, trans, idSistema, idAgrupacion);

                UsuariosAbonados ua = new UsuariosAbonados();
                ua.IdSistema = idSistema;
                ua.IdNucleo = idNucleo;
                ua.IdSector = idAgrupacion;
                GestorBDCD40.DeleteSQL(ua, trans);

                foreach (DataRow la in listaAbonados.Tables[0].Rows)
                {
                    ua.IdPrefijo = (uint)la["IdPrefijo"];
                    ua.IdAbonado = (string)la["IdAbonado"];

                    GestorBDCD40.InsertSQL(ua, trans);
                }

                /** AGL ID.83/84
                 * Para controlar la transaccion null */
                //if (trans != null)
                /** Fin de Modificacion */
                GestorBDCD40.Commit(trans);
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                /** AGL ID.83/84 
                 * Para controlar la transacción null */
                //if (trans != null)
                /** Fin de modificacion */
                GestorBDCD40.RollBack(trans);
            }
        }

        [WebMethod(Description = "Obtiene el primer IdSacta para mantenimiento (mayor que 10000) sin asignar a ningún sector")]
        public int GetIdSactaMantenimiento(string idSistema)
        {
            //Procedimientos p = new Procedimientos();

            return Procedimientos.IdManttoLibre(GestorBDCD40.ConexionMySql, idSistema);
        }

        [WebMethod(Description = "Obtener los históricos que cumplan las condiciones del filtro")]
        public DataSet GetHistoricos(string idSistema, int scv, string listaIncidencias, int tipo, string idHw, DateTime desde, DateTime hasta)
        {
            //Procedimientos p = new Procedimientos();

            return Procedimientos.GetHistoricos(GestorBDCD40.ConexionMySql, idSistema, scv, listaIncidencias, tipo, idHw, desde, hasta);
        }

        [WebMethod(Description = "Retorna los sectores que no están en una agrupación dada o todos si la agrupación es null")]
        public DataSet GetSectoresFueraDeAgrupacion(string idSistema, string idAgrupacion)
        {
            //Procedimientos p = new Procedimientos();

            return Procedimientos.SectoresFueraDeAgrupacion(GestorBDCD40.ConexionMySql, idSistema, idAgrupacion);
        }

        [WebMethod(Description = "Retorna los números de abonado a los que atiende un sector con el nombre de la red correspondiente")]
        public DataSet RedesUsuariosAbonados(string idSistema, string idNucleo, string idSector)
        {
            //Procedimientos p = new Procedimientos();
            return Procedimientos.RedesUsuariosAbonados(GestorBDCD40.ConexionMySql, idSistema, idNucleo, idSector);
        }

        [WebMethod(Description = "Retorna los rangos de una central ATS con el identificador de la red del número abonado asociado")]
        public DataSet RangosConIdRed(string idSistema, string idCentral)
        {
            //Procedimientos p = new Procedimientos();

            return Procedimientos.RangosConIdRed(GestorBDCD40.ConexionMySql, idSistema, idCentral);
        }

        [WebMethod(Description = "Retorna el nombre de la central ATS definida como propia o null si no existe ninguna central propia.")]
        public string ExisteCentralPropia(string idSistema)
        {
            Encaminamientos e = new Encaminamientos();
            e.IdSistema = idSistema;

            string retorno = Convert.ToString(GestorBDCD40.ExecuteScalar(e.CentralPropiaSQL()));
            if (retorno != null && retorno != "")
                return retorno;


            return null;
        }

        [WebMethod(Description = "Actualiza el nombre de un sector con un nuevo nombre. Esto provoca una actualización de las sectorizaciones" +
                                " en las que este sector estuviese implicado.")]
        public void ActualizaNombreSector(Sectores s, string nuevoNombre)
        {
            Procedimientos.ActualizaNombreSector(GestorBDCD40.ConexionMySql, null, s.IdSistema, s.IdNucleo, s.IdSector, nuevoNombre);
        }

        [WebMethod]
        public string GeneraAlgoritmo(string idSistema, string idNucleo, string listaSectores)
        {
            return GeneraAlgoritmo(idSistema, idNucleo, listaSectores, null);
        }

        [WebMethod]
        public string ExisteSectorConNumeroAbonado(string idSistema, string idNucleo, uint idPrefijo, string idAbonado)
        {
            //CD40.BD.Procedimientos p = new Procedimientos();

            string[] listaSectores = Procedimientos.SectorConNumeroAbonado(GestorBDCD40.ConexionMySql, idSistema, idNucleo, idPrefijo, idAbonado);

            if (listaSectores != null && listaSectores.Length > 0)
                return listaSectores[0];
            else
                return "";
        }

        [WebMethod(Description = "Inserta en el usuario destino del colateral una posición que coincida con la original o, " +
                               "en su defecto, la primera libre cuyo destino sea el usuario original del colateral.")]
        public bool InsertaColateralEnUsuarioReciproco(ref DestinosInternosSector destino, int ultimaPosicion)
        {
            uint posicion = 0;
            //Procedimientos p = new Procedimientos();

            DestinosInternosSector di = new DestinosInternosSector();
            di.IdSistema = destino.IdSistema;
            di.IdNucleo = destino.IdNucleo;
            di.IdDestino = destino.IdSector;
            di.Literal = destino.IdSector;
            di.IdSector = destino.IdDestino;
            di.OrigenR2 = destino.IdDestino;
            di.Prioridad = destino.Prioridad;
            di.PrioridadSIP = destino.PrioridadSIP;
            di.TipoDestino = destino.TipoDestino;
            di.TipoAcceso = destino.TipoAcceso;
            di.IdPrefijo = destino.IdPrefijo;

            if (Procedimientos.PosicionOcupadaEnSector(GestorBDCD40.ConexionMySql, destino.IdSistema, destino.IdNucleo, destino.IdDestino, destino.TipoAcceso, destino.PosHMI))
            {
                if (Procedimientos.PrimeraPosicionLibre(GestorBDCD40.ConexionMySql, destino.IdSistema, destino.IdNucleo, destino.IdDestino, destino.TipoAcceso, out posicion))
                {
                    if (posicion != 0 && posicion < ultimaPosicion)
                    {
                        di.PosHMI = destino.PosHMI = posicion;
                        destino.IdDestino = di.IdDestino;
                        destino.Literal = di.Literal;
                        destino.IdSector = di.IdSector;
                        destino.OrigenR2 = di.OrigenR2;

                        return GestorBDCD40.InsertSQL(di, null) > 0;
                    }
                }

                return false;
            }

            di.PosHMI = destino.PosHMI;
            destino.IdDestino = di.IdDestino;
            destino.Literal = di.Literal;
            destino.IdSector = di.IdSector;
            destino.OrigenR2 = di.OrigenR2;

            return GestorBDCD40.InsertSQL(di, null) > 0;
        }

        [WebMethod(Description = "Elimina en el usuario destino del colateral la posición que coincida con el usuario original del colateral.")]
        public bool EliminaColateralEnUsuarioReciproco(ref DestinosInternosSector destino)
        {
            DestinosInternosSector di = new DestinosInternosSector();
            di.IdSistema = destino.IdSistema;
            di.IdNucleo = destino.IdNucleo;
            di.IdSector = destino.IdDestino;
            di.IdDestino = destino.IdSector;
            di.TipoAcceso = destino.TipoAcceso;

            destino.IdDestino = di.IdDestino;
            destino.IdSector = di.IdSector;

            return GestorBDCD40.DeleteSQL(di, null) > 0;
        }

        [WebMethod(Description = "Retorna una lista de los recursos que pertenecen a una red concreta y su asignación en la pasarela.")]
        public DataSet RecursosDeUnaRed(string idSistema, string idRed)
        {
            //Procedimientos p = new Procedimientos();
            return Procedimientos.AsignacionRecursosDeUnaRed(GestorBDCD40.ConexionMySql, idSistema, idRed);
        }

        [WebMethod(Description = "Retorna las utilidades configuradas para el sector dado o, si este forma parte de una agrupación, " +
                               "del sector dominante dentro de la agrupación.")]
        public DataSet TeclasSector(string id_sistema, string id_sector)
        {
            //Procedimientos p = new Procedimientos();
            return Procedimientos.TeclasSector(GestorBDCD40.ConexionMySql, id_sistema, id_sector);
        }

        [WebMethod(Description = "Retorna los niveles de intrusión configurados para el sector dado o, si este forma parte de una agrupación, " +
                               "del sector dominante dentro de la agrupación.")]
        public DataSet NivelesIntrusion(string id_sistema, string id_sector)
        {
            //Procedimientos p = new Procedimientos();
            return Procedimientos.NivelesIntrusion(GestorBDCD40.ConexionMySql, id_sistema, id_sector);
        }

        [WebMethod(Description = "Retorna las funciones de radio o telefonía presentes en el sistema.")]
        public DataSet GeneraIndicadores(string id_sistema, int indicador)
        {
            int[] listaIndicadores = { 0 };
            //Procedimientos p = new Procedimientos();
            switch (indicador)
            {
                case 501:   // funciones de telefonía
                    return Procedimientos.GetFunciones(GestorBDCD40.ConexionMySql, id_sistema, 1);
                case 301:   // funciones de radio
                    return Procedimientos.GetFunciones(GestorBDCD40.ConexionMySql, id_sistema, 0);
            }

            return null;
        }

        [WebMethod(Description = "Genera los datos de backup para históricos e indicadores")]
        public void GeneraDatosControlBackup(string id_sistema, DatosControlBackup datosBackup)
        {
            MySql.Data.MySqlClient.MySqlTransaction tran = GestorBDCD40.StartTransaction(true);

            try
            {
                string consulta = "DELETE FROM ControlBackup";
                GestorBDCD40.ExecuteNonQuery(consulta, tran);
                //AccesoABaseDeDatos.ExecuteNonQuery(consulta);

                for (int i = 0; i < 8; i++)
                {
                    consulta = "INSERT INTO ControlBackup VALUES ('" + id_sistema + "'," +
                                                                    i + "," +
                                                                    datosBackup.Profundidad[i] + ",'" +
                                                                    (i < 4 ? datosBackup.NomRecursoHistoricos : datosBackup.NomRecursoIndicadores) +
                                                                    "')";
                    GestorBDCD40.ExecuteNonQuery(consulta, tran);
                    //AccesoABaseDeDatos.ExecuteNonQuery(consulta);
                }
                GestorBDCD40.Commit(tran);
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                GestorBDCD40.RollBack(tran);
            }
        }

        [WebMethod(Description = "Recupera los recursos radio asociados a una tabla bss dada")]
        public string[] GetRecursosRadioFromTablaBss(int idTabla)
        {
            string consulta;

            if (idTabla != 0)
            {
                consulta = "SELECT rr.IdRecurso FROM recursosradio rr " +
                                    "INNER JOIN tabla_bss tb ON tb.idtabla_bss = rr.tabla_bss_idtabla_bss " +
                                    "WHERE tb.idtabla_bss=" + idTabla;

                DataSet datosControl = GestorBDCD40.GetDataSet(consulta, null);
                if (datosControl != null)
                {
                    int i = 0;
                    string[] listaRecursos = new string[datosControl.Tables[0].Rows.Count];
                    foreach (DataRow dr in datosControl.Tables[0].Rows)
                    {
                        listaRecursos[i++] = (string)dr["IdRecurso"];
                    }

                    return listaRecursos;
                }
            }

            return null;
        }

        [WebMethod(Description = "Genera los datos de backup para históricos e indicadores")]
        public DatosControlBackup RecuperarDatosControlBackup(string id_sistema)
        {
            DatosControlBackup dControl = new DatosControlBackup();
            dControl.Profundidad = new uint[8];
            string consulta = "SELECT * FROM ControlBackup WHERE IdSistema='" + id_sistema + "'";

            //AccesoABaseDeDatos accesoABaseDeDatos = new AccesoABaseDeDatos();
            //DataSet datosControl = accesoABaseDeDatos.GetDataSet(consulta);
            DataSet datosControl = GestorBDCD40.GetDataSet(consulta, null);
            if (datosControl != null)
            {
                foreach (DataRow dr in datosControl.Tables[0].Rows)
                {
                    if (dr["TipoBackup"] != System.DBNull.Value)
                    {
                        if ((int)dr["TipoBackup"] < 4)
                            dControl.NomRecursoHistoricos = (string)dr["Recurso"];
                        else
                            dControl.NomRecursoIndicadores = (string)dr["Recurso"];

                        dControl.Profundidad[(int)dr["TipoBackup"]] = (uint)dr["Profundidad"];
                    }
                }
            }

            return dControl;
        }

        [WebMethod(Description = "Retorna los parámetros de los sectores pasados en listaSectores.")]
        public DataSet ParametrosSectores(string idSistema, string listaNucleos, string listaSectores)
        {
            return Procedimientos.ParametrosSectores(GestorBDCD40.ConexionMySql, idSistema, listaNucleos, listaSectores);
        }

        [WebMethod(Description = "Retorna los número de abonado y redes de los sectores que componen la sectorizacion pasada.")]
        public DataSet RedesUsuariosAbonadosParaXML(string idSistema, string idSectorizacion)
        {
            return Procedimientos.RedesUsuariosAbonadosParaXML(GestorBDCD40.ConexionMySql, idSistema, idSectorizacion);
        }

        [WebMethod(Description = "Retorna las utilidades configuradas para los sectores que pertenecen a una " +
                               "sectorización o, si este forma parte de una agrupación, del sector dominante dentro de la agrupación.")]
        public DataSet TeclasSectorParaXML(string id_sistema, string id_sectorizacion)
        {
            //Procedimientos p = new Procedimientos();
            return Procedimientos.TeclasSectorParaXML(GestorBDCD40.ConexionMySql, id_sistema, id_sectorizacion);
        }

        [WebMethod(Description = "Retorna los niveles de intrusión configurados para los sectores de una sectorización, " +
                                    "o, si este forma parte de una agrupación, del sector dominante dentro de la agrupación.")]
        public DataSet NivelesIntrusionParaXML(string id_sistema, string id_sectorizacion)
        {
            //Procedimientos p = new Procedimientos();
            return Procedimientos.NivelesIntrusionParaXML(GestorBDCD40.ConexionMySql, id_sistema, id_sectorizacion);
        }

        [WebMethod(Description = "Pasándole el identificador del sistema y el identificador de la sectorizacion," +
                            " retorna una lista de destinos de telefonía o línea caliente, internos o externos de cada sector.")]
        public DataSet DestinosTelefoniaSectorizadosParaXML(string idSistema, string idSectorizacion, bool lc)
        {
            //Procedimientos p = new Procedimientos();
            return Procedimientos.DestinosTelefoniaSectorizadosParaXML(GestorBDCD40.ConexionMySql, idSistema, idSectorizacion, lc);
        }

        [WebMethod(Description = "Pasándole el identificador del sistema y el identificador de la sectorizacion," +
                            " retorna una lista de destinos exgernos de cada sector.")]
        public DataSet DestinosRadioSectorizadosParaXML(string idSistema, string idSectorizacion)
        {
            //Procedimientos p = new Procedimientos();
            return Procedimientos.DestinosRadioSectorizadosParaXML(GestorBDCD40.ConexionMySql, idSistema, idSectorizacion);
        }

        [WebMethod(Description = "Retorna los parámetros de los sectores pasados en listaSectores.")]
        public DataSet ParametrosSectoresParaXML(string idSistema, string idSectorizacion)
        {
            return Procedimientos.ParametrosSectoresParaXML(GestorBDCD40.ConexionMySql, idSistema, idSectorizacion);
        }

        [WebMethod(Description = "Retorna los troncales del sistema con los recursos asignados.")]
        public DataSet RecursosPorTroncalParaXML(string idSistema)
        {
            return Procedimientos.RecursosPorTroncalParaXML(GestorBDCD40.ConexionMySql, idSistema);
        }

        [WebMethod(Description = "Retorna los emplazamientos del sistema con los recursos asignados.")]
        public DataSet RecursosPorEmplazamientoParaXML(string idSistema)
        {
            return Procedimientos.RecursosPorEmplazamientoParaXML(GestorBDCD40.ConexionMySql, idSistema);
        }

        [WebMethod(Description = "Retorna los grupos de telefonía del sistema con los destinos asignados.")]
        public DataSet DestinosPorGruposTelefonia(string idSistema)
        {
            return Procedimientos.DestinosPorGruposTelefonia(GestorBDCD40.ConexionMySql, idSistema);
        }

        [WebMethod(Description = "Retorna los recursos de telefonía del sistema.")]
        public DataSet RecursosTfParaInformeXML(string idSistema)
        {
            return Procedimientos.RecursosTfParaInformeXML(GestorBDCD40.ConexionMySql, idSistema);
        }

        [WebMethod(Description = "Retorna los recursos de radio del sistema.")]
        public DataSet RecursosRdParaInformeXML(string idSistema)
        {
            return Procedimientos.RecursosRdParaInformeXML(GestorBDCD40.ConexionMySql, idSistema);
        }

        [WebMethod(Description = "Retorna los recursos de acceso instantáneo del sistema.")]
        public DataSet RecursosLcParaInformeXML(string idSistema)
        {
            return Procedimientos.RecursosLcParaInformeXML(GestorBDCD40.ConexionMySql, idSistema);
        }

        [WebMethod(Description = "Retorna true si existe la dirección IP o false en caso contrario.")]
        public bool ExisteIP(string idSistema, string iP, string idEh)
        {
            return Procedimientos.ExisteIP(GestorBDCD40.ConexionMySql, idSistema, iP, idEh) > 0;
        }

        [WebMethod(Description = "Actualiza los nombres de los grupos de sectores asignados en las tops de las sectorizaciones.")]
        public void ActualizaSectoresSectorizacion(string idSistema, string idNucleo, string grupoSectores, string idAgrupacion)
        {
            Procedimientos.ActualizaSectoresSectorizacion(GestorBDCD40.ConexionMySql, null, idSistema, idNucleo, grupoSectores, idAgrupacion);
        }

        [WebMethod(Description = "Obtiene el número de sectores que comparten puesto de operador junto con un sector dado")]
        public bool GetSectoresCompartiendoPuestoOperador(string idSistema, string idNucleo, string idSector)
        {
            string consulta = "SELECT IdSector,COUNT(*) FROM SectoresSector " +
                                "WHERE IdSector IN (SELECT IdSector FROM SectoresSector WHERE " +
                                    "IdSistema='" + idSistema + "' AND IdNucleo='" + idNucleo + "' AND IdSectorOriginal='" + idSector + "')" +
                                "GROUP BY IdSector";

            DataSet datosControl = GestorBDCD40.GetDataSet(consulta, null);
            if (datosControl != null)
            {
                bool comparten = false;
                foreach (DataRow r in datosControl.Tables[0].Rows)
                {
                    if ((int)r[1] > 1)
                    {
                        comparten = true;
                        break;
                    }
                }

                return comparten;
            }

            return false;
        }

        [WebMethod(Description = "Ofrece a los clientes el estado de SACTA.")]
        public byte GetEstadoSacta()
        {
            //Ref_InterfazSacta.ServicioInterfazSacta servicioSacta = new Ref_InterfazSacta.ServicioInterfazSacta();
            try
            {
                return ServicioSacta.GetEstadoSacta();
            }
            catch
            {
                // 30/06/10 salio el Assert en maqueta. Si vuelve a salir, crear servcicioSacta estático en la clase ServiciosCD40
                // 08/07/10 como ha seguido saliendo (aunque en maquete de laboratorio) se ha hecho estático... Observar si deja de salir el assert.
                // System.Diagnostics.Debug.Assert(false, e.Message);
                return (byte)0;
            }
        }

        [WebMethod]
        public string GetVersionConfiguracion(string idSistema)
        {
            //Procedimientos pr = new Procedimientos();

            return Procedimientos.VersionSectorizacion(GestorBDCD40.ConexionMySql, idSistema);
        }

        [WebMethod]
        public bool NumTestValido(string idsistema, string idCentral, string rango, out string inicial, out string final)
        {
            bool retorno = false;
            Rangos r = new Rangos();
            inicial = final = string.Empty;

            System.Text.StringBuilder consulta = new System.Text.StringBuilder("SELECT COUNT(*) FROM Rangos " +
                                "WHERE IdSistema='" + idsistema + "' AND " +
                                "Central='" + idCentral + "' AND " +
                                "CAST(Inicial AS UNSIGNED)<= CAST('" + rango + "' AS UNSIGNED) AND " +
                                "CAST(Final AS UNSIGNED)>= CAST('" + rango + "' AS UNSIGNED)");

            DataSet dsCuantos = GestorBDCD40.GetDataSet(consulta.ToString(), null);
            retorno = dsCuantos.Tables.Count > 0 && dsCuantos.Tables[0].Rows.Count > 0 && Convert.ToUInt16(dsCuantos.Tables[0].Rows[0][0]) > 0;

            consulta.Remove(0, consulta.Length);
            consulta.Append("SELECT * FROM Rangos " +
                                "WHERE IdSistema='" + idsistema + "' AND " +
                                "Central='" + idCentral + "'");

            DataSet ds = GestorBDCD40.GetDataSet(consulta.ToString(), null);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                System.Data.DataRow dr = ds.Tables[0].Rows[0];
                if (dr["Inicial"] != System.DBNull.Value)
                    inicial = (string)dr["Inicial"];
                if (dr["Final"] != System.DBNull.Value)
                    final = (string)dr["Final"];
            }

            return retorno;
        }

        [WebMethod]
        public bool SectoresManttoEnTop(string idSistema, string idSectorizacion, string idTop)
        {
            DataSet ds = Procedimientos.SectoresManttoEnTop(GestorBDCD40.ConexionMySql, idSistema, idSectorizacion, idTop);
            return ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0;
        }

        [WebMethod]
        public bool AnadeDestino(DestinosExternos dExterno, string strRecurso, bool anadirGrupo)
        {
            MySql.Data.MySqlClient.MySqlTransaction tran = GestorBDCD40.StartTransaction(false);

            try
            {
                // Añadir grupo si no existe
                if (anadirGrupo)
                {
                    GruposTelefonia gTel = new GruposTelefonia();
                    gTel.IdSistema = dExterno.IdSistema;
                    gTel.IdGrupo = dExterno.IdGrupo;

                    GestorBDCD40.InsertSQL(gTel, tran);
                }

                /**
                 * AGL 2012.06.15. ID.125. "No se anade el grupo a la tabla de Destinos de Telefonía"
                 */
                // Insertar destino externo
                GestorBDCD40.InsertSQL(dExterno, tran);

                if (dExterno.IdPrefijo == 1)  // LCEN
                {
                    //	ActualizaRecursoLCEN(dExterno, TBRecurso.Text);
                    RecursosLCEN rTelefonia = new RecursosLCEN();
                    rTelefonia.IdSistema = dExterno.IdSistema;
                    rTelefonia.IdRecurso = strRecurso;
                    rTelefonia.IdDestino = dExterno.IdDestino;
                    rTelefonia.IdPrefijo = dExterno.IdPrefijo;
                    rTelefonia.TipoDestino = dExterno.TipoDestino;

                    Utilidades.AsignaDestinoARecurso((ParametrosRecursoGeneral)rTelefonia, tran);
                }
                else if (dExterno.IdPrefijo == 32)    // PP
                {
                    RecursosTF rTelefonia = new RecursosTF();
                    rTelefonia.IdSistema = dExterno.IdSistema;
                    rTelefonia.IdRecurso = strRecurso;
                    rTelefonia.IdDestino = dExterno.IdDestino;
                    rTelefonia.IdPrefijo = dExterno.IdPrefijo;
                    rTelefonia.TipoDestino = dExterno.TipoDestino;

                    Utilidades.AsignaDestinoARecurso((ParametrosRecursoGeneral)rTelefonia, tran);
                }
                GestorBDCD40.Commit(tran);

                return true;
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                GestorBDCD40.RollBack(tran);

                return false;
            }
        }

        [WebMethod]
        public bool ActualizaDestino(DestinosExternos dExterno, string strRecurso, bool anadirGrupo)
        {
            MySql.Data.MySqlClient.MySqlTransaction tran = GestorBDCD40.StartTransaction(false);

            try
            {
                // Añadir grupo si no existe
                if (anadirGrupo)
                {
                    GruposTelefonia gTel = new GruposTelefonia();
                    gTel.IdSistema = dExterno.IdSistema;
                    gTel.IdGrupo = dExterno.IdGrupo;

                    GestorBDCD40.InsertSQL(gTel, tran);
                }

                /**
                 * AGL 2012.06.15. ID.125. "No se anade el grupo a la tabla de Destinos de Telefonía"
                 */
                // Insertar destino externo
                GestorBDCD40.UpdateSQL(dExterno, tran);

                if (dExterno.IdPrefijo == 1)  // LCEN
                {
                    // Liberar el destino al cual el recurso está asociado
                    RecursosLCEN rTf = new RecursosLCEN();
                    rTf.IdSistema = dExterno.IdSistema;
                    rTf.IdDestino = dExterno.IdDestino;
                    rTf.TipoDestino = 1;
                    Utilidades.LiberaDestinoDeRecurso(rTf, tran);

                    rTf.IdRecurso = strRecurso;
                    rTf.IdPrefijo = dExterno.IdPrefijo;
                    Utilidades.AsignaDestinoARecurso((ParametrosRecursoGeneral)rTf, tran);
                }
                else if (dExterno.IdPrefijo == 32)    // PP
                {
                    // Liberar el destino al cual el recurso está asociado
                    RecursosTF rTf = new RecursosTF();
                    rTf.IdSistema = dExterno.IdSistema;
                    rTf.IdDestino = dExterno.IdDestino;
                    rTf.TipoDestino = 1;
                    Utilidades.LiberaDestinoDeRecurso(rTf, tran);

                    rTf.IdRecurso = strRecurso;
                    rTf.IdPrefijo = dExterno.IdPrefijo;
                    Utilidades.AsignaDestinoARecurso((ParametrosRecursoGeneral)rTf, tran);
                }

                GestorBDCD40.Commit(tran);

                return true;
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                GestorBDCD40.RollBack(tran);

                return false;
            }
        }

        [WebMethod]
        public bool EliminaDestino(Destinos dExterno, ParametrosRecursoGeneral recurso)
        {
            MySql.Data.MySqlClient.MySqlTransaction tran = GestorBDCD40.StartTransaction(false);

            try
            {
                // Liberar el destino del recurso
                Utilidades.LiberaDestinoDeRecurso(recurso, tran);

                // Una vez el destino ha sido desasignado del recurso, se elimina aquel.
                GestorBDCD40.DeleteSQL(dExterno, tran);

                GestorBDCD40.Commit(tran);
                return true;
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                GestorBDCD40.RollBack(tran);
                return false;
            }
        }

        [WebMethod]
        public bool AnadeDestinoRadio(DestinosRadio destino, RecursosRadio recurso, Tablas[] listaRecursos)
        {
            MySql.Data.MySqlClient.MySqlTransaction tran = GestorBDCD40.StartTransaction(true);

            try
            {
                GestorBDCD40.InsertSQL(destino, tran);
                Utilidades.LiberaDestinoDeRecurso((ParametrosRecursoGeneral)recurso, tran);
                AsignaEnlaceARecurso(listaRecursos, tran);

                GestorBDCD40.Commit(tran);
                return true;
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                GestorBDCD40.RollBack(tran);
                return false;
            }
        }

        [WebMethod]
        public bool ModificaDestinoRadio(DestinosRadio destino, RecursosRadio recurso, Tablas[] listaRecursos)
        {
            MySql.Data.MySqlClient.MySqlTransaction tran = GestorBDCD40.StartTransaction(true);

            try
            {
                GestorBDCD40.UpdateSQL(destino, tran);
                Utilidades.LiberaDestinoDeRecurso((ParametrosRecursoGeneral)recurso, tran);
                AsignaEnlaceARecurso(listaRecursos, tran);

                GestorBDCD40.Commit(tran);
                return true;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                CD40.BD.GestorBaseDatos.logFile.Error(string.Format("ModificaDestinoRadio- Error al modificar el destino ({0}). Error:", destino.IdDestino), ex);
                GestorBDCD40.RollBack(tran);
                return false;
            }
        }

        [WebMethod]
        public bool DemasiadasTeclasConPrioridadUno(string idSistema, string idNucleo, bool interna)
        {
            //Devuelve true, si se ha superado el número máximo de teclas con prioridad 1 configuradas por página
            // en el panel de línea caliente

            //int iMaximo = interna ? 16 : 18;
            int iMaximo = 1;

            //Se obtiene el número de enlaces máximo
            iMaximo = (int)ObtenerNumEnlacesPorPanel(idSistema, Tipos.Tipo_Panel.PANEL_LC);


            return Procedimientos.DemasiadasTeclasConPrioridadUno(GestorBDCD40.ConexionMySql, idSistema, idNucleo, iMaximo);
        }

        [WebMethod]
        public void ActualizaSector(bool modificando, Sectores n, Niveles niv, TeclasSector tec, ParametrosSector parsec,
                                    DataSet dsNumerosAbonado, DataSet dsAgenda)
        {

            MySql.Data.MySqlClient.MySqlTransaction tran = GestorBDCD40.StartTransaction(true);

            try
            {
                if (!modificando) //Sector nuevo
                {
                    GestorBDCD40.InsertSQL(n, tran);
                    GestorBDCD40.InsertSQL(niv, tran);
                    GestorBDCD40.InsertSQL(tec, tran);
                    GestorBDCD40.InsertSQL(parsec, tran);
                }
                else
                {
                    GestorBDCD40.UpdateSQL(n, tran);
                    GestorBDCD40.UpdateSQL(niv, tran);
                    GestorBDCD40.UpdateSQL(tec, tran);
                    GestorBDCD40.UpdateSQL(parsec, tran);
                }
                GuardarUsuariosAbonados(n.IdSistema, n.IdSector, n.IdNucleo, tran, dsNumerosAbonado);
                GuardarAgenda(n.IdSistema, n.IdSector, n.IdNucleo, tran, dsAgenda);

                GestorBDCD40.Commit(tran);
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                GestorBDCD40.RollBack(tran);
            }
        }

        [WebMethod]
        public DataSet GetIncidencias(int rango, string idioma)
        {
            string tabla = "Incidencias";
            string consulta = string.Empty;

            switch (idioma)
            {
                case "es":
                    tabla = "Incidencias";
                    break;
                case "en":
                    tabla = "Incidencias_Ingles";
                    break;
                case "fr":
                    tabla = "Incidencias_Frances";
                    break;
            }


            switch (rango)
            {
                case 0: // Todas
                    consulta = "SELECT * FROM " + tabla;
                    break;
                case 1: // Tops
                    consulta = "SELECT * FROM " + tabla + " WHERE IdIncidencia >= 1000 AND IdIncidencia <= 1999";
                    break;
                case 2: // GW
                    consulta = "SELECT * FROM " + tabla + " WHERE IdIncidencia >= 2000 AND IdIncidencia <= 2999";
                    break;
            }

            DataSet datosControl = GestorBDCD40.GetDataSet(consulta, null);
            return datosControl;
        }

        [WebMethod(Description = "Actualiza el estado del CD30 para sistemas 1+1")]
        public void SetEstadoCD30(bool activo)
        {
            // Servicio llamado desde SincroCD30CD40 para informar del estado del CD30.
            EstadoCD30Activo = activo;
            TimerPresenciaCD30.Change(0, 10000);
        }

        [WebMethod(Description = "Devuelve la lista de sectores ordenada por NumSacta")]
        public DataSet SectoresNumSactaSorted(string idSistema, string idnucleo, string listaSectores)
        {
            DataSet datosControl = null;

            if (!string.IsNullOrEmpty(listaSectores) && !string.IsNullOrEmpty(idSistema) && !string.IsNullOrEmpty(idnucleo))
            {
                string consulta = "SELECT IdSector,Tipo FROM Sectores WHERE IdSistema='" + idSistema +
                                  "' AND IdNucleo='" + idnucleo + "' AND IdSector IN (" + listaSectores + ") ORDER BY NumSacta";

                datosControl = GestorBDCD40.GetDataSet(consulta, null);
            }

            return datosControl;
        }

        [WebMethod(Description = "Retorna los permisos de un sector sobre las redes del sistema")]
        public DataSet PermisoRedesSector(string idSistema, string idSector)
        {
            return Procedimientos.PermisosDeRedes(GestorBDCD40.ConexionMySql, idSistema, idSector);
        }

        [WebMethod(Description = "Guarda el estado de los servidores que componen el cluster")]
        public void SetEstadoNodes(string nameNode1, int estadoNode1, string nameNode2, int estadoNode2, string idioma, string sistema, string ip)
        {
            if (Estado1 != estadoNode1)
            {
                Estado1 = estadoNode1;

                switch (estadoNode1)
                {
                    case 2: // NodeState.Active:
                        CreateEvent(201, nameNode1, idioma, sistema, ip);
                        break;
                    case 3: //NodeState.NoActive:
                        CreateEvent(202, nameNode1, idioma, sistema, ip);
                        break;
                    case 0: //NodeState.NoValid:
                        CreateEvent(203, nameNode1, idioma, sistema, ip);
                        break;
                    default:
                        break;
                }
            }

            if (Estado2 != estadoNode2)
            {
                Estado2 = estadoNode2;

                switch (estadoNode2)
                {
                    case 2: // NodeState.Active:
                        CreateEvent(201, nameNode2, idioma, sistema, ip);
                        break;
                    case 3: //NodeState.NoActive:
                        CreateEvent(202, nameNode2, idioma, sistema, ip);
                        break;
                    case 0: //NodeState.NoValid:
                        CreateEvent(203, nameNode2, idioma, sistema, ip);
                        break;
                    default:
                        break;
                }
            }

            if (nameNode1 != null)
                GestorBDCD40ToMantto.ExecuteNonQuery("REPLACE INTO estadocluster (Name,Presencia,Estado)" +
                                                        "VALUES ('" + nameNode1 + "'," +
                                                                     ((estadoNode1 != (int)NodeState.NoValid) ? (int)Tipos.PresenciaNodes.PRESENTE : (int)Tipos.PresenciaNodes.NO_PRESENTE) + "," +
                                                                     (int)estadoNode1 + ")", null);
            if (nameNode2 != null)
                GestorBDCD40ToMantto.ExecuteNonQuery("REPLACE INTO estadocluster (Name,Presencia,Estado)" +
                                                        "VALUES ('" + nameNode2 + "'," +
                                                                     ((estadoNode2 != (int)NodeState.NoValid) ? (int)Tipos.PresenciaNodes.PRESENTE : (int)Tipos.PresenciaNodes.NO_PRESENTE) + "," +
                                                                     (int)estadoNode2 + ")", null);
        }


        [WebMethod(Description = "Devuelve true si en la BD existe otro grabador con la misma IP ")]
        public bool ExisteGrabadorMismaIP(string idSistema, string iP, string idEh)
        {
            StringBuilder strCadena = new StringBuilder();
            bool bExiste = false;
            int iCuenta = -1;

            try
            {
                strCadena.AppendFormat("select count(1) FROM EquiposEu where IdSistema='{0}' AND idEquipos!='{1}' AND (IpRed1='{2}' OR IpRed2='{2}') and TipoEquipo=5", idSistema, idEh, iP);

                iCuenta = Convert.ToInt32(GestorBDCD40.ExecuteScalar(strCadena.ToString()));

                if (iCuenta > 0)
                    bExiste = true;
            }
            catch (Exception ex)
            {
                iCuenta = -1;
                CD40.BD.GestorBaseDatos.logFile.Error(string.Format("ExisteGrabadorMismaIP- Error al ejecutar la consulta ({0}). Error:", strCadena.ToString()), ex);
            }

            strCadena.Clear();

            return bExiste;
        }

        [WebMethod(Description = "Devuelve true si terminal de operador tiene asignado algún sector en alguna sectorización")]
        public bool TerminalOperadorAsignadoEnSectorizacion(string idSistema, string idTop)
        {
            StringBuilder strCadena = new StringBuilder();
            bool bExiste = false;
            int iCuenta = -1;

            try
            {
                if (!string.IsNullOrEmpty(idSistema) && !string.IsNullOrEmpty(idTop))
                {
                    strCadena.AppendFormat("select count(1) from sectoressectorizacion where IdSistema='{0}' AND idTop='{1}'", idSistema, idTop);

                    iCuenta = Convert.ToInt32(GestorBDCD40.ExecuteScalar(strCadena.ToString()));

                    if (iCuenta > 0)
                        bExiste = true;
                }
            }
            catch (Exception ex)
            {
                iCuenta = -1;
                CD40.BD.GestorBaseDatos.logFile.Error(string.Format("TerminalOperadorAsignadoEnSectorización- Error al ejecutar la consulta ({0}). Error:", strCadena.ToString()), ex);
            }

            strCadena.Clear();

            return bExiste;
        }

        [WebMethod(Description = "Devuelve true si el sector está asignado a alguna sectorización")]
        public bool SectorAsignadoEnSectorizacion(string idSistema, string idsector)
        {
            StringBuilder strCadena = new StringBuilder();
            bool bExiste = false;
            int iCuenta = -1;

            try
            {
                if (!string.IsNullOrEmpty(idSistema) && !string.IsNullOrEmpty(idsector))
                {
                    strCadena.Append("SELECT COUNT(1) FROM  ViewSectoresEnTops V, SECTORESSECTOR S ");
                    strCadena.Append("WHERE V.IdSistema=S.IdSistema AND V.IdNucleo=s.IdNucleo AND V.IdSector=S.IdSector AND ");
                    strCadena.AppendFormat("S.IdSistema='{0}' AND S.idSectorOriginal='{1}' AND V.IdSectorizacion IN (SELECT S1.idSectorizacion ", idSistema, idsector);
                    strCadena.Append("FROM SECTORIZACIONES S1 LEFT JOIN (SELECT IdSistema,");
                    strCadena.Append("(CASE WHEN ACTIVA=1 THEN DATE_FORMAT(FECHAACTIVACION,'%e/%m/%Y %H:%i:%S') ELSE IDSECTORIZACION END) idSectorizacionC ");
                    strCadena.Append("FROM SECTORIZACIONES WHERE IdSectorizacion IN ('SCV','SACTA') OR ACTIVA=true) S2 ON S1.idSectorizacion=S2.idSectorizacionc AND S1.IdSistema=S2.IdSistema ");
                    strCadena.Append("WHERE S2.IdSectorizacionc IS NULL)");

                    iCuenta = Convert.ToInt32(GestorBDCD40.ExecuteScalar(strCadena.ToString()));

                    if (iCuenta > 0)
                        bExiste = true;
                }
            }
            catch (Exception ex)
            {
                iCuenta = -1;
                CD40.BD.GestorBaseDatos.logFile.Error(string.Format("SectorAsignadoEnSectorizacion- Error al ejecutar la consulta ({0}). Error:", strCadena.ToString()), ex);
            }

            strCadena.Clear();

            return bExiste;
        }

        [WebMethod(Description = "Devuelve true si en el sistema existe otra agrupación con el mismo conjunto de sectores")]
        public bool ExisteAgrupacionConSectores(string ListaSectores, string idAgrupacion)
        {
            StringBuilder strCadena = new StringBuilder();
            StringBuilder strLista = new StringBuilder();
            bool bExiste = false;
            int iCuenta = -1;
            int iNumSectores = 0;

            try
            {
                //Cada sector debe venir en la lista entre comillas simples
                if (!string.IsNullOrEmpty(ListaSectores))
                {
                    string[] arrSectores = ListaSectores.Split(new char[] { ',', '\'' }, StringSplitOptions.RemoveEmptyEntries);
                    iNumSectores = arrSectores.Length;

                    if (iNumSectores > 0)
                    {
                        if (1 == iNumSectores)
                        {
                            strLista.AppendFormat("idSector='{0}'", arrSectores[0]);
                        }
                        else
                        {
                            for (int i = 0; i < (iNumSectores - 1); i++)
                            {
                                strCadena.AppendFormat("'{0}',", arrSectores[i]);
                            }

                            //Añadimos el último elemento y completamos la parte de la consulta
                            strLista.AppendFormat("idSector IN ({0}'{1}')", strCadena.ToString(), arrSectores[iNumSectores - 1]);
                            strCadena.Clear();
                        }

                        strCadena.Append("SELECT COUNT(1) FROM (");

                        strCadena.Append("SELECT IdAgrupacion,COUNT(*) NUM_TOTAL,");
                        strCadena.AppendFormat("SUM((CASE WHEN {0} THEN 1 ELSE 0 END)) NUM_SECTORES ", strLista.ToString());
                        strCadena.Append("FROM sectoresagrupacion GROUP BY IdAgrupacion HAVING ");

                        if (!string.IsNullOrEmpty(idAgrupacion))
                        {
                            strCadena.AppendFormat(" IdAgrupacion<>'{0}' AND ", idAgrupacion);
                        }

                        strCadena.AppendFormat("NUM_TOTAL=NUM_SECTORES AND NUM_TOTAL={0} ) T1 ", iNumSectores);

                        //Se ejecuta la consulta
                        iCuenta = Convert.ToInt32(GestorBDCD40.ExecuteScalar(strCadena.ToString()));

                        if (iCuenta > 0)
                            bExiste = true;
                        else if (iCuenta < 0)
                        {
                            CD40.BD.GestorBaseDatos.logFile.Error(string.Format("ExisteAgrupacionConSectores- Error {0} al ejecutar la consulta: {1}", iCuenta, strCadena.ToString()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                iCuenta = -1;
                CD40.BD.GestorBaseDatos.logFile.Error(string.Format("ExisteAgrupacionConSectores- Error al ejecutar la consulta ({0}).", strCadena.ToString()));
                CD40.BD.GestorBaseDatos.logFile.Error("ExisteAgrupacionConSectores- Error:", ex);
            }

            strCadena.Clear();
            strLista.Clear();

            return bExiste;
        }

        [WebMethod(Description = "Devuelve true si la Central es IP y tiene asociado algún Destino ATS configurado en el panel de LC de algún sector")]
        public bool ExisteDestinoATSenPanelLC_AsociadoCentralIP(string idSistema, string idCentral)
        {
            StringBuilder strCadena = new StringBuilder();
            bool bExiste = false;
            int iCuenta = -1;

            try
            {
                if (!string.IsNullOrEmpty(idSistema) && !string.IsNullOrEmpty(idCentral))
                {
                    strCadena.Append("SELECT COUNT(1) FROM DestinosExternosSector S,DestinosExternos D ");
                    strCadena.Append("WHERE S.IdSistema=D.IdSistema AND S.IdDestino=D.IdDestino AND S.TipoDestino=D.TipoDestino AND S.IdPrefijo=D.IdPrefijo AND ");
                    strCadena.AppendFormat("S.IdSistema='{0}' AND S.IdPrefijo=3 AND S.TipoDestino=1 AND S.TipoAcceso='IA' AND ", idSistema);
                    strCadena.Append("EXISTS (SELECT 1 from encaminamientos E LEFT JOIN rangos as R ON  R.IdSistema=E.IdSistema AND R.Central = E.Central ");
                    strCadena.AppendFormat("WHERE  E.CENTRAL='{0}' AND E.centralip=1 AND E.IdSistema=D.IdSistema AND D.IdAbonado BETWEEN R.inicial and R.final) ", idCentral);

                    iCuenta = Convert.ToInt32(GestorBDCD40.ExecuteScalar(strCadena.ToString()));

                    if (iCuenta > 0)
                        bExiste = true;
                }
            }
            catch (Exception ex)
            {
                iCuenta = -1;
                CD40.BD.GestorBaseDatos.logFile.Error(string.Format("ExisteDestinoATSenPanelLC_AsociadoCentralIP- Error al ejecutar la consulta ({0}). Error:", strCadena.ToString()), ex);
            }

            strCadena.Clear();

            return bExiste;
        }

        [WebMethod(Description = "Devuelve true si el número de abonado del destino ATS está dentro de los rangos de algún encaminamiento que es central IP")]
        public bool DestinoATS_ConCentralIP(string idSistema, string idDestinoTlf)
        {
            StringBuilder strCadena = new StringBuilder();
            bool bExiste = false;
            int iCuenta = -1;

            try
            {
                if (!string.IsNullOrEmpty(idSistema) && !string.IsNullOrEmpty(idDestinoTlf))
                {
                    strCadena.Append("SELECT count(1) FROM destinosTelefonia A, destinosexternos D ");
                    strCadena.Append("WHERE A.IdSistema=D.IdSistema AND A.TipoDestino=D.TipoDestino AND A.IdPrefijo=D.IdPrefijo AND A.IdDestino=D.IdDestino AND ");
                    strCadena.Append("EXISTS (SELECT 1 from encaminamientos E LEFT JOIN rangos as R ON  R.IdSistema=E.IdSistema AND R.Central = E.Central ");
                    strCadena.Append("WHERE E.centralip=1 AND E.IdSistema=D.IdSistema AND D.IdAbonado BETWEEN R.inicial and R.final) AND ");
                    strCadena.AppendFormat("(A.IdPrefijo=3 OR A.IdPrefijo=2) AND A.TipoDestino=1 AND A.IdSistema='{0}' AND A.IdDestino='{1}'", idSistema, idDestinoTlf);

                    iCuenta = Convert.ToInt32(GestorBDCD40.ExecuteScalar(strCadena.ToString()));

                    if (iCuenta > 0)
                        bExiste = true;
                }
            }
            catch (Exception ex)
            {
                iCuenta = -1;
                CD40.BD.GestorBaseDatos.logFile.Error(string.Format("DestinoATS_ConCentralIP- Error al ejecutar la consulta ({0}). Error:", strCadena.ToString()), ex);
            }

            strCadena.Clear();

            return bExiste;
        }

        [WebMethod(Description = "Genera la incidencia de configuración que se pasa como parámetro")]
        public void GeneraIncidenciaConfiguracion(string id_sistema, uint idIncidencia, string[] arrParametros)
        {
            try
            {
                System.Configuration.Configuration webConfiguracion = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                string toServerMantto = webConfiguracion.AppSettings.Settings["ServerManttoIp"].Value;


                if (null != id_sistema && null != arrParametros && !string.IsNullOrWhiteSpace(toServerMantto))
                {
                    CD40.BD.Utilidades util = new CD40.BD.Utilidades(GestorBDCD40.ConexionMySql);

                    if (null != util)
                    {
                        util.CreaEventoConfiguracion(id_sistema, idIncidencia, arrParametros, toServerMantto);
                    }
                }
            }
            catch (Exception exp)
            {
                CD40.BD.GestorBaseDatos.logFile.Error(string.Format("GeneraIncidenciaConfiguracion- Error al generar la incidencia ({0}). Error:", idIncidencia.ToString()), exp);
            }
        }

        [WebMethod(Description = "Obtiene el número de enlaces que se puede configurar como máximo en el panel indicado")]
        private uint ObtenerNumEnlacesPorPanel(string idSistema, Tipos.Tipo_Panel eTipoPanel)
        {
            //Obtiene el número de enlaces que se puede configurar como máximo por panel
            uint iNumEnlaces = 0;
            uint iValor = 0;
            uint iValor2 = 0;

            try
            {
                if (!string.IsNullOrEmpty(idSistema))
                {
                    Sistema objCnfSistema = new Sistema();
                    objCnfSistema.IdSistema = idSistema;

                    List<Tablas> d = GestorBDCD40.ListSelectSQL(objCnfSistema, null);

                    if (null != d && d.Count > 0)
                    {
                        switch (eTipoPanel)
                        {
                            case Tipos.Tipo_Panel.PANEL_LC:
                                iValor = ((Sistema)d[0]).NumEnlacesAI;
                                break;
                            case Tipos.Tipo_Panel.PANEL_RADIO:
                                iValor = ((Sistema)d[0]).NumFrecPagina;
                                iValor2 = ((Sistema)d[0]).NumPagFrec;
                                iValor = iValor * iValor2;
                                break;
                            case Tipos.Tipo_Panel.PANEL_TELEFONIA:
                                iValor = ((Sistema)d[0]).NumDestinosInternosPag;
                                iValor2 = ((Sistema)d[0]).NumPagDestinosInt;
                                iValor = iValor * iValor2;

                                break;
                            default:
                                break;
                        }
                    }

                    if (iValor >= 0)
                    {
                        iNumEnlaces = iValor;
                    }

                }
            }
            catch (Exception ex)
            {
                //Por defecto, se configuran 18 enlaces
                iNumEnlaces = 18;
                CD40.BD.GestorBaseDatos.logFile.Error(string.Format("ObtenerNumEnlacesPorPanel- Error al obtener el número total de enlaces. Por defecto, se toma el valor {0}. Error:", iNumEnlaces.ToString()), ex);
            }

            return iNumEnlaces;
        }

        [WebMethod(Description = "Devuelve true si en el sistema existe algún destino de telefonía asociado al prefijo de la red que se pasa como parámetro.")]
        public bool Red_ConDestinosTlf(string idSistema, uint idPrefijoRed)
        {
            StringBuilder strCadena = new StringBuilder();
            bool bExiste = false;
            int iCuenta = -1;

            try
            {
                if (!string.IsNullOrEmpty(idSistema))
                {
                    strCadena.Append("SELECT count(1) FROM destinosTelefonia ");
                    strCadena.AppendFormat("WHERE IdSistema='{0}' AND IdPrefijo={1} AND TipoDestino=1", idSistema, idPrefijoRed);

                    iCuenta = Convert.ToInt32(GestorBDCD40.ExecuteScalar(strCadena.ToString()));

                    if (iCuenta > 0)
                        bExiste = true;
                }
            }
            catch (Exception ex)
            {
                iCuenta = -1;
                CD40.BD.GestorBaseDatos.logFile.Error(string.Format("Red_ConDestinosTlf- Error al ejecutar la consulta ({0}). Error:", strCadena.ToString()), ex);
            }

            strCadena.Clear();

            return bExiste;
        }

        [WebMethod(Description = "Devuelve 1 si el identificador del tipo que se le pasa como parámetro existe en el sistema, 0 si no existe y -1 si se ha producido error. El tipo puede tomar uno de los siguientes valores:<blockquote> EF (Pasarelas,Tops, Equipos Externos y Encaminamientos).<br/> R (Recursos Radio y de Telefonía).<br/> D (Destinos Radio y de Telefonía).<br/> NA (Emplazamientos y Zonas) </blockquote>")]
        public int CheckIdentificadorAsignado(string strTipo, string idSistema, string strIdentificador)
        {
            //Tipo puede tomar uno de los siguientes valores: 
            //  EF = Elementos físicos (pasarelas, top y equipos externos)
            //  R  = Recursos de telefonía y radio
            //  D  = Destinos Radio y de telefonía.
            //  NA = emplazamientos y zonas

            return Procedimientos.CheckIdentificadorAsignado(GestorBDCD40.ConexionMySql, strTipo, idSistema, strIdentificador);
        }

        [WebMethod(Description = "Devuelve un dataset con el resultado de la consulta que se pasa como parámetro")]
        public DataSet ObtenerDataSet(string pstrConsulta)
        {
            DataSet dsDatos = null;

            try
            {
                if (!string.IsNullOrWhiteSpace(pstrConsulta) && (pstrConsulta.ToUpper().StartsWith("SELECT") || pstrConsulta.ToUpper().StartsWith("(SELECT")))
                {
                    StringBuilder strConsulta = new System.Text.StringBuilder();
                    strConsulta.Append(pstrConsulta);

                    dsDatos = GestorBDCD40.GetDataSet(strConsulta.ToString(), null);

                    strConsulta.Clear();
                }
            }
            catch (Exception ex)
            {
                CD40.BD.GestorBaseDatos.logFile.Error(string.Format("ObtenerDataSet- Error al ejecutar la consulta {0}. Error:", pstrConsulta.ToString()), ex);
            }

            return dsDatos;
        }


        #endregion WEB_METHODS

        [WebMethod]
        public string HelloWorld()
        {
            var service = new Ref_InterfazSacta.ServicioInterfazSactaSoapClient("https://localhost:44310/WebServices/InterfazSacta/ServicioInterfazSacta.asmx");
            var std = service.GetEstadoSacta();
            return "Hola a todos";
        }

    }
}
