using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class SectoresSector : Tablas
    {
        #region Propiedades de SectoresSector

        private string _idSector;
        public string IdSector
        {
            get { return _idSector; }
            set { _idSector = value; }
        }

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

        private string _IdSectorOriginal;
        public string IdSectorOriginal
        {
            get { return _IdSectorOriginal; }
            set { _IdSectorOriginal = value; }
        }

        private bool _EsDominante;
        public bool EsDominante
        {
            get { return _EsDominante; }
            set { _EsDominante = value; }
        }

        #endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public SectoresSector()
        {
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
        }

        public override string DataSetSelectSQL()
        {
            StringBuilder strConsulta = new StringBuilder();

            if (IdSistema != null && IdSector != null && IdNucleo != null && IdSectorOriginal != null)
                strConsulta.Append("SELECT * FROM SectoresSector WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "' AND IdSectorOriginal='" + IdSectorOriginal + "'");
            else if (IdSistema != null && IdSector != null && IdNucleo != null)
                strConsulta.Append("SELECT * FROM SectoresSector WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "' ORDER BY IdSectorOriginal");
            else if (IdSistema != null && IdNucleo != null)
                strConsulta.Append("SELECT * FROM SectoresSector WHERE IdSistema='" + IdSistema + "' AND IdNucleo='" + IdNucleo + "'");
            else if (IdSector != null && IdNucleo != null)
                strConsulta.Append("SELECT * FROM SectoresSector WHERE IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "'");
            else if (IdSistema != null && IdSector != null && EsDominante)
                strConsulta.Append("SELECT * FROM SectoresSector WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND EsDominante=" + EsDominante);
            else
                strConsulta.Append("SELECT * FROM SectoresSector");

            return strConsulta.ToString();
        }

        public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds)
        {
            ListaResultado.Clear();

			//DataSetResultado = this.DataSetSelectSQL();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    SectoresSector r = new SectoresSector();

                    r.IdSector = (string)dr["IdSector"];
                    r.IdSistema = (string)dr["IdSistema"];
                    r.IdNucleo = (string)dr["IdNucleo"];
                    r.IdSectorOriginal = (string)dr["IdSectorOriginal"];
                    r.EsDominante = (bool)dr["EsDominante"];    //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0;

                    ListaResultado.Add(r);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
			string[] consulta = new string[2];

            StringBuilder strConsulta = new StringBuilder();

            strConsulta.Append("INSERT INTO SectoresSector (IdSistema,IdNucleo,IdSector,IdSectorOriginal,EsDominante)" +
                            " VALUES ('" + IdSistema + "','" +
                                         IdNucleo + "','" +
                                         IdSector + "','" +
                                         IdSectorOriginal + "'," +
                                         EsDominante + ")");

            consulta[0] = strConsulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "SectoresSector");

            strConsulta.Clear();
			return consulta;
        }

		public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];
			
			Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE SectoresSector SET IdSector='" + IdSector + "'," +
                                            "IdNucleo='" + IdNucleo + "'," +
                                            "IdSistema='" + IdSistema + "'," +
                                            "IdSectorOriginal='" + IdSectorOriginal + "'," +
                                            "EsDominante=" + EsDominante + " " +
                                            "WHERE IdSector='" + IdSector + "' AND " + "IdSistema='" + IdSistema + "'"
                                            );

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "SectoresSector");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
            string[] consulta = new string[2];

            StringBuilder strConsulta = new StringBuilder();

            if (IdSistema != null && IdSector != null && IdNucleo != null)
                strConsulta.Append("DELETE FROM SectoresSector WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "'");
            else if (IdSistema != null && IdNucleo != null)
                strConsulta.Append("DELETE FROM SectoresSector WHERE IdSistema='" + IdSistema + "' AND IdNucleo='" + IdNucleo + "'");
            else if (IdSector != null && IdNucleo != null)
                strConsulta.Append("DELETE FROM SectoresSector WHERE IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "'");
            else
                strConsulta.Append("DELETE FROM SectoresSector");

            consulta[0] = strConsulta.ToString();
            consulta[1] = ReplaceSQL(IdSistema, "SectoresSector");
            strConsulta.Clear();

            return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM SectoresSector WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
    }
}
