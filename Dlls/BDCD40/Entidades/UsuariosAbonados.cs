using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class UsuariosAbonados : Tablas
    {
        #region Propiedades de UsuariosAbonados

        private string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }

        private uint _IdPrefijo;
        public uint IdPrefijo
        {
            get { return _IdPrefijo; }
            set { _IdPrefijo = value; }
        }

        private string _IdAbonado;
        public string IdAbonado
        {
            get { return _IdAbonado; }
            set { _IdAbonado = value; }
        }

        private string _IdNucleo;
        public string IdNucleo
        {
            get { return _IdNucleo; }
            set { _IdNucleo = value; }
        }

        private string _IdSector;
        public string IdSector
        {
            get { return _IdSector; }
            set { _IdSector = value; }
        }

        #endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public UsuariosAbonados()
        {
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
            /*if (IdSistema != null && IdSector != null && IdNucleo != null && IdPrefijo!=null)
                Consulta.Append("SELECT * FROM UsuariosAbonados WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "' AND IdPrefijo=" + IdPrefijo);
            else*/
			if (IdSistema != null && IdSector != null && IdNucleo != null)
				Consulta.Append("SELECT * FROM UsuariosAbonados WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "'");
			else if (IdSistema != null && IdNucleo != null && IdAbonado != null)
				Consulta.Append("SELECT * FROM UsuariosAbonados WHERE IdSistema='" + IdSistema + "' AND IdNucleo='" + IdNucleo + "' AND IdPrefijo=" + IdPrefijo + " AND IdAbonado=" + IdAbonado + "'");
			else if (IdSistema != null)
				Consulta.Append("SELECT * FROM UsuariosAbonados WHERE IdSistema='" + IdSistema + "'");
			else
				Consulta.Append("SELECT * FROM UsuariosAbonados");

			return Consulta.ToString();
        }

        public override List<Tablas> ListSelectSQL(DataSet ds)
        {
            ListaResultado.Clear();

			//DataSetResultado = this.DataSetSelectSQL();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    UsuariosAbonados r = new UsuariosAbonados();

                    if (dr["IdSistema"] != System.DBNull.Value)
                        r.IdSistema = (string)dr["IdSistema"];
                    if (dr["IdPrefijo"] != System.DBNull.Value)
                        r.IdPrefijo = (uint)dr["IdPrefijo"];
                    if (dr["IdAbonado"] != System.DBNull.Value)
                        r.IdAbonado = (string)dr["IdAbonado"];
                    if (dr["IdSector"] != System.DBNull.Value)
                        r.IdSector = (string)dr["IdSector"];
                    if (dr["IdNucleo"] != System.DBNull.Value)
                        r.IdNucleo = (string)dr["IdNucleo"];

                    ListaResultado.Add(r);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO UsuariosAbonados (IdSistema,IdPrefijo,IdAbonado,IdNucleo,IdSector)" +
                            " VALUES ('" + IdSistema + "'," +
                                             IdPrefijo + ",'" +
                                             IdAbonado + "','" +
                                             IdNucleo + "','" +
                                             IdSector + "')");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "UsuariosAbonados");
			return consulta;
        }

		public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];
			
			Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE UsuariosAbonados SET IdSistema='" + IdSistema + "'," +
                                            "IdNucleo='" + IdNucleo + "'," +
                                            "IdPrefijo=" + IdPrefijo + "," +
                                            "IdAbonado='" + IdAbonado + "'," +
                                            "IdSector='" + IdSector + "' " +
                                            "WHERE IdSector='" + IdSector + "' AND IdSistema='" + IdSistema + "' AND IdNucleo='" + IdNucleo + "'"
                                            );

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "UsuariosAbonados");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];
			
			Consulta.Remove(0, Consulta.Length);
            
			if (IdSistema != null && IdSector != null && IdNucleo != null)
                Consulta.Append("DELETE FROM UsuariosAbonados WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "'");
            else if (IdSistema != null)
                Consulta.Append("DELETE FROM UsuariosAbonados WHERE IdSistema='" + IdSistema + "'");
            else
                Consulta.Append("DELETE FROM UsuariosAbonados");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "UsuariosAbonados");
			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM UsuariosAbonados WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
    }
}
