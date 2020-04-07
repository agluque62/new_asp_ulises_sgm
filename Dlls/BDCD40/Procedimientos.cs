using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using CD40.BD.Entidades;
using CD40.BD;
using System.Xml.Serialization;

namespace CD40.BD
{
	[XmlRoot]
	public struct GetVersion
	{
		public string IdSistema;

		[XmlElement]
		public string Version;
	}

    public class Procedimientos
    {
        public Procedimientos()
        {
            //if (WebConfiguration == null)
            //    WebConfiguration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");

            //if (CadenaConexion == null && WebConfiguration.ConnectionStrings.ConnectionStrings.Count > 0)
            //{
            //    CadenaConexion = WebConfiguration.ConnectionStrings.ConnectionStrings["ConexionBaseDatosCD40"].ToString();
            //}
        }

        public Procedimientos(string cadenaConexion)
        {
            //dataResultado = new DataSet();
            //CadenaConexion = cadenaConexion;
        }

        //(Description = "Pasándole el identificador de sistema, devuelve la versión de la configuración activa registrada en la base de datos")
        public static string VersionSectorizacion(MySqlConnection mySqlConnectionToCd40, string id_sistema)
        {
            if (mySqlConnectionToCd40 != null)
            {
                System.Data.IDataParameter[] parametros;
                DataSet dataResultado = new DataSet();

                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);

                MySqlCommand myCommand = new MySqlCommand("VersionConfiguracion", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("versionCfg", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Output;

                p1.Value = id_sistema;

                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        parametros = myDataAdapter.GetFillParameters();
                        myCommand.Connection.Close();
                        if (parametros[1].Value != null)
                            return parametros[1].Value.ToString();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        parametros = myDataAdapter.GetFillParameters();
                        myCommand.Connection.Close();

                        if (parametros[1].Value != null)
                            return parametros[1].Value.ToString();
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                }

            }
            return string.Empty;
        }

        public static string[] UsuariosImplicadosEnRecurso(MySqlConnection mySqlConnectionToCd40, string id_sistema, string id_destino)
        {
            int i = 0;
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);

                MySqlCommand myCommand = new MySqlCommand("UsuariosImplicadosEnRecurso", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_destino", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;

                p1.Value = id_sistema;
                p2.Value = id_destino;

                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);

                        string[] listaUsuarios = new string[dataResultado.Tables[0].Rows.Count];

                        foreach (System.Data.DataRow dr in dataResultado.Tables[0].Rows)
                            listaUsuarios[i++] = (string)dr[0];

                        myCommand.Connection.Close();
                        return listaUsuarios;
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);

                        string[] listaUsuarios = new string[dataResultado.Tables[0].Rows.Count];

                        foreach (System.Data.DataRow dr in dataResultado.Tables[0].Rows)
                            listaUsuarios[i++] = (string)dr[0];

