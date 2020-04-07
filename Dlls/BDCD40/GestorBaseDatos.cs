using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using MySql.Data.MySqlClient;
using CD40.BD.Entidades;
using log4net;
using log4net.Config;

namespace CD40.BD
{
    /// <summary>
    /// 
    /// </summary>
	public enum Tipo_Backup
	{
		TB_Historicos_Hardware, TB_Historicos_Telefonia, TB_Historicos_Radio, TB_Historicos_Gestion,
		TB_Indicadores_Hardware, TB_Indicadores_Telefonia, TB_Indicadores_Radio, TB_Indicadores_Gestion
	};

	public class GestorBaseDatos
	{
        /// <summary>
        /// 
        /// </summary>
		private MySqlConnection MySqlConnectionToCd40;
        /// <summary>
        /// 
        /// </summary>
		private static object _Sync = new object();
		public MySqlConnection ConexionMySql
		{
			get { return MySqlConnectionToCd40; }
		}
        /// <summary>
        /// 
        /// </summary>
		private static ILog _log;
        /// <summary>
        /// 
        /// </summary>
		public static ILog logFile
		{
			get
			{
				if (_log == null)
				{
					log4net.Config.XmlConfigurator.Configure();
					_log = LogManager.GetLogger("File");
				}
				return _log;
			}
		}

        /// <summary>
        /// 
        /// </summary>
		private static ILog _logDebugView;
		public static ILog logDebugView
		{
			get
			{
				if (_logDebugView == null)
				{
					log4net.Config.XmlConfigurator.Configure();
					_logDebugView = LogManager.GetLogger("BDCD40");
				}
				return _logDebugView;
			}
		}

        private long _lastInsertedValue;
        public long LastInsertedValue
        {
            get { return _lastInsertedValue; }
        }
		
