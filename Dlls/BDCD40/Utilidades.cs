using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Configuration;
using CD40.BD.Entidades;
using GeneraSectorizacionDll;
//using Lextm.SharpSnmpLib;

namespace CD40.BD
{
	public class SactaInfo : Dictionary<string, object>
	{
	}
	
	public delegate void SectorizacionEventHandler<T>(T msg);

    public class Utilidades
    {
        private GestorBaseDatos GestorBDCD40;

        static System.Threading.Semaphore SemaforoSectorizacionSacta;
        static object _Sync = new object();
        public event SectorizacionEventHandler<SactaInfo> EventResultSectorizacion;

        private struct InfoSector
        {
            public int numSacta;
            public string idSector;
            public string idNucleo;
            /*
            public string tipo;
            public int prioridadR2;
             * */
            public bool dominante;
        };

        private struct InfoPosicion
        {
            public int numPosicion;
            public string idPosicion;
            public string nombreResultante;
            public List<InfoSector> sectores;
        };

        public Utilidades(MySqlConnection conexion)
        {
            GestorBDCD40 = new GestorBaseDatos(conexion);

            if (SemaforoSectorizacionSacta == null)
                SemaforoSectorizacionSacta = new System.Threading.Semaphore(1, 1);
        }

        public static void StartSnmp(string ip, string toIp)
        {
            UtilitiesCD40.GeneraIncidencias.StartSnmp(ip,toIp);
        }

        public static void EndSnmp()
        {
            UtilitiesCD40.GeneraIncidencias.EndSnmp();
        }

        public int SelectCountDistinctEnlaceSQL(DestinosRadioSector e)
        {
            string consulta = e.SelectCountDistinctDestinoSQL();
            return Convert.ToInt32(GestorBDCD40.ExecuteScalar(consulta));
        }

        public string[] ListaDeTroncalesEnUnaRuta(string idSistema, string idCentral, string idRuta)
        {
            TroncalesRuta tr = new TroncalesRuta();
            tr.IdSistema = idSistema;
            tr.IdRuta = idRuta;
            tr.Central = idCentral;

            DataSet ds = GestorBDCD40.DataSetSelectSQL(tr, null);
            List<Tablas> listaTroncales = tr.ListSelectSQL(ds);

            string[] listaIdTroncales = new string[listaTroncales.Count];
            for (ushort i = 0; i < listaTroncales.Count; i++)
                listaIdTroncales[i] = ((TroncalesRuta)listaTroncales[i]).IdTroncal;

            return listaIdTroncales;
        }

        public DataSet GetEstadosRecursoDestino(string idSistema, string idDestino, MySqlTransaction tran)
        {

            DataSet objDSSalida = null;

            StringBuilder strConsulta = new StringBuilder();

            //Se obtiene la lista de recursos del destino asignados a algún sector
            //En la consulta se excluyen los recursos del destino  que se encuentran en estadosRecursos y que no están asignados a ningún sector.
            //Se e algún caso, hemos detectado que aunque hay Fk a la tabla 
            strConsulta.Append("SELECT DISTINCT er.IdSistema,er.IdNucleo,er.IdSector,er.IdDestino,er.PosHMI,er.TipoRecurso FROM EstadosRecursos er ");
            strConsulta.AppendFormat("WHERE IdSistema='{0}' AND  IdDestino='{1}' ", idSistema, idDestino);
            strConsulta.Append ("AND EXISTS(SELECT 1 FROM destinosRadioSector S WHERE S.IdSistema=ER.IdSistema AND S.IdNucleo=ER.IdNucleo AND ");
            strConsulta.Append("S.IdSector=ER.IdSector AND S.PosHMI=ER.PosHMI AND ER.IdDestino=S.IdDestino) ");

            objDSSalida = GestorBDCD40.GetDataSet(strConsulta.ToString(), tran);

            strConsulta.Clear();

            return objDSSalida;

        }

