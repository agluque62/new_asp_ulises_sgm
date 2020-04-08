using System;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
using System.Collections.Generic;
using CD40.BD.Entidades;
using CD40.BD;
namespace UlisesWebConfig.WebServices.MainService
{
	/// <summary>
	/// Descripción breve de Class1
	/// </summary>
	public partial class MainService : System.Web.Services.WebService
	{
		[WebMethod]
		public DataSet ControladoresRecuperaSectorizaciones(string idSistema)
		{
			//// ResetServiciosCD40("ConexionBaseDatosCD40");

			Sectorizaciones s = new Sectorizaciones();
			s.IdSistema = idSistema;
			System.Data.DataSet ds = DataSetSelectSQL(s);

			//// ResetServiciosCD40("ConexionBaseDatosCD40_Trans");

			return ds;
		}

		[WebMethod]
		public DataSet ControladoresRecuperaIdSectorizacionActiva(string idSistema)
		{
			//// ResetServiciosCD40("ConexionBaseDatosCD40");

			Sectorizaciones s = new Sectorizaciones();
			s.IdSistema = idSistema;
			s.Activa = true;
			System.Data.DataSet ds = DataSetSelectSQL(s);

			//// ResetServiciosCD40("ConexionBaseDatosCD40_Trans");

			return ds;
		}

		[WebMethod]
		public DataSet ControladoresRecuperaSectoresActiva(string idSistema)
		{
			//// ResetServiciosCD40("ConexionBaseDatosCD40");

			Sectorizaciones s = new Sectorizaciones();
			s.IdSistema = idSistema;
			s.Activa = true;
			System.Data.DataSet ds = AsignacionDeUsuariosATops(idSistema, GetVersionConfiguracion(idSistema));

			//// ResetServiciosCD40("ConexionBaseDatosCD40_Trans");

			return ds;
		}

		[WebMethod]
		public DataSet ControladoresRecuperaListaSectores(string idSistema, string idNucleo, string idSector)
		{
			string consulta = "SELECT ss.*, s.Tipo FROM SectoresSector ss " +
								"JOIN Sectores s " +
								"WHERE ss.IdSistema='" + idSistema + "' AND " +
										"ss.IdNucleo='" + idNucleo + "' AND " +
										"ss.IdSector='" + idSector + "' AND " +
										"s.idsistema = ss.IdSistema AND s.idNucleo = ss.IdNucleo AND s.IdSector = ss.IdSector";
			return GestorBDCD40.GetDataSet(consulta, null);
		}

		[WebMethod]
		public DataSet ControladoresRecuperaTops(string idSistema)
		{
			//// ResetServiciosCD40("ConexionBaseDatosCD40");

			Top top = new Top();
			top.IdSistema = idSistema;

			System.Data.DataSet ds = DataSetSelectSQL(top);

			//// ResetServiciosCD40("ConexionBaseDatosCD40_Trans");

			return ds;
		}