        /// <summary>
        /// 
        /// </summary>
		public GestorBaseDatos()
        {
            try
            {
				string cadenaConexion;
                System.Configuration.Configuration webConfiguration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
				if (webConfiguration.ConnectionStrings.ConnectionStrings.Count > 0)
				{
					cadenaConexion = webConfiguration.ConnectionStrings.ConnectionStrings["ConexionBaseDatosCD40"].ToString();

					if (MySqlConnectionToCd40 == null)
						MySqlConnectionToCd40 = new MySqlConnection(cadenaConexion);
				}
			}
            catch (System.Configuration.ConfigurationErrorsException e)
            {
                logDebugView.Error("(GestorBaseDatos-constructor):",e);
                logFile.Error("(GestorBaseDatos-constructor):",e);
                /** 
                 * AGL. ID.83/84. A este nivel, las excepciones se deberían enviar a niveles superiores..
                 * */
                //throw e;
                /** Fin Modificacion */
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cadenaConexion"></param>
		public GestorBaseDatos(string cadenaConexion)
		{
			try
			{
				MySqlConnectionToCd40 = new MySqlConnection(cadenaConexion);
			}
			catch (MySqlException e)
			{
				logDebugView.Error("(GestorBaseDatos-constructor):", e);
				logFile.Error("(GestorBaseDatos-constructor):", e);
                /**
                 * AGL. ID.83/84. A este nivel, las excepciones se deberían enviar a niveles superiores..
                 * */
                //throw e;
                /** Fin Modificacion */
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conexion"></param>
		public GestorBaseDatos(MySqlConnection conexion)
		{
			MySqlConnectionToCd40 = conexion;
		}

        /// <summary>
        /// (Description = "Pasándole una sentencia SELECT devuelve un DataSet con los registros que cumplan la SELECT")
        /// </summary>
        /// <param name="consulta"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
		public DataSet GetDataSet(string consulta, MySqlTransaction tran)
		{
            if (consulta.Length > 0)
            {
                logDebugView.Debug(String.Format("(GestorBaseDatos-GetDataSet):  {0}", consulta));
                if (MySqlConnectionToCd40 != null)
                {
                    lock (_Sync)
                    {

                        DataSet DataSetResultado;
                        DataSetResultado = new DataSet();

                        try
                        {
                            if (tran == null && MySqlConnectionToCd40.State != ConnectionState.Open)
                                MySqlConnectionToCd40.Open();
                            MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(consulta, MySqlConnectionToCd40);
                            using (myDataAdapter)
                            {
                                myDataAdapter.Fill(DataSetResultado, "TablaCliente");
                            }
                        }
                        catch (MySqlException e)
                        {
                            logDebugView.Error("(GestorBaseDatos-GetDataSet):", e);
                            logFile.Error("(GestorBaseDatos-GetDataSet): error al ejecutar la consulta: " + consulta);
                            logFile.Error("(GestorBaseDatos-GetDataSet):", e);
                            /**
                             * AGL. ID.83/84. A este nivel, las excepciones se deberían enviar a niveles superiores..
                             * */
                            //if (tran == null)	// Si no hay una transacción, entonces se cierra la conexión
                            //    MySqlConnectionToCd40.Close();
                            //throw e;
                            /** Fin Modificacion */
                        }
                        finally
                        {
                            if (tran == null)	// Si no hay una transacción, entonces se cierra la conexión
                                if (MySqlConnectionToCd40.State == ConnectionState.Open)
                                    MySqlConnectionToCd40.Close();
                        }

                        return DataSetResultado;
                    }
                }
                else
                    logDebugView.Warn("(GestorBaseDatos-GetDataSet): No existe conexion con la base de datos.");
            }

			return null;
		}

        /// <summary>
        /// (Description = "Pasándole una sentencia INSERT, UPDATE, DELETE devuelve el número de registros afectados o un número negativo cuyo valor" +
        ///               " absoluto indica el código de excepción en caso de error")
        /// </summary>
        /// <param name="sentencia"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sentencia, MySqlTransaction tran)
		{
			int numRows = 0;

			logDebugView.Debug(String.Format("(GestorBaseDatos-ExecuteNonQuery):  {0}", sentencia));

			if (MySqlConnectionToCd40 != null)
			{
				lock (_Sync)
				{
                    if (tran == null && MySqlConnectionToCd40.State != ConnectionState.Open)
                        MySqlConnectionToCd40.Open();
                    
                    MySqlCommand myCommand = new MySqlCommand(sentencia, MySqlConnectionToCd40);
					myCommand.Transaction = tran;

                    try
                    {
                        numRows = myCommand.ExecuteNonQuery();
                        _lastInsertedValue = myCommand.LastInsertedId;
                    }
                    catch (InvalidOperationException ex )
                    {
                        logFile.Error("(GestorDatos-ExecuteNonQuery InvalidOp) Error al ejecutar la sentencia: " + sentencia);
                        logFile.Error("(GestorDatos-ExecuteNonQuery) Error: ", ex);

                        if (MySqlConnectionToCd40.State == ConnectionState.Open)
                            MySqlConnectionToCd40.Close();
                    }
                    catch (MySqlException e)
                    {
                        if (e.Number != 1062)
                        {
                            // Si el error es distinto de clave duplicada, registramos el error
                            logDebugView.Error("(GestorBaseDatos-ExecuteNonQuery) Error al ejecutar: " + sentencia, e);
                            logFile.Error("(GestorDatos-ExecuteNonQuery) Error al ejecutar la sentencia: " + sentencia);
                            logFile.Error("(GestorDatos-ExecuteNonQuery) Error: ", e);
                        }

                        if (MySqlConnectionToCd40.State == ConnectionState.Open)
                            MySqlConnectionToCd40.Close();
                        numRows = 0 - e.Number;
                        /**
                         * AGL. ID.83/84. A este nivel, las excepciones se deberían enviar a niveles superiores..
                         * */
                        //if (tran == null)	// Si no hay una transacción, entonces se cierra la conexión
                        //    MySqlConnectionToCd40.Close();
                        //throw e;
                        /** Fin Modificacion */
                    }
					finally
					{
                        if (tran == null)	// Si no hay una transacción, entonces se cierra la conexión
                        {
                            if (MySqlConnectionToCd40.State == ConnectionState.Open)
                                MySqlConnectionToCd40.Close();
                        }
					}
				}
			}
			else
				logDebugView.Warn("(AccesoABaseDeDatos-ExecuteNonQuery): MySqlConnection null.");

			return numRows;
		}

        /// <summary>
        /// "Pasándole una sentencia SELECT devuelve la primera columna de la primera fila " 
        /// "(útil en sentencias del tipo SELECT COUNT)o un número negativo " 
        /// "cuyo valor absoluto indica el código de excepción en caso de error.")
        /// </summary>
        /// <param name="consulta"></param>
        /// <returns></returns>
		public object ExecuteScalar(string consulta)
		{
			object numEscalar = (object)0;
			bool cerrar = true;

			if (MySqlConnectionToCd40 != null)
			{
				lock (_Sync)
				{
					MySqlCommand myCommand = new MySqlCommand(consulta, MySqlConnectionToCd40);

					try
					{
						if (MySqlConnectionToCd40.State != ConnectionState.Open)
							MySqlConnectionToCd40.Open();
						else
							cerrar = false;

						numEscalar = myCommand.ExecuteScalar();
					}
					catch (MySqlException e)
					{
						numEscalar = (object)(0 - e.Number);
                        /**
                         * AGL. ID.83/84. A este nivel, las excepciones se deberían enviar a niveles superiores..
                         * */
                        //if (cerrar)	// Si no hay una transacción, entonces se cierra la conexión
                        //    MySqlConnectionToCd40.Close();
                        //throw e;
                        /** Fin Modificacion */
					}
					finally
					{
						if (cerrar)
							MySqlConnectionToCd40.Close();
					}

					return numEscalar;
				}
			}

			return -1;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="permitirLecturasSucias"></param>
        /// <returns></returns>
		public MySqlTransaction StartTransaction(bool permitirLecturasSucias)
		{
			MySqlTransaction t;

			if (MySqlConnectionToCd40 != null)
			{
				lock (_Sync)
				{
					try
					{
						if (MySqlConnectionToCd40.State != ConnectionState.Open)
							MySqlConnectionToCd40.Open();

						t = MySqlConnectionToCd40.BeginTransaction(IsolationLevel.ReadUncommitted);
					}
					catch (MySqlException e)
					{
						logDebugView.Error("(GestorBaseDatos-StartTransaction):", e);
						logFile.Error("(GestorBaseDatos-StartTransaction):", e);

                        return null;
                        /**
                         * AGL. ID.83/84. A este nivel, las excepciones se deberían enviar a niveles superiores..
                         * */
                        /** return null; */
                        //throw e;
                        /** Fin Modificacion */
					}

					return t;
				}
			}

			return null;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
		public void Commit(MySqlTransaction t)
		{
			try
			{
				t.Commit();
			}
			catch (MySqlException e)
			{
				logDebugView.Error("(GestorBaseDatos-Commit):", e);
				logFile.Error("(GestorBaseDatos-Commit):", e);
                /**
                 * AGL. ID.83/84. A este nivel, las excepciones se deberían enviar a niveles superiores..
                 * */
                //MySqlConnectionToCd40.Close();
                //throw e;
                /** Fin Modificacion */
			}
			finally
			{
                if (MySqlConnectionToCd40.State == ConnectionState.Open)
                    MySqlConnectionToCd40.Close();
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
		public void RollBack(MySqlTransaction t)
		{
			try
			{
				t.Rollback();
			}
			catch (MySqlException e)
			{
				logDebugView.Error("(GestorBaseDatos-Rollback):", e);
				logFile.Error("(GestorBaseDatos-Rollback):", e);
			}
			finally
			{
				MySqlConnectionToCd40.Close();
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public DataSet DataSetSelectSQL(Tablas t, MySqlTransaction tran)
        {
            string strQuery = t.DataSetSelectSQL();
            return this.GetDataSet(strQuery, tran);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="tran"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public DataSet DataSetSelectSQL(Tablas t, string parameter, MySqlTransaction tran)
        {
            string strQuery = t.DataSetSelectSQL(parameter);
            if (strQuery != null)
                return this.GetDataSet(strQuery, tran);

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public List<Tablas> ListSelectSQL(Tablas t, MySqlTransaction tran)
        {
            DataSet ds = this.DataSetSelectSQL(t, tran);
            return t.ListSelectSQL(ds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="tran"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public List<Tablas> ListSelectSQL(Tablas t, string parameter, MySqlTransaction tran)
        {
            DataSet ds = this.DataSetSelectSQL(t, parameter, tran);
            if (ds != null)
                return t.ListSelectSQL(ds);

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
		public int InsertSQL(Tablas t, MySqlTransaction trans)
		{
			int cuantos = 0;
			try
			{
				//MySqlTransaction trans = this.StartTransaction(false);

				string[] listaInserciones = t.InsertSQL();
				for (int i = 0; i < listaInserciones.Length; i++)
				{
                    if (listaInserciones[i] != null)
                    {
                        if (LastInsertedValue != 0)
                        {
                            listaInserciones[i] = SetParameterLastInsertedValue(listaInserciones[i], "<LastInsertedValue>", LastInsertedValue);
                        }

                        cuantos += this.ExecuteNonQuery(listaInserciones[i], trans);
                        if (LastInsertedValue != 0)
                            t.InsertedId = LastInsertedValue;
                    }
				}

				//this.Commit(trans);

				return cuantos;
			}
			catch (MySqlException e)
			{
				logDebugView.Error("(GestorBaseDatos-InsertSQL):", e);
				logFile.Error("(GestorBaseDatos-InsertSQL):", e);

                return 0;
                /**
                 * AGL. ID.83/84. A este nivel, las excepciones se deberían enviar a niveles superiores..
                 * */
                /** return 0; */
                //throw e;
                /** Fin Modificacion */
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
		public int UpdateSQL(Tablas t, MySqlTransaction trans)
		{
			int cuantos = 0;
			try
			{
				//MySqlTransaction trans = this.StartTransaction(false);

				string[] listaInserciones = t.UpdateSQL();
				for (int i = 0; i < listaInserciones.Length; i++)
				{
                    if (listaInserciones[i] != null)
                        cuantos += this.ExecuteNonQuery(listaInserciones[i], trans);
				}

				//this.Commit(trans);

				return cuantos;
			}
			catch (MySqlException e)
			{
				logDebugView.Error("(GestorBaseDatos-UpdateSQL):", e);
				logFile.Error("(GestorBaseDatos-UpdateSQL):", e);

                return 0;
                /**
                 * AGL. ID.83/84. A este nivel, las excepciones se deberían enviar a niveles superiores..
                 * */
                /** return 0; */
                //throw e;
                /** Fin Modificacion */
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
		public int DeleteSQL(Tablas t, MySqlTransaction trans)
		{
			int cuantos = 0;
			try
			{
				//MySqlTransaction trans = this.StartTransaction(false);

				string[] listaInserciones = t.DeleteSQL();
				for (int i = 0; i < listaInserciones.Length; i++)
				{
                    if (listaInserciones[i] != null)
					    cuantos += this.ExecuteNonQuery(listaInserciones[i], trans);
				}

				//this.Commit(trans);

				return cuantos;
			}
			catch (MySqlException e)
			{
				logDebugView.Error("(GestorBaseDatos-InsertSQL):", e);
				logFile.Error("(GestorBaseDatos-InsertSQL):", e);

                return 0;
                /**
                 * AGL. ID.83/84. A este nivel, las excepciones se deberían enviar a niveles superiores..
                 * */
                /** return 0; */
                //throw e;
                /** Fin Modificacion */
			}
		}

        public DataSet DataSetComponents(Tablas t, DateTime desde, DateTime hasta)
        {
            string strQuery = t.DataSetComponentsSQL(desde, hasta);
            return this.GetDataSet(strQuery, null);
        }

        /**
         * AGL 2012.06.18. ID.119 
         * 
         * Nueva Rutina.
         * */

        /// <summary>
        /// A partir de los identificadores de 'sectorizacion' y de 'sistema', determina si está todos los
        /// sectores del nucleo asignados.
        /// </summary>
        /// <param name="idSect">Identificador de la Sectorizacion</param>
        /// <param name="idSistema">Identificador del Sistema</param>
        /// <returns></returns>
        public bool TodosLosSectoresEnSectorizacion(string idSect, string idSistema)
        {
            bool ret = false;

            string sql1 = string.Format("SELECT * from SectoresSectorizacion where IdSectorizacion='{0}' and IdSistema='{1}'", idSect, idSistema);
            DataSet SectoresSectorizacion = GetDataSet(sql1, null);
            if (SectoresSectorizacion.Tables.Count == 1)
            {
                int nSect1 = SectoresSectorizacion.Tables[0].Rows.Count;

                if (nSect1 > 0)
                {
                    int nCol = SectoresSectorizacion.Tables[0].Rows[0].ItemArray.GetLength(0);
                    if (nCol > 2)
                    {
                        string idNucleo = SectoresSectorizacion.Tables[0].Rows[0].ItemArray[2].ToString();
                        string sql2 = string.Format("SELECT * from Sectores where idnucleo='{0}' and sectorsimple=1", idNucleo);
                        DataSet SectoresNucleo = GetDataSet(sql2, null);
                        if (SectoresNucleo.Tables.Count > 0)
                        {
                            if (nSect1 == SectoresNucleo.Tables[0].Rows.Count)
                            {
                                ret = true;
                            }
                        }
                    }
                        
                }

            }

            return ret;
        }


        private static string SetParameterLastInsertedValue(string strSource, string find, long value)
        {
            if (value != 0)
                return strSource.Replace(find, value.ToString());

            return strSource;
        }
	}

}