        //(Description = "Activa una sectorización de un sistema concreto, actualizando 'Activa' a true en la sectorización pasada y 'Activa'" 
        //               " a false en el resto de sectorizaciones del sistema")
        public void ActivaSectorizacion(string id_sistema, string id_sectorizacion, DateTime now, bool sactaPresente)
        {
            // Eliminar la sectorización activa hasta este momento.

            GestorBaseDatos.logFile.Debug("Inicio Utilidades.ActivaSectorizacion: id_sectorizacion= " + id_sectorizacion+ " now="+now.ToString("dd/MM/yyyy HH:mm:ss"));

            // SE INICIA TRANSACCION
            MySqlTransaction tran = GestorBDCD40.StartTransaction(false);

            GestorBaseDatos.logFile.Debug("Se elimina la sectorización activa para el sistema " + id_sistema);

            Procedimientos.EliminaActiva(GestorBDCD40.ConexionMySql, tran, id_sistema);

           

            StringBuilder consulta = new StringBuilder();
            consulta.AppendFormat("UPDATE Sectorizaciones SET Activa=false WHERE IdSectorizacion<>'{0}' AND IdSistema='{1}'", id_sectorizacion, id_sistema);

            GestorBaseDatos.logFile.Debug("Se ejecuta la sentencia " + consulta.ToString());

            GestorBDCD40.ExecuteNonQuery(consulta.ToString(), tran);

            consulta.Clear();
            consulta.AppendFormat("UPDATE Sectorizaciones SET Activa=true,FechaActivacion=STR_TO_DATE('{0}','%d/%m/%Y %H:%i:%s') WHERE IdSectorizacion='{1}' AND IdSistema='{2}'", now.ToString("dd/MM/yyyy HH:mm:ss"), id_sectorizacion, id_sistema);

            GestorBaseDatos.logFile.Debug("Se ejecuta la sentencia " + consulta.ToString());

            GestorBDCD40.ExecuteNonQuery(consulta.ToString(), tran);

            consulta.Clear();
            consulta.Append("REPLACE INTO TablasModificadas (IdTabla) VALUES ('Sectorizaciones')");

            GestorBaseDatos.logFile.Debug("Se ejecuta la sentencia " + consulta.ToString());

            GestorBDCD40.ExecuteNonQuery(consulta.ToString(), tran);

            GestorBaseDatos.logFile.Debug("Se invoca al procedimiento CrearSectorizacionActiva: sistema=" + id_sistema + " Sectorizacion=" + id_sectorizacion + " sactaPresente=" + sactaPresente);
            CrearSectorizacionActiva(tran, id_sistema, id_sectorizacion, now, sactaPresente);

            GestorBaseDatos.logFile.Debug("Se finaliza la ejecución del procedimiento CrearSectorizacionActiva");
            // FIN TRANSACCION

            GestorBaseDatos.logFile.Debug("Se ejecuta commit para finalizar la transaccion");
            GestorBDCD40.Commit(tran);

            GestorBaseDatos.logFile.Debug("Se libera la transaccion");
            tran.Dispose();
            GestorBaseDatos.logFile.Debug("Transaccion liberada");

            GestorBaseDatos.logFile.Debug("Fin procedimiento Utilidades.ActivaSectorizacion");
        }

        private void CrearSectorizacionActiva(MySqlTransaction tran, string idSistema, string idSectorizacion, DateTime now, bool sactaPresente)
        {
            StringBuilder strMsg = new StringBuilder();
            Sectorizaciones s = new Sectorizaciones();
            s.IdSistema = idSistema;
            s.IdSectorizacion = now.ToString("dd/MM/yyyy HH:mm:ss");
            s.Activa = false;
            s.FechaActivacion = now;

            GestorBaseDatos.logFile.Debug("Inicio ejecución CrearSectorizacionActiva: idSistema=" + idSistema + " idSectorizacion=" + idSectorizacion + " now=" + now.ToString("dd/MM/yyyy HH:mm:ss"));

            GestorBaseDatos.logFile.Debug("Se inserta la sectorización: " + s.ToString());
            // Crear la nueva activa
            GestorBDCD40.ExecuteNonQuery(s.InsertSQL()[0], tran);

            GestorBaseDatos.logFile.Debug("Sectorización insertada");
            // Generar la nueva sectorización activa como copia de la original.
            //	Crea SectoresSectorizacion con IdSectorizacion el timestamp.

            strMsg.AppendFormat("Se llama al procedimiento Procedimientos.CreaSectoresActiva: idSistema={0},idSectorizacion={1},idActiva={2},sactaPresente={3} ", idSistema, idSectorizacion, s.IdSectorizacion, sactaPresente);
            GestorBaseDatos.logFile.Debug(strMsg.ToString());
            Procedimientos.CreaSectoresActiva(GestorBDCD40.ConexionMySql, tran, idSistema, idSectorizacion, s.IdSectorizacion, sactaPresente);
            if (sactaPresente)
            {
                StringBuilder consulta = new StringBuilder();
                consulta.Remove(0, consulta.Length);
                consulta.Append("REPLACE INTO TablasModificadas (IdTabla) VALUES ('SectoresSectorizacion')");

                GestorBaseDatos.logFile.Debug("SactaPresente=true. Se ejecuta la consulta: " + consulta.ToString());
                GestorBDCD40.ExecuteNonQuery(consulta.ToString(), tran);
            }

            strMsg.Clear();
            strMsg.AppendFormat("Se llama al procedimiento Procedimientos.CreaPosicionesActiva: idSistema={0},idSectorizacion={1},idActiva={2}", idSistema, idSectorizacion, s.IdSectorizacion);
            GestorBaseDatos.logFile.Debug("Se llama al procedimiento  Procedimientos.CreaPosicionesActiva");
            // Crea las posiciones en radio, internos y externos.
            Procedimientos.CreaPosicionesActiva(GestorBDCD40.ConexionMySql, tran, idSistema, idSectorizacion, s.IdSectorizacion);

            GestorBaseDatos.logFile.Debug("Fin la ejecución del procedimiento CrearSectorizacionActiva");
        }

