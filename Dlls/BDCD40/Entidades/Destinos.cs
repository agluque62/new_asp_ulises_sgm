using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class Destinos : Tablas
    {
        #region Propiedades de Destinos

        private string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }

        private string _IdDestino;
        public string IdDestino
        {
            get { return _IdDestino; }
            set { _IdDestino = value; }
        }

        private uint _TipoDestino;
        public uint TipoDestino
        {
            get { return _TipoDestino; }
            set { _TipoDestino = value; }
        }

        #endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public Destinos()
        {
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdDestino != null)
                Consulta.Append("SELECT * FROM Destinos WHERE IdDestino='" + IdDestino + "' AND IdSistema='" + IdSistema + "' AND TipoDestino=" + TipoDestino);
            else if (IdSistema != null && IdDestino != null)
                Consulta.Append("SELECT * FROM Destinos WHERE IdDestino='" + IdDestino + "' AND IdSistema='" + IdSistema + "'");
            else if (IdSistema != null)
                Consulta.Append("SELECT * FROM Destinos WHERE IdSistema='" + IdSistema + "'");
            else
                Consulta.Append("SELECT * FROM Destinos");

            return Consulta.ToString();
        }

        public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds)
        {
            ListaResultado.Clear();

            //DataSetResultado = this.DataSetSelectSQL();
            if (ds != null)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    Destinos r = new Destinos();

                    if (dr["IdSistema"] != System.DBNull.Value)
                        r.IdSistema = (string)dr["IdSistema"];
                    if (dr["IdDestino"] != System.DBNull.Value)
                        r.IdDestino = (string)dr["IdDestino"];
                    if (dr["TipoDestino"] != System.DBNull.Value)
                        r.TipoDestino = (uint)dr["TipoDestino"];

                    ListaResultado.Add(r);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO Destinos VALUES ('" + IdSistema + "','" +
                                                         IdDestino + "'," +
                                                         TipoDestino + ")");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Destinos");
			return consulta;
        }

        public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];
			
			Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE Destinos SET IdSistema='" + IdSistema + "'," +
                                            "IdDestino='" + IdDestino + "'," +
                                            "TipoDestino=" + TipoDestino + " " +
                                            "WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "' AND TipoDestino=" + TipoDestino
                                            );

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Destinos");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[4];

			Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdDestino != null)
                Consulta.Append("DELETE FROM Destinos WHERE IdDestino='" + IdDestino + "' AND IdSistema='" + IdSistema + "' AND TipoDestino=" + TipoDestino);
            else if (IdSistema != null && IdDestino != null)
                Consulta.Append("DELETE FROM Destinos WHERE IdDestino='" + IdDestino + "' AND IdSistema='" + IdSistema + "'");
            else if (IdSistema != null)
                Consulta.Append("DELETE FROM Destinos WHERE IdSistema='" + IdSistema + "'");
            else
                Consulta.Append("DELETE FROM Destinos");

            consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(IdSistema, "Destinos");

            if (TipoDestino == 1)
            {
                consulta[2] = "UPDATE RECURSOSTF SET IdDestino=NULL,TipoDestino=NULL,IdPrefijo=NULL " +
                                "WHERE IdSistema = '" + IdSistema + "' AND IdDestino='" + IdDestino + "' AND TipoDestino='" + TipoDestino + "'";
                consulta[3] = "UPDATE RECURSOSLCEN SET IdDestino=NULL,TipoDestino=NULL,IdPrefijo=NULL " +
                                "WHERE IdSistema = '" + IdSistema + "' AND IdDestino='" + IdDestino + "' AND TipoDestino='" + TipoDestino + "'";
            }

			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM Destinos WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
    }
}