		[WebMethod(Description = "Desencadena proceso multicast para activación de una sectorización desde ventana controladores")]
		public bool ControladoresComunicaSectorizacionActiva(string id_sistema, string id_sectorizacion, ref DateTime fechaActivacion)
		{
			System.Configuration.Configuration webConfiguracion = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
			string listenIp = webConfiguracion.AppSettings.Settings["ListenIp"].Value;
			string toServerMantto = webConfiguracion.AppSettings.Settings["ServerManttoIp"].Value;

			byte iEstadoSacta = 0;

			iEstadoSacta = GetEstadoSacta();

			if (iEstadoSacta == 0 || iEstadoSacta == 16 || id_sectorizacion == "SACTA") // No hay enlace con SACTA o la sectorización es de SACTA
			{
				// Servicio no arrancado (iEstadoSacta==0) o servicio arrancado pero sin conexion con SACTA (iEstadoSacta == 16) o la sectorización es de SACTA

				if (!EstanTodosLosReales(id_sistema, id_sectorizacion))
				{
					CD40.BD.Utilidades util = new CD40.BD.Utilidades(GestorBDCD40.ConexionMySql);
					string[] parametros = { id_sectorizacion };
					util.CreaEventoConfiguracion(id_sistema, 108, parametros, toServerMantto);

					fechaActivacion = DateTime.MinValue;
					return false;
				}

				// Activa una sectorizacion en la base de datos.
				Utilidades.ActualizaActiva(GestorBDCD40.ConexionMySql, id_sistema, id_sectorizacion, fechaActivacion, false);
				// ResetServiciosCD40("ConexionBaseDatosCD40_Trans");

				// Activa una sectorizacion en la base de datos CD40_Trans.
				Utilidades.ActualizaActiva(GestorBDCD40.ConexionMySql, id_sistema, id_sectorizacion, fechaActivacion, false);

				// Elimina los registros de las tablas modificadas en la bd CD40 como
				// consecuencia de la llamada a Utilidades.ActualizaActiva sobre dicha bd.
				//GestorBDCD40.ExecuteNonQuery("TRUNCATE TABLE TablasModificadas", null);
			}
			else            // La sectorización es generada desde configuración pero con enlace SACTA
			{
				GenerarSectorizacion(id_sistema, id_sectorizacion, false);

				Utilidades.ActualizaActiva(GestorBDCD40.ConexionMySql, id_sistema, id_sectorizacion, fechaActivacion, true);
				// ResetServiciosCD40("ConexionBaseDatosCD40_Trans");

				// Activa una sectorizacion en la base de datos CD40_Trans.
				Utilidades.ActualizaActiva(GestorBDCD40.ConexionMySql, id_sistema, id_sectorizacion, fechaActivacion, false);

				// Elimina los registros de las tablas modificadas en la bd CD40 como
				// consecuencia de la llamada a Utilidades.ActualizaActiva sobre dicha bd.
				//GestorBDCD40.ExecuteNonQuery("TRUNCATE TABLE TablasModificadas", null);
			}

			Rollback();

			return Utilidades.NotificaSectorizacion(GestorBDCD40, GestorBDCD40ToMantto, listenIp, id_sistema, id_sectorizacion, out fechaActivacion, toServerMantto);
		}

		[WebMethod]
		public DataSet ControladoresRecuperaSectoresSectorizacion(string idSistema, string idSectorizacion)
		{
			//// ResetServiciosCD40("ConexionBaseDatosCD40");

			//Sectorizaciones s = new Sectorizaciones();
			//s.IdSistema = idSistema;
			//s.Activa = true;
			System.Data.DataSet ds = AsignacionDeUsuariosATops(idSistema, idSectorizacion);

			//// ResetServiciosCD40("ConexionBaseDatosCD40_Trans");

			return ds;
		}

		[WebMethod]
		public void ControladoresRegeneraSectorizacion(string idsistema, string[] listaTops, string[][] listaSectores, string[] listaNucleos)
		{
			/*
			CD40.BD.Utilidades ut = new CD40.BD.Utilidades(GestorBDCD40.ConexionMySql);

			GeneraSectorizacionDll.Tipo_Sectorizacion tipoSectorizacion = GeneraSectorizacionDll.Tipo_Sectorizacion.Sectorizacion_Completa;

			lock (_Sync)
			{
				// Crea la sectorización con IdSectorizacion="TEMPORARY_CONTROLLER_SCTZ"
				ResetSectorizacion(null, idsistema);
				ut.GeneraSectorizacion(idsistema, "TEMPORARY_CONTROLLER_SCTZ", tipoSectorizacion, false);
			}
			*/

			int i = 0;
			int numTop = 0;
			//// ResetServiciosCD40("ConexionBaseDatosCD40");
			//MySql.Data.MySqlClient.MySqlTransaction trans = GestorBDCD40.StartTransaction(true);


			Dictionary<string, string[]> asignacionSectores = new Dictionary<string, string[]>(listaTops.Length);
			foreach (string s in listaTops)
			{
				asignacionSectores[s] = listaSectores[numTop++];
			}

			try
			{
				// Crea la sectorización con IdSectorizacion="TEMPORARY_CONTROLLER_SCTZ"
				ResetSectorizacion(null, idsistema);

				foreach (KeyValuePair<string, string[]> top in asignacionSectores)
				{
					//if (top.Value.Length > 0)
					// Crea la asignación de sectores a tops para la sectorización "TEMPORARY_CONTROLLER_SCTZ"
					GenerarTop(null, idsistema, listaNucleos[i], top);

					i++;
				}
				//GestorBDCD40.Commit(null);

				// Generar la sectorización en el servidor
				GenerarSectorizacion(idsistema, "TEMPORARY_CONTROLLER_SCTZ", false);
			}
			catch (MySql.Data.MySqlClient.MySqlException)
			{
				//GestorBDCD40.RollBack(trans);
			}

			//// ResetServiciosCD40("ConexionBaseDatosCD40_Trans");
		}

