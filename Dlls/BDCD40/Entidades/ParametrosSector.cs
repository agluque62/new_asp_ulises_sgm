using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class ParametrosSector : ParametrosSectorRecording
    {
        #region Propiedades de ParametrosSector
        // Identificador del sector/usuario
        private string _idSector;
        public string IdSector
        {
            get { return _idSector; }
            set { _idSector = value; }
        }
        // Núcleo al que pertenece el sector
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

        private uint _NumDestinosInternosPag;
        public uint NumDestinosInternosPag
        {
            get { return _NumDestinosInternosPag; }
            set { _NumDestinosInternosPag = value; _NumEnlacesInternosPag = value; }
        }

        private uint _NumPagDestinosInt;
        public uint NumPagDestinosInt
        {
            get { return _NumPagDestinosInt; }
            set { _NumPagDestinosInt = value; _NumPagEnlacesInt = value; }
        }
        #endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public ParametrosSector()
        {
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();

            Intrusion = true;
            Intruido = true;
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdSector != null && IdNucleo != null)
                Consulta.Append("SELECT * FROM ParametrosSector WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "'");
            else if (IdSistema != null && IdSector != null)
                Consulta.Append("SELECT * FROM ParametrosSector WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "'");
            else if (IdSector != null)
                Consulta.Append("SELECT * FROM ParametrosSector WHERE IdSector='" + IdSector + "'");
            else
                Consulta.Append("SELECT * FROM ParametrosSector");

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
                    ParametrosSector s = new ParametrosSector();

                    s.IdSector = (string)dr["IdSector"];
                    s.IdSistema = (string)dr["IdSistema"];
                    s.IdNucleo = (string)dr["IdNucleo"];
                    if (dr["NumLlamadasEntrantesIDA"] != System.DBNull.Value)
                        s.NumLlamadasEntrantesIda = (uint)dr["NumLlamadasEntrantesIDA"];
                    if (dr["NumLlamadasEnIDA"] != System.DBNull.Value)
                        s.NumLlamadasEnIda = (uint)dr["NumLlamadasEnIDA"];
                    if (dr["NumFreqPagina"] != System.DBNull.Value)
                        s.NumFrecPagina = (uint)dr["NumFreqPagina"];
                    if (dr["NumPagFreq"] != System.DBNull.Value)
                        s.NumPagFrec = (uint)dr["NumPagFreq"];
                    if (dr["NumDestinosInternosPag"] != System.DBNull.Value)
                        s.NumDestinosInternosPag = (uint)dr["NumDestinosInternosPag"];
                    if (dr["NumPagDestinosInt"] != System.DBNull.Value)
                        s.NumPagDestinosInt = (uint)dr["NumPagDestinosInt"];
                    if (dr["Intrusion"] != System.DBNull.Value)
                        s.Intrusion = (bool)dr["Intrusion"];    //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0;
                    if (dr["Intruido"] != System.DBNull.Value)
                        s.Intruido = (bool)dr["Intruido"];  //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0;
					if (dr["KeepAlivePeriod"] != System.DBNull.Value)
						s.KeepAlivePeriod = (uint)dr["KeepAlivePeriod"];
					if (dr["KeepAliveMultiplier"] != System.DBNull.Value)
						s.KeepAliveMultiplier = (uint)dr["KeepAliveMultiplier"];
                    if (dr["NumEnlacesAI"] != System.DBNull.Value)
                        s.NumEnlacesAI = (uint)dr["NumEnlacesAI"];
                    if (dr["GrabacionEd137"] != System.DBNull.Value)
                        s.GrabacionEd137 = (bool)dr["GrabacionEd137"];    

                    ListaResultado.Add(s);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO ParametrosSector (IdSistema,IdSector,IdNucleo,NumLlamadasEntrantesIDA,NumLlamadasEnIDA,NumFreqPagina,NumPagFreq," +
                                "NumDestinosInternosPag,NumPagDestinosInt,Intrusion,Intruido,KeepAlivePeriod,KeepAliveMultiplier,NumEnlacesAI,GrabacionEd137)" +
                            " VALUES ('" + IdSistema + "','" +
                                         IdSector + "','" +
                                         IdNucleo + "'," +
                                         NumLlamadasEntrantesIda + "," +
                                         NumLlamadasEnIda + "," +
                                         NumFrecPagina + "," +
                                         NumPagFrec + "," +
                                         NumDestinosInternosPag + "," +
                                         NumPagDestinosInt + "," +
										 Intrusion + "," +
										 Intruido + "," +
										 KeepAlivePeriod + "," +
                                         KeepAliveMultiplier + "," +
                                         NumEnlacesAI + "," +
                                         GrabacionEd137 +
                                         ")");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "ParametrosSector");
			return consulta;
        }

		public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];
			
			Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE ParametrosSector SET IdSector='" + IdSector + "'," +
                                            "IdSistema='" + IdSistema + "'," +
                                            "IdNucleo='" + IdNucleo + "'," +
                                            "NumLlamadasEntrantesIDA=" + NumLlamadasEntrantesIda + "," +
                                            "NumLlamadasEnIDA=" + NumLlamadasEnIda + "," +
                                            "NumFreqPagina=" + NumFrecPagina + "," +
                                            "NumPagFreq=" + NumPagFrec + "," +
                                            "NumDestinosInternosPag=" + NumDestinosInternosPag + "," +
                                            "NumPagDestinosInt=" + NumPagDestinosInt + "," +
											"Intrusion=" + Intrusion + "," +
											"Intruido=" + Intruido + "," +
											"KeepAliveMultiplier=" + KeepAliveMultiplier + "," +
                                            "KeepAlivePeriod=" + KeepAlivePeriod + "," +
                                            "NumEnlacesAI=" + NumEnlacesAI + "," +
                                            "GrabacionEd137=" + GrabacionEd137 +
                                            " WHERE IdSector='" + IdSector + "' AND IdSistema='" + IdSistema + "' AND IdNucleo='" + IdNucleo + "'"
                                            );

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "ParametrosSector");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdSector != null && IdNucleo != null)
                Consulta.Append("DELETE FROM ParametrosSector WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "'");
            else if (IdSistema != null)
                Consulta.Append("DELETE FROM ParametrosSector WHERE IdSistema='" + IdSistema + "'");
            else if (IdSector != null)
                Consulta.Append("DELETE FROM ParametrosSector WHERE IdSector='" + IdSector + "'");
            else
                Consulta.Append("DELETE FROM ParametrosSector");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "ParametrosSector");
			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM ParametrosSector WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
    }
}