                        myCommand.Connection.Close();
                        return listaUsuarios;
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                    return null;
                }
            }
            return null;
        }

        public static List<string> ListaUsuariosImplicadosEnRecurso(MySqlConnection mySqlConnectionToCd40, string id_sistema, string id_destino, int? id_prefijo)
        {
            if (mySqlConnectionToCd40 != null && id_destino != null)
            {
                DataSet dataResultado = new DataSet();

                string strIdSectorSimple = string.Empty;
                string strNombreSector = string.Empty;
                string strNombreSectorAnterior = string.Empty;
                bool bPrimerEltoInsertado = false;

                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);

                //Se invoca al procedimiento UsuariosImplicadosEnRecursos, que devuelve el sector o los sectores en los que el recurso está configurado.
                //Si es un recurso LCEN, sólo se puede configurar en un único sector dentro del sistema.
                //Si el recurso no es LCEN, puede estar configurado en varios sectores. En este caso, si está configurado en una agrupación y el recurso está presente 
                //en varios sectores se debe devolver sólo el sector dominante. Si sólo está configurado en un sector, el sector que esté configurado.

                MySqlCommand myCommand = new MySqlCommand("UsuariosImplicadosEnRecurso", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_destino", MySqlDbType.Text);
                MySqlParameter p3 = new MySqlParameter("id_Prefijo", MySqlDbType.Int32);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;

                p1.Value = id_sistema;
                p2.Value = id_destino;
                p3.Value = id_prefijo;

                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        if (myCommand.Connection.State != ConnectionState.Open)
                        {
                            CD40.BD.GestorBaseDatos.logFile.Error("Error en Procedimientos.ListaUsuariosImplicadosEnRecurso: no se ha podido establecer la conexión con la BD");
                            return null;
                        }
                    }

                    MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                    dataResultado.Clear();
                    myDataAdapter.Fill(dataResultado);

                    List<string> listaUsuarios = new List<string>();

                    if (dataResultado.Tables.Count > 0)
                    {
                        foreach (System.Data.DataRow dr in dataResultado.Tables[0].Rows)
                        {
                            //Se leen los datos del registro actual
                            strIdSectorSimple = string.Empty;
                            strNombreSector = string.Empty;


                            if (dr["IDSECTOR"] != System.DBNull.Value)
                            {
                                strIdSectorSimple = (string)dr["IDSECTOR"];

                                if (dr["SECTORAGRUPADO"] != System.DBNull.Value)
                                {
                                    strNombreSector = (string)dr["SECTORAGRUPADO"];
                                }

                                if (string.Compare(strNombreSectorAnterior, strNombreSector) != 0)
                                {
                                    //Si se ha cambiado de sector, se resetea la variable 
                                    bPrimerEltoInsertado = false;
                                }

                                //Si el nombre del sector simple y el nombre del sector son distintos el destino está asociado 
                                // a uno o varios sectores dentro de la agrupación
                                //Si son iguales, el destino está asociado a un único terminal de operador

                                if (string.Compare(strIdSectorSimple, strNombreSector) == 0)
                                {
                                    listaUsuarios.Add(strIdSectorSimple);
                                    strNombreSectorAnterior = strNombreSector;
                                }
                                else
                                {
                                    //Sólo insertamos el primero de la lista, porque los datos vienen ordenados por el dominante
                                    if (!bPrimerEltoInsertado)
                                    {
                                        listaUsuarios.Add(strIdSectorSimple);
                                        bPrimerEltoInsertado = true;
                                        strNombreSectorAnterior = strNombreSector;
                                    }
                                }
                            }

                        }
                    }

                    myCommand.Connection.Close();

                    return listaUsuarios;
                }
                catch (MySqlException)
                {
                    if (myCommand.Connection.State == ConnectionState.Open)
                        myCommand.Connection.Close();

                    return null;
                }
            }
            return null;
        }

        public static string[] LoginTop(MySqlConnection mySqlConnectionToCd40, string id_sistema, string id_hw)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();

                System.Data.IDataParameter[] parametros;
                string[] listaParametros = new string[2];

                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);

                MySqlCommand myCommand = new MySqlCommand("LoginTop", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_Sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_Hw", MySqlDbType.Text);
                MySqlParameter p3 = new MySqlParameter("id_Usuario", MySqlDbType.Text);
                MySqlParameter p4 = new MySqlParameter("modo_arranque", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Output;
                p4.Direction = System.Data.ParameterDirection.Output;

                p1.Value = id_sistema;
                p2.Value = id_hw;

                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);
                myCommand.Parameters.Add(p4);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        parametros = myDataAdapter.GetFillParameters();
                        listaParametros[0] = parametros[2].Value.ToString();
                        listaParametros[1] = parametros[3].Value.ToString();

                        myCommand.Connection.Close();

                        return listaParametros;
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        parametros = myDataAdapter.GetFillParameters();
                        listaParametros[0] = parametros[2].Value.ToString();
                        listaParametros[1] = parametros[3].Value.ToString();

                        myCommand.Connection.Close();
                        return listaParametros;
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                    return null;
                }
            }
            return null;
        }

        public static DataSet NumerosAbonadoInternos(MySqlConnection mySqlConnectionToCd40, string id_sistema, string id_usuario)
        {
            if (mySqlConnectionToCd40 != null)
            {
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                DataSet dataResultado = new DataSet();

                MySqlCommand myCommand = new MySqlCommand("NumerosAbonadoInternos", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_Sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_Usuario", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;

                p1.Value = id_sistema;
                p2.Value = id_usuario;

                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);

                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        public static DataSet NumerosAbonadoExternos(MySqlConnection mySqlConnectionToCd40, string id_sistema, string id_usuario)
        {
            if (mySqlConnectionToCd40 != null)
            {
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                DataSet dataResultado = new DataSet();

                MySqlCommand myCommand = new MySqlCommand("NumerosAbonadoExternos", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_Sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_Usuario", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;

                p1.Value = id_sistema;
                p2.Value = id_usuario;

                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);

                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        public static DataSet PermisosDeRedes(MySqlConnection mySqlConnectionToCd40, string id_sistema, string id_sector)
        {
            if (mySqlConnectionToCd40 != null)
            {
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                DataSet dataResultado = new DataSet();

                MySqlCommand myCommand = new MySqlCommand("PermisosRedes", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("idSistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("idUsuario", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;

                p1.Value = id_sistema;
                p2.Value = id_sector;

                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);

                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        //(Description = "Retorna una lista de Recursos libres que pueden ser asignados")
        public static DataSet RecursosSinAsignar(MySqlConnection mySqlConnectionToCd40, string id, string id_sistema)
        {
            if (mySqlConnectionToCd40 != null)
            {
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                DataSet dataResultado = new DataSet();

                MySqlCommand myCommand = new MySqlCommand("RecursosSinAsignar", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_Sistema", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;

                p1.Value = id;
                p2.Value = id_sistema;

                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        //(Description = "Retorna una lista de Troncales sin asignar a Rutas")
        public static DataSet TroncalesSinAsignarARutas(MySqlConnection mySqlConnectionToCd40, string id_sistema, string idCentral, string idRuta)
        {
            if (mySqlConnectionToCd40 != null)
            {
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                DataSet dataResultado = new DataSet();

                MySqlCommand myCommand = new MySqlCommand("TroncalesSinAsignarARutas", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_Sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_Central", MySqlDbType.Text);
                MySqlParameter p3 = new MySqlParameter("id_Ruta", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;

                p1.Value = id_sistema;
                p2.Value = idCentral;
                p3.Value = idRuta;

                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        //(Description = "Retorna una lista de Troncales sin asignar a Rutas")
        public static DataSet PrefijosSinAsignarARedes(MySqlConnection mySqlConnectionToCd40, string id_sistema)
        {
            if (mySqlConnectionToCd40 != null)
            {
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                DataSet dataResultado = new DataSet();

                MySqlCommand myCommand = new MySqlCommand("PrefijosSinAsignarARedes", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_Sistema", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                myCommand.Parameters.Add(p1);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        //(Description = "Retorna una tabla con los identificadores hardware de TIFX y TOP junto con sus direcciones IP")
        public static DataSet PlanDireccionamientoIP(MySqlConnection mySqlConnectionToCd40, string id_sistema)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                MySqlCommand myCommand = new MySqlCommand("PlanDireccionamientoIP", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_Sistema", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                myCommand.Parameters.Add(p1);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        //(Description = "Pasándole el identificador del sistema y de la central, retorna una lista" 
        //             " con los identificadores de ruta, el tipo de la misma y los troncales asignados.")
        public static DataSet ListaRutas(MySqlConnection mySqlConnectionToCd40, string id_sistema, string id_central)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                MySqlCommand myCommand = new MySqlCommand("ListaRutas", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_Sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_Central", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                p2.Value = id_central;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        //(Description = "Pasándole el identificador del sistema, retorna una lista de sectores asignados a tops " 
        //                        "en la sectorización activa.")
        public static DataSet AsignacionUsuariosATops(MySqlConnection mySqlConnectionToCd40, string id_sistema)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                MySqlCommand myCommand = new MySqlCommand("AsignacionUsuariosATops", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_Sistema", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                myCommand.Parameters.Add(p1);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        //(Description = "Pasándole el identificador del sistema, retorna una lista de números de abonado " 
        //               "con sus respectivos prefijos a los que un usuario atiende en la sectorizacion activa")
        public static DataSet PlanDireccionamientoSIP(MySqlConnection mySqlConnectionToCd40, string id_sistema)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                MySqlCommand myCommand = new MySqlCommand("PlanDireccionamientoSIP", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_Sistema", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                myCommand.Parameters.Add(p1);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        //(Description = "Pasándole el identificador del sistema, retorna una lista de destinos internos sin asignar a enlaces")
        public static DataSet DestinosInternosSinAsignarAEnlaces(MySqlConnection mySqlConnectionToCd40, string id_sistema)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                MySqlCommand myCommand = new MySqlCommand("DestinosInternosSinAsignarAEnlaces", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                myCommand.Parameters.Add(p1);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        //(Description = "Pasándole el identificador del sistema, retorna una lista de destinos instantáneos sin asignar a enlaces")
        public static DataSet DestinosInstantaneosSinAsignarAEnlaces(MySqlConnection mySqlConnectionToCd40, string id_sistema)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                MySqlCommand myCommand = new MySqlCommand("DestinosInstantaneosSinAsignarAEnlaces", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_Sistema", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                myCommand.Parameters.Add(p1);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        //(Description = "Pasándole el identificador del sistema, retorna una lista de recursos radio sin asignar a enlaces.")
        // 18/01/2017
        // Se amplia la consulta con la posibilidad de consultar por emplazamiento
        public static DataSet RecursosRadioSinAsignarAEnlaces(MySqlConnection mySqlConnectionToCd40, string id_sistema, string site = null)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                MySqlCommand myCommand = new MySqlCommand("RecursosRadioSinAsignarAEnlaces", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_Sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_Site", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p1.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                p2.Value = site;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException /*e*/)
                {
                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        //(Description = "Pasándole el identificador del sistema, retorna una lista de recursos telefonía sin asignar a enlaces.")
        public static DataSet RecursosTFSinAsignarAEnlaces(MySqlConnection mySqlConnectionToCd40, string id_sistema)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                MySqlCommand myCommand = new MySqlCommand("RecursosTFSinAsignarAEnlaces", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_Sistema", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                myCommand.Parameters.Add(p1);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        //(Description = "Pasándole el identificador del sistema, retorna una lista de recursos radio sin asignar a enlaces.")
        public static DataSet RecursosLCENSinAsignarAEnlaces(MySqlConnection mySqlConnectionToCd40, string id_sistema)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                MySqlCommand myCommand = new MySqlCommand("RecursosLCENSinAsignarAEnlaces", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_Sistema", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                myCommand.Parameters.Add(p1);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        //(Description = "Pasándole el identificador del sistema, el identificador del sector y la página del enlace a configurar, retorna una lista de destinos radio"
        //               " sin asignar a ningún enlace del sector y página.")
        public static List<string> DestinosRadioSinAsignarALaPaginaDelSector(MySqlConnection mySqlConnectionToCd40, string id_sistema, string id_usuario, uint pagina, uint num_frec_pag)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                MySqlCommand myCommand = new MySqlCommand("DestinosRadioSinAsignarALaPaginaDelSector", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_usuario", MySqlDbType.Text);
                MySqlParameter p3 = new MySqlParameter("pagina", MySqlDbType.Int32);
                MySqlParameter p4 = new MySqlParameter("frecPorPagina", MySqlDbType.Int32);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p4.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                p2.Value = id_usuario;
                p3.Value = pagina;
                p4.Value = num_frec_pag;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);
                myCommand.Parameters.Add(p4);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        List<string> listaDestinos = new List<string>();
                        foreach (DataRow dr in dataResultado.Tables[0].Rows)
                            listaDestinos.Add((string)dr["IdDestino"]);
                        myCommand.Connection.Close();
                        return listaDestinos;
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        List<string> listaDestinos = new List<string>();
                        foreach (DataRow dr in dataResultado.Tables[0].Rows)
                            listaDestinos.Add((string)dr["IdDestino"]);

                        myCommand.Connection.Close();
                        return listaDestinos;
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                    return null;
                }
            }
            return null;
        }

        //(Description = "Pasándole el identificador del sistema, el identificador del sector y la página del enlace a configurar, retorna una lista de destinos radio" 
        //               " asignados al sector y página correspondiente.")
        public static DataSet DestinosRadioAsignadosASector(MySqlConnection mySqlConnectionToCd40, string id_sistema, string id_usuario, uint pagina, uint num_frec_pag)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                MySqlCommand myCommand = new MySqlCommand("DestinosRadioAsignadosASector", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_usuario", MySqlDbType.Text);
                MySqlParameter p3 = new MySqlParameter("pagina", MySqlDbType.Int32);
                MySqlParameter p4 = new MySqlParameter("frecPorPagina", MySqlDbType.Int32);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p4.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                p2.Value = id_usuario;
                p3.Value = pagina;
                p4.Value = num_frec_pag;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);
                myCommand.Parameters.Add(p4);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        //(Description = "Pasándole el identificador del sistema y el identificador del sector, retorna una lista de destinos de línea caliente" 
        //               " sin asignar a ningún enlace del sector.")
        public static List<Tablas> DestinosLineaCalienteSinAsignarAlSector(MySqlConnection mySqlConnectionToCd40, string id_sistema, string id_usuario, string id_nucleo)
        {
            if (mySqlConnectionToCd40 != null)
            {
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                DataSet dataResultado = new DataSet();

                MySqlCommand myCommand = new MySqlCommand("DestinosLineaCalienteSinAsignarAlSector", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_usuario", MySqlDbType.Text);
                MySqlParameter p3 = new MySqlParameter("id_nucleo", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                p2.Value = id_usuario;
                p3.Value = id_nucleo;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        List<Tablas> listaDestinos = new List<Tablas>();
                        foreach (DataRow dr in dataResultado.Tables[0].Rows)
                        {
                            DestinosTelefonia r = new DestinosTelefonia();
                            if (dr["IdSistema"] != System.DBNull.Value)
                                r.IdSistema = (string)dr["IdSistema"];
                            if (dr["IdDestino"] != System.DBNull.Value)
                                r.IdDestino = (string)dr["IdDestino"];
                            if (dr["TipoDestino"] != System.DBNull.Value)
                                r.TipoDestino = (uint)dr["TipoDestino"];
                            if (dr["IdPrefijo"] != System.DBNull.Value)
                                r.IdPrefijo = (uint)dr["IdPrefijo"];
                            if (dr["IdGrupo"] != System.DBNull.Value)
                                r.IdGrupo = (string)dr["IdGrupo"];

                            listaDestinos.Add((Tablas)r);
                        }
                        myCommand.Connection.Close();
                        return listaDestinos;
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        List<Tablas> listaDestinos = new List<Tablas>();
                        foreach (DataRow dr in dataResultado.Tables[0].Rows)
                        {
                            DestinosTelefonia r = new DestinosTelefonia();
                            if (dr["IdSistema"] != System.DBNull.Value)
                                r.IdSistema = (string)dr["IdSistema"];
                            if (dr["IdDestino"] != System.DBNull.Value)
                                r.IdDestino = (string)dr["IdDestino"];
                            if (dr["TipoDestino"] != System.DBNull.Value)
                                r.TipoDestino = (uint)dr["TipoDestino"];
                            if (dr["IdPrefijo"] != System.DBNull.Value)
                                r.IdPrefijo = (uint)dr["IdPrefijo"];
                            if (dr["IdGrupo"] != System.DBNull.Value)
                                r.IdGrupo = (string)dr["IdGrupo"];

                            listaDestinos.Add((Tablas)r);
                        }

                        myCommand.Connection.Close();
                        return listaDestinos;
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                    return null;
                }
            }
            return null;
        }

        //(Description = "Pasándole el identificador del sistema , retorna una lista de destinos de línea caliente" 
        //               " sin asignar a ningún enlace del sector.")
        public static List<Tablas> DestinosLineaCalienteSinAsignar(MySqlConnection mySqlConnectionToCd40, string id_sistema)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();

                MySqlCommand myCommand = new MySqlCommand("DestinosLCEN_SinAsignar", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);

                p1.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                myCommand.Parameters.Add(p1);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        List<Tablas> listaDestinos = new List<Tablas>();
                        foreach (DataRow dr in dataResultado.Tables[0].Rows)
                        {
                            DestinosTelefonia r = new DestinosTelefonia();
                            if (dr["IdSistema"] != System.DBNull.Value)
                                r.IdSistema = (string)dr["IdSistema"];
                            if (dr["IdDestino"] != System.DBNull.Value)
                                r.IdDestino = (string)dr["IdDestino"];
                            if (dr["TipoDestino"] != System.DBNull.Value)
                                r.TipoDestino = (uint)dr["TipoDestino"];
                            if (dr["IdPrefijo"] != System.DBNull.Value)
                                r.IdPrefijo = (uint)dr["IdPrefijo"];
                            if (dr["IdGrupo"] != System.DBNull.Value)
                                r.IdGrupo = (string)dr["IdGrupo"];

                            listaDestinos.Add((Tablas)r);
                        }
                        myCommand.Connection.Close();
                        return listaDestinos;
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        List<Tablas> listaDestinos = new List<Tablas>();
                        foreach (DataRow dr in dataResultado.Tables[0].Rows)
                        {
                            DestinosTelefonia r = new DestinosTelefonia();
                            if (dr["IdSistema"] != System.DBNull.Value)
                                r.IdSistema = (string)dr["IdSistema"];
                            if (dr["IdDestino"] != System.DBNull.Value)
                                r.IdDestino = (string)dr["IdDestino"];
                            if (dr["TipoDestino"] != System.DBNull.Value)
                                r.TipoDestino = (uint)dr["TipoDestino"];
                            if (dr["IdPrefijo"] != System.DBNull.Value)
                                r.IdPrefijo = (uint)dr["IdPrefijo"];
                            if (dr["IdGrupo"] != System.DBNull.Value)
                                r.IdGrupo = (string)dr["IdGrupo"];

                            listaDestinos.Add((Tablas)r);
                        }

                        myCommand.Connection.Close();
                        return listaDestinos;
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                    return null;
                }
            }
            return null;
        }

        //(Description = "Pasándole el identificador del sistema y el identificador del sector, retorna una lista de destinos de telefonía" 
        //               " sin asignar a ningún enlace del sector.")
        public static List<Tablas> DestinosTelefoniaSinAsignarAlSector(MySqlConnection mySqlConnectionToCd40, string id_sistema, string id_usuario, string id_nucleo)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                MySqlCommand myCommand = new MySqlCommand("DestinosTelefoniaSinAsignarAlSector", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_usuario", MySqlDbType.Text);
                MySqlParameter p3 = new MySqlParameter("id_nucleo", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                p2.Value = id_usuario;
                p3.Value = id_nucleo;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        List<Tablas> listaDestinos = new List<Tablas>();
                        foreach (System.Data.DataRow dr in dataResultado.Tables[0].Rows)
                        {
                            DestinosTelefonia r = new DestinosTelefonia();
                            if (dr["IdSistema"] != System.DBNull.Value)
                                r.IdSistema = (string)dr["IdSistema"];
                            if (dr["IdDestino"] != System.DBNull.Value)
                                r.IdDestino = (string)dr["IdDestino"];
                            if (dr["TipoDestino"] != System.DBNull.Value)
                                r.TipoDestino = (uint)dr["TipoDestino"];
                            if (dr["IdPrefijo"] != System.DBNull.Value)
                                r.IdPrefijo = (uint)dr["IdPrefijo"];
                            if (dr["IdGrupo"] != System.DBNull.Value)
                                r.IdGrupo = (string)dr["IdGrupo"];

                            listaDestinos.Add((Tablas)r);
                        }
                        myCommand.Connection.Close();
                        return listaDestinos;
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        List<Tablas> listaDestinos = new List<Tablas>();
                        foreach (System.Data.DataRow dr in dataResultado.Tables[0].Rows)
                        {
                            DestinosTelefonia r = new DestinosTelefonia();
                            if (dr["IdSistema"] != System.DBNull.Value)
                                r.IdSistema = (string)dr["IdSistema"];
                            if (dr["IdDestino"] != System.DBNull.Value)
                                r.IdDestino = (string)dr["IdDestino"];
                            if (dr["TipoDestino"] != System.DBNull.Value)
                                r.TipoDestino = (uint)dr["TipoDestino"];
                            if (dr["IdPrefijo"] != System.DBNull.Value)
                                r.IdPrefijo = (uint)dr["IdPrefijo"];
                            if (dr["IdGrupo"] != System.DBNull.Value)
                                r.IdGrupo = (string)dr["IdGrupo"];

                            listaDestinos.Add((Tablas)r);
                        }

                        myCommand.Connection.Close();
                        return listaDestinos;
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                    return null;
                }
            }
            return null;
        }

        //(Description = "Pasándole el identificador del sistema y el identificador del sector, retorna una lista de destinos de telefonía " 
        //               "o línea caliente, internos o externos asignados al sector.")
        public static DataSet DestinosTelefoniaAsignadosAlSector(MySqlConnection mySqlConnectionToCd40, string id_sistema, string id_usuario, bool telefonia, bool internos)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                MySqlCommand myCommand;
                if (internos)
                    myCommand = new MySqlCommand("DestinosInternosAsignadosASector", mySqlConnectionToCd40);
                else
                    myCommand = new MySqlCommand("DestinosExternosAsignadosASector", mySqlConnectionToCd40);

                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_sector", MySqlDbType.Text);
                MySqlParameter p3 = new MySqlParameter("telefonia", MySqlDbType.Int16);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                p2.Value = id_usuario;
                p3.Value = telefonia;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);

                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        //(Description = "Pasándole el identificador del sistema y el identificador del sector y el identificador de la sectorizacion," 
        //               " retorna una lista de destinos de telefonía internos o externos asignados al sector.")
        public static DataSet DestinosTelefoniaSectorizados(MySqlConnection mySqlConnectionToCd40, string id_sistema, string id_usuario, string id_sectorizacion, bool internos)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                MySqlCommand myCommand;
                myCommand = new MySqlCommand("DestinosTelefoniaSectorizados", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_sector", MySqlDbType.Text);
                MySqlParameter p3 = new MySqlParameter("id_sectorizacion", MySqlDbType.Text);
                MySqlParameter p4 = new MySqlParameter("internos", MySqlDbType.Int16);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p4.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                p2.Value = id_usuario;
                p3.Value = id_sectorizacion;
                p4.Value = internos;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);
                myCommand.Parameters.Add(p4);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);

                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                }

                return dataResultado;
            }

            return null;
        }

        //(Description = "Pasándole el identificador del sistema y el identificador del sector y el identificador de la sectorizacion," 
        //               " retorna una lista de destinos de radio asignados al sector.")
        public static DataSet DestinosRadioSectorizados(MySqlConnection mySqlConnectionToCd40, string id_sistema, string id_usuario, string id_sectorizacion)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                MySqlCommand myCommand;
                myCommand = new MySqlCommand("DestinosRadioSectorizados", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_sector", MySqlDbType.Text);
                MySqlParameter p3 = new MySqlParameter("id_sectorizacion", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                p2.Value = id_usuario;
                p3.Value = id_sectorizacion;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        public static string[] SlotsLibresEnTifX(MySqlConnection mySqlConnectionToCd40, string id_sistema, int cuantos)
        {
            int i = 0;
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                MySqlCommand myCommand = new MySqlCommand("SlotsLibresEnTifX", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_Sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("cuantos", MySqlDbType.Int32);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                p2.Value = cuantos;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        string[] listaTifx = new string[dataResultado.Tables[0].Rows.Count];
                        foreach (System.Data.DataRow dr in dataResultado.Tables[0].Rows)
                            listaTifx[i++] = (string)dr[0];

                        myCommand.Connection.Close();
                        return listaTifx;
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        string[] listaTifx = new string[dataResultado.Tables[0].Rows.Count];
                        foreach (System.Data.DataRow dr in dataResultado.Tables[0].Rows)
                            listaTifx[i++] = (string)dr[0];

                        myCommand.Connection.Close();
                        return listaTifx;
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                    return null;
                }
            }
            return null;
        }

        public static DataSet SectoresSinAsignarASectorizacion(MySqlConnection mySqlConnectionToCd40, string id_sistema, string id_nucleo, string id_sectorizacion)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                MySqlCommand myCommand;
                myCommand = new MySqlCommand("SectoresSinAsignarASectorizacion", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_sectorizacion", MySqlDbType.Text);
                MySqlParameter p3 = new MySqlParameter("id_nucleo", MySqlDbType.Text);

                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                p2.Value = id_sectorizacion;
                p3.Value = id_nucleo;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();
                    }

                    if (myCommand.Connection.State == ConnectionState.Open)
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }

                }
                catch (MySqlException)
                {
                    if (myCommand.Connection.State == ConnectionState.Open)
                        myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        //(Description = "Comprobar si la sectorización pasada como parámetro contiene todos los sectores reales.")
        public static bool TodosLosSectoresReales(MySqlConnection mySqlConnectionToCd40, string id_sistema, string id_sectorizacion)
        {
            bool bOk = false;

            if (null != mySqlConnectionToCd40)
            {
                DataSet dataResultado = new DataSet();
                System.Data.IDataParameter[] parametros;

                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);

                MySqlCommand myCommand = new MySqlCommand("SectoresReales", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_sectorizacion", MySqlDbType.Text);
                MySqlParameter p3 = new MySqlParameter("cuantos", MySqlDbType.Int32);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Output;
                p1.Value = id_sistema;
                p2.Value = id_sectorizacion;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();
                    }

                    MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                    dataResultado.Clear();
                    myDataAdapter.Fill(dataResultado);
                    parametros = myDataAdapter.GetFillParameters();
                    myCommand.Connection.Close();
                    if (Int32.Parse(parametros[2].Value.ToString()) == 0)
                        bOk = true;

                }
                catch (MySqlException ex)
                {
                    if (myCommand.Connection.State == ConnectionState.Open)
                    {
                        myCommand.Connection.Close();
                    }

                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.TodosLosSectoresReales:idSistema={0},strIdentificador={1}. Error de MySQL:{2}", id_sistema, id_sectorizacion, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();
                    bOk = false;
                }
            }
            else
            {
                StringBuilder strMsg = new StringBuilder();
                strMsg.AppendFormat("Error en Procedimientos.TodosLosSectoresReales:idSistema={0},strIdentificador={1}. mySqlConnectionToCd40=null", id_sistema, id_sectorizacion);
                CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                strMsg.Clear();
            }

            return bOk;
        }

        public static string SectoresManttoEnActiva(MySqlConnection mySqlConnectionToCd40, string id_sistema)
        {
            if (null != mySqlConnectionToCd40)
            {
                DataSet dataResultado = new DataSet();
                StringBuilder listaSectores = new StringBuilder();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);

                MySqlCommand myCommand = new MySqlCommand("SectoresManttoEnActiva", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                myCommand.Parameters.Add(p1);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);

                        foreach (System.Data.DataRow dr in dataResultado.Tables[0].Rows)
                        {
                            listaSectores.Append(string.Format("{0},{1};", (uint)dr["NumSacta"], (uint)dr["PosicionSacta"]));
                        }

                        myCommand.Connection.Close();
                        return listaSectores.ToString();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);

                        foreach (System.Data.DataRow dr in dataResultado.Tables[0].Rows)
                        {
                            listaSectores.Append(string.Format("{0},{1};", (uint)dr["NumSacta"], (uint)dr["PosicionSacta"]));
                        }

                        myCommand.Connection.Close();
                        return listaSectores.ToString();
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                    return string.Empty;
                }
            }

            return string.Empty;
        }

        //(Description = "Comprobar si la sectorización pasada como parámetro contiene todos los sectores reales.")
        public static int IdManttoLibre(MySqlConnection mySqlConnectionToCd40, string id_sistema)
        {
            if (null != mySqlConnectionToCd40)
            {
                DataSet dataResultado = new DataSet();
                System.Data.IDataParameter[] parametros;

                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);

                MySqlCommand myCommand = new MySqlCommand("IdManttoLibre", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_Sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_Sacta", MySqlDbType.Int32);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Output;
                p1.Value = id_sistema;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        parametros = myDataAdapter.GetFillParameters();

                        myCommand.Connection.Close();

                        return Int32.Parse(parametros[1].Value.ToString());
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        parametros = myDataAdapter.GetFillParameters();

                        myCommand.Connection.Close();
                        return Int32.Parse(parametros[1].Value.ToString());
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                    return 0;
                }
            }
            return 0;
        }

        public static bool DemasiadasTeclasConPrioridadUno(MySqlConnection mySqlConnectionToCd40, string idSistema, string idNucleo, int maximo)
        {
            if (null != mySqlConnectionToCd40)
            {
                DataSet dataResultado = new DataSet();
                System.Data.IDataParameter[] parametros;

                MySqlCommand myCommand = new MySqlCommand("CuantasTeclasConPrioridadUno", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_nucleo", MySqlDbType.Text);
                MySqlParameter p3 = new MySqlParameter("cuantas", MySqlDbType.Int32);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Output;
                p1.Value = idSistema;
                p2.Value = idNucleo;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        if (myCommand.Connection.State != ConnectionState.Open)
                        {
                            CD40.BD.GestorBaseDatos.logFile.Error("Error en Procedimientos.DemasiadasTeclasConPrioridadUno: no se ha podido establecer la conexión con la BD");
                            return true;
                        }
                    }

                    MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                    dataResultado.Clear();
                    myDataAdapter.Fill(dataResultado);
                    parametros = myDataAdapter.GetFillParameters();
                    myCommand.Connection.Close();

                    return Int32.Parse(parametros[2].Value.ToString()) >= maximo;
                }
                catch (MySqlException exp)
                {
                    if (myCommand.Connection.State == ConnectionState.Open)
                    {
                        myCommand.Connection.Close();
                    }

                    CD40.BD.GestorBaseDatos.logFile.Error("Error en Procedimientos.DemasiadasTeclasConPrioridadUno: ", exp);

                    return true;
                }
            }

            return true;
        }

        public static DataSet GetEventosRadio(MySqlConnection mySqlConnectionToCd40, string idSistema, DateTime desde, DateTime hasta)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                MySqlCommand myCommand;
                myCommand = new MySqlCommand("GetEventosRadio", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("desde", MySqlDbType.Date);
                MySqlParameter p3 = new MySqlParameter("hasta", MySqlDbType.Date);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p1.Value = idSistema;
                p2.Value = desde;
                p3.Value = hasta;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        public static DataSet GetEventosTelefonia(MySqlConnection mySqlConnectionToCd40, string idSistema, DateTime desde, DateTime hasta)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                MySqlCommand myCommand;
                myCommand = new MySqlCommand("GetEventosTelefonia", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("desde", MySqlDbType.Date);
                MySqlParameter p3 = new MySqlParameter("hasta", MySqlDbType.Date);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p1.Value = idSistema;
                p2.Value = desde;
                p3.Value = hasta;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        public static DataSet GetHistoricos(MySqlConnection mySqlConnectionToCd40, string idSistema, int numScv, string listaIncidencias, int tipo, string idHw, DateTime desde, DateTime hasta)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                MySqlCommand myCommand;
                myCommand = new MySqlCommand("GetHistoricos", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("num_scv", MySqlDbType.Int32);
                MySqlParameter p3 = new MySqlParameter("lista_incidencias", MySqlDbType.Text);
                MySqlParameter p4 = new MySqlParameter("tipo_hw", MySqlDbType.Int32);
                MySqlParameter p5 = new MySqlParameter("id_hw", MySqlDbType.Text);
                MySqlParameter p6 = new MySqlParameter("desde", MySqlDbType.DateTime);
                MySqlParameter p7 = new MySqlParameter("hasta", MySqlDbType.DateTime);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p4.Direction = System.Data.ParameterDirection.Input;
                p4.Direction = System.Data.ParameterDirection.Input;
                p6.Direction = System.Data.ParameterDirection.Input;
                p7.Direction = System.Data.ParameterDirection.Input;
                p1.Value = idSistema;
                p2.Value = numScv;
                p3.Value = listaIncidencias;
                p4.Value = tipo;
                p5.Value = idHw;
                p6.Value = desde;
                p7.Value = hasta;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);
                myCommand.Parameters.Add(p4);
                myCommand.Parameters.Add(p5);
                myCommand.Parameters.Add(p6);
                myCommand.Parameters.Add(p7);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);

                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        public static DataSet GetUnHistorico(MySqlConnection mySqlConnectionToCd40, string idSistema, string listaIncidencias, int tipo, string idHw, DateTime desde, DateTime hasta)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                MySqlCommand myCommand;
                myCommand = new MySqlCommand("GetUnHistorico", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("lista_incidencias", MySqlDbType.Text);
                MySqlParameter p3 = new MySqlParameter("tipo_hw", MySqlDbType.Int32);
                MySqlParameter p4 = new MySqlParameter("id_hw", MySqlDbType.Text);
                MySqlParameter p5 = new MySqlParameter("desde", MySqlDbType.Date);
                MySqlParameter p6 = new MySqlParameter("hasta", MySqlDbType.Date);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p4.Direction = System.Data.ParameterDirection.Input;
                p5.Direction = System.Data.ParameterDirection.Input;
                p6.Direction = System.Data.ParameterDirection.Input;
                p1.Value = idSistema;
                p2.Value = listaIncidencias;
                p3.Value = tipo;
                p4.Value = idHw;
                p5.Value = desde;
                p6.Value = hasta;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);
                myCommand.Parameters.Add(p4);
                myCommand.Parameters.Add(p5);
                myCommand.Parameters.Add(p6);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);

                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        //(Description = "Retorna los sectores que no están en una agrupación dada o todos si la agrupación es null")
        public static DataSet SectoresFueraDeAgrupacion(MySqlConnection mySqlConnectionToCd40, string id_sistema, string id_agrupacion)
        {
            if (null != mySqlConnectionToCd40)
            {
                //System.Data.IDataParameter[] parametros;

                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                DataSet dataResultado = new DataSet();

                MySqlCommand myCommand = new MySqlCommand("SectoresFueraDeAgrupacion", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_agrupacion", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                p2.Value = id_agrupacion;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);

                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        public static DataSet RedesUsuariosAbonados(MySqlConnection mySqlConnectionToCd40, string id_sistema, string id_nucleo, string id_sector)
        {
            if (null != mySqlConnectionToCd40)
            {
                //System.Data.IDataParameter[] parametros;

                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                DataSet dataResultado = new DataSet();

                MySqlCommand myCommand = new MySqlCommand("RedesUsuariosAbonados", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_Sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_Nucleo", MySqlDbType.Text);
                MySqlParameter p3 = new MySqlParameter("id_Sector", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                p2.Value = id_nucleo;
                p3.Value = id_sector;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);

                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                }

                return dataResultado;

                //if (AccesoABaseDeDatos.ConexionMySql == null)
                //{
                //    myCommand.Dispose();
                //    mySqlConnectionToCd40.Close();
                //    mySqlConnectionToCd40.Dispose();
                //}
            }
            return null;
        }

        public static DataSet RangosConIdRed(MySqlConnection mySqlConnectionToCd40, string id_sistema, string id_central)
        {
            if (null != mySqlConnectionToCd40)
            {
                //System.Data.IDataParameter[] parametros;

                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                DataSet dataResultado = new DataSet();

                MySqlCommand myCommand = new MySqlCommand("RangosConIdRed", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_central", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                p2.Value = id_central;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();
                    }

                    MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                    dataResultado.Clear();
                    myDataAdapter.Fill(dataResultado);

                    myCommand.Connection.Close();
                }
                catch (MySqlException)
                {
                    if (myCommand.Connection.State == ConnectionState.Open)
                        myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        public static void EliminaActiva(MySqlConnection mySqlConnectionToCd40, MySqlTransaction tran, string id_sistema)
        {
            if (null != mySqlConnectionToCd40)
            {
                //MySqlConnection mySqlConnectionToCd40 = new MySqlConnection(CadenaConexion.ToString());

                MySqlCommand myCommand = new MySqlCommand("EliminaActiva", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_Sistema", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                myCommand.Parameters.Add(p1);

                try
                {
                    if (tran != null)
                    {
                        myCommand.Transaction = tran;
                        myCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        myCommand.Connection.Open();
                        myCommand.ExecuteNonQuery();
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.EliminaActiva(id_sistema={0}) . Error:{1}", id_sistema, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();

                }
            }
        }

        public static void CreaSectoresActiva(MySqlConnection mySqlConnectionToCd40, MySqlTransaction tran, string id_sistema, string id_sectorizacion, string id_activa, bool sactaPresente)
        {
            if (null != mySqlConnectionToCd40)
            {
                string nomProcedimiento;

                if (!sactaPresente)
                    nomProcedimiento = "CreaSectoresActiva";
                else
                    nomProcedimiento = "CreaSectoresActivaConexionSacta";

                MySqlCommand myCommand = new MySqlCommand(nomProcedimiento, mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_sectorizacion", MySqlDbType.Text);
                MySqlParameter p3 = new MySqlParameter("id_activa", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                p2.Value = id_sectorizacion;
                p3.Value = id_activa;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);

                try
                {
                    if (tran != null)
                    {
                        myCommand.Transaction = tran;
                        myCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        myCommand.Connection.Open();
                        myCommand.ExecuteNonQuery();
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.CreaSectoresActiva - {5} (id_sistema={0},id_sectorizacion={1}, id_activa={2},sactaPresente={3}) . Error:{4}", id_sistema, id_sectorizacion, id_activa, sactaPresente, ex.Message.ToString(), nomProcedimiento);
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                }
            }
        }

        public static void CreaPosicionesActiva(MySqlConnection mySqlConnectionToCd40, MySqlTransaction tran, string id_sistema, string id_sectorizacion, string id_activa)
        {
            if (null != mySqlConnectionToCd40)
            {
                MySqlCommand myCommand = new MySqlCommand("CreaPosicionesActiva", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_sectorizacion", MySqlDbType.Text);
                MySqlParameter p3 = new MySqlParameter("id_activa", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                p2.Value = id_sectorizacion;
                p3.Value = id_activa;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);

                try
                {
                    if (tran != null)
                    {
                        myCommand.Transaction = tran;
                        myCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        myCommand.Connection.Open();
                        myCommand.ExecuteNonQuery();
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.CreaPosicionesActiva: id_sistema={0},id_sectorizacion={1}, id_activa={2}) . Error:{3}", id_sistema, id_sectorizacion, id_activa, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                }
            }
        }

        public static DataSet TeclasSector(MySqlConnection mySqlConnectionToCd40, string id_sistema, string id_sector)
        {
            if (null != mySqlConnectionToCd40)
            {
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                DataSet dataResultado = new DataSet();

                MySqlCommand myCommand = new MySqlCommand("TeclasSector", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_Sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_Sector", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                p2.Value = id_sector;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);

                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.TeclasSector: id_sistema={0},id_sector={1}. Error:{2}", id_sistema, id_sector, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        public static DataSet NivelesIntrusion(MySqlConnection mySqlConnectionToCd40, string id_sistema, string id_usuario)
        {
            if (null != mySqlConnectionToCd40)
            {
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                DataSet dataResultado = new DataSet();

                MySqlCommand myCommand = new MySqlCommand("NivelesIntrusion", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_usuario", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                p2.Value = id_usuario;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);

                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.NivelesIntrusion: id_sistema={0},id_usuario={1}. Error:{2}", id_sistema, id_usuario, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();
                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        public static string[] SectoresCompuestosPorSectorOriginal(MySqlConnection mySqlConnectionToCd40, MySqlTransaction tran, string id_sistema, string id_sector_original)
        {
            int i = 0;
            if (mySqlConnectionToCd40 != null)
            {
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                DataSet dataResultado = new DataSet();

                MySqlCommand myCommand;
                myCommand = new MySqlCommand("SectoresCompuestosPorSectorOriginal", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_sector_original", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                p2.Value = id_sector_original;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);

                try
                {
                    if (tran == null)
                        myCommand.Connection.Open();

                    MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                    dataResultado.Clear();
                    myDataAdapter.Fill(dataResultado);

                    if (dataResultado.Tables.Count > 0)
                    {
                        string[] listaSectores = new string[dataResultado.Tables[0].Rows.Count];
                        foreach (System.Data.DataRow dr in dataResultado.Tables[0].Rows)
                            listaSectores[i++] = (string)dr[0];

                        //if (AccesoABaseDeDatos.ConexionMySql == null)
                        //{
                        //    myCommand.Dispose();
                        //    mySqlConnectionToCd40.Close();
                        //    mySqlConnectionToCd40.Dispose();
                        //}
                        if (tran == null)
                            myCommand.Connection.Close();

                        return listaSectores;
                    }

                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.SectoresCompuestosPorSectorOriginal: id_sistema={0},id_sector_original={1}. Error:{2}", id_sistema, id_sector_original, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    if (tran != null)
                        tran.Rollback();
                    else
                        myCommand.Connection.Close();

                    return null;
                }
            }
            return null;
        }

        public static DataSet ParametrosSectorNoAgrupado(MySqlConnection mySqlConnectionToCd40, MySqlTransaction tran, string id_sistema, string id_sector_agrupado)
        {
            if (null != mySqlConnectionToCd40)
            {
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                DataSet dataResultado = new DataSet();

                MySqlCommand myCommand = new MySqlCommand("ParametrosSectorNoAgrupado", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_sector_agrupado", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                p2.Value = id_sector_agrupado;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);

                try
                {
                    if (tran == null)
                        myCommand.Connection.Open();

                    MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                    dataResultado.Clear();
                    myDataAdapter.Fill(dataResultado);

                    if (tran == null)
                        myCommand.Connection.Close();
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.ParametrosSectorNoAgrupado: id_sistema={0},id_sector_agrupado={1}. Error:{2}", id_sistema, id_sector_agrupado, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    if (tran == null)
                        myCommand.Connection.Close();
                    else
                        tran.Rollback();
                }

                return dataResultado;
            }
            return null;
        }

        public static DataSet NumerosAbonadosSectorNoAgrupado(MySqlConnection mySqlConnectionToCd40, MySqlTransaction tran, string id_sistema, string id_nucleo, string id_sector_agrupado)
        {
            if (null != mySqlConnectionToCd40)
            {
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                DataSet dataResultado = new DataSet();

                MySqlCommand myCommand = new MySqlCommand("NumerosAbonadosSectorNoAgrupado", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_nucleo", MySqlDbType.Text);
                MySqlParameter p3 = new MySqlParameter("id_sector_agrupado", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                p2.Value = id_nucleo;
                p3.Value = id_sector_agrupado;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);

                try
                {
                    if (tran == null)
                        myCommand.Connection.Open();

                    MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                    dataResultado.Clear();
                    myDataAdapter.Fill(dataResultado);
                    if (tran == null)
                        myCommand.Connection.Close();
                }
                catch (MySqlException)
                {
                    if (tran == null)
                        myCommand.Connection.Close();
                    else
                        tran.Rollback();
                }

                return dataResultado;
            }
            return null;
        }

        public static DataSet ParametrosSectorAgrupado(MySqlConnection mySqlConnectionToCd40, MySqlTransaction tran, string id_sistema, string id_sector_agrupado)
        {
            if (null != mySqlConnectionToCd40)
            {
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                DataSet dataResultado = new DataSet();

                MySqlCommand myCommand = new MySqlCommand("ParametrosSectorAgrupado", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_sector_agrupado", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                p2.Value = id_sector_agrupado;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);

                try
                {
                    if (tran == null)
                        myCommand.Connection.Open();

                    MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                    dataResultado.Clear();
                    myDataAdapter.Fill(dataResultado);

                    if (tran == null)
                        myCommand.Connection.Close();
                }
                catch (MySqlException ex)
                {

                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.ParametrosSectorAgrupado: id_sistema={0},id_sector_agrupado={1}. Error:{2}", id_sistema, id_sector_agrupado, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    if (tran == null)
                        myCommand.Connection.Close();
                    else
                        tran.Rollback();
                }

                return dataResultado;
            }
            return null;
        }

        public static DataSet NumerosAbonadosSectorAgrupado(MySqlConnection mySqlConnectionToCd40, MySqlTransaction tran, string id_sistema, string id_sector_agrupado)
        {
            if (null != mySqlConnectionToCd40)
            {
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                DataSet dataResultado = new DataSet();

                MySqlCommand myCommand = new MySqlCommand("NumerosAbonadosSectorAgrupado", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_sector_agrupado", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                p2.Value = id_sector_agrupado;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);

                try
                {
                    if (tran == null)
                        myCommand.Connection.Open();

                    MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                    dataResultado.Clear();
                    myDataAdapter.Fill(dataResultado);

                    if (tran == null)
                        myCommand.Connection.Close();
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.NumerosAbonadosSectorAgrupado: id_sistema={0},id_sector_agrupado={1}. Error:{2}", id_sistema, id_sector_agrupado, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    if (tran == null)
                        myCommand.Connection.Close();
                    else
                        tran.Rollback();
                }

                return dataResultado;
            }
            return null;
        }

        public static void ActualizaNombreSector(MySqlConnection mySqlConnectionToCd40, MySqlTransaction tran, string id_sistema, string id_nucleo, string id_sector, string id_nuevo_nombre_sector)
        {
            if (null != mySqlConnectionToCd40)
            {
                //MySqlConnection mySqlConnectionToCd40 = new MySqlConnection(CadenaConexion.ToString());

                MySqlCommand myCommand = new MySqlCommand("ActualizaNombreSector", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_nucleo", MySqlDbType.Text);
                MySqlParameter p3 = new MySqlParameter("id_sector", MySqlDbType.Text);
                MySqlParameter p4 = new MySqlParameter("id_nuevo_nombre_sector", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p4.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                p2.Value = id_nucleo;
                p3.Value = id_sector;
                p4.Value = id_nuevo_nombre_sector;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);
                myCommand.Parameters.Add(p4);

                try
                {
                    if (tran != null)
                    {
                        myCommand.Transaction = tran;
                        myCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        myCommand.Connection.Open();
                        myCommand.ExecuteNonQuery();
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.ActualizaNombreSector: id_sistema={0},id_nucleo={1},id_sector={2},id_nuevo_nombre_sector={3}. Error:{4}", id_sistema, id_nucleo, id_sector, id_nuevo_nombre_sector, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                }
            }
        }

        public static string[] SectorConNumeroAbonado(MySqlConnection mySqlConnectionToCd40, string id_sistema, string id_nucleo, uint id_prefijo, string id_abonado)
        {
            int i = 0;
            if (mySqlConnectionToCd40 != null)
            {
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                DataSet dataResultado = new DataSet();
                MySqlCommand myCommand;
                myCommand = new MySqlCommand("SectorConNumeroAbonado", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_nucleo", MySqlDbType.Text);
                MySqlParameter p3 = new MySqlParameter("id_prefijo", MySqlDbType.UInt32);
                MySqlParameter p4 = new MySqlParameter("id_abonado", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p4.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                p2.Value = id_nucleo;
                p3.Value = id_prefijo;
                p4.Value = id_abonado;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);
                myCommand.Parameters.Add(p4);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        string[] listaSectores = new string[dataResultado.Tables[0].Rows.Count];
                        foreach (System.Data.DataRow dr in dataResultado.Tables[0].Rows)
                            listaSectores[i++] = (string)dr[0];

                        myCommand.Connection.Close();
                        return listaSectores;
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        string[] listaSectores = new string[dataResultado.Tables[0].Rows.Count];
                        foreach (System.Data.DataRow dr in dataResultado.Tables[0].Rows)
                            listaSectores[i++] = (string)dr[0];

                        myCommand.Connection.Close();
                        return listaSectores;
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.SectorConNumeroAbonado: id_sistema={0},id_nucleo={1},id_prefijo={2},id_abonado={3},. Error:{4}", id_sistema, id_nucleo, id_prefijo, id_abonado, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                    return null;
                }
            }
            return null;
        }

        public static bool PrimeraPosicionLibre(MySqlConnection mySqlConnectionToCd40, string id_sistema, string id_nucleo, string id_sector, string tipo_acceso, out uint posicion)
        {
            posicion = 0;

            if (mySqlConnectionToCd40 != null)
            {
                System.Data.IDataParameter[] parametros;
                DataSet dataResultado = new DataSet();

                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);

                MySqlCommand myCommand = new MySqlCommand("PrimeraPosicionLibre", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_nucleo", MySqlDbType.Text);
                MySqlParameter p3 = new MySqlParameter("id_sector", MySqlDbType.Text);
                MySqlParameter p4 = new MySqlParameter("tipo_acceso", MySqlDbType.Text);
                MySqlParameter p5 = new MySqlParameter("hueco", MySqlDbType.UInt32);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p4.Direction = System.Data.ParameterDirection.Input;
                p5.Direction = System.Data.ParameterDirection.Output;

                p1.Value = id_sistema;
                p2.Value = id_nucleo;
                p3.Value = id_sector;
                p4.Value = tipo_acceso;

                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);
                myCommand.Parameters.Add(p4);
                myCommand.Parameters.Add(p5);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        parametros = myDataAdapter.GetFillParameters();
                        posicion = Convert.ToUInt32(parametros[4].Value);
                        myCommand.Connection.Close();
                        return posicion != 0;
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        parametros = myDataAdapter.GetFillParameters();
                        posicion = Convert.ToUInt32(parametros[4].Value);
                        myCommand.Connection.Close();

                        return posicion != 0;
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.PrimeraPosicionLibre: id_sistema={0},id_nucleo={1},id_sector={2},tipo_acceso={3},posicion={4}. Error:{5}", id_sistema, id_nucleo, id_sector, tipo_acceso, posicion, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                    return false;
                }
            }

            return false;
        }

        public static bool PosicionOcupadaEnSector(MySqlConnection mySqlConnectionToCd40, string id_sistema, string id_nucleo, string id_sector, string tipo_acceso, uint posicion)
        {
            if (mySqlConnectionToCd40 != null)
            {
                System.Data.IDataParameter[] parametros;
                DataSet dataResultado = new DataSet();


                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);

                MySqlCommand myCommand = new MySqlCommand("PosicionOcupadaEnSector", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_nucleo", MySqlDbType.Text);
                MySqlParameter p3 = new MySqlParameter("id_sector", MySqlDbType.Text);
                MySqlParameter p4 = new MySqlParameter("tipo_acceso", MySqlDbType.Text);
                MySqlParameter p5 = new MySqlParameter("posicion", MySqlDbType.Int32);
                MySqlParameter p6 = new MySqlParameter("existe", MySqlDbType.Int16);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p4.Direction = System.Data.ParameterDirection.Input;
                p5.Direction = System.Data.ParameterDirection.Input;
                p6.Direction = System.Data.ParameterDirection.Output;

                p1.Value = id_sistema;
                p2.Value = id_nucleo;
                p3.Value = id_sector;
                p4.Value = tipo_acceso;
                p5.Value = posicion;

                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);
                myCommand.Parameters.Add(p4);
                myCommand.Parameters.Add(p5);
                myCommand.Parameters.Add(p6);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        parametros = myDataAdapter.GetFillParameters();
                        myCommand.Connection.Close();

                        return Convert.ToInt16(parametros[5].Value) != 0;
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        parametros = myDataAdapter.GetFillParameters();
                        myCommand.Connection.Close();

                        return Convert.ToInt16(parametros[5].Value) != 0;
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.PosicionOcupadaEnSector: id_sistema={0},id_nucleo={1},id_sector={2},tipo_acceso={3},posicion={4}. Error:{5}", id_sistema, id_nucleo, id_sector, tipo_acceso, posicion, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                    return true;
                }
            }

            return true;
        }

        public static DataSet AsignacionRecursosDeUnaRed(MySqlConnection mySqlConnectionToCd40, string id_sistema, string id_red)
        {
            if (null != mySqlConnectionToCd40)
            {
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                DataSet dataResultado = new DataSet();

                MySqlCommand myCommand = new MySqlCommand("AsignacionRecursosDeUnaRed", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_red", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                p2.Value = id_red;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.AsignacionRecursosDeUnaRed: id_sistema={0},id_red={1}. Error:{2}", id_sistema, id_red, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        public static DataSet GetFunciones(MySqlConnection mySqlConnectionToCd40, string id_sistema, int tipo)
        {
            if (null != mySqlConnectionToCd40)
            {
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                DataSet dataResultado = new DataSet();

                MySqlCommand myCommand = new MySqlCommand("GetFunciones", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("_tipo", MySqlDbType.Int32);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                p2.Value = tipo;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.GetFunciones: id_sistema={0},tipo={1}. Error:{2}", id_sistema, tipo, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        public static uint CrearFicheroBackup(MySqlConnection mySqlConnectionToCd40, MySqlTransaction tran, string idSistema, string tabla, string ficheroSalida, DateTime desde, DateTime hasta)
        {
            if (mySqlConnectionToCd40 != null)
            {
                //MySqlConnection mySqlConnectionToCd40 = new MySqlConnection(CadenaConexion.ToString());
                MySqlCommand myCommand;
                myCommand = new MySqlCommand("CrearFicheroBackup", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("tabla", MySqlDbType.Text);
                MySqlParameter p3 = new MySqlParameter("fichero", MySqlDbType.Text);
                MySqlParameter p4 = new MySqlParameter("fDesde", MySqlDbType.DateTime);
                MySqlParameter p5 = new MySqlParameter("fHasta", MySqlDbType.DateTime);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p4.Direction = System.Data.ParameterDirection.Input;
                p5.Direction = System.Data.ParameterDirection.Input;
                p1.Value = idSistema;
                p2.Value = tabla;
                p3.Value = ficheroSalida;
                p4.Value = desde;
                p5.Value = hasta;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);
                myCommand.Parameters.Add(p4);
                myCommand.Parameters.Add(p5);

                try
                {
                    if (tran != null)
                    {
                        myCommand.Transaction = tran;
                        myCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        myCommand.Connection.Open();
                        myCommand.ExecuteNonQuery();
                        myCommand.Connection.Close();
                    }
                    return 1;
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.CrearFicheroBackup: id_sistema={0},tabla={1},ficheroSalida={2}. Error:{3}", idSistema, tabla, ficheroSalida, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                    return 0;
                }
            }
            return 0;
        }

        public static void GeneraRegistroBackup(MySqlConnection mySqlConnectionToCd40, MySqlTransaction tran, string idSistema, int tipoBackup, DateTime desde, DateTime hasta, string recurso)
        {
            if (mySqlConnectionToCd40 != null)
            {
                //MySqlConnection mySqlConnectionToCd40 = new MySqlConnection(CadenaConexion.ToString());
                MySqlCommand myCommand;
                myCommand = new MySqlCommand("GeneraRegistroBackup", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("tipo_backup", MySqlDbType.Int32);
                MySqlParameter p3 = new MySqlParameter("fDesde", MySqlDbType.DateTime);
                MySqlParameter p4 = new MySqlParameter("fHasta", MySqlDbType.DateTime);
                MySqlParameter p5 = new MySqlParameter("recurso", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p4.Direction = System.Data.ParameterDirection.Input;
                p5.Direction = System.Data.ParameterDirection.Input;
                p1.Value = idSistema;
                p2.Value = tipoBackup;
                p3.Value = desde;
                p4.Value = hasta;
                p5.Value = recurso;

                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);
                myCommand.Parameters.Add(p4);
                myCommand.Parameters.Add(p5);

                try
                {
                    if (tran != null)
                    {
                        myCommand.Transaction = tran;
                        myCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        myCommand.Connection.Open();
                        myCommand.ExecuteNonQuery();
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.GeneraRegistroBackup: id_sistema={0},tipoBackup={1},recurso={2}. Error:{3}", idSistema, tipoBackup, recurso, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                }
            }
        }

        public static DataSet GetUriRecursoDestino(MySqlConnection mySqlConnectionToCd40, string idRecurso)
        {
            if (null != mySqlConnectionToCd40)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);

                MySqlCommand myCommand = new MySqlCommand("GetUriRecursoDestino", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_recurso", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p1.Value = idRecurso;
                myCommand.Parameters.Add(p1);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        myDataAdapter.Fill(dataResultado);

                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.GetUriRecursoDestino: idRecurso={0}. Error:{1}", idRecurso, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        public static DataSet ParametrosSectores(MySqlConnection mySqlConnectionToCd40, string id_Sistema, string lista_nucleos, string lista_sectores)
        {
            if (null != mySqlConnectionToCd40)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);

                MySqlCommand myCommand = new MySqlCommand("ParametrosSectores", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_Sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("lista_nucleos", MySqlDbType.Text);
                MySqlParameter p3 = new MySqlParameter("lista_sectores", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_Sistema;
                p2.Value = lista_nucleos;
                p3.Value = lista_sectores;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.ParametrosSectores: sistema={0},lista_nucleos={1},lista_sectores={2}. Error:{3}", id_Sistema, lista_nucleos, lista_sectores, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        public static DataSet RedesUsuariosAbonadosParaXML(MySqlConnection mySqlConnectionToCd40, string id_Sistema, string idSectorizacion)
        {
            if (null != mySqlConnectionToCd40)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);

                MySqlCommand myCommand = new MySqlCommand("RedesUsuariosAbonadosParaXML", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_Sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_sectorizacion", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_Sistema;
                p2.Value = idSectorizacion;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.RedesUsuariosAbonadosParaXML: sistema={0},idSectorizacion={1}. Error:{2}", id_Sistema, idSectorizacion, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        public static DataSet TeclasSectorParaXML(MySqlConnection mySqlConnectionToCd40, string id_sistema, string id_sectorizacion)
        {
            if (null != mySqlConnectionToCd40)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);

                MySqlCommand myCommand = new MySqlCommand("TeclasSectorParaXML", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_sectorizacion", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                p2.Value = id_sectorizacion;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.TeclasSectorParaXML: sistema={0},idSectorizacion={1}. Error:{2}", id_sistema, id_sectorizacion, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        public static DataSet NivelesIntrusionParaXML(MySqlConnection mySqlConnectionToCd40, string id_sistema, string id_sectorizacion)
        {
            if (null != mySqlConnectionToCd40)
            {
                DataSet dataResultado = new DataSet();

                MySqlCommand myCommand = new MySqlCommand("NivelesIntrusionParaXML", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_sectorizacion", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                p2.Value = id_sectorizacion;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.NivelesIntrusionParaXML:sistema={0},idSectorizacion={1}. Error:{2}", id_sistema, id_sectorizacion, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();
                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        //(Description = "Pasándole el identificador del sistema  y el identificador de la sectorizacion," 
        //               " retorna una lista de destinos de telefonía internos o externos asignados a cada sector.")
        public static DataSet DestinosTelefoniaSectorizadosParaXML(MySqlConnection mySqlConnectionToCd40, string id_sistema, string id_sectorizacion, bool lc)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                MySqlCommand myCommand;
                myCommand = new MySqlCommand("DestinosTelefoniaSectorizadosParaXML", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_sectorizacion", MySqlDbType.Text);
                MySqlParameter p3 = new MySqlParameter("lc", MySqlDbType.Int16);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                p2.Value = id_sectorizacion;
                p3.Value = lc;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.DestinosTelefoniaSectorizadosParaXML:sistema={0},idSectorizacion={1},lc={2}. Error:{3}", id_sistema, id_sectorizacion, lc, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                }

                return dataResultado;
            }

            return null;
        }

        //(Description = "Pasándole el identificador del sistema  y el identificador de la sectorizacion," 
        //               " retorna una lista de destinos de radio asignados a cada sector.")
        public static DataSet DestinosRadioSectorizadosParaXML(MySqlConnection mySqlConnectionToCd40, string id_sistema, string id_sectorizacion)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                MySqlCommand myCommand;
                myCommand = new MySqlCommand("DestinosRadioSectorizadosParaXML", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_sectorizacion", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_sistema;
                p2.Value = id_sectorizacion;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.DestinosRadioSectorizadosParaXML:sistema={0},idSectorizacion={1}. Error:{2}", id_sistema, id_sectorizacion, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                }

                return dataResultado;
            }

            return null;
        }

        public static DataSet ParametrosSectoresParaXML(MySqlConnection mySqlConnectionToCd40, string id_Sistema, string idSectorizacion)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);

                MySqlCommand myCommand = new MySqlCommand("ParametrosSectoresParaXML", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_Sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_sectorizacion", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p1.Value = id_Sistema;
                p2.Value = idSectorizacion;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.ParametrosSectoresParaXML: sistema={0},idSectorizacion={1}. Error:{2}", id_Sistema, idSectorizacion, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                }

                return dataResultado;
            }

            return null;
        }

        public static DataSet ListaRecursosDestino(MySqlConnection mySqlConnectionToCd40, string idSistema, string idDestino, int prefijo, string dependencia)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                MySqlCommand myCommand = new MySqlCommand("ListaRecursosDestino", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_destino", MySqlDbType.Text);
                MySqlParameter p3 = new MySqlParameter("id_prefijo", MySqlDbType.Int32);
                MySqlParameter p4 = new MySqlParameter("id_dependencia", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p4.Direction = System.Data.ParameterDirection.Input;
                p1.Value = idSistema;
                p2.Value = idDestino;
                p3.Value = prefijo;
                p4.Value = dependencia == null ? string.Empty : dependencia;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);
                myCommand.Parameters.Add(p4);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.ListaRecursosDestino: sistema={0},idDestino={1},prefijo={2},dependencia={3}. Error:{4}", idSistema, idDestino, prefijo, dependencia, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                }

                return dataResultado;
            }

            return null;
        }

        public static DataSet ListaRecursosLCEN_DestinoATS(MySqlConnection mySqlConnectionToCd40, string idSistema, string idSectorizacion, string idNucleo, string idSector, string strGrupo)
        {
            //ListaRecursosLCEN_DestinoATS`(IN id_sistema varchar(32),IN id_sectorizacion varchar(32),IN id_Nucleo varchar(32),IN id_Sector varchar(32), IN id_Grupo varchar(80))
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                MySqlCommand myCommand = new MySqlCommand("ListaRecursosLCEN_DestinoATS", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_sectorizacion", MySqlDbType.Text);
                MySqlParameter p3 = new MySqlParameter("id_Nucleo", MySqlDbType.Text);
                MySqlParameter p4 = new MySqlParameter("id_Sector", MySqlDbType.Text);
                MySqlParameter p5 = new MySqlParameter("id_Grupo", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p4.Direction = System.Data.ParameterDirection.Input;
                p5.Direction = System.Data.ParameterDirection.Input;
                p1.Value = idSistema;
                p2.Value = idSectorizacion;
                p3.Value = idNucleo;
                p4.Value = idSector;
                p5.Value = strGrupo;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);
                myCommand.Parameters.Add(p4);
                myCommand.Parameters.Add(p5);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.ListaRecursosLCEN_DestinoATS: id_sistema={0},id_Sectorizacion={1},id_Nucleo={2},id_Sector={3},id_Grupo={4}. Error:{5}", idSistema, idSectorizacion, idNucleo, idSector, strGrupo, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    if (myCommand.Connection.State == ConnectionState.Open)
                        myCommand.Connection.Close();
                }

                return dataResultado;
            }

            return null;
        }

        public static DataSet RecursosPorTroncalParaXML(MySqlConnection mySqlConnectionToCd40, string idSistema)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);

                MySqlCommand myCommand = new MySqlCommand("RecursosPorTroncalParaXML", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p1.Value = idSistema;
                myCommand.Parameters.Add(p1);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        myDataAdapter.Fill(dataResultado);

                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.RecursosPorTroncalParaXML: id_sistema={0}. Error:{1}", idSistema, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                }

                return dataResultado;
            }

            return null;
        }

        public static DataSet RecursosPorEmplazamientoParaXML(MySqlConnection mySqlConnectionToCd40, string idSistema)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();

                MySqlCommand myCommand = new MySqlCommand("RecursosPorEmplazamientoParaXML", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p1.Value = idSistema;
                myCommand.Parameters.Add(p1);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.RecursosPorEmplazamientoParaXML: id_sistema={0}. Error:{1}", idSistema, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                }

                return dataResultado;
            }

            return null;
        }

        public static DataSet DestinosPorGruposTelefonia(MySqlConnection mySqlConnectionToCd40, string idSistema)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();

                MySqlCommand myCommand = new MySqlCommand("DestinosPorGruposTelefonia", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p1.Value = idSistema;
                myCommand.Parameters.Add(p1);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.DestinosPorGruposTelefonia: id_sistema={0}. Error:{1}", idSistema, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        public static DataSet RecursosTfParaInformeXML(MySqlConnection mySqlConnectionToCd40, string idSistema)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();

                MySqlCommand myCommand = new MySqlCommand("RecursosTfParaInformeXML", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p1.Value = idSistema;
                myCommand.Parameters.Add(p1);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.RecursosTfParaInformeXML: id_sistema={0}. Error:{1}", idSistema, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                }

                return dataResultado;
            }

            return null;
        }

        public static DataSet RecursosRdParaInformeXML(MySqlConnection mySqlConnectionToCd40, string idSistema)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);

                MySqlCommand myCommand = new MySqlCommand("RecursosRdParaInformeXML", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p1.Value = idSistema;
                myCommand.Parameters.Add(p1);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.RecursosRdParaInformeXML: id_sistema={0}. Error:{1}", idSistema, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                }

                return dataResultado;
            }

            return null;
        }

        public static DataSet RecursosLcParaInformeXML(MySqlConnection mySqlConnectionToCd40, string idSistema)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();

                MySqlCommand myCommand = new MySqlCommand("RecursosLcParaInformeXML", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p1.Value = idSistema;
                myCommand.Parameters.Add(p1);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.RecursosLcParaInformeXML: id_sistema={0}. Error:{1}", idSistema, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        public static int ExisteIP(MySqlConnection mySqlConnectionToCd40, string idSistema, string ip, string idEh)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();

                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                MySqlCommand myCommand;
                myCommand = new MySqlCommand("ExisteIP", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("ip", MySqlDbType.Text);
                MySqlParameter p3 = new MySqlParameter("id_hw", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p1.Value = idSistema;
                p2.Value = ip;
                p3.Value = idEh;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();

                        return Convert.ToInt32(dataResultado.Tables[0].Rows[0][0]);
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();

                        return Convert.ToInt32(dataResultado.Tables[0].Rows[0][0]);
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.ExisteIP: id_sistema={0},ip={1},id_hw={2}. Error:{3}", idSistema, ip, idEh, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                    return 0;
                }
            }

            return 0;
        }

        public static void ActualizaSectoresSectorizacion(MySqlConnection mySqlConnectionToCd40, MySqlTransaction tran, string idSistema, string idNucleo, string grupoSectores, string idAgrupacion)
        {
            if (mySqlConnectionToCd40 != null)
            {
                MySqlCommand myCommand = new MySqlCommand("ActualizaSectoresSectorizacion", mySqlConnectionToCd40);

                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_nucleo", MySqlDbType.Text);
                MySqlParameter p3 = new MySqlParameter("id_grupo", MySqlDbType.Text);
                MySqlParameter p4 = new MySqlParameter("id_agrupacion", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p4.Direction = System.Data.ParameterDirection.Input;
                p1.Value = idSistema;
                p2.Value = idNucleo;
                p3.Value = grupoSectores;
                p4.Value = idAgrupacion;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);
                myCommand.Parameters.Add(p4);

                try
                {
                    if (tran != null)
                    {
                        myCommand.Transaction = tran;
                        myCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        myCommand.Connection.Open();
                        myCommand.ExecuteNonQuery();
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException objEx)
                {
                    string strCadena = string.Empty;
                    strCadena = string.Format("ActualizaSectoresSectorizacion- Error al invocar al procedimiento ActualizaSectoresSectorizacion({0},{1},{2},{3})", idSistema, idNucleo, grupoSectores, idAgrupacion);
                    CD40.BD.GestorBaseDatos.logFile.Error(strCadena);
                    CD40.BD.GestorBaseDatos.logFile.Error("Error:", objEx);

                    if (myCommand.Connection.State == ConnectionState.Open)
                        myCommand.Connection.Close();
                }
            }
        }

        public static DataSet SectoresManttoEnTop(MySqlConnection mySqlConnectionToCd40, string idSistema, string idSectorizacion, string idTop)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                MySqlCommand myCommand = new MySqlCommand("SectoresManttoEnTop", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_sectorizacion", MySqlDbType.Text);
                MySqlParameter p3 = new MySqlParameter("id_top", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p1.Value = idSistema;
                p2.Value = idSectorizacion;
                p3.Value = idTop;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.SectoresManttoEnTop: id_sistema={0},idSectorizacion={1},idTop={2}. Error:{3}", idSistema, idSectorizacion, idTop, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                }

                return dataResultado;
            }

            return null;
        }

        public static DataSet SectoresNumSactaSorted(MySqlConnection mySqlConnectionToCd40, string idSistema, string idNucleo, string listaSectores)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                MySqlCommand myCommand = new MySqlCommand("SectoresNumSactaSorted", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("id_nucleo", MySqlDbType.Text);
                MySqlParameter p3 = new MySqlParameter("lista_sectores", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p1.Value = idSistema;
                p2.Value = idNucleo;
                p3.Value = listaSectores;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.SectoresNumSactaSorted: id_sistema={0},idNucleo={1},listaSectores={2}. Error:{3}", idSistema, idNucleo, listaSectores, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                }

                return dataResultado;
            }

            return null;
        }

        public static DataSet GetEstadoCluster(MySqlConnection mySqlConnectionToCd40)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                MySqlCommand myCommand = new MySqlCommand("GetEstadoCluster", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.GetEstadoCluster. Error:{0}", ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                }

                return dataResultado;
            }

            return null;
        }

        public static DataSet GetRate(MySqlConnection mySqlConnectionToCd40, string idSistema, DateTime desde, DateTime hasta)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                MySqlCommand myCommand;
                myCommand = new MySqlCommand("GetRate", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("desde", MySqlDbType.Date);
                MySqlParameter p3 = new MySqlParameter("hasta", MySqlDbType.Date);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p1.Value = idSistema;
                p2.Value = desde.Date;
                p3.Value = hasta.Date;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);

                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.GetRate. Error:{0}", ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        public static DataSet GeMtbf(MySqlConnection mySqlConnectionToCd40, string idSistema, DateTime desde, DateTime hasta)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                MySqlCommand myCommand;
                myCommand = new MySqlCommand("GetMtbf", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("desde", MySqlDbType.Date);
                MySqlParameter p3 = new MySqlParameter("hasta", MySqlDbType.Date);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p1.Value = idSistema;
                p2.Value = desde.Date;
                p3.Value = hasta.Date;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);

                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.GeMtbf -> Error:{0}", ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        public static DataSet GetRate(MySqlConnection mySqlConnectionToCd40, string idSistema, DateTime desde, DateTime hasta, string components)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                MySqlCommand myCommand;
                myCommand = new MySqlCommand("GetRateComponents", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("desde", MySqlDbType.Date);
                MySqlParameter p3 = new MySqlParameter("hasta", MySqlDbType.Date);
                MySqlParameter p4 = new MySqlParameter("components", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p4.Direction = System.Data.ParameterDirection.Input;
                p1.Value = idSistema;
                p2.Value = desde.Date;
                p3.Value = hasta.Date;
                p4.Value = components;

                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);
                myCommand.Parameters.Add(p4);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);

                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.GetRate -> Error:{0}", ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        public static DataSet GeMtbf(MySqlConnection mySqlConnectionToCd40, string idSistema, DateTime desde, DateTime hasta, string components)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                MySqlCommand myCommand;
                myCommand = new MySqlCommand("GetMtbfComponents", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("desde", MySqlDbType.Date);
                MySqlParameter p3 = new MySqlParameter("hasta", MySqlDbType.Date);
                MySqlParameter p4 = new MySqlParameter("components", MySqlDbType.Text);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p4.Direction = System.Data.ParameterDirection.Input;
                p1.Value = idSistema;
                p2.Value = desde.Date;
                p3.Value = hasta.Date;
                p4.Value = components;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);
                myCommand.Parameters.Add(p4);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);

                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.GeMtbf -> Error:{0}", ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        public static DataSet GetRate(MySqlConnection mySqlConnectionToCd40, string idSistema, DateTime desde, DateTime hasta, int evento)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                MySqlCommand myCommand;
                myCommand = new MySqlCommand("GetRateEvents", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("desde", MySqlDbType.Date);
                MySqlParameter p3 = new MySqlParameter("hasta", MySqlDbType.Date);
                MySqlParameter p4 = new MySqlParameter("evento", MySqlDbType.Int32);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p4.Direction = System.Data.ParameterDirection.Input;
                p1.Value = idSistema;
                p2.Value = desde.Date;
                p3.Value = hasta.Date;
                p4.Value = evento;

                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);
                myCommand.Parameters.Add(p4);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);

                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException)
                {
                    myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        public static DataSet GeMtbf(MySqlConnection mySqlConnectionToCd40, string idSistema, DateTime desde, DateTime hasta, int evento)
        {
            if (mySqlConnectionToCd40 != null)
            {
                DataSet dataResultado = new DataSet();
                //MySqlConnection mySqlConnectionToCd40 = AccesoABaseDeDatos.ConexionMySql != null ? AccesoABaseDeDatos.ConexionMySql : new MySqlConnection(CadenaConexion);
                MySqlCommand myCommand;
                myCommand = new MySqlCommand("GetMtbfEvents", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlParameter p1 = new MySqlParameter("id_sistema", MySqlDbType.Text);
                MySqlParameter p2 = new MySqlParameter("desde", MySqlDbType.Date);
                MySqlParameter p3 = new MySqlParameter("hasta", MySqlDbType.Date);
                MySqlParameter p4 = new MySqlParameter("evento", MySqlDbType.Int32);
                p1.Direction = System.Data.ParameterDirection.Input;
                p2.Direction = System.Data.ParameterDirection.Input;
                p3.Direction = System.Data.ParameterDirection.Input;
                p4.Direction = System.Data.ParameterDirection.Input;
                p1.Value = idSistema;
                p2.Value = desde.Date;
                p3.Value = hasta.Date;
                p4.Value = evento;
                myCommand.Parameters.Add(p1);
                myCommand.Parameters.Add(p2);
                myCommand.Parameters.Add(p3);
                myCommand.Parameters.Add(p4);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();

                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);

                        myCommand.Connection.Close();
                    }
                    else
                    {
                        MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myCommand);
                        dataResultado.Clear();
                        myDataAdapter.Fill(dataResultado);
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException)
                {
                    if (myCommand.Connection.State == ConnectionState.Open)
                        myCommand.Connection.Close();
                }

                return dataResultado;
            }
            return null;
        }

        //Invoca al procedimiento almacenado en la BD CheckIdentificadorAsignado
        //Devuelve 1 si el identificador del elemento existe existe en el conjunto de tablas agrupadas dentro del tipo (que se le pasa como parámetro) existe en el sistema, 0 si no existe y -1 si 
        // se ha producido error. 
        //El tipo puede ser:EF (Pasarelas,Tops, Equipos Externos y Encaminamientos), R (Recursos Radio y de Telefonía), D (Destinos Radio y de Telefonía), NA (Emplazamientos y Zonas) 

        public static int CheckIdentificadorAsignado(MySqlConnection mySqlConnectionToCd40, string strTipo, string idSistema, string strIdentificador)
        {
            int iResultado = -1;
            if (mySqlConnectionToCd40 != null)
            {
                MySqlCommand myCommand;

                myCommand = new MySqlCommand("CheckIdentificadorAsignado", mySqlConnectionToCd40);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;

                MySqlParameter pTipo = new MySqlParameter("Tipo", MySqlDbType.Text);
                MySqlParameter pSistema = new MySqlParameter("pid_sistema", MySqlDbType.Text);
                MySqlParameter pIdentificador = new MySqlParameter("id_identificador", MySqlDbType.Text);
                MySqlParameter piAsignado = new MySqlParameter("iAsignado", MySqlDbType.Int32);

                pTipo.Direction = System.Data.ParameterDirection.Input;
                pTipo.Value = strTipo;
                pSistema.Direction = System.Data.ParameterDirection.Input;
                pSistema.Value = idSistema;
                pIdentificador.Direction = System.Data.ParameterDirection.Input;
                pIdentificador.Value = strIdentificador;
                piAsignado.Direction = System.Data.ParameterDirection.Output;
                piAsignado.Value = -2;

                myCommand.Parameters.Add(pTipo);
                myCommand.Parameters.Add(pSistema);
                myCommand.Parameters.Add(pIdentificador);
                myCommand.Parameters.Add(piAsignado);

                try
                {
                    if (myCommand.Connection.State != ConnectionState.Open)
                    {
                        myCommand.Connection.Open();
                    }

                    if (myCommand.Connection.State == ConnectionState.Open)
                    {
                        myCommand.ExecuteNonQuery();

                        iResultado = (int)myCommand.Parameters["iAsignado"].Value;
                        myCommand.Connection.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    StringBuilder strMsg = new StringBuilder();

                    strMsg.AppendFormat("Error en Procedimientos.CheckIdentificadorAsignado: strTipo={0},idSistema={1},strIdentificador={2}. Error:{3}", strTipo, idSistema, strIdentificador, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    if (myCommand.Connection.State == ConnectionState.Open)
                    {
                        myCommand.Connection.Close();
                    }
                    iResultado = -1;
                }
                catch (Exception ex)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error en Procedimientos.CheckIdentificadorAsignado: strTipo={0},idSistema={1},strIdentificador={2}. Error genérico:{3}", strTipo, idSistema, strIdentificador, ex.Message.ToString());
                    CD40.BD.GestorBaseDatos.logFile.Error(strMsg.ToString());
                    strMsg.Clear();

                    if (myCommand.Connection.State == ConnectionState.Open)
                    {
                        myCommand.Connection.Close();
                    }
                    iResultado = -1;
                }
            }

            return iResultado;
        }
    }


}
