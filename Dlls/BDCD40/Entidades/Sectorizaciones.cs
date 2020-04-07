using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Web.Configuration;
using System.Configuration;
//using SincroCD30;

namespace CD40.BD.Entidades
{
    public class Sectorizaciones : Tablas
    {
        #region Propiedades de Sectorizaciones

        private string _IdSectorizacion;
        public string IdSectorizacion
        {
            get { return _IdSectorizacion; }
            set { _IdSectorizacion = value; }
        }

        private string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }

        private bool _Activa;
        public bool Activa
        {
            get { return _Activa; }
            set { _Activa = value; }
        }

        private DateTime _FechaActivacion;
        public DateTime FechaActivacion
        {
            get { return _FechaActivacion; }
            set { _FechaActivacion = value; }
        }

        #endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public Sectorizaciones()
        {
        }

        public override string DataSetSelectSQL()
        {
            StringBuilder strCadena = new StringBuilder();

            if (IdSectorizacion != null && IdSistema != null)
                strCadena.AppendFormat("SELECT * FROM Sectorizaciones WHERE IdSistema='{0}' AND IdSectorizacion='{1}' ", IdSistema, IdSectorizacion);
            else if (IdSistema != null && Activa)
                strCadena.AppendFormat("SELECT * FROM Sectorizaciones WHERE IdSistema='{0}' AND Activa=true", IdSistema);
            else if (IdSistema != null)
                strCadena.AppendFormat("SELECT * FROM Sectorizaciones WHERE IdSistema='{0}' ", IdSistema);
            else if (IdSectorizacion != null)
                strCadena.AppendFormat("SELECT * FROM Sectorizaciones WHERE IdSectorizacion='{0}'", IdSectorizacion);
            else
                strCadena.Append("SELECT * FROM Sectorizaciones ");

            return strCadena.ToString();
        }

        public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds)
        {
            ListaResultado.Clear();

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                    {
                        Sectorizaciones s = new Sectorizaciones();

                        s.IdSectorizacion = (string)dr["IdSectorizacion"];
                        s.IdSistema = (string)dr["IdSistema"];
                        if (dr["Activa"] != System.DBNull.Value)
                            s.Activa = (bool)dr["Activa"];  //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0;
                        if (dr["FechaActivacion"] != System.DBNull.Value)
                            s.FechaActivacion = (DateTime)dr["FechaActivacion"];

                        ListaResultado.Add(s);
                    }
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
            string[] consulta = new string[2];

            StringBuilder strCadena = new StringBuilder();

            strCadena.AppendFormat("INSERT INTO Sectorizaciones (IdSistema,IdSectorizacion,Activa,FechaActivacion) VALUES ('{0}','{1}',{2},STR_TO_DATE('{3}','%d/%m/%Y %H:%i:%s'))", IdSistema,
                                   IdSectorizacion, Activa,FechaActivacion.ToString("dd/MM/yyyy HH:mm:ss"));

            consulta[0] = strCadena.ToString();
            consulta[1] = ReplaceSQL(IdSistema, "Sectorizaciones");

            strCadena.Clear();

            return consulta;
    }

        public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];

			Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE Sectorizaciones SET IdSectorizacion='" + IdSectorizacion + "'," +
                                            "IdSistema='" + IdSistema + "'," +
                                            "Activa=" + Activa + " " +
                                            "WHERE IdSectorizacion='" + IdSectorizacion + "' AND " + "IdSistema='" + IdSistema + "'"
                                            );


			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Sectorizaciones");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
            string[] consulta = new string[2];

            StringBuilder strCadena = new StringBuilder();

            if (IdSistema != null && IdSectorizacion != null)
                strCadena.Append("DELETE FROM Sectorizaciones WHERE IdSistema='" + IdSistema + "' AND IdSectorizacion='" + IdSectorizacion + "'");
            else if (IdSistema != null)
                strCadena.Append("DELETE FROM Sectorizaciones WHERE IdSistema='" + IdSistema + "'");
            else if (IdSectorizacion != null)
                strCadena.Append("DELETE FROM Sectorizaciones WHERE IdSectorizacion='" + IdSectorizacion + "'");
            else
                strCadena.Append("DELETE FROM Sectorizaciones");

            consulta[0] = strCadena.ToString();
            consulta[1] = ReplaceSQL(IdSistema, "Sectorizaciones");

            strCadena.Clear();

            return consulta;
        }

		//public void ActivaSectorizacion(DateTime now)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("UPDATE Sectorizaciones SET Activa=false" +
		//                                    " WHERE IdSectorizacion<>'" + IdSectorizacion + "' AND " + "IdSistema='" + IdSistema + "'"
		//                                    );
		//    AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());

		//    Consulta.Remove(0, Consulta.Length);
		//    //Consulta.Append("UPDATE Sectorizaciones SET Activa=true, FechaActivacion = NOW() " +
		//    //					"WHERE IdSectorizacion='" + IdSectorizacion + "' AND " + "IdSistema='" + IdSistema + "'");
		//    Consulta.Append("UPDATE Sectorizaciones SET Activa=true, FechaActivacion=STR_TO_DATE('" + now + "','%d/%m/%Y %k:%i:%s') " +
		//             "WHERE IdSectorizacion='" + IdSectorizacion + "' AND " + "IdSistema='" + IdSistema + "'");
		//    AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		//}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM Sectorizaciones WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
    }
}