        //(Description = "Retorna una cadena con el nombre de la agrupacion compuesta por los sectores pasados o null si no" +
        //               " existe ninguna agrupación.")
        public string GetAgrupacion(int cuantos, string listaUsuarios)
        {
            string consulta = "SELECT ag.IdAgrupacion FROM viewSectoresEnAgrupacion v, (SELECT COUNT(*) AS contador, sa.IdAgrupacion AS IdAgrupacion " +
                                                    "FROM sectoresagrupacion sa WHERE sa.idsector IN (" + listaUsuarios + ") GROUP BY sa.IdAgrupacion) ag " +
                                                    " WHERE ag.contador=" + cuantos + " AND ag.contador=v.contador AND ag.IdAgrupacion=v.IdAgrupacion";

            return (string)GestorBDCD40.ExecuteScalar(consulta);
        }

        //(Description = "Retorna el valor mínimo de la PrioridadR2 (máxima prioridad) de los usuarios que componen un sector.")
        public uint GetPrioridadSector(string id_sistema, string id_nucleo, string listaUsuarios)
        {
            string consulta = "SELECT MIN(PrioridadR2) FROM Sectores WHERE IdSistema='" + id_sistema + "' AND " +
                                                "IdNucleo='" + id_nucleo + "' AND IdSector IN (" + listaUsuarios + ")";

            return (uint)GestorBDCD40.ExecuteScalar(consulta);
        }

        public int UpdateDestinoSQL(ParametrosRecursoGeneral p, MySqlTransaction tran)
        {
            string[] cadena = p.UpdateDestinoSQL();
            int cuantos = 0;
            for (int i = 0; i < cadena.Length; i++)
            {
                cuantos += GestorBDCD40.ExecuteNonQuery(cadena[i], tran);
            }

            return cuantos;
        }

        public int LiberaDestinoSQL(ParametrosRecursoGeneral p, MySqlTransaction tran)
        {
            string[] cadena = p.LiberaDestinoSQL();
            int cuantos = 0;
            for (int i = 0; i < cadena.Length; i++)
            {
                cuantos += GestorBDCD40.ExecuteNonQuery(cadena[i], tran);
            }

            return cuantos;
        }

        public int SelectCountDistinctEnlaceSQL(Radio e,bool bListaEnlacesExternos=false )
        {
            string consulta = e.SelectCountDistinctDestinoSQL(bListaEnlacesExternos);
            return Convert.ToInt32(GestorBDCD40.ExecuteScalar(consulta));
        }