		[WebMethod]
		public void ControladoresRenombraSectorizacion(string idSistema, string idSectorizacion)
		{
			Sectorizaciones s = new Sectorizaciones();
			s.IdSistema = idSistema;
			s.IdSectorizacion = idSectorizacion;

			// // ResetServiciosCD40("ConexionBaseDatosCD40");
			MySql.Data.MySqlClient.MySqlTransaction trans = GestorBDCD40.StartTransaction(true);

			try
			{
				string consulta = "DELETE FROM Sectorizaciones WHERE IdSistema='" + idSistema + "' AND IdSectorizacion='" + idSectorizacion + "'";
				GestorBDCD40.ExecuteNonQuery(consulta, trans);

				consulta = "UPDATE Sectorizaciones SET IdSectorizacion='" + idSectorizacion + "' WHERE IdSistema='" + idSistema +
									"' AND IdSectorizacion='TEMPORARY_CONTROLLER_SCTZ'";
				GestorBDCD40.ExecuteNonQuery(consulta, trans);
				GestorBDCD40.Commit(trans);
			}
			catch (MySql.Data.MySqlClient.MySqlException)
			{
				GestorBDCD40.RollBack(trans);
			}
			//finally
			//{
			// // ResetServiciosCD40("ConexionBaseDatosCD40_Trans");
			//}

		}

		private void GenerarTop(MySql.Data.MySqlClient.MySqlTransaction trans, string idsistema, string nucleo, KeyValuePair<string, string[]> top)
		{
			try
			{
				if (top.Value.Length > 0)
				{
					string nomSector = DameNombreSector(trans, top.Value, idsistema, nucleo);
					if (nomSector != "")
					{
						// Crear el nuevo sector
						GeneraSector(trans, idsistema, nucleo, nomSector, top.Value);

						// Insertar el nuevo sector en la sectorización
						SectoresSectorizacion ss = new SectoresSectorizacion();
						ss.IdSistema = idsistema;
						ss.IdSectorizacion = "TEMPORARY_CONTROLLER_SCTZ";
						ss.IdTop = top.Key;
						ss.IdNucleo = nucleo;
						ss.IdSector = nomSector;

						GestorBDCD40.InsertSQL(ss, trans);

						// Crear el nuevo SectoresSector
						GeneraSectoresSector(trans, idsistema, nucleo, nomSector, top);
					}
				}
				else
				{
					string nomSector = "**FS**";

					// Crear el nuevo sector
					GeneraSector(trans, idsistema, nucleo, nomSector, null);

					// Insertar el nuevo sector en la sectorización
					SectoresSectorizacion ss = new SectoresSectorizacion();
					ss.IdSistema = idsistema;
					ss.IdSectorizacion = "TEMPORARY_CONTROLLER_SCTZ";
					ss.IdTop = top.Key;
					ss.IdNucleo = nucleo;
					ss.IdSector = nomSector;

					GestorBDCD40.InsertSQL(ss, trans);
				}
			}
			catch (Exception)
			{

			}
		}

