using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class RecursosLCEN : ParametrosRecursoLCEN
    {
        #region Propiedades de LCEN
        // Identificador del recurso
        protected string _IdRecursos;
        public string IdRecurso
        {
            get { return _IdRecursos; }
            set { _IdRecursos = value; _IdRecursoSCV = value; }
        }
        // Identificador del sistema al que pertenece el recurso
        protected string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; _IdSistemaSCV = value; }
        }
        // Prefijo del destino asociado
        private uint _IdPrefijo;
        public uint IdPrefijo
        {
            get { return _IdPrefijo; }
            set { _IdPrefijo = value; }
        }
        // Tipo de Destino al que está asociado.
        private uint _TipoDestino;
        public uint TipoDestino
        {
            get { return _TipoDestino; }
            set { _TipoDestino = value; }
        }
        // Identificador del Destino al que está asociado.
        private string _IdDestino;
        public string IdDestino
        {
            get { return _IdDestino; }
            set { _IdDestino = value; }
        }
        //Supervisa colateral IP   
        private uint _isuperv_options;
        public uint isuperv_options
        {
            get { return _isuperv_options; }
            set { _isuperv_options = value; }
        }
        //Tiempo entre options cuando el colateral está presente 
        private uint _itm_superv_options;
        public uint itm_superv_options
        {
            get { return _itm_superv_options; }
            set { _itm_superv_options = value; }
        }
        #endregion

        public RecursosLCEN()
            : base()
        {
            TipoRecurso = (uint)Tipos.Tipo_Recurso.TR_LC;
        }

		public override string DataSetSelectSQL()
