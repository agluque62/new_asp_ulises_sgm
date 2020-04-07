using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class Niveles : NivelesSCV
    {
        #region Propiedades de Niveles
        // Identificador del sector/usuario
        private string _IdSector;
        public string IdSector
        {
            get { return _IdSector; }
            set { _IdSector = value; }
        }
        // Identificador del núcleo
        private string _IdNucleo;
        public string IdNucleo
        {
            get { return _IdNucleo; }
            set { _IdNucleo = value; }
        }

        // Identificador del sistema
        private string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }
        #endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public Niveles()
        {
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdSector != null && IdNucleo != null)
                Consulta.Append("SELECT * FROM Niveles WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "'");
            else if (IdSistema != null)
                Consulta.Append("SELECT * FROM Niveles WHERE IdSistema='" + IdSistema + "'");
            else
                Consulta.Append("SELECT * FROM Niveles");

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
                    Niveles r = new Niveles();

                    if (dr["IdSector"] != System.DBNull.Value)
                        r.IdSector = (string)dr["IdSector"];
                    if (dr["IdNucleo"] != System.DBNull.Value)
                        r.IdNucleo = (string)dr["IdNucleo"];
                    if (dr["IdSistema"] != System.DBNull.Value)
                        r.IdSistema = (string)dr["IdSistema"];
                    if (dr["CICL"] != System.DBNull.Value)
                        r.CICL = (uint)dr["CICL"];
                    if (dr["CIPL"] != System.DBNull.Value)
                        r.CIPL = (uint)dr["CIPL"];
                    if (dr["CPICL"] != System.DBNull.Value)
                        r.CPICL = (uint)dr["CPICL"];
                    if (dr["CPIPL"] != System.DBNull.Value)
                        r.CPIPL = (uint)dr["CPIPL"];

                    ListaResultado.Add(r);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO Niveles (IdSistema,IdNucleo,IdSector,CICL,CIPL,CPICL,CPIPL)" +
                            " VALUES ('" + IdSistema + "','" +
                                         IdNucleo + "','" +
                                         IdSector + "'," +
                                         CICL + "," +
                                         CIPL + "," +
                                         CPICL + "," +
                                         CPIPL + ")");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Niveles");
			return consulta;
        }

        public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE Niveles SET IdSector='" + IdSector + "'," +
                                            "IdNucleo='" + IdNucleo + "'," +
                                            "IdSistema='" + IdSistema + "'," +
                                            "CICL=" + CICL + "," +
                                            "CIPL=" + CIPL + "," +
                                            "CPICL=" + CPICL + "," +
                                            "CPIPL=" + CPIPL + " " +
                                            "WHERE IdSector='" + IdSector + "' AND IdSistema='" + IdSistema + "' AND IdNucleo='" + IdNucleo + "'"
                                            );

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Niveles");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdSector != null && IdNucleo != null)
                Consulta.Append("DELETE FROM Niveles WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "'");
            else if (IdSistema != null)
                Consulta.Append("DELETE FROM Niveles WHERE IdSistema='" + IdSistema + "'");
            else
                Consulta.Append("DELETE FROM Niveles");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Niveles");
			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM Niveles WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
    }
}
