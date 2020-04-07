using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class Redes : Tablas
    {
        #region Propiedades de Redes

        private string _idRed;
        public string IdRed
        {
            get { return _idRed; }
            set { _idRed = value; }
        }

        private string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }

        private uint _Prefijo;
        public uint Prefijo
        {
            get { return _Prefijo; }
            set { _Prefijo = value; }
        }

        #endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public Redes()
        {
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdRed != null)
                Consulta.Append("SELECT * FROM Redes WHERE IdSistema='" + IdSistema + "' AND IdRed='" + IdRed + "'");
            else if (IdSistema != null)
                Consulta.Append("SELECT * FROM Redes WHERE IdSistema='" + IdSistema + "'");
            else if (IdRed != null)
                Consulta.Append("SELECT * FROM Redes WHERE IdRed='" + IdRed + "'");
            else
                Consulta.Append("SELECT * FROM Redes");

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
                    Redes r = new Redes();

                    r.IdRed = (string)dr["IdRed"];
                    r.IdSistema = (string)dr["IdSistema"];
                    if (dr["IdPrefijo"] != System.DBNull.Value)
                        r.Prefijo = (uint)dr["IdPrefijo"];

                    ListaResultado.Add(r);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO Redes (IdSistema,IdRed,IdPrefijo)" +
                            " VALUES ('" + IdSistema + "','" +
                                         IdRed + "'," +
                                         Prefijo + ")");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Redes");
			return consulta;
        }

		public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE Redes SET IdRed='" + IdRed + "'," +
                                            "IdSistema='" + IdSistema + "'," +
                                            "IdPrefijo=" + Prefijo + " " +
                                            "WHERE IdRed='" + IdRed + "' AND " + "IdSistema='" + IdSistema + "'"
                                            );

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Redes");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdRed != null)
                Consulta.Append("DELETE FROM Redes WHERE IdSistema='" + IdSistema + "' AND IdRed='" + IdRed + "'");
            else if (IdSistema != null)
                Consulta.Append("DELETE FROM Redes WHERE IdSistema='" + IdSistema + "'");
            else if (IdRed != null)
                Consulta.Append("DELETE FROM Redes WHERE IdRed='" + IdRed + "'");
            else
                Consulta.Append("DELETE FROM Redes");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Redes");
			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM Redes WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
    }
}