        //(Description = "Servicio encargado de obtener los datos necesarios para la elaboración de la sectorización")
        public SectorizationResult GeneraSectorizacion(string idSistema, string idSectorizacion, Tipo_Sectorizacion tipoSectorizacion, bool comprobarSectoresReales, bool estadoSacta = false)
        {
            string data = "";
            using (MySqlCommand myCommand = GestorBDCD40.ConexionMySql.CreateCommand())
            {
                GestorBDCD40.ConexionMySql.Open();
                MySqlTransaction trans = GestorBDCD40.StartTransaction(true);
                myCommand.Transaction = trans;

                try
                {
                    if (estadoSacta)
                        myCommand.CommandText = "SELECT sc.NumSacta,t.PosicionSacta	" +
                                            "FROM sectoressector ss, sectoressectorizacion s, sectores sc, top t, sectorizaciones z " +
                                            "WHERE z.Activa='1' AND " +
                                                    "s.IdSectorizacion=z.IdSectorizacion AND " +
                                                    "s.IdSistema='" + idSistema + "' AND " +
                                                    "sc.NumSacta != 20000 AND " +
                                                    "ss.IdSistema=s.IdSistema AND " +
                                                    "ss.IdNucleo=s.IdNucleo AND " +
                                                    "ss.IdSector=s.IdSector AND " +
                                                    "t.IdSistema=s.IdSistema AND " +
                                                    "t.IdTop=s.IdTOP AND " +
                                                    "sc.IdSistema=s.IdSistema AND " +
                                                    "sc.IdNucleo=s.IdNucleo AND " +
                                                    "sc.IdSector=ss.IdSectorOriginal UNION " +
                                            "SELECT sc.NumSacta,t.PosicionSacta	" +
                                                "FROM sectoressector ss, sectoressectorizacion s, sectores sc, top t " +
                                                "WHERE s.IdSectorizacion='" + idSectorizacion + "' AND " +
                                                        "s.IdSistema='" + idSistema + "' AND " +
                                                        "(sc.NumSacta >= 10000 AND sc.NumSacta < 20000) AND " +
                                                        "ss.IdSistema=s.IdSistema AND " +
                                                        "ss.IdNucleo=s.IdNucleo AND " +
                                                        "ss.IdSector=s.IdSector AND " +
                                                        "t.IdSistema=s.IdSistema AND  " +
                                                        "t.IdTop=s.IdTOP AND  " +
                                                        "sc.IdSistema=s.IdSistema AND  " +
                                                        "sc.IdNucleo=s.IdNucleo AND  " +
                                                        "sc.IdSector=ss.IdSectorOriginal AND " +
                                                        "t.PosicionSacta not in (SELECT t.PosicionSacta	" +
                                                                                    "FROM sectoressector ss, sectoressectorizacion s, sectores sc, top t , sectorizaciones z " +
                                                                                            "WHERE z.Activa='1' AND " +
                                                                                                   "s.IdSectorizacion=z.IdSectorizacion AND " +
                                                                                                    "s.IdSistema='" + idSistema + "' AND " +
                                                                                                    "sc.NumSacta < 10000 AND " +
                                                                                                    "ss.IdSistema=s.IdSistema AND " +
                                                                                                    "ss.IdNucleo=s.IdNucleo AND " +
                                                                                                    "ss.IdSector=s.IdSector AND " +
                                                                                                    "t.IdSistema=s.IdSistema AND " +
                                                                                                    "t.IdTop=s.IdTOP AND " +
                                                                                                    "sc.IdSistema=s.IdSistema AND " +
                                                                                                    "sc.IdNucleo=s.IdNucleo AND " +
                                                                                                    "sc.IdSector=ss.IdSectorOriginal) " +
                                              "order by numsacta";
                    else
                        myCommand.CommandText = "SELECT sc.NumSacta,t.PosicionSacta	" +
                                            "FROM sectoressector ss, sectoressectorizacion s, sectores sc, top t " +
                                            "WHERE s.IdSectorizacion='" + idSectorizacion + "' AND " +
                                                    "s.IdSistema='" + idSistema + "' AND " +
                                                    "sc.NumSacta != 20000 AND " +
                                                    "ss.IdSistema=s.IdSistema AND " +
                                                    "ss.IdNucleo=s.IdNucleo AND " +
                                                    "ss.IdSector=s.IdSector AND " +
                                                    "t.IdSistema=s.IdSistema AND " +
                                                    "t.IdTop=s.IdTOP AND " +
                                                    "sc.IdSistema=s.IdSistema AND " +
                                                    "sc.IdNucleo=s.IdNucleo AND " +
                                                    "sc.IdSector=ss.IdSectorOriginal ORDER BY sc.NumSacta";

                    using (MySqlDataReader dr = myCommand.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            data += string.Format("{0},{1};", dr.GetUInt32(0), dr.GetUInt32(1));
                        }
                    }

                    GeneraSectorizacionDll.Sectorization generaSectorizacion = new GeneraSectorizacionDll.Sectorization(myCommand, idSistema, idSectorizacion, data, 'C', 'P', tipoSectorizacion, DateTime.Now, comprobarSectoresReales);
                    GestorBDCD40.Commit(trans);
                }
                catch (SectorizationException sEx)
                {
                    GestorBDCD40.RollBack(trans);
                }
                catch (System.Data.SqlClient.SqlException sqlException)
                {
                    GestorBaseDatos.logFile.Error("Utilidades.GeneraSectorizacion SqlException Error:", sqlException);
                    GestorBaseDatos.logFile.Error("En la consulta " + myCommand.CommandText);
                    GestorBDCD40.RollBack(trans);

                    System.Diagnostics.Debug.Assert(false, sqlException.Number.ToString());
                }
                catch (SystemException objEx)
                {
                    GestorBaseDatos.logFile.Error("Utilidades.GeneraSectorizacion--> SystemException");
                    GestorBaseDatos.logFile.Error("Error:", objEx);
                }
                finally
                {
                    GestorBDCD40.ConexionMySql.Close();
                }
            }

            return GeneraSectorizacionDll.SectorizationResult.Ok;
        }

        // Generar sectorización recibida desde SACTA
        public GeneraSectorizacionDll.Sectorization GeneraSectorizacion(SactaInfo info, DateTime mementoActivacion)
        {
            GeneraSectorizacionDll.Sectorization generaSectorizacion = null;
            bool releaseSem = false;

            info["Resultado"] = 0;

            try
            {
                releaseSem = SemaforoSectorizacionSacta.WaitOne(60000, false);
                if (!releaseSem)
                {
                    throw new SectorizationException(SectorizationResult.TimeOutError, "SectorizationResult.TimeOutError");
                }

                lock (_Sync)
                {
                    GestorBDCD40.ConexionMySql.Open();
                    using (MySqlCommand myCommand = GestorBDCD40.ConexionMySql.CreateCommand())
                    {
                        generaSectorizacion = new GeneraSectorizacionDll.Sectorization(myCommand, (string)info["IdSistema"], (string)info["SectName"], (string)info["SectData"], 'C', 'P', Tipo_Sectorizacion.Sectorizacion_Completa, mementoActivacion, true);
                    }
                }
            }
            catch (SectorizationException x)
            {
                info["Resultado"] = 1;
                info["ErrorCause"] = x.Message;
            }
            catch (System.Data.SqlClient.SqlException sqlException)
            {
                info["Resultado"] = 1;
                System.Diagnostics.Debug.Assert(false, sqlException.Number.ToString());
                info["ErrorCause"] = sqlException.Message;
            }
            catch (Exception ex)
            {
                info["Resultado"] = 1;
                System.Diagnostics.Debug.Assert(false, ex.Message);
                info["ErrorCause"] = ex.Message;
            }
            finally
            {
                SemaforoSectorizacionSacta.Release();
                GestorBDCD40.ConexionMySql.Close();
            }

            EventResultSectorizacion(info);

            return (int)info["Resultado"] == 0 ? generaSectorizacion : null;
        }

