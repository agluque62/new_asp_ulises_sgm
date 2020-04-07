using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Web.Configuration;
using System.Configuration;
//using SincroCD30;

namespace CD40.BD.Entidades
{
    public class Emplazamientos : Tablas
    {
        #region Propiedades de Emplazamientos

        private string _IdEmplazamiento;
        public string IdEmplazamiento
        {
            get { return _IdEmplazamiento; }
            set { _IdEmplazamiento = value; }
        }

        private string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }

        #endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public Emplazamientos()
        {
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null)
                Consulta.Append("SELECT * FROM Emplazamientos WHERE IdSistema='" + IdSistema + "'");
            else if (IdEmplazamiento != null)
                Consulta.Append("SELECT * FROM Emplazamientos WHERE IdEmplazamiento='" + IdEmplazamiento + "'");
            else
                Consulta.Append("SELECT * FROM Emplazamientos");

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
                    Emplazamientos e = new Emplazamientos();

                    e.IdEmplazamiento = (string)dr["IdEmplazamiento"];
                    e.IdSistema = (string)dr["IdSistema"];

                    ListaResultado.Add(e);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO Emplazamientos (IdSistema,IdEmplazamiento)" +
                            " VALUES ('" + IdSistema + "','" +
                                         IdEmplazamiento + "')");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Emplazamientos");
			return consulta;
        }

        public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];
			
			Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE Emplazamientos SET IdEmplazamiento='" + IdEmplazamiento + "','" +
                                            "idSistema='" + IdSistema + "'" +
                                            "WHERE IdEmplazamiento='" + IdEmplazamiento + "' AND " + "idSistema='" + IdSistema + "'"
                                            );

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Emplazamientos");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];
			
			Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdEmplazamiento != null)
                Consulta.Append("DELETE FROM Emplazamientos WHERE IdSistema='" + IdSistema + "' AND IdEmplazamiento='" + IdEmplazamiento + "'");
            else if (IdSistema != null)
                Consulta.Append("DELETE FROM Emplazamientos WHERE IdSistema='" + IdSistema + "'");
            else if (IdEmplazamiento != null)
                Consulta.Append("DELETE FROM Emplazamientos WHERE IdEmplazamiento='" + IdEmplazamiento + "'");
            else
                Consulta.Append("DELETE FROM Emplazamientos");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Emplazamientos");
			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM Emplazamientos WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
    }
}
