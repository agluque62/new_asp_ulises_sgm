using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class Troncales : Tablas
    {
        #region Propiedades de Troncales

        private string _IdTroncal;
        public string IdTroncal
        {
            get { return _IdTroncal; }
            set { _IdTroncal = value; }
        }

        private string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }

        private string _NumTest;
        public string NumTest
        {
            get { return _NumTest; }
            set { _NumTest = value; }
        }

        #endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public Troncales()
        {
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdTroncal != null)
                Consulta.Append("SELECT * FROM Troncales WHERE IdSistema='" + IdSistema + "' AND IdTroncal='" + IdTroncal + "'");
            else if (IdSistema != null)
                Consulta.Append("SELECT * FROM Troncales WHERE IdSistema='" + IdSistema + "'");
            else if (IdTroncal != null)
                Consulta.Append("SELECT * FROM Troncales WHERE IdTroncal='" + IdTroncal + "'");
            else
                Consulta.Append("SELECT * FROM Troncales");

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
                    Troncales r = new Troncales();

                    r.IdTroncal = (string)dr["IdTroncal"];
                    r.IdSistema = (string)dr["IdSistema"];
                    if (dr["NumTest"] != System.DBNull.Value)
                        r.NumTest = (string)dr["NumTest"];

                    ListaResultado.Add(r);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO Troncales (IdSistema,IdTroncal,NumTest)" +
                            " VALUES ('" + IdSistema + "','" +
                                         IdTroncal + "','" +
                                         NumTest + "')");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Troncales");
			return consulta;
        }

		public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE Troncales SET IdTroncal='" + IdTroncal + "'," +
                                            "IdSistema='" + IdSistema + "'," +
                                            "NumTest='" + NumTest + "' " +
                                            "WHERE IdTroncal='" + IdTroncal + "' AND " + "IdSistema='" + IdSistema + "'"
                                            );

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Troncales");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];
			
			Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdTroncal != null)
                Consulta.Append("DELETE FROM Troncales WHERE IdSistema='" + IdSistema + "' AND IdTroncal='" + IdTroncal + "'");
            else if (IdSistema != null)
                Consulta.Append("DELETE FROM Troncales WHERE IdSistema='" + IdSistema + "'");
            else if (IdTroncal != null)
                Consulta.Append("DELETE FROM Troncales WHERE IdTroncal='" + IdTroncal + "'");
            else
                Consulta.Append("DELETE FROM Troncales");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Troncales");
			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM Troncales WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
    }
}