        public void CreaEventoConfiguracion(string id_sistema, uint idIncidencia)
        {
            HistoricoIncidencias hIncidencia = new HistoricoIncidencias();
            hIncidencia.IdSistema = id_sistema;
            hIncidencia.IdIncidencia = idIncidencia;
            hIncidencia.TipoHw = Tipos.Tipo_Elemento_HW.TEH_SISTEMA;
            hIncidencia.IdHw = System.Globalization.CultureInfo.InstalledUICulture.TwoLetterISOLanguageName == "es" ? "Configuración" : "Configuration";

            GestorBDCD40.ExecuteNonQuery(hIncidencia.InsertSQL()[0], null);
        }

        public void CreaEventoConfiguracion(string id_sistema, uint idIncidencia, string[] parametros, string ipserverMantto = null)
        {
            /** 20170616. AGL. Para evitar la repeticion de registros de incidencias */
            if (ipserverMantto != null)
            {
                string param = string.Empty;
                foreach (string s in parametros)
                {
                    param += s + ",";
                }
                if (param.Length > 0)
                    param = param.Remove(param.Length - 1);

                UtilitiesCD40.GeneraIncidencias.SendTrap(ipserverMantto,
                    /*".1.1.600.1." + */idIncidencia.ToString(), string.Format("{0},{1}", idIncidencia, param));
                return;
            }

            Incidencias incidencia = new Incidencias(System.Globalization.CultureInfo.InstalledUICulture.TwoLetterISOLanguageName);
            HistoricoIncidencias hIncidencia = new HistoricoIncidencias();

            incidencia.IdIncidencia = idIncidencia;
            DataSet ds = GestorBDCD40.DataSetSelectSQL(incidencia, null);
            List<Tablas> i = incidencia.ListSelectSQL(ds);
            if (i.Count > 0)
            {
                hIncidencia.IdSistema = id_sistema;
                hIncidencia.IdIncidencia = idIncidencia;
                hIncidencia.TipoHw = Tipos.Tipo_Elemento_HW.TEH_SISTEMA;
                hIncidencia.IdHw = System.Globalization.CultureInfo.InstalledUICulture.TwoLetterISOLanguageName == "es" ? "Configuración" : "Configuration";
                hIncidencia.Descripcion=string.Format(((Incidencias)i[0]).Descripcion, parametros);

                GestorBDCD40.ExecuteNonQuery(hIncidencia.InsertSQL()[0], null);

                /** 20170619. AGL. Esto genera un doble registro de historico, el segundo además mal formateado. */
                // Generar histórico para visualizar en ventana de eventos de supervisión
                //System.Configuration.Configuration webConfiguracion = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                //string toServerMantto = webConfiguracion.AppSettings.Settings["ServerManttoIp"].Value;

                //UtilitiesCD40.GeneraIncidencias.SendTrap(toServerMantto, idIncidencia.ToString(), hIncidencia.Descripcion);
                /***********************************/
            }

            //if (ipserverMantto != null)
            //{
            //    string param = string.Empty;
            //    foreach (string s in parametros)
            //    {
            //        param += s + ",";
            //    }
            //    if (param.Length > 0)
            //        param = param.Remove(param.Length - 1);

            //    // TrapTo(ipserverMantto, "public", ((Incidencias)i[0]).Oid + "." + idIncidencia, string.Format("{0},{1}", idIncidencia,param));
            //}
        }

