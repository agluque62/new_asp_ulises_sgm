using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Configuration;
using System.Data;
//using System.Data.OracleClient;
using log4net;
using log4net.Config;

namespace SincroCD30
{
    public class CD30BD
    {
        //
		private MySql.Data.MySqlClient.MySqlCommand cmd;
		private MySql.Data.MySqlClient.MySqlConnection myConnection;

        private static System.Configuration.Configuration WebConfiguration;
        string CadenaConexion;
        private static ILog _logDebugView;
        
		public static ILog logDebugView
        {
            get
            {
                if (_logDebugView == null)
                {
                    log4net.Config.XmlConfigurator.Configure();
                    _logDebugView = LogManager.GetLogger("SincroCD30");
                }
                return _logDebugView;
            }
        }

        public CD30BD()
        {
            try
            {
                if (WebConfiguration == null)
                    WebConfiguration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                if (CadenaConexion == null && WebConfiguration.ConnectionStrings.ConnectionStrings.Count > 0)
                    CadenaConexion = WebConfiguration.ConnectionStrings.ConnectionStrings["ConexionBaseDatosCD30"].ToString();
                ConexionBD();
            }
            catch (System.Configuration.ConfigurationErrorsException e)
            {
                logDebugView.Error("(constructor):", e);                
            }
        }

        public void Dispose()
        {
            cmd.Dispose();
            myConnection.Close();
            myConnection.Dispose();
        }

		public void CloseDB()
		{
			myConnection.Close();
		}

        private void ConexionBD()
        {
            try
            {
                if (CadenaConexion != null && CadenaConexion.Length>0)
                {                    
					myConnection = new MySql.Data.MySqlClient.MySqlConnection(CadenaConexion);
					cmd = new MySql.Data.MySqlClient.MySqlCommand();
                    myConnection.Open();
                    cmd.Connection = myConnection;
                }
            }
			//catch (OracleException ex) // catches only Oracle errors
			//{
			//    switch (ex.Code)
			//    {
			//        case 12545:
			//            logDebugView.Error("The database is unavailable.", ex);
			//            break;
			//        default:
			//            logDebugView.Error("Database error", ex);
			//            break;
			//    }
			//}
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
            }

		}

