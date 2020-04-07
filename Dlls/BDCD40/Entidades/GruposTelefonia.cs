using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class GruposTelefonia : Tablas
    {
        #region Propiedades de Dependencias

        private string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }

        private string _IdGrupo;
        public string IdGrupo
        {
            get { return _IdGrupo; }
            set { _IdGrupo = value; }
        }

        #endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public GruposTelefonia()
        {
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null)
                Consulta.Append("SELECT * FROM GruposTelefonia WHERE IdSistema='" + IdSistema + "'");
            else if (IdGrupo != null)
                Consulta.Append("SELECT * FROM GruposTelefonia WHERE IdGrupo='" + IdGrupo + "'");
            else
                Consulta.Append("SELECT * FROM GruposTelefonia");

            return Consulta.ToString();
        }

        public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds)
        {
            ListaResultado.Clear();

			//DataSetResultado = this.DataSetSelectSQL();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    GruposTelefonia a = new GruposTelefonia();

                    a.IdGrupo = (string)dr["IdGrupo"];
                    a.IdSistema = (string)dr["IdSistema"];

                    ListaResultado.Add(a);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO GruposTelefonia (IdSistema,IdGrupo)" +
                            " VALUES ('" + IdSistema + "','" +
                                          IdGrupo + "')");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "GruposTelefonia");
			return consulta;
        }

        public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];

			Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE GruposTelefonia SET IdGrupo='" + IdGrupo + "'," +
                                            "idSistema='" + IdSistema + "' " +
                                            "WHERE IdGrupo='" + IdGrupo + "' AND " + "IdSistema='" + IdSistema + "'"
                                            );

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "GruposTelefonia");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];
			
			Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdGrupo != null)
                Consulta.Append("DELETE FROM GruposTelefonia WHERE IdSistema='" + IdSistema + "' AND IdGrupo='" + IdGrupo + "'");
            else if (IdSistema != null)
                Consulta.Append("DELETE FROM GruposTelefonia WHERE IdSistema='" + IdSistema + "'");
            else if (IdGrupo != null)
                Consulta.Append("DELETE FROM GruposTelefonia WHERE IdGrupo='" + IdGrupo + "'");
            else
                Consulta.Append("DELETE FROM GruposTelefonia");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "GruposTelefonia");
			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM GruposTelefonia WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
    }
}