        public SectorizationResult GeneraSectorizacion(string idSistema, string idSectorizacion, string data)
        {
            Dictionary<int, InfoPosicion> infoSectorizacion = new Dictionary<int, InfoPosicion>();

            //            if (null != CadenaConexion)
            //            {
            try
            {
                #region
                //                    using (MySqlConnection mySqlConnectionToCd40 = new MySqlConnection(CadenaConexion.ToString()))
                //					using (MySqlCommand myCommand = mySqlConnectionToCd40.CreateCommand())
                using (MySqlCommand myCommand = GestorBDCD40.ConexionMySql.CreateCommand())
                {
                    GestorBDCD40.ConexionMySql.Open();

                    //MySqlTransaction mytrans = mySqlConnectionToCd40.BeginTransaction(IsolationLevel.ReadCommitted);
                    MySqlTransaction mytrans = GestorBDCD40.StartTransaction(true);
                    myCommand.Transaction = mytrans;

                    if ((idSectorizacion == null) || (idSectorizacion.Length == 0))
                    {
                        GestorBDCD40.RollBack(mytrans);
                        //mytrans.Rollback();
                        mytrans.Dispose();
                        return GeneraSectorizacionDll.SectorizationResult.Error;
                    }
                    string[] sectorUcs = data.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                    if (sectorUcs.Length % 2 != 0)
                    {
                        GestorBDCD40.RollBack(mytrans);
                        //mytrans.Rollback();
                        mytrans.Dispose();
                        return GeneraSectorizacionDll.SectorizationResult.Error;
                    }
                    if (sectorUcs.Length == 0)
                    {
                        GestorBDCD40.RollBack(mytrans);
                        //mytrans.Rollback();
                        mytrans.Dispose();
                        return GeneraSectorizacionDll.SectorizationResult.EmptySectorization;
                    }

                    myCommand.CommandText = "DELETE FROM Sectorizaciones WHERE IdSistema='" + idSistema +
                        "' AND IdSectorizacion='" + idSectorizacion + "'";
                    myCommand.ExecuteNonQuery();
                    // Comprobar que el IdSectorizacion existe en la tabla Sectorizaciones.                
                    myCommand.CommandText = "SELECT COUNT(*) FROM Sectorizaciones WHERE IdSistema='" + idSistema +
                        "' AND IdSectorizacion='" + idSectorizacion + "'";
                    object objCount = myCommand.ExecuteScalar();
                    if ((objCount != null) && (Int32.Parse(objCount.ToString()) > 0))
                    {   //Eliminar la asignacion de sectores a posiciones
                        myCommand.CommandText = "DELETE FROM SectoresSectorizacion WHERE IdSistema='" + idSistema +
                            "' AND IdSectorizacion='" + idSectorizacion + "'";
                        myCommand.ExecuteNonQuery();
                    }
                    else
                    { //La sectorizacion no existe, intentar crearla
                        myCommand.CommandText = "INSERT INTO Sectorizaciones SET IdSistema='" + idSistema + "',IdSectorizacion='" + idSectorizacion + "'";
                        myCommand.ExecuteNonQuery();
                    }

                    for (int i = 0, total = sectorUcs.Length; i < total; i += 2)
                    {
                        InfoSector elSector = new InfoSector();
                        elSector.numSacta = Int32.Parse(sectorUcs[i]);
                        InfoPosicion laPosicion;
                        bool nuevaPosicion = false;
                        if (!infoSectorizacion.TryGetValue(Int32.Parse(sectorUcs[i + 1]), out laPosicion))
                        {
                            laPosicion = new InfoPosicion();
                            nuevaPosicion = true;
                        }
                        laPosicion.numPosicion = Int32.Parse(sectorUcs[i + 1]);

                        //Obtener el IdTop que ocupa esa posicion
                        myCommand.CommandText = "SELECT IdTop FROM TOP WHERE IdSistema='" + idSistema +
                                                "' AND PosicionSacta=" + laPosicion.numPosicion.ToString();
                        MySqlDataReader dr = myCommand.ExecuteReader();
                        if (dr.HasRows)
                        {
                            if (dr.Read())
                            {
                                laPosicion.idPosicion = dr.GetString(0);
                            }
                        }
                        else
                        {
                            dr.Close();
                            GestorBDCD40.RollBack(mytrans);
                            //mytrans.Rollback();
                            mytrans.Dispose();
                            return GeneraSectorizacionDll.SectorizationResult.Error;
                        }
                        dr.Close();
                        //Obtener los datos del sector
                        myCommand.CommandText = "SELECT IdNucleo,IdSector FROM Sectores WHERE IdSistema='" + idSistema +
                            "' AND NumSacta=" + elSector.numSacta.ToString();
                        MySqlDataReader dr1 = myCommand.ExecuteReader();
                        //string idNucleo = "";
                        //string idSector = "";
                        if (dr1.HasRows)
                        {
                            if (dr1.Read())
                            {
                                elSector.idNucleo = dr1.GetString(0);
                                elSector.idSector = dr1.GetString(1);
                            }
                        }
                        else
                        {
                            dr1.Close();
                            GestorBDCD40.RollBack(mytrans);
                            //mytrans.Rollback();
                            mytrans.Dispose();
                            return GeneraSectorizacionDll.SectorizationResult.Error;
                        }
                        dr1.Close();

                        if (laPosicion.sectores == null)
                        {
                            laPosicion.sectores = new List<InfoSector>();
                            elSector.dominante = true;
                        }
                        else
                            elSector.dominante = false;
                        laPosicion.sectores.Add(elSector);

                        if (nuevaPosicion)
                            infoSectorizacion.Add(laPosicion.numPosicion, laPosicion);

                    }

                    foreach (KeyValuePair<int, InfoPosicion> kvp in infoSectorizacion)
                    {
                        if (kvp.Value.sectores.Count == 1)
                        {//la posicion solo tiene un sector 
                            //Insertar los datos del sector de la posicion en SectoresSectorizacion
                            myCommand.CommandText = "INSERT INTO SectoresSectorizacion VALUES ('" + idSistema + "','" + idSectorizacion + "','" +
                                kvp.Value.sectores[0].idNucleo + "','" + kvp.Value.sectores[0].idSector + "','" + kvp.Value.idPosicion + "')";
                            myCommand.ExecuteNonQuery();
                        }
                        else
                        { //Obtener el nombre de la posicion
                            System.Text.StringBuilder lista = new System.Text.StringBuilder();
                            for (int i=0;i<kvp.Value.sectores.Count;i++)
                                lista.AppendFormat("'{0}',", kvp.Value.sectores[i].idSector);
                            lista = lista.Remove(lista.Length - 1, 1);
                            string nomAgrupacion = GetAgrupacion(kvp.Value.sectores.Count, lista.ToString());
                            if (nomAgrupacion == null)
                            {
                                lista.Remove(0, lista.Length);
                                for (int i = 0; i < kvp.Value.sectores.Count; i++)
                                    lista.AppendFormat("{0},", kvp.Value.sectores[i].idSector);
                                lista = lista.Remove(lista.Length - 1, 1);
                                nomAgrupacion = GeneraAlgoritmo(idSistema, lista.ToString(), mytrans);
                            }
                            InfoPosicion po;
                            if (infoSectorizacion.TryGetValue(kvp.Key, out po))
                                po.nombreResultante = nomAgrupacion;
                            //((InfoPosicion)infoSectorizacion[kvp.Key]).nombreResultante = nomAgrupacion;
                            //Comprobar si existe ese nombre en la tabla SECTORES
                            myCommand.CommandText = "SELECT COUNT(*) FROM Sectores WHERE IdSistema='" + idSistema +
                                                   "' AND IdSector='" + nomAgrupacion + "'";
                            object objCount1 = myCommand.ExecuteScalar();
                            if ((objCount1 != null) && (Int32.Parse(objCount1.ToString()) > 0))
                            {

                            }
                            else
                            {//no existe el sector => crearlo  
                                //myCommand.CommandText = "INSERT INTO Sectores VALUES ('" + idSistema + "','" + kvp.Value.sectores[0].idNucleo +
                                //        "','" + po.nombreResultante + "','','','',0,'R','C',4,0,0)";
                                myCommand.CommandText = "INSERT INTO Sectores SET Idsistema='" + idSistema + "',IdNucleo='" + kvp.Value.sectores[0].idNucleo
                                    + "',IdSector='" + po.nombreResultante + "',SectorSimple=0,Tipo='R',TipoPosicion='C',PrioridadR2=4,TipoHMI=0,NumSacta=0";
                                myCommand.ExecuteNonQuery();
                                //SectoresSector
                                foreach (InfoSector sector in kvp.Value.sectores)
                                {
                                    myCommand.CommandText = "INSERT INTO SectoresSector VALUES ('" + idSistema + "','" + sector.idNucleo + "','" +
                                        po.nombreResultante + "','" + sector.idSector + "'," + (sector.dominante ? "1" : "0") + ")";
                                    myCommand.ExecuteNonQuery();
                                }
                            }
                            //Actualizar SectoresSectorizacion
                            myCommand.CommandText = "INSERT INTO SectoresSectorizacion VALUES ('" + idSistema + "','" + idSectorizacion + "','" +
                                kvp.Value.sectores[0].idNucleo + "','" + po.nombreResultante + "','" + kvp.Value.idPosicion + "')";
                            myCommand.ExecuteNonQuery();
                        }
                    }

                    //						GestorBDCD40.Commit(mytrans);
                    //mytrans.Commit();
                    //                        mytrans.Dispose();

                    try
                    {
                        GeneraSectorizacionDll.Sectorization generaSectorizacion = new GeneraSectorizacionDll.Sectorization(myCommand, idSistema, idSectorizacion, data, 'C', 'P', Tipo_Sectorizacion.Sectorizacion_Completa, DateTime.Now, true);
                        // DateTime now = DateTime.Now;
                        // CrearSectorizacionActiva(idSistema, idSectorizacion, now);
                        GestorBDCD40.Commit(mytrans);
                    }
                    catch (SectorizationException)
                    {
                        //sException.Result==SectorizationResult.EmptySectorization
                    }
                    catch (System.Data.SqlClient.SqlException sqlException)
                    {
                        System.Diagnostics.Debug.Assert(false, sqlException.Number.ToString());
                    }
                    finally
                    {
                        //mytrans.Commit();
                        mytrans.Dispose();
                    }
                }
                #endregion
            }
            catch (MySqlException)
            {
                return GeneraSectorizacionDll.SectorizationResult.Error;
            }
            // }

            return GeneraSectorizacionDll.SectorizationResult.Ok;
        }