		private void GeneraSectoresSector(MySql.Data.MySqlClient.MySqlTransaction trans, string idsistema, string nucleo, string nomSector, KeyValuePair<string, string[]> top)
		{
			try
			{
				SectoresSector ss = new SectoresSector();

				foreach (string s in top.Value)
				{
					ss.IdSistema = idsistema;
					ss.IdNucleo = nucleo;
					ss.IdSector = nomSector;
					ss.IdSectorOriginal = s;

					// Comprobar que esta combinación de sectores no existe en la tabla Sectores
					// en cuyo caso no hay nada que generar
					if (GestorBDCD40.ListSelectSQL(ss, trans).Count > 0)
						continue;

					ss.EsDominante = top.Value[0] == s; // si es el primer usuario, es dominante

					GestorBDCD40.InsertSQL(ss, trans);
				}
			}
			catch (Exception)
			{
			}
		}

		private void GeneraSector(MySql.Data.MySqlClient.MySqlTransaction trans, string idSistema, string idNucleo, string nomSector, string[] listSectores)
		{
			try
			{
				//ServiciosCD40.ServiciosCD40 g = new ServiciosCD40.ServiciosCD40();
				Sectores s = new Sectores();

				s.IdSistema = idSistema;
				s.IdSector = nomSector;
				s.IdNucleo = idNucleo;

				// Comprobar que esta combinación de sectores no existe en la tabla Sectores
				// en cuyo caso no hay nada que generar
				if (GestorBDCD40.ListSelectSQL(s, trans).Count > 0)
					return;

				s.IdParejaUCS = s.IdNucleoParejaUCS = s.IdSistemaParejaUCS = null;
				s.SectorSimple = false;
				s.Tipo = "R";
				s.TipoPosicion = "C";

				// Obtener literales de los usuarios
				if (listSectores != null)
				{
					System.Text.StringBuilder lista = new System.Text.StringBuilder();
					foreach (string user in listSectores)
					{
						lista.AppendFormat("'{0}',", user);
					}
					lista = lista.Remove(lista.Length - 1, 1);
					s.PrioridadR2 = GetPrioridadSector(s.IdSistema, s.IdNucleo, lista.ToString());
				}

				s.TipoHMI = 0;
				s.NumSacta = 0;

				GestorBDCD40.InsertSQL(s, trans);
			}
			catch (Exception)
			{
			}
		}

		private string DameNombreSector(MySql.Data.MySqlClient.MySqlTransaction trans, string[] listaSectores, string idSistema, string idNucleo)
		{
			System.Text.StringBuilder lista = new System.Text.StringBuilder();
			foreach (string s in listaSectores)
				lista.AppendFormat("'{0}',", s);

			// Buscar una agrupación
			if (lista.Length > 0)
			{
				lista = lista.Remove(lista.Length - 1, 1);

				CD40.BD.Utilidades u = new CD40.BD.Utilidades(GestorBDCD40.ConexionMySql);
				string nomAgrupacion = u.GetAgrupacion(listaSectores.Length, lista.ToString());
				// Si no la encuentra, aplicar algoritmo
				if (nomAgrupacion == null)
					nomAgrupacion = GeneraAlgoritmo(idSistema, idNucleo, lista.ToString(), trans);

				return nomAgrupacion;
			}

			return "";
		}

		private void ResetSectorizacion(MySql.Data.MySqlClient.MySqlTransaction trans, string idsistema)
		{
			Sectorizaciones sctz = new Sectorizaciones();
			sctz.IdSistema = idsistema;
			sctz.IdSectorizacion = "TEMPORARY_CONTROLLER_SCTZ";

			GestorBDCD40.DeleteSQL(sctz, trans);
			GestorBDCD40.InsertSQL(sctz, trans);
		}

		//private void // ResetServiciosCD40(string idConexion)
		//{
		//    MySqlConnectionToCd40.Dispose();

		//    string cadenaConexion;
		//    System.Configuration.Configuration webConfiguracion = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
		//    if (webConfiguracion.ConnectionStrings.ConnectionStrings.Count > 0)
		//    {
		//        cadenaConexion = webConfiguracion.ConnectionStrings.ConnectionStrings[idConexion].ToString();

		//        MySqlConnectionToCd40 = new MySql.Data.MySqlClient.MySqlConnection(cadenaConexion);

		//        GestorBDCD40 = new GestorBaseDatos(MySqlConnectionToCd40);
		//    }
		//}
	}

}