//		public new string DataSetSelectSQL()
		{
            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdRecurso != null)
                Consulta.Append("SELECT r.*,pr.* FROM RecursosLCEN r, ParametrosRecurso pr WHERE r.IdSistema='" + IdSistema + "' AND r.IdRecurso='" + IdRecurso + "'" +
									" AND r.IdSistema=pr.IdSistema AND r.IdRecurso=pr.IdRecurso AND r.TipoRecurso=pr.TipoRecurso");
            else if (IdSistema != null && IdDestino != null)
				Consulta.Append("SELECT r.*,pr.* FROM RecursosLCEN r, ParametrosRecurso pr WHERE r.IdSistema='" + IdSistema + "' AND r.IdDestino='" + IdDestino + "' AND r.IdPrefijo=" + IdPrefijo +
									" AND r.IdSistema=pr.IdSistema AND r.IdRecurso=pr.IdRecurso AND r.TipoRecurso=pr.TipoRecurso");
            else if (IdRecurso != null)
				Consulta.Append("SELECT r.*,pr.* FROM RecursosLCEN r, ParametrosRecurso pr WHERE r.IdRecurso='" + IdRecurso + "'" +
									" AND r.IdSistema=pr.IdSistema AND r.IdRecurso=pr.IdRecurso AND r.TipoRecurso=pr.TipoRecurso");
			else if (IdSistema != null)
				Consulta.Append("SELECT r.*,pr.* FROM RecursosLCEN r, ParametrosRecurso pr WHERE r.IdSistema='" + IdSistema + "'" +
									" AND r.IdSistema=pr.IdSistema AND r.IdRecurso=pr.IdRecurso AND r.TipoRecurso=pr.TipoRecurso");
			else
				Consulta.Append("SELECT r.*,pr.* FROM RecursosLCEN r, ParametrosRecurso pr WHERE" +
									" r.IdSistema=pr.IdSistema AND r.IdRecurso=pr.IdRecurso AND r.TipoRecurso=pr.TipoRecurso");

            return Consulta.ToString();
        }

        public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds)
        {
            // base.ListSelectSQL();

            ListaResultado.Clear();

			//DataSetResultado = this.DataSetSelectSQL();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    RecursosLCEN lc = new RecursosLCEN();

                    IdSistema = lc.IdSistema = (string)dr["IdSistema"];
                    IdRecurso = lc.IdRecurso = (string)dr["IdRecurso"];
                    TipoRecurso = lc.TipoRecurso = (uint)dr["TipoRecurso"];

					// Puede que haya que poner r. y pr. antes de cada campo para identificarlo
                    if (dr["TipoDestino"] != System.DBNull.Value)
                        lc.TipoDestino = (uint)dr["TipoDestino"];
                    if (dr["IdPrefijo"] != System.DBNull.Value)
                        lc.IdPrefijo = (uint)dr["IdPrefijo"];
                    if (dr["IdDestino"] != System.DBNull.Value)
                        lc.IdDestino = (string)dr["IdDestino"];
                    if (dr["T1"] != System.DBNull.Value)
                        lc.T1 = (uint)dr["T1"];
                    if (dr["T1Max"] != System.DBNull.Value)
                        lc.T1Max = (uint)dr["T1Max"];
                    if (dr["T2"] != System.DBNull.Value)
                        lc.T2 = (uint)dr["T2"];
                    if (dr["T2Max"] != System.DBNull.Value)
                        lc.T2Max = (uint)dr["T2Max"];
                    if (dr["T3"] != System.DBNull.Value)
                        lc.T3 = (uint)dr["T3"];
                    if (dr["T4"] != System.DBNull.Value)
                        lc.T4 = (uint)dr["T4"];
                    if (dr["T4Max"] != System.DBNull.Value)
                        lc.T4Max = (uint)dr["T4Max"];
                    if (dr["T5"] != System.DBNull.Value)
                        lc.T5 = (uint)dr["T5"];
                    if (dr["T5Max"] != System.DBNull.Value)
                        lc.T5Max = (uint)dr["T5Max"];
                    if (dr["T6"] != System.DBNull.Value)
                        lc.T6 = (uint)dr["T6"];
                    if (dr["T6Max"] != System.DBNull.Value)
                        lc.T6Max = (uint)dr["T6Max"];
                    if (dr["T8"] != System.DBNull.Value)
                        lc.T8 = (uint)dr["T8"];
                    if (dr["T8Max"] != System.DBNull.Value)
                        lc.T8Max = (uint)dr["T8Max"];
                    if (dr["T9"] != System.DBNull.Value)
                        lc.T9 = (uint)dr["T9"];
                    if (dr["T9Max"] != System.DBNull.Value)
                        lc.T9Max = (uint)dr["T9Max"];
                    if (dr["T10"] != System.DBNull.Value)
                        lc.T10 = (uint)dr["T10"];
                    if (dr["T10Max"] != System.DBNull.Value)
                        lc.T10Max = (uint)dr["T10Max"];
                    if (dr["T11"] != System.DBNull.Value)
                        lc.T11 = (uint)dr["T11"];
                    if (dr["T11Max"] != System.DBNull.Value)
                        lc.T11Max = (uint)dr["T11Max"];
                    if (dr["T12"] != System.DBNull.Value)
                        lc.T12 = (uint)dr["T12"];
                    if (dr["FrqTonoSQ"] != System.DBNull.Value)
                        lc.FrqTonoSQ = (uint)dr["FrqTonoSQ"];
                    if (dr["UmbralTonoSQ"] != System.DBNull.Value)
                        lc.UmbralTonoSQ = (int)dr["UmbralTonoSQ"];
                    if (dr["FrqTonoPTT"] != System.DBNull.Value)
                        lc.FrqTonoPTT = (uint)dr["FrqTonoPTT"];
                    if (dr["UmbralTonoPTT"] != System.DBNull.Value)
                        lc.UmbralTonoPTT = (int)dr["UmbralTonoPTT"];
                    if (dr["RefrescoEstados"] != System.DBNull.Value)
                        lc.RefrescoEstados = (uint)dr["RefrescoEstados"];
                    if (dr["Timeout"] != System.DBNull.Value)
                        lc.Timeout = (uint)dr["Timeout"];
                    if (dr["LongRafagas"] != System.DBNull.Value)
                        lc.LongRafagas = (uint)dr["LongRafagas"];
					if (dr["GananciaAGCTX"] != System.DBNull.Value)
						lc.GananciaAGCTX = (uint)dr["GananciaAGCTX"];
					if (dr["GananciaAGCTXdBm"] != System.DBNull.Value)
						lc.GananciaAGCTXdBm = (float)dr["GananciaAGCTXdBm"];
					if (dr["GananciaAGCRX"] != System.DBNull.Value)
						lc.GananciaAGCRX = (uint)dr["GananciaAGCRX"];
					if (dr["GananciaAGCRXdBm"] != System.DBNull.Value)
						lc.GananciaAGCRXdBm = (float)dr["GananciaAGCRXdBm"];
					if (dr["SupresionSilencio"] != System.DBNull.Value)
                        lc.SupresionSilencio = (bool)dr["SupresionSilencio"];   //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0;
					if (dr["TamRTP"] != System.DBNull.Value)
						lc.TamRTP = (uint)dr["TamRTP"];
					if (dr["Codec"] != System.DBNull.Value)
						lc.Codec = (uint)dr["Codec"];
                    if (dr["GrabacionEd137"] != System.DBNull.Value)
                        lc.GrabacionEd137 = (bool)dr["GrabacionEd137"];   //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0;
                    if (dr["isuperv_options"] != System.DBNull.Value)
                        lc.isuperv_options = Convert.ToUInt16(dr["isuperv_options"]);
                    if (dr["itm_superv_options"] != System.DBNull.Value)
                        lc.itm_superv_options = (uint)dr["itm_superv_options"];

					//System.Collections.Generic.List<Tablas> parametroRecurso = base.ListSelectSQL();
					//if (parametroRecurso.Count > 0)
					//{
					//    lc.GananciaAGCRX = ((ParametrosRecursoLCEN)parametroRecurso[0]).GananciaAGCRX;
					//    lc.GananciaAGCRXdBm = ((ParametrosRecursoLCEN)parametroRecurso[0]).GananciaAGCRXdBm;
					//    lc.GananciaAGCTX = ((ParametrosRecursoLCEN)parametroRecurso[0]).GananciaAGCTX;
					//    lc.GananciaAGCTXdBm = ((ParametrosRecursoLCEN)parametroRecurso[0]).GananciaAGCTXdBm;
					//    lc.SupresionSilencio = ((ParametrosRecursoLCEN)parametroRecurso[0]).SupresionSilencio;
					//    lc.TamRTP = ((ParametrosRecursoLCEN)parametroRecurso[0]).TamRTP;
					//    lc.Codec = ((ParametrosRecursoLCEN)parametroRecurso[0]).Codec;
					//}

                    ListaResultado.Add(lc);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
            //base.InsertSQL();
			string[] consulta=new string[4];

			Array.Copy(base.InsertSQL(), consulta, 2);

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO RecursosLCEN (IdSistema,IdRecurso,TipoRecurso,IdPrefijo,IdDestino,TipoDestino,T1,T1Max,T2,T2Max,T3,T4,T4Max,T5,T5Max,T6,T6Max,T8,T8Max,T9," +
                                                "T9Max,T10,T10Max,T11,T11Max,T12,FrqTonoSQ,UmbralTonoSQ,FrqTonoPTT,UmbralTonoPTT,RefrescoEstados,TimeOut,LongRafagas,isuperv_options,itm_superv_options)" +
                            " VALUES ('" + IdSistema + "','" +
                                         IdRecurso + "'," +
                                         TipoRecurso + "," +
                                         IdPrefijo + "," +
                                         ((IdDestino == null) ? "null," : ("'" + IdDestino + "',")) +
                                         TipoDestino + "," +
                                         T1 + "," +
                                         T1Max + "," +
                                         T2 + "," +
                                         T2Max + "," +
                                         T3 + "," +
                                         T4 + "," +
                                         T4Max + "," +
                                         T5 + "," +
                                         T5Max + "," +
                                         T6 + "," +
                                         T6Max + "," +
                                         T8 + "," +
                                         T8Max + "," +
                                         T9 + "," +
                                         T9Max + "," +
                                         T10 + "," +
                                         T10Max + "," +
                                         T11 + "," +
                                         T11Max + "," +
                                         T12 + "," +
                                         FrqTonoSQ + "," +
                                         UmbralTonoSQ + "," +
                                         FrqTonoPTT + "," +
                                         UmbralTonoPTT + "," +
                                         RefrescoEstados + "," +
                                         Timeout + "," +
                                         LongRafagas + "," +
                                         isuperv_options + "," +
                                         itm_superv_options + ")");

			consulta[2] = Consulta.ToString();
			consulta[3] = ReplaceSQL(IdSistema, "RecursosLCEN");

            return consulta;
        }

        public override string[] UpdateSQL()
        {
            //base.UpdateSQL();

			string[] consulta = new string[5];

			Array.Copy(base.UpdateSQL(), consulta, 3);

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE RecursosLCEN SET IdRecurso='" + IdRecurso + "'," +
                                            "IdSistema='" + IdSistema + "'," +
                                            "TipoRecurso=" + TipoRecurso + "," +
                                            "IdPrefijo=" + IdPrefijo + "," +
                                            "IdDestino=" + ((IdDestino == null) ? "null, " : ("'" + IdDestino + "', ")) +
                                            "TipoDestino=" + TipoDestino + "," +
                                            "T1=" + T1 + "," +
                                            "T1Max=" + T1Max + "," +
                                            "T2=" + T2 + "," +
                                            "T2Max=" + T2Max + "," +
                                            "T3=" + T3 + "," +
                                            "T4=" + T4 + "," +
                                            "T4Max=" + T4Max + "," +
                                            "T5=" + T5 + "," +
                                            "T5Max=" + T5Max + "," +
                                            "T6=" + T6 + "," +
                                            "T6Max=" + T6Max + "," +
                                            "T8=" + T8 + "," +
                                            "T8Max=" + T8Max + "," +
                                            "T9=" + T9 + "," +
                                            "T9Max=" + T9Max + "," +
                                            "T10=" + T10 + "," +
                                            "T10Max=" + T10Max + "," +
                                            "T11=" + T11 + "," +
                                            "T11Max=" + T11Max + "," +
                                            "T12=" + T12 + "," +
                                            "FrqTonoSQ=" + FrqTonoSQ + "," +
                                            "UmbralTonoSQ=" + UmbralTonoSQ + "," +
                                            "FrqTonoPTT=" + FrqTonoPTT + "," +
                                            "UmbralTonoPTT=" + UmbralTonoPTT + "," +
                                            "RefrescoEstados=" + RefrescoEstados + "," +
                                            "Timeout=" + Timeout + "," +
                                            "LongRafagas=" + LongRafagas + "," +
                                            "isuperv_options=" + isuperv_options + "," +
                                            "itm_superv_options=" + itm_superv_options + " " +
                                            "WHERE IdRecurso='" + IdRecurso + "' AND " + "IdSistema='" + IdSistema + "'"
                                            );

			consulta[3] = Consulta.ToString();
			consulta[4] = ReplaceSQL(IdSistema, "RecursosLCEN");

			return consulta;
		}

        public override string[] DeleteSQL()
        {
			//base.DeleteSQL();

			string[] consulta = new string[4];

			Array.Copy(base.InsertSQL(), consulta, 2);
			
			Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdRecurso != null)
                Consulta.Append("DELETE FROM RecursosLCEN WHERE IdSistema='" + IdSistema + "' AND IdRecurso='" + IdRecurso + "'");
            else if (IdSistema != null && IdDestino != null)
                Consulta.Append("DELETE FROM RecursosLCEN WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "' AND IdPrefijo=" + IdPrefijo);
            else if (IdRecurso != null)
                Consulta.Append("DELETE FROM RecursosLCEN WHERE IdRecurso='" + IdRecurso + "'");
            else
                Consulta.Append("DELETE FROM RecursosLCEN");

			consulta[2] = Consulta.ToString();
			consulta[3] = ReplaceSQL(IdSistema, "RecursosLCEN");

			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM RecursosLCEN WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}

        public override string[] UpdateDestinoSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE RecursosLCEN SET IdDestino='" + IdDestino + "'," +
                                            "IdPrefijo=" + IdPrefijo + "," +
                                            "TipoDestino=" + TipoDestino + " " +
                                            "WHERE IdRecurso='" + IdRecurso + "' AND " + "IdSistema='" + IdSistema + "'"
                                            );
			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "RecursosLCEN");

            return consulta;
        }

        public override string[] LiberaDestinoSQL()
        {
			string[] consulta = new string[2];

			Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE RecursosLCEN SET IdDestino=null, TipoDestino=null, IdPrefijo=null " +
                                            "WHERE IdDestino='" + IdDestino + "' AND " + "IdSistema='" + IdSistema + "' AND TipoDestino=" + TipoDestino
                                            );

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "RecursosLCEN");

			return consulta;
		}

    }
}