        private string GeneraAlgoritmo(string idSistema, string listaSectores, MySqlTransaction tran)
        {
            System.Text.StringBuilder nombreAlgoritmo = new System.Text.StringBuilder();

            string[] sectores = listaSectores.Split(new char[] { ',', '\'' }, StringSplitOptions.RemoveEmptyEntries);

            // Obtener los parámetros de longitud de identificadores del sistema
            Sistema pSistema = new Sistema();
            pSistema.IdSistema = idSistema;

            //AccesoABaseDeDatos a = new AccesoABaseDeDatos();
            DataSet ds = GestorBDCD40.DataSetSelectSQL(pSistema, tran);
            List<Tablas> sistema = pSistema.ListSelectSQL(ds);
            if (sistema.Count > 0)
                pSistema = (Sistema)sistema[0];

            if (sectores.Length > 0)
            {
                int numCaracteresPorSector = (int)pSistema.TamLiteralDA / sectores.Length;

                foreach (string s in sectores)
                    nombreAlgoritmo.Append(s.Substring(0, System.Math.Min(s.Length, numCaracteresPorSector)));
            }

            return nombreAlgoritmo.ToString();
        }

        //static public void TrapTo(string ipTo, string community, string oid, string val)
        //{
        //    List<Variable> lst = new List<Variable>();
        //    Variable var = new Variable(new ObjectIdentifier(oid), new OctetString(val));
        //    lst.Add(var);

