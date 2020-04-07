using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class PermisosRedes : PermisosRedesSCV
    {
        #region Propiedades de PermisosRedes

        // Nombre del sistema
        private string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }
        // Nombre del sector
        private string _IdSector;
        public string IdSector
        {
            get { return _IdSector; }
            set { _IdSector = value; }
        }
        // Núcleo al que pertenece el sector
        private string _IdNucleo;
        public string IdNucleo
        {
            get { return _IdNucleo; }
            set { _IdNucleo = value; }
        }

        #endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public PermisosRedes()
        {
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdRed != null && IdSector != null && IdNucleo != null)
                Consulta.Append("SELECT * FROM PermisosRedes WHERE IdSistema='" + IdSistema + "' AND IdRed='" + IdRed + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "'");
            else if (IdSistema != null && IdSector != null)
                Consulta.Append("SELECT * FROM PermisosRedes WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "'");
            else if (IdSistema != null)
                Consulta.Append("SELECT * FROM PermisosRedes WHERE IdSistema='" + IdSistema + "'");
            else
                Consulta.Append("SELECT * FROM PermisosRedes");

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
                    PermisosRedes r = new PermisosRedes();

                    r.IdSistema = (string)dr["IdSistema"];
                    r.IdRed = (string)dr["IdRed"];
                    r.IdSector = (string)dr["IdSector"];
                    r.IdNucleo = (string)dr["IdNucleo"];
                    if (dr["Llamar"] != System.DBNull.Value)
                        r.Llamar = (string)dr["Llamar"] != "false";
                    if (dr["Recibir"] != System.DBNull.Value)
                        r.Recibir = (string)dr["Recibir"] != "false";

                    ListaResultado.Add(r);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO PermisosRedes (IdSistema,IdRed,IdSector,IdNucleo,Llamar,Recibir)" +
                            " VALUES ('" + IdSistema + "','" +
                                             IdRed + "','" +
                                             IdSector + "','" +
                                             IdNucleo + "','" +
                                             Llamar + "','" +
                                             Recibir + "')");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "PermisosRedes");
			return consulta;
        }

		public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE PermisosRedes SET IdSistema='" + IdSistema + "'," +
                                            "IdRed='" + IdRed + "'," +
                                            "IdSector='" + IdSector + "'," +
                                            "IdNucleo='" + IdNucleo + "'," +
                                            "Llamar='" + Llamar + "', " +
                                            "Recibir='" + Recibir + "' " +
                                            "WHERE IdSector='" + IdSector + "' AND " + "IdSistema='" + IdSistema +"' AND IdNucleo='" + IdNucleo + "' AND IdRed='" + IdRed + "'"
                                            );

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "PermisosRedes");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];
			
			Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdRed != null && IdSector != null && IdNucleo != null)
                Consulta.Append("DELETE FROM PermisosRedes WHERE IdSistema='" + IdSistema + "' AND IdRed='" + IdRed + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "'");
            else if (IdSistema != null && IdSector != null && IdNucleo != null)
                Consulta.Append("DELETE FROM PermisosRedes WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "'");
            else if (IdRed != null)
                Consulta.Append("DELETE FROM PermisosRedes WHERE IdRed='" + IdRed + "'");
            else
                Consulta.Append("DELETE FROM PermisosRedes");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "PermisosRedes");
			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM PermisosRedes WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
    }
}
