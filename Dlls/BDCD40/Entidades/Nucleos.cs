using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Web.Configuration;
using System.Configuration;
//using SincroCD30;

namespace CD40.BD.Entidades
{
    public class Nucleos : Tablas
    {
        #region Propiedades de Nucleos

        private string _IdNucleo;
        public string IdNucleo
        {
            get { return _IdNucleo; }
            set { _IdNucleo = value; }
        }

        private string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }

        #endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public Nucleos()
        {
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null)
                Consulta.Append("SELECT * FROM Nucleos WHERE IdSistema='" + IdSistema + "'");
            else if (IdNucleo != null)
                Consulta.Append("SELECT * FROM Nucleos WHERE IdNucleo='" + IdNucleo + "'");
            else
                Consulta.Append("SELECT * FROM Nucleos");

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
                    Nucleos n = new Nucleos();

                    n.IdNucleo = (string)dr["IdNucleo"];
                    n.IdSistema = (string)dr["IdSistema"];

                    ListaResultado.Add(n);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
		{
			string[] consulta = new string[2];

			Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO Nucleos (IdSistema,IdNucleo)" +
                            " VALUES ('" + IdSistema + "','" +
                                          IdNucleo + "')");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Nucleos");
			return consulta;
        }

        public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE Nucleos SET IdNucleo='" + IdNucleo + "'," +
                                            "IdSistema='" + IdSistema + "' " +
                                            "WHERE IdNucleo='" + IdNucleo + "' AND " + "IdSistema='" + IdSistema + "'"
                                            );

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Nucleos");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];
			
			Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdNucleo != null)
                Consulta.Append("DELETE FROM Nucleos WHERE IdSistema='" + IdSistema + "' AND IdNucleo='" + IdNucleo + "'");
            else if (IdSistema != null)
                Consulta.Append("DELETE FROM Nucleos WHERE IdSistema='" + IdSistema + "'");
            else if (IdNucleo != null)
                Consulta.Append("DELETE FROM Nucleos WHERE IdNucleo='" + IdNucleo + "'");
            else
                Consulta.Append("DELETE FROM Nucleos");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Nucleos");
			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM Nucleos WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
    }
}
