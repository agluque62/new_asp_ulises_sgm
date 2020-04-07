using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
	public class RecursosDatos : ParametrosRecursosRadioKASiccip
	{
		#region Propiedades de RecursosDatos
		//protected string _IdRecursos;
		public string IdRecurso
		{
			get { return _IdRecursoSCV; }
			set { _IdRecursoSCV = value; }
		}
		// Identificador del sistema al que pertenece el recurso
		public string IdSistema
		{
			get { return _IdSistemaSCV; }
			set { _IdSistemaSCV = value; }
		}
		// Tipo de Destino al que está asociado.
        //public uint TipoRecurso
        //{
        //    get { return _TipoRecurso; }
        //    set { _TipoRecurso = value; }
        //}
		#endregion

		public RecursosDatos()
			: base()
		{
			TipoRecurso = (uint)Tipos.Tipo_Recurso.TR_DATOS;
		}

		public override string DataSetSelectSQL()
		{
			//base.DataSetSelectSQL();

			Consulta.Remove(0, Consulta.Length);
			if (IdSistema != null && IdRecurso != null)
				Consulta.Append("SELECT r.*, pr.* FROM Recursos r, ParametrosRecurso pr WHERE r.IdSistema='" + IdSistema + "' AND r.IdRecurso='" + IdRecurso + "' AND r.TipoRecurso=" + TipoRecurso +
			" AND r.IdSistema=pr.IdSistema AND r.IdRecurso=pr.IdRecurso AND r.TipoRecurso=pr.TipoRecurso");
			else if (IdSistema != null)
				Consulta.Append("SELECT r.*, pr.* FROM Recursos r, ParametrosRecurso pr WHERE r.IdSistema='" + IdSistema + "'" +
									" AND r.IdSistema=pr.IdSistema AND r.IdRecurso=pr.IdRecurso AND r.TipoRecurso=pr.TipoRecurso");
			else
				Consulta.Append("SELECT r.*, pr.* FROM Recursos r, ParametrosRecurso pr WHERE" +
									" r.IdSistema=pr.IdSistema AND r.IdRecurso=pr.IdRecurso AND r.TipoRecurso=pr.TipoRecurso");

			return Consulta.ToString();
		}

		public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds)
		{
			//base.ListSelectSQL();

			ListaResultado.Clear();

            if (ds != null && ds.Tables.Count > 0)
			{
				foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
				{
					RecursosDatos pr = new RecursosDatos();

					IdRecurso = pr.IdRecurso = (string)dr["IdRecurso"];
					IdSistema = pr.IdSistema = (string)dr["IdSistema"];
					TipoRecurso = pr.TipoRecurso = (uint)dr["TipoRecurso"];

					if (dr["GananciaAGCTX"] != System.DBNull.Value)
						pr.GananciaAGCTX = (uint)dr["GananciaAGCTX"];
					if (dr["GananciaAGCTXdBm"] != System.DBNull.Value)
						pr.GananciaAGCTXdBm = (int)dr["GananciaAGCTXdBm"];
					if (dr["GananciaAGCRX"] != System.DBNull.Value)
						pr.GananciaAGCRX = (uint)dr["GananciaAGCRX"];
					if (dr["GananciaAGCRXdBm"] != System.DBNull.Value)
						pr.GananciaAGCRXdBm = (int)dr["GananciaAGCRXdBm"];
					if (dr["SupresionSilencio"] != System.DBNull.Value)
                        pr.SupresionSilencio = (bool)dr["SupresionSilencio"];   //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0;
					if (dr["TamRTP"] != System.DBNull.Value)
						pr.TamRTP = (uint)dr["TamRTP"];
					if (dr["Codec"] != System.DBNull.Value)
						pr.Codec = (uint)dr["Codec"];

					ListaResultado.Add(pr);
				}
			}
			return ListaResultado;
		}

		/*
		public override string[] InsertSQL()
		{
			string[] consulta = new string[4];

			Array.Copy(base.InsertSQL(), consulta, 2);

			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("INSERT INTO RecursosDatos (IdSistema,IdRecurso,TipoRecurso,TipoDestino,IdDestino,IdEmplazamiento,EM,SQ,PTT,FrqTonoE,UmbralTonoE," +
														"FrqTonoM,UmbralTonoM,FrqTonoSQ,UmbralTonoSQ,FrqTonoPTT,UmbralTonoPTT,BSS,NTZ,TipoNTZ,Cifrado," +
														"SupervPortadoraTX,SupervModuladoraTX,ModoConfPTT,RepSQyBSS," +
														"DesactivacionSQ,TimeoutPTT,MetodoBSS,UmbralVAD,TiempoPTT,NumFlujosAudio,KeepAlivePeriod,KeepAliveMultiplier)" +
							" VALUES ('" + IdSistema + "','" +
										 IdRecurso + "'," +
										 TipoRecurso + "," +
										 TipoDestino + "," +
										 ((IdDestino == null) ? "null,'" : ("'" + IdDestino + "','")) +
										 IdEmplazamiento + "'," +
										 EM + ",'" +
										 SQ + "','" +
										 PTT + "'," +
										 FrqTonoE + "," +
										 UmbralTonoE + "," +
										 FrqTonoM + "," +
										 UmbralTonoM + "," +
										 FrqTonoSQ + "," +
										 UmbralTonoSQ + "," +
										 FrqTonoPTT + "," +
										 UmbralTonoPTT + "," +
										 BSS + "," +
										 NTZ + "," +
										 TipoNTZ + "," +
										 Cifrado + "," +
										 SupervPortadoraTx + "," +
										 SupervModuladoraTx + "," +
										 ModoConfPTT + "," +
										 RepSQyBSS + "," +
										 DesactivacionSQ + "," +
										 TimeoutPTT + "," +
										 MetodoBSS + "," +
										 UmbralVAD + "," +
										 TiempoPTT + "," +
										 NumFlujosAudio + "," +
										 KeepAlivePeriod + "," +
										 KeepAliveMultiplier +
												")");

			consulta[2] = Consulta.ToString();
			consulta[3] = ReplaceSQL(IdSistema, "RecursosDatos");

			return consulta;
		}

		public override string[] UpdateSQL()
		{
			string[] consulta = new string[4];

			Array.Copy(base.InsertSQL(), consulta, 2);

			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("UPDATE RecursosDatos SET IdRecurso='" + IdRecurso + "'," +
											"IdSistema='" + IdSistema + "'," +
											"TipoRecurso=" + TipoRecurso + "," +
											"IdDestino=" + ((IdDestino == null) ? "null, " : ("'" + IdDestino + "', ")) +
											"TipoDestino=" + TipoDestino + "," +
											"IdEmplazamiento='" + IdEmplazamiento + "'," +
											"EM=" + EM + "," +
											"SQ='" + SQ + "'," +
											"PTT='" + PTT + "'," +
											"FrqTonoE=" + FrqTonoE + "," +
											"UmbralTonoE=" + UmbralTonoE + "," +
											"FrqTonoM=" + FrqTonoM + "," +
											"UmbralTonoM=" + UmbralTonoM + "," +
											"FrqTonoSQ=" + FrqTonoSQ + "," +
											"UmbralTonoSQ=" + UmbralTonoSQ + "," +
											"FrqTonoPTT=" + FrqTonoPTT + "," +
											"UmbralTonoPTT=" + UmbralTonoPTT + "," +
											"BSS=" + BSS + "," +
											"NTZ=" + NTZ + "," +
											"TipoNTZ=" + TipoNTZ + "," +
											"Cifrado=" + Cifrado + "," +
											"SupervPortadoraTx=" + SupervPortadoraTx + "," +
											"SupervModuladoraTx=" + SupervModuladoraTx + "," +
											"ModoConfPTT=" + ModoConfPTT + "," +
											"RepSQyBSS=" + RepSQyBSS + "," +
											"DesactivacionSQ=" + DesactivacionSQ + "," +
											"TimeoutPTT=" + TimeoutPTT + "," +
											"MetodoBSS=" + MetodoBSS + "," +
											"UmbralVAD=" + UmbralVAD + "," +
											"TiempoPTT=" + TiempoPTT + "," +
											"NumFlujosAudio=" + NumFlujosAudio + "," +
											"KeepAlivePeriod=" + KeepAlivePeriod + "," +
											"KeepAliveMultiplier=" + KeepAliveMultiplier + " " +
											"WHERE IdRecurso='" + IdRecurso + "' AND " + "IdSistema='" + IdSistema + "'"
											);

			consulta[2] = Consulta.ToString();
			consulta[3] = ReplaceSQL(IdSistema, "RecursosDatos");

			return consulta;
		}

		public override string[] DeleteSQL()
		{
			string[] consulta = new string[4];

			Array.Copy(base.InsertSQL(), consulta, 2);

			Consulta.Remove(0, Consulta.Length);
			if (IdSistema != null && IdRecurso != null)
				Consulta.Append("DELETE FROM RecursosDatos WHERE IdSistema='" + IdSistema + "' AND IdRecurso='" + IdRecurso + "'");
			else if (IdSistema != null)
				Consulta.Append("DELETE FROM RecursosDatos WHERE IdSistema='" + IdSistema + "'");
			else
				Consulta.Append("DELETE FROM RecursosDatos");

			consulta[2] = Consulta.ToString();
			consulta[3] = ReplaceSQL(IdSistema, "RecursosDatos");

			return consulta;
		}
		 */
	}
}
