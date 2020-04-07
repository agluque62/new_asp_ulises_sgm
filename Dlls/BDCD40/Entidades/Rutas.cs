using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class Rutas : Tablas
    {
        #region Propiedades de Rutas

        private string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
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

        private string _Tipo;
        public string Tipo
        {
            get { return _Tipo; }
            set { _Tipo = value; }
        }

		private int _Orden;
		public int Orden
		{
			get { return _Orden; }
			set { _Orden = value; }
		}

        #endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public Rutas()
        {
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && Central != null && IdRuta != null)
                Consulta.Append("SELECT * FROM Rutas WHERE IdSistema='" + IdSistema + "' AND Central='" + Central + "' AND IdRuta='" + IdRuta + "' ORDER BY Orden");
            else if (IdSistema != null && Central != null)
				Consulta.Append("SELECT * FROM Rutas WHERE IdSistema='" + IdSistema + "' AND Central='" + Central + "' ORDER BY Orden");
            else if (IdSistema != null)
                Consulta.Append("SELECT * FROM Rutas WHERE IdSistema='" + IdSistema + "'");
            else
                Consulta.Append("SELECT * FROM Rutas");

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
                    Rutas r = new Rutas();

                    r.IdSistema = (string)dr["IdSistema"];
                    r.Central = (string)dr["Central"];
                    r.IdRuta = (string)dr["IdRuta"];
                    if (dr["Tipo"] != System.DBNull.Value)
                        r.Tipo = (string)dr["Tipo"];
					if (dr["Orden"] != System.DBNull.Value)
						r.Orden = (int)dr["Orden"];

                    ListaResultado.Add(r);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO Rutas (IdSistema,IdRuta,Central,Tipo,Orden)" +
                            " VALUES ('" + IdSistema + "','" +
                                         IdRuta + "','" +
                                         Central + "','" +
                                         Tipo + "'," +
										 Orden + ")");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Rutas");
			return consulta;
        }

        public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE Rutas SET IdSistema='" + IdSistema + "'," +
                                            "Central='" + Central + "'," +
                                            "IdRuta='" + IdRuta + "'," +
											"Tipo='" + Tipo + "'," +
											"Orden=" + Orden + " " +
											"WHERE Central='" + Central + "' AND " + "IdSistema='" + IdSistema + "' AND IdRuta='" + IdRuta + "'"
                                            );

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Rutas");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[3];
			
			Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && Central != null && IdRuta != null)
            {
                Consulta.Append("DELETE FROM Rutas WHERE IdSistema='" + IdSistema + "' AND Central='" + Central + "' AND IdRuta='" + IdRuta + "'");
                consulta[0] = Consulta.ToString();
                Consulta.Remove(0, Consulta.Length);

                Consulta.Append("UPDATE Rutas SET orden = orden - 1 WHERE orden > " + Orden);
                consulta[1] = Consulta.ToString();
                
                consulta[2] = ReplaceSQL(IdSistema, "Rutas");
            }
            else if (IdSistema != null && Central != null)
            {
                Consulta.Append("DELETE FROM Rutas WHERE IdSistema='" + IdSistema + "' AND Central='" + Central + "'");
                consulta[0] = Consulta.ToString();
                consulta[1] = ReplaceSQL(IdSistema, "Rutas");
            }
            else if (IdSistema != null)
            {
                Consulta.Append("DELETE FROM Rutas WHERE IdSistema='" + IdSistema + "'");
                consulta[0] = Consulta.ToString();
                consulta[1] = ReplaceSQL(IdSistema, "Rutas");
            }
            else
            {
                Consulta.Append("DELETE FROM Rutas");
                consulta[0] = Consulta.ToString();
                consulta[1] = ReplaceSQL(IdSistema, "Rutas");
            }

			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM Rutas WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
    }
}
