using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using CD40.BD;
using CD40.BD.Entidades;

/// <summary>
/// Summary description for Utilidades
/// </summary>
public class Utilidades
{
    public Utilidades()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    private static void ComunicacionMulticast(string listenIp, string grupoMulticast, uint puertoMulticast, byte[] buffer)
    {
        foreach (IPAddress ipAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
        {
            if (ipAddress.ToString() == listenIp)
            {
                IPEndPoint ipLocalEndPoint = new IPEndPoint(ipAddress, (int)puertoMulticast);

                ////////////////////////////////////////////////////////////////////////////////////////////////
                // Cambiar UdpClient por un constructor al que no se le pase ni puerto ni dirección IP de origen
                ////////////////////////////////////////////////////////////////////////////////////////////////
                //UdpClient sckMulticast = new UdpClient(ipLocalEndPoint);

                UdpClient sckMulticast = new UdpClient();

                IPEndPoint ipTo = new IPEndPoint(IPAddress.Parse(grupoMulticast), (int)puertoMulticast);
                sckMulticast.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastInterface, IPAddress.Any.GetAddressBytes());
                sckMulticast.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 10);
                // Por problemas en la recepción multicast en la pasarela, se cambia a broadcast
                //sckMulticast.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
                sckMulticast.Send(buffer, buffer.Length, ipTo);
                sckMulticast.Close();
            }
        }
    }
	/*
    public DataSet SelectView(string viewName, params string[] strWhere)
    {
        AccesoABaseDeDatos a = new AccesoABaseDeDatos();
        return a.SelectView(viewName, strWhere);
    }
	*/
	public static void ActualizaActiva(MySql.Data.MySqlClient.MySqlConnection conexionCD40, string idSistema, string idSectorizacion, DateTime now, bool sactaPresente)
	{
		if (string.Compare(idSectorizacion, "SACTA") != 0)
            now = DateTime.Now;
        else
        {
            //Si la sectorización es SACTA y la fecha de activación no está inicializada, se actualiza a la fecha actual
            if (now == DateTime.MinValue)
                now = DateTime.Now;
        }

		CD40.BD.Utilidades u = new CD40.BD.Utilidades(conexionCD40);
		u.ActivaSectorizacion(idSistema, idSectorizacion, now, sactaPresente);
		//u.CrearSectorizacionActiva(idSistema, idSectorizacion, now, sactaPresente);
	}

	public static void GeneraVersion(MySql.Data.MySqlClient.MySqlConnection conexionCD40, string idSistema, string path)
	{
		// Procedimientos pr = new Procedimientos();

		GetVersion v;
		v.IdSistema = idSistema;
		v.Version = Procedimientos.VersionSectorizacion(conexionCD40, idSistema);

		try
		{
			System.Xml.Serialization.XmlSerializer xmlWriter = new System.Xml.Serialization.XmlSerializer(typeof(GetVersion));

			System.IO.StreamWriter xmlFile = new System.IO.StreamWriter(path);
			xmlWriter.Serialize(xmlFile, v);
			xmlFile.Close();
		}
		catch (System.IO.IOException e)
		{
			System.Diagnostics.Debug.Assert(false, e.Message);
		}
	}

	//public void CreaEventoConfiguracion(string id_sistema, uint idIncidencia)
	//{
	//    CD40.BD.Entidades.HistoricoIncidencias hIncidencia = new CD40.BD.Entidades.HistoricoIncidencias();
	//    hIncidencia.IdSistema = id_sistema;
	//    hIncidencia.IdIncidencia = idIncidencia;
	//    hIncidencia.TipoHw = CD40.BD.Entidades.Tipos.Tipo_Elemento_HW.TEH_SISTEMA;
	//    hIncidencia.IdHw = "Configuracion";

	//    AccesoABaseDeDatos a = new AccesoABaseDeDatos();
	//    a.InsertSQL(hIncidencia);
	//}

	public static CD40.BD.Entidades.ParametrosSector RegeneraParametrosSector(DataSet ps)
	{
		CD40.BD.Entidades.ParametrosSector param = new CD40.BD.Entidades.ParametrosSector();

        param.Intruido = false; 
        param.Intrusion = false;

        if (ps.Tables.Count > 0)
        {
            //Se inicializa el atributo intruido con el valor true
            //Si algun sector no tiene permiso intruido, la agrupación tampoco la tiene
            param.Intruido = true;

            foreach (DataRow r in ps.Tables[0].Rows)
            {
                param.NumLlamadasEntrantesIda = param.NumLlamadasEntrantesIda > (uint)r["NumLLamadasEntrantesIDA"] ? param.NumLlamadasEntrantesIda : (uint)r["NumLLamadasEntrantesIDA"];
                param.NumLlamadasEnIda = param.NumLlamadasEnIda > (uint)r["NumLlamadasEnIDA"] ? param.NumLlamadasEnIda : (uint)r["NumLlamadasEnIDA"];
                param.NumFrecPagina = param.NumFrecPagina > (uint)r["NumFreqPagina"] ? param.NumFrecPagina : (uint)r["NumFreqPagina"];
                param.NumPagFrec = param.NumPagFrec > (uint)r["NumPagFreq"] ? param.NumPagFrec : (uint)r["NumPagFreq"];
                param.NumDestinosInternosPag = param.NumDestinosInternosPag > (uint)r["NumDestinosInternosPag"] ? param.NumDestinosInternosPag : (uint)r["NumDestinosInternosPag"];
                param.NumPagDestinosInt = param.NumPagDestinosInt > (uint)r["NumPagDestinosInt"] ? param.NumPagDestinosInt : (uint)r["NumPagDestinosInt"];
                param.Intrusion |= (bool)r["Intrusion"];

                //Solo tendrá permiso intruido si todos los sectores lo tienen permiso de intruido
                param.Intruido &= (bool)r["Intruido"];
                param.KeepAlivePeriod = param.KeepAlivePeriod > (uint)r["KeepAlivePeriod"] ? param.KeepAlivePeriod : (uint)r["KeepAlivePeriod"];
                param.KeepAliveMultiplier = param.KeepAliveMultiplier > (uint)r["KeepAliveMultiplier"] ? param.KeepAliveMultiplier : (uint)r["KeepAliveMultiplier"];
            }
        }

		return param;
	}

	public static bool NotificaSectorizacion(GestorBaseDatos gestorBDCD40, GestorBaseDatos gestorBDCD40ToMantto,string listenIp, string id_sistema, string id_sectorizacion, out DateTime fechaActivacion, string serverManttoIp = null)
	{
		//AccesoABaseDeDatos a = new AccesoABaseDeDatos();
		CD40.BD.Utilidades util = new CD40.BD.Utilidades(gestorBDCD40ToMantto.ConexionMySql);
		Sectorizaciones s = new Sectorizaciones();
		string[] parametros ={ id_sectorizacion };

		s.IdSistema = id_sistema;
		s.IdSectorizacion = id_sectorizacion;
		List<Tablas> sActiva = gestorBDCD40.ListSelectSQL(s, null);
		if (sActiva.Count > 0)
		{
			s.FechaActivacion = ((Sectorizaciones)sActiva[0]).FechaActivacion;
			Sistema sis = new Sistema();
			sis.IdSistema = id_sistema;
			List<Tablas> sSistema = gestorBDCD40.ListSelectSQL(sis, null);
			if (sSistema.Count > 0)
			{
				sis.GrupoMulticastConfiguracion = ((Sistema)sSistema[0]).GrupoMulticastConfiguracion;
				sis.PuertoMulticastConfiguracion = ((Sistema)sSistema[0]).PuertoMulticastConfiguracion;
                ComunicacionMulticast(listenIp, sis.GrupoMulticastConfiguracion, sis.PuertoMulticastConfiguracion, System.Text.ASCIIEncoding.ASCII.GetBytes("1" + s.FechaActivacion.ToString("dd/MM/yyyy HH:mm:ss")));
			}

            util.CreaEventoConfiguracion(id_sistema, 105, parametros, serverManttoIp);
			fechaActivacion = s.FechaActivacion;
			return true;
		}

		util.CreaEventoConfiguracion(id_sistema, 106, parametros, serverManttoIp);
		fechaActivacion = DateTime.MinValue;
		return false;
	}

	public static bool NotificaCambioSistemaActivo(string listenIp, GestorBaseDatos gestorBDCD40, string id_sistema, string scvActivo)
	{
		//AccesoABaseDeDatos a = new AccesoABaseDeDatos();
		Sistema sis = new Sistema();
		sis.IdSistema = id_sistema;
		List<Tablas> sSistema = gestorBDCD40.ListSelectSQL(sis, null);
		if (sSistema.Count > 0)
		{
			ComunicacionMulticast(listenIp, ((Sistema)sSistema[0]).GrupoMulticastConfiguracion, ((Sistema)sSistema[0]).PuertoMulticastConfiguracion, System.Text.ASCIIEncoding.ASCII.GetBytes(scvActivo));

			return true;
		}

		return false;
	}

	public static void AsignaDestinoARecurso(ParametrosRecursoGeneral t, MySql.Data.MySqlClient.MySqlTransaction tran)
	{
		try
		{
			CD40.BD.Utilidades u = new CD40.BD.Utilidades(tran.Connection);

			u.UpdateDestinoSQL(t, tran);
		}
		catch (MySql.Data.MySqlClient.MySqlException)
		{
		}
	}

	public static void LiberaDestinoDeRecurso(ParametrosRecursoGeneral t, MySql.Data.MySqlClient.MySqlTransaction tran)
	{
		try
		{
			CD40.BD.Utilidades u = new CD40.BD.Utilidades(tran.Connection);

			//MySql.Data.MySqlClient.MySqlTransaction trans = GestorBDCD40.StartTransaction(true);
			u.LiberaDestinoSQL(t, tran);
			//GestorBDCD40.Commit(trans);
		}
		catch (MySql.Data.MySqlClient.MySqlException)
		{
		}
	}

	public static string GetTablasModificadas(GestorBaseDatos gestorBDCD40)
	{
		System.Text.StringBuilder listaTablas = new System.Text.StringBuilder();

		string consulta = "SELECT * FROM TablasModificadas";

		DataSet dsTablas = gestorBDCD40.GetDataSet(consulta, null);
        if (dsTablas.Tables.Count == 0)
            return null;

		foreach (DataRow dr in dsTablas.Tables[0].Rows)
		{
			listaTablas.Append((string)dr[0] + " ");
		}
		if (listaTablas.Length > 0)
			listaTablas.Remove(listaTablas.Length - 1, 1);

		if (listaTablas.Length > 0)
			return "\"" + listaTablas.ToString() + "\"";

		return null;
	}

	public static bool FindTableInTablasModificadas(GestorBaseDatos gestorBDCD40, string table)
	{
		string consulta = "SELECT COUNT(*) FROM TablasModificadas WHERE UPPER(IdTabla)='" + table + "'";
		return (long)(gestorBDCD40.ExecuteScalar(consulta)) != 0;
	}

	public static void ErrorLog(string sPathName, string sErrMsg)
	{
		try
		{
			System.IO.StreamWriter sw = new System.IO.StreamWriter(HttpContext.Current.Server.MapPath("~/") + sPathName, true);

            System.Text.StringBuilder sLogFormat = new System.Text.StringBuilder();
                
            sLogFormat.AppendFormat("[{0} {1}] ",DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),sErrMsg) ;

			sw.WriteLine(sLogFormat.ToString());
			sw.Flush();
			sw.Close();

            sLogFormat.Clear();
		}
		catch (System.IO.IOException)
		{
		}
	}

	//public static void OnTimeoutTransaccionCumplido()
	//{
	//    Session["Transaccion"] = false;
	//}
}
