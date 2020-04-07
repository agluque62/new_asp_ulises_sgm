using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class TeclasSector : TeclasSectorSCV
    {
        #region Propiedades de TeclasSector
        // Identificador del sector/usuario
        private string _idSector;
        public string IdSector
        {
            get { return _idSector; }
            set { _idSector = value; }
        }
        // Identificador del sector/usuario
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

        public TeclasSector()
        {
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();

            TransConConsultaPrev = true;
            TransDirecta = false;
            Conferencia = false;
            Escucha = false;
            Retener = true;
            Captura = false;
            Redireccion = false;
            RepeticionUltLlamada = true;
            RellamadaAut = false;
            TeclaPrioridad = true;
            Tecla55mas1 = false;
            Monitoring = true;
            CoordinadorTF = false;
            CoordinadorRD = false;
            IntegracionRDTF = false;
            LlamadaSelectiva = false;
            GrupoBSS = true;
            LTT = true;
            SayAgain = true;
            InhabilitacionRedirec = false;
            Glp = false;
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdSector != null && IdNucleo != null)
                Consulta.Append("SELECT * FROM TeclasSector WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "'");
            else if (IdSistema != null && IdNucleo != null)
                Consulta.Append("SELECT * FROM TeclasSector WHERE IdSistema='" + IdSistema + "' AND IdNucleo='" + IdNucleo + "'");
            else if (IdSector != null)
                Consulta.Append("SELECT * FROM TeclasSector WHERE IdSector='" + IdSector + "'");
            else
                Consulta.Append("SELECT * FROM TeclasSector");

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
                    TeclasSector s = new TeclasSector();

                    s.IdSector = (string)dr["IdSector"];
                    s.IdSistema = (string)dr["IdSistema"];
                    s.IdNucleo = (string)dr["IdNucleo"];
                    if (dr["TransConConsultaPrev"] != System.DBNull.Value)
                        s.TransConConsultaPrev = (bool)dr["TransConConsultaPrev"] ;     //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0
                    if (dr["TransDirecta"] != System.DBNull.Value)
                        s.TransDirecta = (bool)dr["TransDirecta"] ;     //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0
                    if (dr["Conferencia"] != System.DBNull.Value)
                        s.Conferencia = (bool)dr["Conferencia"] ;     //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0
                    if (dr["Escucha"] != System.DBNull.Value)
                        s.Escucha = (bool)dr["Escucha"] ;     //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0
                    if (dr["Retener"] != System.DBNull.Value)
                        s.Retener = (bool)dr["Retener"] ;     //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0
                    if (dr["Captura"] != System.DBNull.Value)
                        s.Captura = (bool)dr["Captura"] ;     //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0
                    if (dr["Redireccion"] != System.DBNull.Value)
                        s.Redireccion = (bool)dr["Redireccion"] ;     //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0
                    if (dr["RepeticionUltLlamada"] != System.DBNull.Value)
                        s.RepeticionUltLlamada = (bool)dr["RepeticionUltLlamada"] ;     //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0
                    if (dr["RellamadaAut"] != System.DBNull.Value)
                        s.RellamadaAut = (bool)dr["RellamadaAut"] ;     //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0
                    if (dr["TeclaPrioridad"] != System.DBNull.Value)
                        s.TeclaPrioridad = (bool)dr["TeclaPrioridad"] ;     //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0
                    if (dr["Tecla55mas1"] != System.DBNull.Value)
                        s.Tecla55mas1 = (bool)dr["Tecla55mas1"] ;     //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0
                    if (dr["Monitoring"] != System.DBNull.Value)
                        s.Monitoring = (bool)dr["Monitoring"] ;     //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0
                    if (dr["CoordinadorTF"] != System.DBNull.Value)
                        s.CoordinadorTF = (bool)dr["CoordinadorTF"] ;     //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0
                    if (dr["CoordinadorRD"] != System.DBNull.Value)
                        s.CoordinadorRD = (bool)dr["CoordinadorRD"] ;     //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0
                    if (dr["IntegracionRDTF"] != System.DBNull.Value)
                        s.IntegracionRDTF = (bool)dr["IntegracionRDTF"] ;     //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0
                    if (dr["LlamadaSelectiva"] != System.DBNull.Value)
                        s.LlamadaSelectiva = (bool)dr["LlamadaSelectiva"] ;     //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0
                    if (dr["GrupoBSS"] != System.DBNull.Value)
                        s.GrupoBSS = (bool)dr["GrupoBSS"] ;     //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0
                    if (dr["LTT"] != System.DBNull.Value)
                        s.LTT = (bool)dr["LTT"] ;     //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0
                    if (dr["SayAgain"] != System.DBNull.Value)
                        s.SayAgain = (bool)dr["SayAgain"] ;     //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0
                    if (dr["InhabilitacionRedirec"] != System.DBNull.Value)
                        s.InhabilitacionRedirec = (bool)dr["InhabilitacionRedirec"] ;     //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0
                    if (dr["Glp"] != System.DBNull.Value)
                        s.Glp = (bool)dr["Glp"] ;     //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0
                    //if (dr["Glp"] != System.DBNull.Value)
                    //    s.Glp = (sbyte)dr["Glp"] != 0;

                    ListaResultado.Add(s);
                }
            }
            return ListaResultado;
        }

		public override string[] InsertSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO TeclasSector (IdSistema,IdNucleo,IdSector,TransConConsultaPrev," +
                                                    "TransDirecta,Conferencia,Escucha,Retener,Captura,Redireccion,RepeticionUltLlamada," +
                                                    "RellamadaAut,TeclaPrioridad,Tecla55mas1,Monitoring,CoordinadorTF,CoordinadorRD," +
                                                    "IntegracionRDTF,LlamadaSelectiva,GrupoBSS,LTT,SayAgain,InhabilitacionRedirec,Glp)" +
                            " VALUES ('" + IdSistema + "','" +
                                         IdNucleo + "','" +
                                         IdSector + "'," +
                                         TransConConsultaPrev + "," +
                                         TransDirecta + "," +
                                         Conferencia + "," +
                                         Escucha + "," +
                                         Retener + "," +
                                         Captura + "," +
                                         Redireccion + "," +
                                         RepeticionUltLlamada + "," +
                                         RellamadaAut + "," +
                                         TeclaPrioridad + "," +
                                         Tecla55mas1 + "," +
                                         Monitoring + "," +
                                         CoordinadorTF + "," +
                                         CoordinadorRD + "," +
                                         IntegracionRDTF + "," +
                                         LlamadaSelectiva + "," +
                                         GrupoBSS + "," +
                                         LTT + "," +
                                         SayAgain + "," +
                                         InhabilitacionRedirec + "," +
                                         Glp + ")");


			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "TeclasSector");
			return consulta;
        }

		public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];
			
			Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE TeclasSector SET IdSector='" + IdSector + "'," +
                                            "IdNucleo='" + IdNucleo + "'," +
                                            "IdSistema='" + IdSistema + "'," +
                                            "TransConConsultaPrev=" + TransConConsultaPrev + "," +
                                            "TransDirecta=" + TransDirecta + "," +
                                            "Conferencia=" + Conferencia + "," +
                                            "Escucha=" + Escucha + "," +
                                            "Retener=" + Retener + "," +
                                            "Captura=" + Captura + "," +
                                            "Redireccion=" + Redireccion + "," +
                                            "RepeticionUltLlamada=" + RepeticionUltLlamada + "," +
                                            "RellamadaAut=" + RellamadaAut + "," +
                                            "TeclaPrioridad=" + TeclaPrioridad + "," +
                                            "Tecla55mas1=" + Tecla55mas1 + "," +
                                            "Monitoring=" + Monitoring + "," +
                                            "CoordinadorTF=" + CoordinadorTF + "," +
                                            "CoordinadorRD=" + CoordinadorRD + "," +
                                            "IntegracionRDTF=" + IntegracionRDTF + "," +
                                            "LlamadaSelectiva=" + LlamadaSelectiva + "," +
                                            "GrupoBSS=" + GrupoBSS + "," +
                                            "LTT=" + LTT + "," +
                                            "SayAgain=" + SayAgain + "," +
                                            "InhabilitacionRedirec=" + InhabilitacionRedirec + "," +
                                            "Glp=" + Glp + " " +
                                            "WHERE IdSector='" + IdSector + "' AND " + "IdSistema='" + IdSistema + "' AND IdNucleo='" + IdNucleo + "'"
                                            );

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "TeclasSector");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdSector != null && IdNucleo != null)
                Consulta.Append("SELECT * FROM TeclasSector WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "'");
            else if (IdSistema != null && IdNucleo != null)
                Consulta.Append("SELECT * FROM TeclasSector WHERE IdSistema='" + IdSistema + "' AND IdNucleo='" + IdNucleo + "'");
            else if (IdSector != null)
                Consulta.Append("DELETE FROM TeclasSector WHERE IdSector='" + IdSector + "'");
            else
                Consulta.Append("DELETE FROM TeclasSector");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "TeclasSector");
			return consulta;
		}
    }
}
