using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace CD40.BD.Entidades
{
    public class Sistema : Tablas
    {
        #region Propiedades de Sistema

        private string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }

        private uint _TiempoPtt;
        public uint TiempoPtt
        {
            get { return _TiempoPtt; }
            set { _TiempoPtt = value; }
        }

        private uint _TiempoSinJack1;
        public uint TiempoSinJack1
        {
            get { return _TiempoSinJack1; }
            set { _TiempoSinJack1 = value; }
        }

        private uint _TiempoSinJack2;
        public uint TiempoSinJack2
        {
            get { return _TiempoSinJack2; }
            set { _TiempoSinJack2 = value; }
        }

        private uint _TamLiteralEnlExt;
        public uint TamLiteralEnlExt
        {
            get { return _TamLiteralEnlExt; }
            set { _TamLiteralEnlExt = value; }
        }

        private uint _TamLiteralDA;
        public uint TamLiteralDA
        {
            get { return _TamLiteralDA; }
            set { _TamLiteralDA = value; }
        }

        private uint _TamLiteralIA;
        public uint TamLiteralIA
        {
            get { return _TamLiteralIA; }
            set { _TamLiteralIA = value; }
        }

        private uint _TamLiteralAG;
        public uint TamLiteralAG
        {
            get { return _TamLiteralAG; }
            set { _TamLiteralAG = value; }
        }

        private uint _TamLiteralEmplazamiento;
        public uint TamLiteralEmplazamiento
        {
            get { return _TamLiteralEmplazamiento; }
            set { _TamLiteralEmplazamiento = value; }
        }

        private uint _VersionIP;
        public uint VersionIP
        {
            get { return _VersionIP; }
            set { _VersionIP = value; }
        }

        private string _GrupoMulticastConfiguracion;
        public string GrupoMulticastConfiguracion
        {
            get { return _GrupoMulticastConfiguracion; }
            set { _GrupoMulticastConfiguracion = value; }
        }

        private uint _PuertoMulticastConfiguracion;
        public uint PuertoMulticastConfiguracion
        {
            get { return _PuertoMulticastConfiguracion; }
            set { _PuertoMulticastConfiguracion = value; }
        }

		private uint _EstadoScv1;
		public uint EstadoScv1
		{
			get { return _EstadoScv1; }
			set { _EstadoScv1 = value; }
		}

		private uint _EstadoScv2;
		public uint EstadoScv2
		{
			get { return _EstadoScv2; }
			set { _EstadoScv2 = value; }
		}

        private uint _KeepAlivePeriod;
        public uint KeepAlivePeriod
        {
            get { return _KeepAlivePeriod; }
            set { _KeepAlivePeriod = value; }
        }

        private uint _KeepAliveMultiplier;
        public uint KeepAliveMultiplier
        {
            get { return _KeepAliveMultiplier; }
            set { _KeepAliveMultiplier = value; }
        }

        private uint _NumLlamadasEntrantesIda;
        public uint NumLlamadasEntrantesIda
        {
            get { return _NumLlamadasEntrantesIda; }
            set { _NumLlamadasEntrantesIda = value; }
        }

        private uint _NumLlamadasEnIda;
        public uint NumLlamadasEnIda
        {
            get { return _NumLlamadasEnIda; }
            set { _NumLlamadasEnIda = value; }
        }

        private uint _NumFrecPagina;
        public uint NumFrecPagina
        {
            get { return _NumFrecPagina; }
            set { _NumFrecPagina = value; }
        }

        private uint _NumPagFrec;
        public uint NumPagFrec
        {
            get { return _NumPagFrec; }
            set { _NumPagFrec = value; }
        }

        protected uint _NumEnlacesInternosPag;
        public uint NumEnlacesInternosPag
        {
            get { return _NumEnlacesInternosPag; }
            set { _NumEnlacesInternosPag = value; }
        }

        protected uint _NumPagEnlacesInt;
        public uint NumPagEnlacesInt
        {
            get { return _NumPagEnlacesInt; }
            set { _NumPagEnlacesInt = value; }
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

        private uint _NumEnlacesAI;
        public uint NumEnlacesAI
        {
            get { return _NumEnlacesAI; }
            set { _NumEnlacesAI = value; }
        }

        #endregion

		//protected static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public Sistema()
        {
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null)
                Consulta.Append("SELECT * FROM Sistema WHERE IdSistema='" + IdSistema + "'");
            else
                Consulta.Append("SELECT * FROM Sistema");

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
                    Sistema s = new Sistema();

                    s.IdSistema = (string)dr["IdSistema"];
                    if (dr["TiempoPTT"] != System.DBNull.Value)
                        s.TiempoPtt = (uint)dr["TiempoPTT"];
                    if (dr["TiempoSinJack1"] != System.DBNull.Value)
                        s.TiempoSinJack1 = (uint)dr["TiempoSinJack1"];
                    if (dr["TiempoSinJack2"] != System.DBNull.Value)
                        s.TiempoSinJack2 = (uint)dr["TiempoSinJack2"];
                    if (dr["TamLiteralEnlExt"] != System.DBNull.Value)
                        s.TamLiteralEnlExt = (uint)dr["TamLiteralEnlExt"];
                    if (dr["TamLiteralDA"] != System.DBNull.Value)
                        s.TamLiteralDA = (uint)dr["TamLiteralDA"];
                    if (dr["TamLiteralIA"] != System.DBNull.Value)
                        s.TamLiteralIA = (uint)dr["TamLiteralIA"];
                    if (dr["TamLiteralAG"] != System.DBNull.Value)
                        s.TamLiteralAG = (uint)dr["TamLiteralAG"];
                    if (dr["TamLiteralEmplazamiento"] != System.DBNull.Value)
                        s.TamLiteralEmplazamiento = (uint)dr["TamLiteralEmplazamiento"];
                    if (dr["VersionIP"] != System.DBNull.Value)
                        s.VersionIP = (uint)dr["VersionIP"];
                    if (dr["GrupoMulticastConfiguracion"] != System.DBNull.Value)
                        s.GrupoMulticastConfiguracion = (string)dr["GrupoMulticastConfiguracion"];
                    if (dr["PuertoMulticastConfiguracion"] != System.DBNull.Value)
                        s.PuertoMulticastConfiguracion = (uint)dr["PuertoMulticastConfiguracion"];
					if (dr["EstadoScvA"] != System.DBNull.Value)
						s.EstadoScv1 = (uint)dr["EstadoScvA"];
					if (dr["EstadoScvB"] != System.DBNull.Value)
						s.EstadoScv2 = (uint)dr["EstadoScvB"];
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
                    if (dr["KeepAlivePeriod"] != System.DBNull.Value)
                        s.KeepAlivePeriod = (uint)dr["KeepAlivePeriod"];
                    if (dr["KeepAliveMultiplier"] != System.DBNull.Value)
                        s.KeepAliveMultiplier = (uint)dr["KeepAliveMultiplier"];
                    if (dr["NumEnlacesAI"] != System.DBNull.Value)
                        s.NumEnlacesAI = (uint)dr["NumEnlacesAI"];

                    ListaResultado.Add(s);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO Sistema (IdSistema,TiempoPTT,TiempoSinJack1,TiempoSinJack2," +
                            // TamLiteralEnlExt,TamLiteralDA," +
							//"TamLiteralIA,TamLiteralAG,TamLiteralEmplazamiento,
                            "VersionIP,GrupoMulticastConfiguracion,PuertoMulticastConfiguracion," +
							"EstadoScvA,EstadoScvB,NumLlamadasEntrantesIDA,NumLlamadasEnIDA,NumFreqPagina,NumPagFreq," +
                                "NumDestinosInternosPag,NumPagDestinosInt,KeepAlivePeriod,KeepAliveMultiplier,NumEnlacesAI) VALUES ('" + IdSistema + "'," +
										TiempoPtt + "," +
										TiempoSinJack1 + "," +
										TiempoSinJack2 + "," +
                                        //TamLiteralEnlExt + "," +
                                        //TamLiteralDA + "," +
                                        //TamLiteralIA + "," +
                                        //TamLiteralAG + "," +
                                        //TamLiteralEmplazamiento + "," +
										VersionIP + ",'" +
										GrupoMulticastConfiguracion + "'," +
										PuertoMulticastConfiguracion + "," +
										EstadoScv1 + "," +
                                        EstadoScv2 + "," +
                                        +NumLlamadasEntrantesIda + "," +
                                        NumLlamadasEnIda + "," +
                                        NumFrecPagina + "," +
                                        NumPagFrec + "," +
                                        NumDestinosInternosPag + "," +
                                        NumPagDestinosInt + "," +
                                        KeepAlivePeriod + "," +
                                        KeepAliveMultiplier + "," + 
                                        NumEnlacesAI + ")");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Sistema");
			return consulta;
        }

		public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE Sistema SET IdSistema='" + IdSistema + "'," +
                                            "TiempoPtt=" + TiempoPtt + "," +
                                            "TiempoSinJack1=" + TiempoSinJack1 + "," +
                                            "TiempoSinJack2=" + TiempoSinJack2 + "," +
                                            //"TamLiteralEnlExt=" + TamLiteralEnlExt + "," +
                                            //"TamLiteralDA=" + TamLiteralDA + "," +
                                            //"TamLiteralIA=" + TamLiteralIA + "," +
                                            //"TamLiteralAG=" + TamLiteralAG + "," +
                                            //"TamLiteralEmplazamiento=" + TamLiteralEmplazamiento + "," +
                                            "VersionIP=" + VersionIP + "," +
                                            "GrupoMulticastConfiguracion='" + GrupoMulticastConfiguracion + "'," +
											"PuertoMulticastConfiguracion=" + PuertoMulticastConfiguracion + "," +
											"EstadoScvA=" + EstadoScv1 + "," +
											"EstadoscvB=" + EstadoScv2 + "," +
                                            "NumLlamadasEntrantesIDA=" + NumLlamadasEntrantesIda + "," +
                                            "NumLlamadasEnIDA=" + NumLlamadasEnIda + "," +
                                            "NumFreqPagina=" + NumFrecPagina + "," +
                                            "NumPagFreq=" + NumPagFrec + "," +
                                            "NumDestinosInternosPag=" + NumDestinosInternosPag + "," +
                                            "NumPagDestinosInt=" + NumPagDestinosInt + "," +
                                            "KeepAliveMultiplier=" + KeepAliveMultiplier + "," +
                                            "KeepAlivePeriod=" + KeepAlivePeriod + "," +
                                            "NumEnlacesAI=" + NumEnlacesAI + " " +
                               "WHERE IdSistema='" + IdSistema + "'");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Sistema");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];
			
			Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null)
                Consulta.Append("DELETE FROM Sistema WHERE IdSistema='" + IdSistema + "'");
            else
                Consulta.Append("DELETE FROM Sistema");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Sistema");
			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM Sistema WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
    }
}
