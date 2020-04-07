using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class TroncalesRuta : Tablas
    {
        #region Propiedades de TroncalesRuta

        private string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }

        private string _IdTroncal;
        public string IdTroncal
        {
            get { return _IdTroncal; }
            set { _IdTroncal = value; }
        }

        private string _Central;
        public string Central
        {
            get { return _Central; }
            set { _Central = value; }
        }

        private string _IdRuta;
        public string IdRuta
        {
            get { return _IdRuta; }
            set { _IdRuta = value; }
        }

        #endregion

        public TroncalesRuta()
        {
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdRuta != null && Central != null)
                Consulta.Append("SELECT * FROM TroncalesRuta WHERE IdRuta='" + IdRuta + "' AND IdSistema='" + IdSistema + "' AND Central='" + Central + "'");
            else if (IdSistema != null && IdTroncal != null)
                Consulta.Append("SELECT * FROM TroncalesRuta WHERE IdTroncal='" + IdTroncal + "' AND IdSistema='" + IdSistema + "'");
            else if (IdSistema != null && Central != null)
                Consulta.Append("SELECT * FROM TroncalesRuta WHERE Central='" + Central + "' AND IdSistema='" + IdSistema + "'");
            else if (IdSistema != null && IdRuta != null)
                Consulta.Append("SELECT * FROM TroncalesRuta WHERE IdSistema='" + IdSistema + "' AND IdRuta='" + IdRuta + "'");
            else
                Consulta.Append("SELECT * FROM TroncalesRuta");

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
                    TroncalesRuta r = new TroncalesRuta();

                    if (dr["IdSistema"] != System.DBNull.Value)
                        r.IdSistema = (string)dr["IdSistema"];
                    if (dr["IdTroncal"] != System.DBNull.Value)
                        r.IdTroncal = (string)dr["IdTroncal"];
                    if (dr["Central"] != System.DBNull.Value)
                        r.Central = (string)dr["Central"];
                    if (dr["IdRuta"] != System.DBNull.Value)
                        r.IdRuta = (string)dr["IdRuta"];

                    ListaResultado.Add(r);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO TroncalesRuta VALUES ('" + IdSistema + "','" +
                                                         IdTroncal + "','" +
                                                         Central + "','" +
                                                         IdRuta + "')");


			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "TroncalesRuta");
			return consulta;
		}

        public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE TroncalesRuta SET IdSistema='" + IdSistema + "'," +
                                            "IdTroncal='" + IdTroncal + "'," +
                                            "IdRuta='" + IdRuta + "'," +
                                            "Central='" + Central + "' " +
                                            "WHERE IdTroncal='" + IdTroncal + "' AND " + "IdSistema='" + IdSistema + "'"
                                            );


			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "TroncalesRuta");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
			if (IdSistema != null && IdRuta != null && Central != null && IdTroncal != null)
				Consulta.Append("DELETE FROM TroncalesRuta WHERE IdRuta='" + IdRuta + "' AND IdSistema='" + IdSistema + "' AND Central='" + Central + "' AND IdTroncal='" + IdTroncal + "'");
			else if (IdSistema != null && IdRuta != null && Central != null)
				Consulta.Append("DELETE FROM TroncalesRuta WHERE IdRuta='" + IdRuta + "' AND IdSistema='" + IdSistema + "' AND Central='" + Central + "'");
			else if (IdSistema != null && IdTroncal != null)
				Consulta.Append("DELETE FROM TroncalesRuta WHERE IdTroncal='" + IdTroncal + "' AND IdSistema='" + IdSistema + "'");
			else if (IdSistema != null && Central != null)
				Consulta.Append("DELETE FROM TroncalesRuta WHERE Central='" + Central + "' AND IdSistema='" + IdSistema + "'");
			else if (IdSistema != null && IdRuta != null)
				Consulta.Append("DELETE FROM TroncalesRuta WHERE IdSistema='" + IdSistema + "' AND IdRuta='" + IdRuta + "'");
			else
				Consulta.Append("DELETE FROM TroncalesRuta");


			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "TroncalesRuta");
			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM TroncalesRuta WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
    }
}