        //    Lextm.SharpSnmpLib.Messaging.Messenger.SendTrapV2(0, VersionCode.V2,
        //        new System.Net.IPEndPoint(System.Net.IPAddress.Parse(ipTo), 162),
        //        new OctetString(community),
        //        new ObjectIdentifier(oid),
        //        0, lst);
        //}

        //Devuelve la lista de recursos radio sin asignar 
        public DataSet GetRecursosRadioSinAsignarAEnlaces(string idSistema, string strIdEmpl, MySqlTransaction tran)
        {
            StringBuilder strConsulta = new StringBuilder();

            DataSet objDatos = null;

            // Se obtiene la lista de recursos radio sin asignar excepto los recursos de tipo M+N (R.Tipo>=4 AND R.Tipo<=6) que no son principales HFP.TipoModo!=0.
            //Los recursos R.Tipo 4: M+N RX  Receptores
            //                    5: M+N TX  Transmisores
            //                    6: M+N RX TX  Transceptores

            /* SELECT RR.IdSistema, RR.IdRecurso, R.Tipo,HFP.TipoModo FROM Recursos R,RecursosRadio RR left join hfparams HFP on RR.IdSistema=HFP.IdSistema and RR.IdRecurso = HFP.IdRecurso
                WHERE R.IdRecurso = RR.IdRecurso AND R.IdSistema=RR.IdSistema  AND R.TipoRecurso=RR.TipoRecurso AND 
             
                    RR.IdSistema='departamento' AND
                    RR.IdDestino is null AND 
                    ( (HFP.idRecurso is not null AND 
                    ( (R.Tipo>=4 AND R.Tipo<=6 and HFP.TipoModo=0) OR (R.Tipo<4 AND R.Tipo>6) )
                        ) OR HFP.idRecurso is null
                      );
            */
            try
            {
                strConsulta.Append("SELECT RR.IdSistema, RR.IdRecurso, R.Tipo FROM Recursos R,RecursosRadio RR ");
                strConsulta.Append("LEFT JOIN hfparams HFP ON RR.IdSistema=HFP.IdSistema and RR.IdRecurso = HFP.IdRecurso ");
                strConsulta.Append("WHERE R.IdRecurso = RR.IdRecurso AND R.IdSistema=RR.IdSistema  AND R.TipoRecurso=RR.TipoRecurso AND ");
                strConsulta.AppendFormat("RR.IdSistema='{0}' AND RR.IdDestino IS NULL  ", idSistema);
                strConsulta.Append("AND (HFP.idRecurso IS NULL OR (HFP.idRecurso IS NOT NULL AND ( (R.Tipo>=4 AND R.Tipo<=6 and HFP.TipoModo=0) OR R.Tipo<4 OR R.Tipo>6))) ");


                if (!string.IsNullOrWhiteSpace(strIdEmpl))
                {
                    strConsulta.AppendFormat(" AND RR.IdEmplazamiento='{0}'", strIdEmpl);
                }

                objDatos = GestorBDCD40.GetDataSet(strConsulta.ToString(), tran);
            }
            catch (Exception ex)
            {
                GestorBaseDatos.logFile.Error("Utilidades.GetRecursosRadioSinAsignarAEnlacesError--> al ejecutar la consulta:" + strConsulta);
                GestorBaseDatos.logFile.Error("Error:", ex);
            }
            finally
            {
            
                strConsulta.Clear();
            }

            return objDatos;

        }

    }

}
