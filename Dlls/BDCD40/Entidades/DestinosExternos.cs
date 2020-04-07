using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class DestinosExternos : DestinosTelefonia
    {
        #region Propiedades de DestinosExternos

        /// <summary>
        /// 
        /// </summary>
        private string _IdAbonado;
        public string IdAbonado
        {
            get { return _IdAbonado; }
            set { _IdAbonado = value; }
        }
        #endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        /// <summary>
        /// 
        /// </summary>
        public DestinosExternos()
            : base()
        {
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string DataSetSelectSQL()
        {
            //            base.DataSetSelectSQL();

            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdGrupo != null)
                Consulta.Append("SELECT * FROM DestinosExternos WHERE IdSistema='" + IdSistema + "' AND IdGrupo='" + IdGrupo + "'");
            else if (IdSistema != null && IdDestino != null && IdPrefijo != 0)
                Consulta.Append("SELECT * FROM DestinosExternos WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "' AND IdPrefijo=" + IdPrefijo);
            else if (IdSistema != null && IdDestino != null)
                Consulta.Append("SELECT * FROM DestinosExternos WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "'");
			else if (IdSistema!=null)
				Consulta.Append("SELECT * FROM DestinosExternos WHERE IdSistema='" + IdSistema + "'");
            else
                Consulta.Append("SELECT * FROM DestinosExternos");

            return Consulta.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public override List<Tablas> ListSelectSQL(DataSet ds)
        {
            ListaResultado.Clear();

			//DataSetResultado = this.DataSetSelectSQL();
            if (ds != null)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    DestinosExternos r = new DestinosExternos();

                    if (dr["IdSistema"] != System.DBNull.Value)
                        r.IdSistema = (string)dr["IdSistema"];
                    if (dr["IdDestino"] != System.DBNull.Value)
                        r.IdDestino = (string)dr["IdDestino"];
                    if (dr["TipoDestino"] != System.DBNull.Value)
                        r.TipoDestino = (uint)dr["TipoDestino"];
                    if (dr["IdPrefijo"] != System.DBNull.Value)
                        r.IdPrefijo = (uint)dr["IdPrefijo"];
                    if (dr["IdAbonado"] != System.DBNull.Value)
                        r.IdAbonado = (string)dr["IdAbonado"];

                    ListaResultado.Add(r);
                }
            }
            return ListaResultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string[] InsertSQL()
        {
            /**
             * AGL 2012.06.15. ID.125. "No se anade el grupo a la tabla de Destinos de Telefonía"
             *                         habría que añadir el grupo a la tabla....
             */
            // string[] consulta = new string[2];
            string[] consulta = (IdGrupo == null) ? new string[2] : new string[3];
            /**/

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO DestinosExternos VALUES ('" + IdSistema + "','" +
                                                         IdDestino + "'," +
                                                         TipoDestino + "," +
                                                         IdPrefijo + "," +
                                                         ((IdAbonado == null) ? "null" : ("'" + IdAbonado + "'")) +
                                                         ")");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "DestinosExternos");

            /**
             * AGL 2012.06.15. ID.125. "No se anade el grupo a la tabla de Destinos de Telefonía"
             *                         habría que añadir el grupo a la tabla....
             * Para actualizar el Grupo en 'DestinosTelefonia'                 
             */
            if (IdGrupo != null)
            {
                Consulta.Remove(0, Consulta.Length);
                Consulta.Append(String.Format("UPDATE DestinosTelefonia SET idGrupo='{0}' WHERE idDestino='{1}'",IdGrupo==null ? "null" : IdGrupo, IdDestino));
                consulta[2] = Consulta.ToString();
            }
            /**/

            return consulta;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string[] UpdateSQL()
        {
            /**
             * AGL 2012.06.15. ID.125. "No se anade el grupo a la tabla de Destinos de Telefonía"
             *                         habría que añadir el grupo a la tabla....
             */
            // string[] consulta = new string[2];
            string[] consulta = new string[3];
            /**/
			
            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE DestinosExternos SET IdSistema='" + IdSistema + "'," +
                                            "IdDestino='" + IdDestino + "'," +
                                            "TipoDestino=" + TipoDestino + "," +
                                            "IdPrefijo=" + IdPrefijo + "," +
                                            "IdAbonado=" + ((IdAbonado == null) ? "null " : ("'" + IdAbonado + "' ")) +
                                            "WHERE IdDestino='" + IdDestino + "' AND " + "IdSistema='" + IdSistema + "'");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "DestinosExternos");

            /**
             * AGL 2012.06.15. ID.125. "No se anade el grupo a la tabla de Destinos de Telefonía"
             *                         habría que añadir el grupo a la tabla....
             * Para actualizar el Grupo en 'DestinosTelefonia'                 
             */
            Consulta.Remove(0, Consulta.Length);
            
            if (IdGrupo==null)
                Consulta.Append(String.Format("UPDATE DestinosTelefonia SET idGrupo=null WHERE idDestino='{0}'", IdDestino));
            else
                Consulta.Append(String.Format("UPDATE DestinosTelefonia SET idGrupo='{0}' WHERE idDestino='{1}'", IdGrupo, IdDestino));
            consulta[2] = Consulta.ToString();
            return consulta;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];
			
            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdDestino != null)
                Consulta.Append("DELETE FROM DestinosExternos WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "'");
            else
                Consulta.Append("DELETE FROM DestinosExternos");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "DestinosExternos");
			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM DestinosExternos WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
    }
}