		#region Gestion Operadores
		public int AltaOperador(string idOperador, int nivelAcceso, string clave, string nombre, string apellidos, string telefono)
		{
            try
            {
                cmd.CommandText = "INSERT INTO CD30.OpeUsu  (IdUsuario,Perfil,Clave,Nombre,Apellidos,Tfno) VALUES " +
					"('" + idOperador + "'," +
					nivelAcceso + "," +
					"'" + clave + "'," +
					"'" + nombre + "'," +
					"'" + apellidos + "'," +
					"'" + telefono + "'" +
					")";
                //logDebugView.Info(cmd.CommandText);
                cmd.ExecuteNonQuery();
                return 135;
            }
			catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
                return 136;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 136;
            }
		}

		public int ModificaOperador(string idOperador, int nivelAcceso, string clave, string nombre, string apellidos, string telefono)
		{
			try
			{
				cmd.CommandText = "UPDATE CD30.OpeUsu SET " +
					"Perfil='" + nivelAcceso + "'," +
					"Clave='" + clave + "'," +
					"Nombre='" + nombre + "'," +
					"Apellidos='" + apellidos + "'," +
					"tfno='" + telefono + "'" +
					" WHERE IdUsuario='" + idOperador + "'";
				//logDebugView.Info(cmd.CommandText);
				cmd.ExecuteNonQuery();
				return 139;
			}
			catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
			{
				logDebugView.Error("Database error", ex);
				return 140;
			}
			catch (Exception e)
			{
				logDebugView.Error(e.Message);
				return 140;
			}
		}

		public int BajaOperador(string idOperador)
		{
			try
			{
				cmd.CommandText = "DELETE FROM CD30.OpeUsu WHERE IdUsuario='" + idOperador + "'";
				cmd.ExecuteNonQuery();
				return 137;
			}
			catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
			{
				logDebugView.Error("Database error", ex);

				return 138;
			}
			catch (Exception e)
			{
				logDebugView.Error(e.Message);
				return 138;
			}
		}
		#endregion

		#region Gestion Nucleos
		public int AltaNucleo(string id)
        {
            try
            {
                cmd.CommandText = "INSERT INTO CD30.Config (IdConfig) VALUES ('" + id + "')";
                //logDebugView.Info(cmd.CommandText);
                cmd.ExecuteNonQuery();
                return 100;
            }
			catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
                return 101;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 101;
            }
        }
        public int BajaNucleo(string idNucleo)
        {
            try
            {
                cmd.CommandText = "DELETE FROM PanelAgrupacion WHERE NomAgrupacion IN (SELECT DISTINCT(NomAgrupacion) "+
                    "FROM Agrupaciones WHERE IdSacta IN (SELECT DISTINCT(IdSacta) FROM SectorPosicion WHERE RTRIM(IdNucleo)=RTRIM('"+
                    idNucleo+"')))";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM Agrupaciones WHERE NomAgrupacion IN (SELECT DISTINCT(NomAgrupacion) " +
                                    "FROM Agrupaciones WHERE IdSacta IN (SELECT DISTINCT(IdSacta) FROM SectorPosicion WHERE RTRIM(IdNucleo)=RTRIM('" +
                                    idNucleo + "')))";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM SectorPosicion WHERE IdNucleo='" + idNucleo + "'";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM Sectorizaciones WHERE Id='" + idNucleo + "'";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM NumATS WHERE IdConfig='" + idNucleo+"'";                
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM NumPub WHERE IdConfig='" + idNucleo + "'";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM ColateralTF WHERE IdConfig='" + idNucleo + "'";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM ColateralRD WHERE IdConfig='" + idNucleo + "'";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM Panel WHERE IdConfig='" + idNucleo + "'";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM Permisos WHERE IdConfig='" + idNucleo + "'";
                cmd.ExecuteNonQuery();
                
                //Eliminar los Destinos Internos
                cmd.CommandText = "SELECT IdUsuario FROM Usuarios WHERE IdConfig='" + idNucleo + "'";

				MySql.Data.MySqlClient.MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        cmd.CommandText = "DELETE FROM DESTINOS WHERE RTRIM(Destino)='"+dr.GetString(0)+"' AND PrefijoRed IN (1,7)";
                        cmd.ExecuteNonQuery();
                    }
                }
                dr.Close();

                cmd.CommandText = "DELETE FROM Usuarios WHERE IdConfig='" + idNucleo + "'";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM CONFIG WHERE IdConfig='" + idNucleo + "'";
                cmd.ExecuteNonQuery();

                return 102;
            }
			catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
				
                return 103;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 103;
            }
        }
        #endregion

        #region Gestion Emplazamientos
        public int AltaEmplazamiento(string id)
        {
            try
            {
                StringBuilder s = new StringBuilder(id);
                while (s.Length < 8)
                    s.Append(" ");
                    
                cmd.CommandText = "INSERT INTO Emplazamientos (Emplazamiento) VALUES ('" + s.ToString() + "')";
                cmd.ExecuteNonQuery();
                return 108;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
				
                return 109;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 109;
            }
        }

        public int BajaEmplazamiento(string id)
        {//Se supone que el emplazamiento no esta asignado a ninguna frecuencia
            try
            {
                cmd.CommandText = "DELETE FROM FRECUENCIAS WHERE RTRIM(Emplazamiento)=RTRIM('"+id+"')";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM EMPLAZAMIENTOS WHERE RTRIM(Emplazamiento)=RTRIM('" + id + "')";
                cmd.ExecuteNonQuery();
                return 127;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
                return 128;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 128;
            }
        }
        #endregion

        #region Gestion Sectorizaciones
        public int AltaSectorizacion(string id)
        {
            try
            {
                cmd.CommandText = "INSERT INTO CD30.Sectores (Id) VALUES ('" + id + "')";
                cmd.ExecuteNonQuery();
                return 104;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
				
                return 105;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
            }
            return 105;
        }

        public int BajaSectorizacion(string id)
        {
            try
            {
                cmd.CommandText = "DELETE FROM CD30.Sectores WHERE Id='" + id + "'";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM CD30.Sectorizaciones WHERE Id='" + id + "'";
                cmd.ExecuteNonQuery();
                return 106;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);

                return 107;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 107;
            }
            
        }
        #endregion

        #region Gestion Agrupaciones
        public int AltaAgrupacion(string idAgrupacion, List<string> sectores)
        {
            try
            {
                BajaAgrupacion(idAgrupacion);
                foreach (string sector in sectores)
                {
                    cmd.CommandText = "SELECT IdSacta FROM SectorPosicion WHERE NomSector='"+sector+"'";
                    MySql.Data.MySqlClient.MySqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        int idsacta = dr.GetInt32(0);
                        cmd.CommandText = "INSERT INTO Agrupaciones (NomAgrupacion,IdSacta) VALUES ('"+idAgrupacion+"'," 
                                    + idsacta.ToString() + ")";
                        cmd.ExecuteNonQuery();
                    }
                    dr.Close();
                }
                return 110;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
                return 111;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 111;
            }
        }

        public int BajaAgrupacion(string idAgrupacion)
        {
            try
            {
                cmd.CommandText = "DELETE FROM CD30.Agrupaciones WHERE NomAgrupacion='" + idAgrupacion + "'";
                cmd.ExecuteNonQuery();
                return 112;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
                return 113;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 113;
            }
        }
        #endregion
        
        #region Gestion Asignacion Sectores
        public int AsignarSectoresUCS(string idSectorizacion,List<string> sectores,int posDest)
        {            
            try
            {
                //Comprobar que existen los sectores que se quieren asignar
                StringBuilder cadenaSectores = new StringBuilder();
                foreach (string sector in sectores)
                {
                    if (cadenaSectores.Length > 0)  cadenaSectores.Append(",");

					cadenaSectores.Append("'" + sector + "'");
                }
                cmd.CommandText = "SELECT COUNT(*) FROM SECTORPOSICION WHERE IdNucleo!='ACTIVA' AND "+
                                    "NomSector IN ("+cadenaSectores.ToString()+")";
                if (sectores.Count != Int32.Parse(cmd.ExecuteScalar().ToString()))
                    return 125;
                
                //Liberar la posicion
                LiberarUCS(idSectorizacion, posDest);
                //Comprobar que los sectores no estan asignados en otras posiciones
                StringBuilder cadenaNumUsuarios = new StringBuilder();
                StringBuilder cadenaIdSacta = new StringBuilder();
                List<int> listNumUsuarios = new List<int>();

                foreach (string sector in sectores)
                {
                    cmd.CommandText = "SELECT NumUsuario,IdSacta FROM SectorPosicion WHERE IdNucleo!='ACTIVA' AND NomSector='" + sector + "'";
                    MySql.Data.MySqlClient.MySqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)                        
                    {
                        dr.Read();

                        if (cadenaNumUsuarios.Length > 0)
                        {
                            cadenaNumUsuarios.Append(",");
                            cadenaIdSacta.Append(",");
                        }
                        listNumUsuarios.Add(dr.GetInt32(0));
                        cadenaNumUsuarios.Append("'" + dr.GetInt32(0).ToString() + "'");
                        cadenaIdSacta.Append(dr.GetInt32(1).ToString());
                    }
                    dr.Close();
                }

                cmd.CommandText = "SELECT COUNT(*) FROM SECTORIZACIONES WHERE ID='"+idSectorizacion+"' AND NumUsuario IN ("
                                  + cadenaNumUsuarios + ")";
                if (Int32.Parse(cmd.ExecuteScalar().ToString())>0)
                    return 126;

                //Comprobar si los sectores conforman una agrupacion                
                cmd.CommandText = "SELECT a.NomAgrupacion FROM "+
                        "(SELECT NomAgrupacion, COUNT(*) as NumSectoresA FROM CD30.Agrupaciones GROUP BY NomAgrupacion) a," +
                        "(SELECT NomAgrupacion, COUNT(*) as NumSectoresB FROM CD30.Agrupaciones WHERE IdSacta IN ("+
                            cadenaIdSacta.ToString()+") GROUP BY NomAgrupacion) b WHERE NumSectoresA=NumSectoresB AND "+
                            "NumSectoresA="+ sectores.Count +" AND a.NomAgrupacion=b.NomAgrupacion";
                MySql.Data.MySqlClient.MySqlDataReader dr1 = cmd.ExecuteReader();
                string nomPosicion="";
                if (dr1.HasRows)
                {
                    dr1.Read();
                    nomPosicion = dr1.GetString(0);
                }
                dr1.Close();
                
                if (nomPosicion=="") //Obtener el nombre final de la posicion si no forman una agrupacion
                    nomPosicion = GetNomPosicion(sectores);
                
                //Actualizar la tabla Sectorizaciones
                //Obtener la ope asignada a esa posicion
                cmd.CommandText = "SELECT NumOPE FROM UCS WHERE TIPO='E' AND NUMOPE!=255 AND NUMUCS=" + posDest.ToString();
                int ope = Int32.Parse(cmd.ExecuteScalar().ToString());
                for (int i = 0; i < sectores.Count;i++ )
                {
                    string nucleo = "";
                    cmd.CommandText = "SELECT IdNucleo FROM SectorPosicion WHERE IdNucleo!='ACTIVA' AND NumUsuario=" +
                                listNumUsuarios[i].ToString() + " AND NomSector='" + sectores[i] + "'";
                    MySql.Data.MySqlClient.MySqlDataReader dr2 = cmd.ExecuteReader();
                    if (dr2.HasRows)
                    {
                        dr2.Read();
                        nucleo = dr2.GetString(0);
                    }
                    dr2.Close();
					//cmd.CommandText = "INSERT INTO SECTORIZACIONES VALUES ('" + idSectorizacion + "','" + nucleo + "'," +
					//                listNumUsuarios[i].ToString() + ",'" + nomPosicion + "','" + nomPosicion + "','" + nomPosicion + "'," +
					//                listNumUsuarios[0].ToString() + "," + ope.ToString() + ",'',ORDEN.NEXTVAL)";
					cmd.CommandText = "INSERT INTO SECTORIZACIONES (id,idnucleo,numusuario,idusuario,nombreejecutivo,nombreplanificador,usuariodominante,numope,especial) VALUES ('" + idSectorizacion + "','" + nucleo + "'," +
									listNumUsuarios[i].ToString() + ",'" + nomPosicion + "','" + nomPosicion + "','" + nomPosicion + "'," +
									listNumUsuarios[0].ToString() + "," + ope.ToString() + ",'')";
					cmd.ExecuteNonQuery();
                }
                return 123;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
                return 124;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 124;
            }
        }

        private string GetNomPosicion(List<string> sectores)
        {
            StringBuilder nombre = new StringBuilder();
            int numCaracteresPorSector = (int) (8 / sectores.Count);
            foreach (string s in sectores)
                nombre.Append(s.Substring(0, System.Math.Min(s.Length, numCaracteresPorSector)));

            return nombre.ToString();
        }

        public int IntercambiarUCS(string idSectorizacion, List<string> sectoresOrigen,int posOrigen, List<string> sectoresDestino,int posDestino)
        {
            try
            {
                if (LiberarUCS(idSectorizacion, posOrigen) != 119)
                    return 122;
                if (LiberarUCS(idSectorizacion, posDestino) != 119)
                    return 122;

                if (sectoresOrigen.Count>0)
                    if (AsignarSectoresUCS(idSectorizacion, sectoresOrigen, posDestino) != 123)
                        return 122;
                if (sectoresDestino.Count>0)
                    if (AsignarSectoresUCS(idSectorizacion, sectoresDestino, posOrigen) != 123)
                        return 122;
                return 121;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
                return 122;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 122;
            }
        }

        public int LiberarUCS(string idSectorizacion, int pos)
        {             
            try
            {   //Obtener la ope asignada a esa posicion
                cmd.CommandText = "SELECT NumOPE FROM UCS WHERE TIPO='E' AND NUMOPE!=255 AND NUMUCS="+pos.ToString();
                int ope = Int32.Parse( cmd.ExecuteScalar().ToString());

                //Liberar la UCS
                cmd.CommandText = "DELETE FROM SECTORIZACIONES WHERE NumOPE=" + ope.ToString() + " AND ID='" + idSectorizacion + "'";
                cmd.ExecuteNonQuery();
                return 119;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
                return 120;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 120;
            }
        }

        public void LiberarSectorUCS(string idSectorizacion, string sector)
        {

        }

        public int MoverSectores(string idSectorizacion, List<string> sectoresOrigen, int posOrigen, List<string> sectoresDestino, int posDest)
        {
            try
            {
                if (LiberarUCS(idSectorizacion, posOrigen) == 120)
                    return 128;
                if (LiberarUCS(idSectorizacion, posDest) == 120)
                    return 128;
                if (sectoresOrigen.Count>0)
                    if (AsignarSectoresUCS(idSectorizacion, sectoresOrigen, posOrigen) != 123)
                        return 128;
                if (sectoresDestino.Count > 0)
                    if (AsignarSectoresUCS(idSectorizacion, sectoresDestino, posDest) != 123)
                        return 128;
                 
                return 127;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 128;
            }
        }
        #endregion

        #region Gestion Sectores
        public int AltaUsuario(string idNucleo, string idSector,string tipoSector, int numSacta, int prioR2, List<string> numPub,List<string> numATS)
        {
			MySql.Data.MySqlClient.MySqlTransaction mytrans = myConnection.BeginTransaction(IsolationLevel.ReadCommitted);
            cmd.Transaction = mytrans;
            try
            {
                //Necesito un numero de Usuario, en el CD30 puede haber maximo 51 usuarios
                int numUsu = 0;
                for (int i = 1; i <= 51; i++)
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM USUARIOS WHERE IdConfig!='ACTIVA' AND NumUsuario="+i.ToString();
                    if (0 == Int32.Parse(cmd.ExecuteScalar().ToString()))
                    {
                        numUsu = i;
                        break;
                    }
                }

                if (numUsu == 0)
                {
                    mytrans.Rollback();
                    mytrans.Dispose();
                    return 116;
                }
                cmd.CommandText = "DELETE FROM NumATS WHERE IdConfig='" + idNucleo + "' AND NumUsuario="+numUsu.ToString();
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM NumPub WHERE IdConfig='" + idNucleo + "' AND NumUsuario=" + numUsu.ToString();
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM Permisos WHERE IdConfig='" + idNucleo + "' AND NumUsuario=" + numUsu.ToString();
                cmd.ExecuteNonQuery();

                StringBuilder usuario = new StringBuilder((idSector.Length <= 8) ? idSector : idSector.Substring(0, 8));
                while (usuario.Length < 8)
                    usuario.Append(" ");

                cmd.CommandText = "INSERT INTO USUARIOS VALUES('"+idNucleo+"',"+numUsu.ToString()+",'"+usuario.ToString()+"','"+
                                  usuario.ToString()+"',"+prioR2.ToString()+",0,0)";
                cmd.ExecuteNonQuery();
                string elSector = (idSector.Length <= 8) ? idSector : idSector.Substring(0, 8);
                cmd.CommandText = "INSERT INTO DESTINOS VALUES ('" + elSector + "',1,'" + elSector + "',0)";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO DESTINOS VALUES ('" + elSector + "',7,'" + elSector + "',0)";
                cmd.ExecuteNonQuery();

                foreach (string s in numATS)
                {
                    cmd.CommandText = "INSERT INTO NUMATS VALUES('"+idNucleo+"',"+numUsu.ToString()+",'"+s+"')";
                    cmd.ExecuteNonQuery();
                }

                foreach (string s in numPub)
                {
                    cmd.CommandText = "INSERT INTO NUMPUB (IdConfig, NumUsuario, NumeroPublico) VALUES ('" + idNucleo + "', " + numUsu.ToString() + ", '" + s + "')";
                    cmd.ExecuteNonQuery();
                }

                cmd.CommandText = "INSERT INTO PERMISOS VALUES ('"+idNucleo+"',"+numUsu.ToString()+",0,0)";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO SectorPosicion VALUES ('" + idNucleo + "'," + numUsu.ToString() + ",'" + elSector +
                    "','"+tipoSector+"',"+numSacta.ToString()+",'E',0,0)";
                cmd.ExecuteNonQuery();

                //Darle un TFT al usuario
                cmd.CommandText = "INSERT INTO Panel VALUES ('"+idNucleo+"',"+numUsu.ToString()+",'F',7,0,0,0,0)";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "SELECT DISTINCT(Frecuencia),Emplazamiento,Posicion,Altavoz,Prioritario FROM ColateralRD" +
                    " WHERE IdPanel=7 AND RTRIM(IdConfig)<>RTRIM('ACTIVA')";
                MySql.Data.MySqlClient.MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        cmd.CommandText = "INSERT INTO ColateralRD VALUES ('"+idNucleo+"',"+numUsu.ToString()+",7,"+dr.GetInt32(2)+
                            ",'" + dr.GetString(0) + "','" + dr.GetString(0) + "','" + dr.GetString(1) + "'," + dr.GetInt32(3) + "," + dr.GetInt32(4)+")";
                        cmd.ExecuteNonQuery();
                    }
                }

				dr.Close();
                mytrans.Commit();
                return 114;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);

				mytrans.Rollback();
                mytrans.Dispose();
                return 115;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                mytrans.Rollback();
                mytrans.Dispose();
                return 115;
            }
        }

        public int ModificacionUsuario(string idNucleo, string idSector, string tipoSector, int numSacta, int prioR2, List<string> numPub, List<string> numATS)
        {
            MySql.Data.MySqlClient.MySqlTransaction mytrans = myConnection.BeginTransaction(IsolationLevel.ReadCommitted);
            cmd.Transaction = mytrans;
            try
            {
                //Obtener el numero de usuario
                cmd.CommandText = "SELECT NumUsuario FROM USUARIOS WHERE IdConfig='"+idNucleo+"' AND RTRIM(IdUsuario)=RTRIM('"+idSector+"')";
                int numUsu= Int32.Parse(cmd.ExecuteScalar().ToString());

                cmd.CommandText = "DELETE FROM NumATS WHERE IdConfig='" + idNucleo + "' AND NumUsuario=" + numUsu.ToString();
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM NumPub WHERE IdConfig='" + idNucleo + "' AND NumUsuario=" + numUsu.ToString();
                cmd.ExecuteNonQuery();

                cmd.CommandText = "UPDATE Usuarios SET PrioridadR2=" + prioR2.ToString() +
                                  "WHERE IdConfig='" + idNucleo + "' AND NumUsuario=" + numUsu.ToString() + 
                                  " AND RTRIM(IdUsuario)=RTRIM('" + idSector + "')";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "UPDATE SectorPosicion SET IdSacta=" + numSacta.ToString() +",Tipo='"+tipoSector+"'"+
                                  "WHERE IdNucleo='" + idNucleo + "' AND NumUsuario=" + numUsu.ToString() +
                                  " AND RTRIM(NomSector)=RTRIM('" + idSector + "')";
                cmd.ExecuteNonQuery();

                foreach (string s in numATS)
                {
                    cmd.CommandText = "INSERT INTO NUMATS VALUES('" + idNucleo + "'," + numUsu.ToString() + ",'" + s + "')";
                    cmd.ExecuteNonQuery();
                }

                foreach (string s in numPub)
                {
                    cmd.CommandText = "INSERT INTO NUMPUB (IdConfig, NumUsuario, NumeroPublico) VALUES ('" + idNucleo + "', " + numUsu.ToString() + ", '" + s + "')";
                    cmd.ExecuteNonQuery();
                }
                
                mytrans.Commit();
                return 127;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
                mytrans.Rollback();
                mytrans.Dispose();
                return 128;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                mytrans.Rollback();
                mytrans.Dispose();
                return 128;
            }
        }
        
        public int BajaUsuario(string idNucleo, string idSector)
        {
            try
            {
                //Obtener el numero de usuario
                int numUsu = 0;
                cmd.CommandText = "SELECT NumUsuario FROM Usuarios WHERE IdConfig='"+idNucleo+"' AND RTRIM(IdUsuario)=RTRIM('"+idSector+"')";
                MySql.Data.MySqlClient.MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    numUsu = dr.GetInt32(0);
                }
                dr.Close();

                cmd.CommandText = "DELETE FROM PanelAgrupacion WHERE NomAgrupacion IN (SELECT DISTINCT(NomAgrupacion) " +
                    "FROM Agrupaciones WHERE IdSacta IN (SELECT DISTINCT(IdSacta) FROM SectorPosicion WHERE RTRIM(IdNucleo)=RTRIM('" +
                    idNucleo + "') AND NumUsuario="+numUsu.ToString()+"))";
                cmd.ExecuteNonQuery();
				//cmd.CommandText = "DELETE FROM Agrupaciones WHERE NomAgrupacion IN (SELECT DISTINCT(NomAgrupacion) " +
				//                    "FROM Agrupaciones WHERE IdSacta IN (SELECT DISTINCT(IdSacta) FROM SectorPosicion WHERE RTRIM(IdNucleo)=RTRIM('" +
				//                    idNucleo + "') AND NumUsuario=" + numUsu.ToString() + "))";
				cmd.CommandText = "DELETE FROM Agrupaciones WHERE IdSacta IN (SELECT DISTINCT(IdSacta) FROM SectorPosicion WHERE RTRIM(IdNucleo)=RTRIM('" +
					idNucleo + "') AND NumUsuario=" + numUsu.ToString() + ")";
				cmd.ExecuteNonQuery();

				//cmd.CommandText = "DELETE FROM SECTORIZACIONES WHERE IDUSUARIO  IN (" +
				//                  "SELECT IDUSUARIO FROM SECTORIZACIONES WHERE ID!='ACTIVA' AND NUMUSUARIO=" + numUsu.ToString()
				//                + "AND IDNUCLEO='" + idNucleo + "') AND ID!='ACTIVA' AND IDNUCLEO='" + idNucleo + "'";
				cmd.CommandText = "DELETE FROM SECTORIZACIONES WHERE ID!='ACTIVA' AND NUMUSUARIO=" + numUsu.ToString()
				+ " AND IDNUCLEO='" + idNucleo + "' AND ID!='ACTIVA' AND IDNUCLEO='" + idNucleo + "'";
				cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM SectorPosicion WHERE IdNucleo='" + idNucleo + "' AND RTRIM(NomSector)=RTRIM('"+idSector+"')";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM NumATS WHERE IdConfig='" + idNucleo + "' AND NumUsuario=" + numUsu.ToString();
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM NumPub WHERE IdConfig='" + idNucleo + "' AND NumUsuario=" + numUsu.ToString();
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM ColateralTF WHERE IdConfig='" + idNucleo + "' AND NumUsuario=" + numUsu.ToString();
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM ColateralRD WHERE IdConfig='" + idNucleo + "' AND NumUsuario=" + numUsu.ToString();
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM Panel WHERE IdConfig='" + idNucleo + "' AND NumUsuario=" + numUsu.ToString();
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM Permisos WHERE IdConfig='" + idNucleo + "' AND NumUsuario=" + numUsu.ToString();
                cmd.ExecuteNonQuery();
                //Eliminar los Destinos Internos
                cmd.CommandText = "DELETE FROM DESTINOS WHERE RTRIM(Destino)=RTRIM('" + idSector + "') AND PrefijoRed IN (1,7)";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM Usuarios WHERE IdConfig='" + idNucleo + "' AND RTRIM(IdUsuario)=RTRIM('"+idSector+"')";
                cmd.ExecuteNonQuery();

                return 117;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
                return 118;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 118;
            }
        
        }
        #endregion

        #region Gestion Troncales
        public int AltaTroncal(string id, string numTest)
        {
            try
            {
                //Necesito un numero de troncal
                int numTroncal = 0;
                for (int i = 1; i <= 128; i++)
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM TroncalTlf WHERE NumTroncal=" + i.ToString();
                    if (0 == Int32.Parse(cmd.ExecuteScalar().ToString()))
                    {
                        numTroncal = i;
                        break;
                    }
                }
                if (numTroncal == 0)
                    return 131;

                cmd.CommandText = "INSERT INTO TroncalTlf VALUES ('1'," + numTroncal.ToString()+ ",'"+id+"','"+numTest+"')";
                cmd.ExecuteNonQuery();
                return 129;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
                return 130;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 130;
            }        
        }

        public int BajaTroncal(string id)
        {
            try
            {
                cmd.CommandText = "SELECT NumTroncal FROM TroncalTlf WHERE Identificador='" + id+"'";
                int numTroncal = Int32.Parse(cmd.ExecuteScalar().ToString());
                if (numTroncal > 0)
                {
                    cmd.CommandText = "DELETE FROM TroncalTlf WHERE Identificador='" + id + "' AND NumTroncal=" + numTroncal.ToString();
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "DELETE FROM LinTroncal WHERE NumTroncal=" + numTroncal.ToString();
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "DELETE FROM Rutas WHERE NumTroncal=" + numTroncal.ToString() + " AND Directa>='0'";
                    cmd.ExecuteNonQuery();
                }
                else
                    return 128;
                return 127;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
                return 128;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 128;
            }        
        }

        public int ModificacionTroncal(string id, string numTest)
        {
            try
            {
                cmd.CommandText = "UPDATE TroncalTlf SET NumTest=" + numTest.ToString() + " WHERE Identificador='" + id + "'";
                cmd.ExecuteNonQuery();
                return 127;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
                return 128;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 128;
            }
        }
        #endregion

        #region Gestion Redes
        public int AltaRed(int prefijo, string id)
        {
            try
            {
                if ((prefijo != 3) && (prefijo != 4) && (prefijo != 6) && (prefijo != 8) && (prefijo != 9))
                    return 134;
                cmd.CommandText = "INSERT INTO Red VALUES ('"+prefijo.ToString()+"','" + id + "',0,'',0,'N')";
                cmd.ExecuteNonQuery();
                return 132;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
                return 133;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 133;
            }                
        }

        public int BajaRed(int prefijo, string id)
        {
            try
            {                
                cmd.CommandText = "DELETE FROM CentralR2Opes WHERE PrefijoRed=" + prefijo.ToString();
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM CentralR2Priv WHERE PrefijoRed=" + prefijo.ToString();
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM Red WHERE Prefijo=" + prefijo.ToString();
                cmd.ExecuteNonQuery();

                if (prefijo == 3)//ATS
                {
                    cmd.CommandText = "DELETE FROM DestinosR2";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "DELETE FROM Rutas";
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd.CommandText = "DELETE FROM ParametrosDestino WHERE PrefijoRed=" + prefijo.ToString();
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "DELETE FROM Destinos WHERE PrefijoRed=" + prefijo.ToString();
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "DELETE FROM DestinosSectorizados WHERE PrefijoRed=" + prefijo.ToString();
                    cmd.ExecuteNonQuery();
                }

                return 127;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
                return 128;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 128;
            }
        }
        #endregion

        #region Gestion Recursos Radio
        public int AltaCanalRadio(string id, int exclusividad, char modo, char tipoPTT, char tipoSquelch,int maxPTT)
        {
            try
            {
                int numRec = 0;
                for (int i = 1; i <= 128; i++)
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM CanRad WHERE IdRecurso=" + i.ToString();
                    if (0 == Int32.Parse(cmd.ExecuteScalar().ToString()))
                    {
                        numRec = i;
                        break;
                    }
                }
                if (numRec == 0)
                    return 128;

                cmd.CommandText = "INSERT INTO CanRad VALUES ("+numRec.ToString()+",1,'" + id + "','" + exclusividad +
                                  "','"+modo+"','"+tipoPTT.ToString()+"','"+tipoSquelch.ToString()+"',"+maxPTT.ToString()+")";
                cmd.ExecuteNonQuery();
                return 127;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
                return 128;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 128;
            }        
        }

        public int ModificacionCanalRadio(string id, int exclusividad, char modo, char tipoPTT, char tipoSquelch, int maxPTT)
        {
            try
            {
                int numRec = 0;
                cmd.CommandText = "SELECT IdRecurso FROM CanRad WHERE Identificador='" + id + "'";
                numRec = Int32.Parse(cmd.ExecuteScalar().ToString());  
                if (numRec == 0)
                    return 128;

                cmd.CommandText = "UPDATE CanRad SET Exclusividad='" + exclusividad + "',Modo='" + modo + "',TipoPTT='" + tipoPTT.ToString() + "',tipoSquelch='" + tipoSquelch.ToString() + "',maxPTT=" + maxPTT.ToString() + " WHERE IdRecurso=" + numRec.ToString(); ;
                cmd.ExecuteNonQuery();
                return 127;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
                return 128;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 128;
            }
        }

        public int BajaCanalRadio(string id)
        {
            try
            {
                int numRec = 0;
                cmd.CommandText = "SELECT IdRecurso FROM CanRad WHERE Identificador='" + id+"'";
                numRec = Int32.Parse(cmd.ExecuteScalar().ToString());                
                if (numRec == 0)
                    return 128;

                cmd.CommandText = "DELETE FROM CanRad WHERE IdRecurso="+numRec.ToString();
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM HwRecurso WHERE IdRecurso=" + numRec.ToString() + " AND TipoRecurso=1";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM Frecuencias WHERE IdRecurso=" + numRec.ToString();
                cmd.ExecuteNonQuery();
                return 127;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
                return 128;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 128;
            }
        }

        #endregion

        #region Gestion Frecuencias
        public int AltaFrecuencia(string emplazamiento, string idFrecuencia, int GrupoBSS, string idCanalRad)
        {
            try
            {                
                cmd.CommandText = "SELECT IdRecurso FROM CanRad WHERE Identificador='" + idCanalRad+"'";
                                
                int numRec = 0;                                
                numRec = Int32.Parse(cmd.ExecuteScalar().ToString());
                if (numRec == 0)
                    return 128;
                                
                StringBuilder s = new StringBuilder(idFrecuencia);
                while (s.Length < 8)
                    s.Append(" ");

                StringBuilder s1 = new StringBuilder(emplazamiento);
                while (s1.Length < 8)
                    s1.Append(" ");

                cmd.CommandText = "INSERT INTO Frecuencias VALUES ('" + s1.ToString() + "','" + s.ToString() + "'," +
                                    numRec.ToString() + "," + GrupoBSS.ToString() + ")";
                cmd.ExecuteNonQuery();
                return 127;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
                return 128;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 128;
            }        
        }

        public int ModificacionFrecuencia(string idFrecuencia, string idEmplazamiento, string idCanal, int GrupoBSS)
        {
            try
            {
                cmd.CommandText = "SELECT IdRecurso FROM CanRad WHERE Identificador='" + idCanal + "'";

                int numRec = 0;
                numRec = Int32.Parse(cmd.ExecuteScalar().ToString());
                if (numRec == 0)
                    return 128;
                
                StringBuilder s = new StringBuilder(idFrecuencia);
                while (s.Length < 8)
                    s.Append(" ");

                StringBuilder s1 = new StringBuilder(idEmplazamiento);
                while (s1.Length < 8)
                    s1.Append(" ");

                cmd.CommandText = "DELETE FROM Frecuencias WHERE IdRecurso=" + numRec.ToString();
                cmd.ExecuteNonQuery();

                return AltaFrecuencia(s1.ToString(), s.ToString(), GrupoBSS, idCanal);
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
                return 128;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 128;
            }
        }

        public int BajaFrecuencia(string idFrecuencia, bool borrarPaneles)
        {
            try
            {
                if (borrarPaneles)
                {
                    cmd.CommandText = "DELETE FROM COLATERALRD WHERE RTRIM(Frecuencia)=RTRIM('"+idFrecuencia+"')";
                    cmd.ExecuteNonQuery();
                }
                cmd.CommandText = "DELETE FROM Frecuencias WHERE RTRIM(Frecuencia)=RTRIM('"+idFrecuencia+"')";
                cmd.ExecuteNonQuery();
                return 127;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
                return 128;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 128;
            }
        }
        #endregion

        #region Gestion Telefonia
        public int AltaLineaTelefonia(int tipoRecurso,string id,int tipo, int tipoLinTroncal,int lado,int acceso, string idTroncal)
        {
            try
            {
                int numLin = 0;
                for (int i = 1; i <= 128; i++)
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM LinTel WHERE IdRecurso=" + i.ToString();
                    if (0 == Int32.Parse(cmd.ExecuteScalar().ToString()))
                    {
                        numLin = i;
                        break;
                    }
                }
                if (numLin == 0)
                    return 128;

                cmd.CommandText = "INSERT INTO LinTel VALUES ("+numLin.ToString()+",'"+tipo.ToString()+"','"+id+"','N','"+lado.ToString()
                    +"','"+acceso.ToString()+"','','',0)";
                cmd.ExecuteNonQuery();

                int numTroncal = 0;
                if ((tipo != 6) && (tipo != 12)) //no es R2 ni QSIG => Troncal nuevo
                {
                    for (int i = 1; i <= 128; i++)
                    {
                        cmd.CommandText = "SELECT COUNT(*) FROM TroncalTlf WHERE NumTroncal=" + i.ToString();
                        if (0 == Int32.Parse(cmd.ExecuteScalar().ToString()))
                        {
                            numTroncal = i;
                            break;
                        }
                    }
                    if (numTroncal == 0)
                        return 128;

                    cmd.CommandText = "INSERT INTO TroncalTlf VALUES ('" + tipoRecurso.ToString() + "'," + numTroncal.ToString() + ",'" + id + "','')";
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd.CommandText = "SELECT NumTroncal FROM TroncalTlf WHERE Identificador='" + idTroncal +"'";
                    numTroncal = Int32.Parse(cmd.ExecuteScalar().ToString());
                }

                if (numTroncal > 0)
                {
                    cmd.CommandText = "INSERT INTO LinTroncal VALUES (" + numLin.ToString() + "," + numTroncal.ToString() + "," + tipoLinTroncal.ToString() + ")";
                    cmd.ExecuteNonQuery();
                }

                if (((tipo==10)||(tipo==11)||(tipoLinTroncal==4)) && (idTroncal!=""))
                {//asignar el troncal a la red correspondiente
                    cmd.CommandText = "SELECT Prefijo FROM Red WHERE Literal='" + idTroncal + "'";
                    int prefijo = Int32.Parse(cmd.ExecuteScalar().ToString());

                    cmd.CommandText = "INSERT INTO RED VALUES ('" + prefijo.ToString() + "','" + idTroncal + "'," + numTroncal + ",'',0,'N')";
                    cmd.ExecuteNonQuery();
                }
                return 127;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
				return 128;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 128;
            }
        }

        public int ModificacionLineaTelefonia(int tipoRecurso, string id, int tipo, int tipoLinTroncal, int lado, int acceso, string idTroncal)
        {
            try
            {
                int numLin = 0;
                cmd.CommandText = "SELECT IdRecurso FROM LinTel WHERE Identificador='" + id + "'";
                numLin = Int32.Parse(cmd.ExecuteScalar().ToString());
                   
                if (numLin == 0)
                    return 128;

                cmd.CommandText = "UPDATE LinTel SET Lado='" + lado.ToString() + "', Acceso='" +
                                 acceso.ToString() + "' WHERE IdRecurso=" + numLin.ToString();
                cmd.ExecuteNonQuery();

                int numTroncal = 0;
                if ((tipo != 6) && (tipo != 12)) //no es R2 ni QSIG => Troncal nuevo
                {
                    cmd.CommandText = "SELECT NumTroncal FROM TroncalTlf WHERE Identificador='" + id +"'";
                    numTroncal = Int32.Parse(cmd.ExecuteScalar().ToString());

                    if (numTroncal == 0)
                        return 128;
                }
                else
                {
                    cmd.CommandText = "SELECT NumTroncal FROM TroncalTlf WHERE Identificador='" + idTroncal + "'";
                    numTroncal = Int32.Parse(cmd.ExecuteScalar().ToString());
                }

                if (numTroncal > 0)
                {
                    cmd.CommandText = "DELETE FROM LinTroncal WHERE IdRecurso='"+numLin.ToString()+"'";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "INSERT INTO LinTroncal VALUES (" + numLin.ToString() + "," + numTroncal.ToString() + "," + tipoLinTroncal.ToString() + ")";
                    cmd.ExecuteNonQuery();
                }

                if (((tipo == 10) || (tipo == 11) || (tipoLinTroncal == 4)) && (idTroncal != ""))
                {//asignar el troncal a la red correspondiente
                    cmd.CommandText = "SELECT Prefijo FROM Red WHERE Literal='" + idTroncal + "'";
                    int prefijo = Int32.Parse(cmd.ExecuteScalar().ToString());

                    cmd.CommandText = "DELETE FROM RED WHERE NumTroncal="+numTroncal.ToString();
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "INSERT INTO RED VALUES ('" + prefijo.ToString() + "','" + idTroncal + "'," + numTroncal + ",'',0,'N')";
                    cmd.ExecuteNonQuery();
                }
                return 127;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
				
                return 128;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 128;
            }
        }

        public int BajaLineaTelefonia(string id, int tipo, int tipoLinTroncal, string idTroncal)
        {
            try
            {
                int numLin = 0;
                cmd.CommandText = "SELECT IdRecurso FROM LinTel WHERE Identificador='" + id + "'";
                numLin = Int32.Parse(cmd.ExecuteScalar().ToString());
            
                if (numLin == 0)
                    return 128;
                
                int numTroncal = 0;
                cmd.CommandText = "SELECT NumTroncal FROM TroncalTlf WHERE Identificador='" + idTroncal + "'";
                numTroncal = Int32.Parse(cmd.ExecuteScalar().ToString());

                if (numTroncal == 0)
                    return 128;

                if ((tipo != 6) && (tipo != 12)) //no es R2 ni QSIG => Troncal nuevo
                {   
                    cmd.CommandText = "DELETE FROM TroncalTlf WHERE NumTroncal=" + numTroncal.ToString();
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "DELETE FROM Destinos WHERE Numero='" + numTroncal.ToString()+"'";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "DELETE FROM DestinosSectorizados WHERE Numero='" + numTroncal.ToString()+"'";
                    cmd.ExecuteNonQuery();
                }               

                cmd.CommandText = "DELETE FROM LinTroncal WHERE NumTroncal=" + numTroncal.ToString();
                cmd.ExecuteNonQuery();

                if (((tipo == 10) || (tipo == 11) || (tipoLinTroncal == 4)) && (idTroncal != ""))
                {//quitar el troncal de la red correspondiente
                    cmd.CommandText = "DELETE FROM RED WHERE NumTroncal=" +numTroncal.ToString();
                    cmd.ExecuteNonQuery();
                }

                cmd.CommandText = "DELETE FROM HwRecurso WHERE IdRecurso=" + numLin.ToString() + " AND TipoRecurso<>1";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM LinTel WHERE IdRecurso=" + numLin.ToString();
                cmd.ExecuteNonQuery();
                                
                return 127;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
				return 128;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 128;
            }
        }
        #endregion

        #region Gestion Destinos
        public int AltaDestino(string idDestino, int prefijoRed, string idRecurso, int grupo,string numero)
        {
            try
            {
                //obtener el numero de troncal del recurso
                int numTroncal=0;
                if ((prefijoRed != 3) && (prefijoRed != 8) && (prefijoRed != 9) && (prefijoRed != 4)/*RTB*/&& (prefijoRed != 6)/*PABX*/)
                {
                    cmd.CommandText = "SELECT NumTroncal FROM TroncalTlf WHERE Identificador='" + idRecurso + "'";
                    numTroncal = Int32.Parse(cmd.ExecuteScalar().ToString());
                }
               
                StringBuilder s = new StringBuilder(idDestino);
                while (s.Length < 8)
                    s.Append(" ");
                idDestino = s.ToString();
                
                cmd.CommandText = "INSERT INTO Destinos VALUES ('"+idDestino+"',"+prefijoRed.ToString()+",'"+
                    ((numero.Length >0) ? numero: numTroncal.ToString() ) + "'," + grupo.ToString() + ")";
                cmd.ExecuteNonQuery();
                
                cmd.CommandText = "INSERT INTO DestinosSectorizados VALUES ('" + idDestino + "'," +
                                prefijoRed.ToString() + ",'" + ((numero.Length > 0) ? numero : numTroncal.ToString()) + "',0)";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO ParametrosDestino VALUES ('" + idDestino + "'," + 
                                prefijoRed.ToString() + ",0)";
                cmd.ExecuteNonQuery();
                
                return 127;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
				return 128;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 128;
            }                
        }

        public int ModificacionDestino(string idDestino, int prefijoRed, string idRecurso, int grupo, string numero)
        {
            try
            {
                //obtener el numero de troncal del recurso
                int numTroncal = 0;
                if ((prefijoRed != 3) && (prefijoRed != 8) && (prefijoRed != 9))
                {
                    cmd.CommandText = "SELECT NumTroncal FROM TroncalTlf WHERE Identificador='" + idRecurso + "'";
                    numTroncal = Int32.Parse(cmd.ExecuteScalar().ToString());
                }

                StringBuilder s = new StringBuilder(idDestino);
                while (s.Length < 8)
                    s.Append(" ");
                idDestino = s.ToString();

                cmd.CommandText = "UPDATE Destinos SET Numero='" + ((numero.Length > 0) ? numero : numTroncal.ToString())
                                    + "', Grupo=" + grupo.ToString() + " WHERE Destino='" + idDestino
                                    + "' AND PrefijoRed=" + prefijoRed.ToString() ;
                cmd.ExecuteNonQuery();

                cmd.CommandText = "UPDATE DestinosSectorizados SET Numero='" + ((numero.Length > 0) ? numero : numTroncal.ToString()) 
                                    + "' WHERE Destino='" + idDestino + "' AND PrefijoRed=" + prefijoRed.ToString();
                cmd.ExecuteNonQuery();
                return 127;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
				return 128;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 128;
            }
        }

        public int BajaDestino(string idDestino, int prefijoRed)
        {
            try
            {
                cmd.CommandText = "DELETE FROM ColateralTF WHERE RTRIM(Destino)=RTRIM('" + idDestino + "') AND PrefijoRed=" + prefijoRed.ToString()+" AND IdConfig<>'ACTIVA'";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM ParametrosDestino WHERE RTRIM(Destino)=RTRIM('" + idDestino + "') AND PrefijoRed=" + prefijoRed.ToString();
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM DestinosSectorizados WHERE RTRIM(Destino)=RTRIM('" + idDestino + "') AND PrefijoRed=" + prefijoRed.ToString();
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM Destinos WHERE RTRIM(Destino)=RTRIM('" + idDestino + "') AND PrefijoRed=" + prefijoRed.ToString();
                cmd.ExecuteNonQuery();

                return 127;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
				return 128;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 128;
            }
        }
        #endregion

        #region Gestion Encaminamientos
		public int AltaCentralPropia(string idEncaminamiento, string numTest, bool ts)
		{
			try
			{
				// Comprobar que existe la red ATS en el CD30
				cmd.CommandText = "SELECT COUNT(*) FROM Red WHERE Prefijo='3'";
				if (Int32.Parse(cmd.ExecuteScalar().ToString()) == 0)
				{
					// Si no existe, se da de alta
					cmd.CommandText = "INSERT INTO Red VALUES ('3','" + idEncaminamiento + "',0,'" + numTest + "'," + (ts ? "1" : "0") + ",'N')";
					cmd.ExecuteNonQuery();
				}

				return 127;
			}
			catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
				return 128;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 128;
            }               
		}

        public int AltaRangoR2(int tipo, string desde, string hasta)
        {
            try
            {
                if (tipo == 0)
                {
                    cmd.CommandText = "INSERT INTO CentralR2OPES VALUES (3,'" + desde + "','" + hasta + "')";
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd.CommandText = "INSERT INTO CentralR2Priv VALUES (3,'" + desde + "','" + hasta + "')";
                    cmd.ExecuteNonQuery();
                }

                return 127;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
				return 128;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 128;
            }                 
        }

        public int AltaDestinoR2(string idDestinoR2, int tipo, string desde, string hasta)
        {
            try
            {
                StringBuilder s = new StringBuilder(idDestinoR2);
                while (s.Length < 8)
                    s.Append(" ");
                idDestinoR2 = s.ToString();

                cmd.CommandText = "INSERT INTO DestinosR2 VALUES ('"+idDestinoR2+"','N','"+tipo.ToString()+"','" + desde + "','" + hasta + "')";
                cmd.ExecuteNonQuery();

                return 127;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
				return 128;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 128;
            }                
        }

        public int AltaRuta(string idRuta, bool directa, int numero, string idRecurso)
        {
            try
            {
                StringBuilder s = new StringBuilder(idRuta);
                while (s.Length < 8)
                    s.Append(" ");
                idRuta = s.ToString();

                int numTroncal=0;
                if (idRecurso != "")
                {//Obtener el numero del troncal del recurso 
                    cmd.CommandText = "SELECT NumTroncal FROM TroncalTlf WHERE Identificador='" + idRecurso + "'";
                    numTroncal = Int32.Parse(cmd.ExecuteScalar().ToString());
                }

                cmd.CommandText = "INSERT INTO Rutas VALUES ('" + idRuta + "','"+ (directa?"0":numero.ToString())
                    +"'," + numTroncal.ToString() +")";
                cmd.ExecuteNonQuery();

                return 127;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
				return 128;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 128;
            }
        }

        public int BajaEncaminamiento(string id)
        {   //Baja de encaminamiento sin central propia
            try
            {
                cmd.CommandText = "DELETE FROM DestinosR2 WHERE RTRIM(DestinoR2)=RTRIM('"+id+"')";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM Rutas WHERE RTRIM(DestinoR2)=RTRIM('" + id + "') AND NumTroncal>=0";
                cmd.ExecuteNonQuery();

                return 127;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
            {
				logDebugView.Error("Database error", ex);
				return 128;
            }
            catch (Exception e)
            {
                logDebugView.Error(e.Message);
                return 128;
            }
        }
        #endregion

		#region ColateralRD
		public int AltaColateralRadio(string idNucleo, string idSector, uint posHMI, string literal, string idDestino, string idEmplazamiento)
		{
			try
			{
				//Obtener el numero de usuario
				cmd.CommandText = "SELECT NumUsuario FROM USUARIOS WHERE IdConfig='" + idNucleo + "' AND RTRIM(IdUsuario)=RTRIM('" + idSector + "')";
				int numUsu = Int32.Parse(cmd.ExecuteScalar().ToString());

				cmd.CommandText = "INSERT INTO ColateralRd VALUES ('" + idNucleo + "'," + numUsu + ",7," + posHMI + ",'" + literal +
										"','" + idDestino + "','" + idEmplazamiento + "',0,0)";
				cmd.ExecuteNonQuery();
				return 127;
			}
			catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
			{
				logDebugView.Error("Database error", ex);
				return 128;
			}
			catch (Exception e)
			{
				logDebugView.Error(e.Message);
				return 128;
			}
		}

		public int BajaColateralRadio(string idNucleo, uint posHMI, string idSector)
		{
			try
			{
				//Obtener el numero de usuario
				cmd.CommandText = "SELECT NumUsuario FROM USUARIOS WHERE IdConfig='" + idNucleo + "' AND RTRIM(IdUsuario)=RTRIM('" + idSector + "')";
				int numUsu = Int32.Parse(cmd.ExecuteScalar().ToString());

				cmd.CommandText = "DELETE FROM ColateralRd WHERE IdConfig='" + idNucleo + "' AND NumUsuario=" + numUsu + " AND IdPanel=7 AND Posicion=" + posHMI;
				cmd.ExecuteNonQuery();
				return 127;
			}
			catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
			{
				logDebugView.Error("Database error", ex);
				return 128;
			}
			catch (Exception e)
			{
				logDebugView.Error(e.Message);
				return 128;
			}
		}

		public int ModificaColateralRadio(string idNucleo, string idSector, uint posHMI, string literal, string idEmplazamiento)
		{
			try
			{
				//Obtener el numero de usuario
				cmd.CommandText = "SELECT NumUsuario FROM USUARIOS WHERE IdConfig='" + idNucleo + "' AND RTRIM(IdUsuario)=RTRIM('" + idSector + "')";
				int numUsu = Int32.Parse(cmd.ExecuteScalar().ToString());

				cmd.CommandText = "UPDATE ColateralRd SET Literal='" + literal + "', Emplazamiento='" + idEmplazamiento + 
					"' WHERE IdConfig='" + idNucleo + "' AND NumUsuario=" + numUsu + " AND IdPanel=7 AND Posicion=" + posHMI;
				cmd.ExecuteNonQuery();
				return 127;
			}
			catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
			{
				logDebugView.Error("Database error", ex);
				return 128;
			}
			catch (Exception e)
			{
				logDebugView.Error(e.Message);
				return 128;
			}
		}

		#endregion

		#region ColateralTf
		public int AltaColateralTelefonia(string idNucleo, string idSector, uint posHMI, string literal, string idDestino, uint idPrefijo, string origenR2, uint prioridad)
		{
			try
			{
				//Obtener el numero de usuario
				cmd.CommandText = "SELECT NumUsuario FROM USUARIOS WHERE IdConfig='" + idNucleo + "' AND RTRIM(IdUsuario)=RTRIM('" + idSector + "')";
				int numUsu = Int32.Parse(cmd.ExecuteScalar().ToString());
				//Obtener el numero de usuario del origenR2
				cmd.CommandText = "SELECT NumUsuario FROM USUARIOS WHERE IdConfig='" + idNucleo + "' AND RTRIM(IdUsuario)=RTRIM('" + origenR2 + "')";
				int origen = Int32.Parse(cmd.ExecuteScalar().ToString());

				cmd.CommandText = "INSERT INTO ColateralTf VALUES ('" + idNucleo + "'," + numUsu + ",7," + posHMI + ",'" + literal +
										"','" + idDestino + "'," + idPrefijo + "," + origen + "," + prioridad + ",1,3)";
				cmd.ExecuteNonQuery();
				return 127;
			}
			catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
			{
				logDebugView.Error("Database error", ex);
				return 128;
			}
			catch (Exception e)
			{
				logDebugView.Error(e.Message);
				return 128;
			}
		}

		public int ModificaColateralTelefonia(string idNucleo, string idSector, uint posHMI, string literal, uint prioridad)
		{
			try
			{
				//Obtener el numero de usuario
				cmd.CommandText = "SELECT NumUsuario FROM USUARIOS WHERE IdConfig='" + idNucleo + "' AND RTRIM(IdUsuario)=RTRIM('" + idSector + "')";
				int numUsu = Int32.Parse(cmd.ExecuteScalar().ToString());

				cmd.CommandText = "UPDATE ColateralTf SET Literal='" + literal + "', Prioridad=" + prioridad +
					" WHERE IdConfig='" + idNucleo + "' AND NumUsuario=" + numUsu + " AND IdPanel=7 AND Posicion=" + posHMI;
				cmd.ExecuteNonQuery();
				return 127;
			}
			catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
			{
				logDebugView.Error("Database error", ex);
				return 128;
			}
			catch (Exception e)
			{
				logDebugView.Error(e.Message);
				return 128;
			}
		}

		public int BajaColateralTelefonia(string idNucleo, string idSector, uint posHMI)
		{
			try
			{
				//Obtener el numero de usuario
				cmd.CommandText = "SELECT NumUsuario FROM USUARIOS WHERE IdConfig='" + idNucleo + "' AND RTRIM(IdUsuario)=RTRIM('" + idSector + "')";
				int numUsu = Int32.Parse(cmd.ExecuteScalar().ToString());

				cmd.CommandText = "DELETE FROM ColateralTf WHERE IdConfig='" + idNucleo + "' AND NumUsuario=" + numUsu + " AND IdPanel=7 AND Posicion=" + posHMI;
				cmd.ExecuteNonQuery();
				return 127;
			}
			catch (MySql.Data.MySqlClient.MySqlException ex) // catches only Oracle errors
			{
				logDebugView.Error("Database error", ex);
				return 128;
			}
			catch (Exception e)
			{
				logDebugView.Error(e.Message);
				return 128;
			}
		}
		#endregion
	}
}
