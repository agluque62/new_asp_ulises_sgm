using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace CD40.BD.Entidades
{
    public class Agrupaciones : Tablas
    {
        #region Propiedades de Agrupaciones

        private string _IdAgrupacion;
        public string IdAgrupacion
        {
            get { return _IdAgrupacion; }
            set { _IdAgrupacion = value; }
        }

        private string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }

        #endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public Agrupaciones()
        {
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
            if (!string.IsNullOrEmpty(IdSistema) && !string.IsNullOrEmpty(IdAgrupacion))
                Consulta.AppendFormat("SELECT * FROM Agrupaciones WHERE IdSistema='{0}' AND IdAgrupacion='{1}'", IdSistema, IdAgrupacion);
            else if (IdSistema != null)
                Consulta.Append("SELECT * FROM Agrupaciones WHERE IdSistema='" + IdSistema + "'");
            else if (IdAgrupacion != null)
                Consulta.Append("SELECT * FROM Agrupaciones WHERE IdAgrupacion='" + IdAgrupacion + "'");
            else
                Consulta.Append("SELECT * FROM Agrupaciones");

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
                    Agrupaciones a = new Agrupaciones();

                    a.IdAgrupacion = (string)dr["IdAgrupacion"];
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
            Consulta.Append("INSERT INTO Agrupaciones (IdSistema,IdAgrupacion)" +
                            " VALUES ('" + IdSistema + "','" +
                                         IdAgrupacion + "')");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Agrupaciones");
			return consulta;
        }

        public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];

			Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE Agrupaciones SET IdAgrupacion='" + IdAgrupacion + "'," +
                                            "IdSistema='" + IdSistema + "' " +
                                            "WHERE IdAgrupacion='" + IdAgrupacion + "' AND " + "IdSistema='" + IdSistema + "'"
                                            );

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Agrupaciones");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];
			
			Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdAgrupacion != null)
                Consulta.Append("DELETE FROM Agrupaciones WHERE IdSistema='" + IdSistema + "' AND IdAgrupacion='" + IdAgrupacion + "'");
            else if (IdSistema != null)
                Consulta.Append("DELETE FROM Agrupaciones WHERE IdSistema='" + IdSistema + "'");
            else if (IdAgrupacion != null)
                Consulta.Append("DELETE FROM Agrupaciones WHERE IdAbonado='" + IdAgrupacion + "'");
            else
                Consulta.Append("DELETE FROM Agrupaciones");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Agrupaciones");
			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM Agrupaciones WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
    }
}
