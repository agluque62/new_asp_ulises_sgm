using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class Sectores : SectoresSCV
    {
        #region Propiedades de Sectores
        // Identificador del sistema
        private string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }
        // Identificador del sector/usuario
        private string _idSector;
        public string IdSector
        {
            get { return _idSector; }
            set { _idSector = value; }
        }
        // Identificador del núcleo del sector
        private string _IdNucleo;
        public string IdNucleo
        {
            get { return _IdNucleo; }
            set { _IdNucleo = value; }
        }
        // Identificador del núcleo sector/usuario que compone la pareja (Controlador/Planificador)
        private string _idNucleoParejaUCS;
        public string IdNucleoParejaUCS
        {
            get { return _idNucleoParejaUCS; }
            set { _idNucleoParejaUCS = value; }
        }
        // Identificador del sistema sector/usuario que compone la pareja (Controlador/Planificador)
        private string _idSistemaParejaUCS;
        public string IdSistemaParejaUCS
        {
            get { return _idSistemaParejaUCS; }
            set { _idSistemaParejaUCS = value; }
        }
        // Se trata de un sector original o como composición de una sectorización. Para uso de la sectorización.
        private bool _SectorSimple;
        public bool SectorSimple
        {
            get { return _SectorSimple; }
            set { _SectorSimple = value; }
        }
        // R: Real; V: Virtual; M: Mantenimiento. Para uso de la sectorización.
        private string _Tipo;
        public string Tipo
        {
            get { return _Tipo; }
            set { _Tipo = value; }
        }

        private uint _NumSacta;
        public uint NumSacta
        {
            get { return _NumSacta; }
            set { _NumSacta = value; }
        }

        #endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public Sectores()
        {
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();

            SectorSimple = true;
            Tipo = new string((char)Tipos.Tipo_Sector.TS_REAL, 1);
            TipoPosicion = new string((char)Tipos.Tipo_Posicion.TP_CONTROLADOR, 1);
            PrioridadR2 = (uint)Tipos.Nivel_Prioridad_R2.NVPR2_BAJA;
            TipoHMI = (uint)Tipos.Tipo_HMI.TH_ACC;
            IdSistemaParejaUCS = IdNucleoParejaUCS = IdParejaUCS = null;
            NumSacta = 0;
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
			if (IdSistema != null && IdSector != null && IdNucleo != null)
				Consulta.Append("SELECT * FROM Sectores WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "'");
			else if (IdSistema != null && IdSector != null)
				Consulta.Append("SELECT * FROM Sectores WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "'");
			else if (IdSistema != null && SectorSimple && IdNucleo != null)
				Consulta.Append("SELECT * FROM Sectores WHERE IdSistema='" + IdSistema + "' AND SectorSimple=true" + " AND IdNucleo='" + IdNucleo + "'");
			else if (IdSistema != null && IdNucleo != null)
				Consulta.Append("SELECT * FROM Sectores WHERE IdSistema='" + IdSistema + "' AND IdNucleo='" + IdNucleo + "'");
			else if (IdSector != null && IdNucleo != null)
				Consulta.Append("SELECT * FROM Sectores WHERE IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "'");
			else if (IdSistema != null && NumSacta != 0)
				Consulta.Append("SELECT * FROM Sectores WHERE IdSistema='" + IdSistema + "' AND NumSacta=" + NumSacta);
			else if (IdSistema != null && SectorSimple)
				Consulta.Append("SELECT * FROM Sectores WHERE IdSistema='" + IdSistema + "' AND SectorSimple=true");
			else
				Consulta.Append("SELECT * FROM Sectores");

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
                    Sectores s = new Sectores();

                    s.IdSector = (string)dr["IdSector"];
                    s.IdSistema = (string)dr["IdSistema"];
                    s.IdNucleo = (string)dr["IdNucleo"];
                    if (dr["IdParejaUCS"] != System.DBNull.Value)
                        s.IdParejaUCS = (string)dr["IdParejaUCS"];
                    if (dr["IdSistemaParejaUCS"] != System.DBNull.Value)
                        s.IdSistemaParejaUCS = (string)dr["IdSistemaParejaUCS"];
                    if (dr["IdNucleoParejaUCS"] != System.DBNull.Value)
                        s.IdNucleoParejaUCS = (string)dr["IdNucleoParejaUCS"];
                    if (dr["SectorSimple"] != System.DBNull.Value)
                        s.SectorSimple = (bool)dr["SectorSimple"];  //Incompatibilidad MySql Server 5.6.11 y 5.0 != 0;
                    if (dr["Tipo"] != System.DBNull.Value)
                        s.Tipo = (string)dr["Tipo"];
                    if (dr["TipoPosicion"] != System.DBNull.Value)
                        s.TipoPosicion = (string)dr["TipoPosicion"];
                    if (dr["PrioridadR2"] != System.DBNull.Value)
                        s.PrioridadR2 = (uint)dr["PrioridadR2"];
                    if (dr["TipoHMI"] != System.DBNull.Value)
                        s.TipoHMI = (uint)dr["TipoHMI"];
                    if (dr["NumSacta"] != System.DBNull.Value)
                        s.NumSacta = (uint)dr["NumSacta"];

                    ListaResultado.Add(s);
                }
            }
            return ListaResultado;
        }

        // Al insertar Sector==>Trigger AltaSector (da de alta dos destinos internos con prefijos LCI y TFI)
        public override string[] InsertSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO Sectores (IdSistema,IdNucleo,IdSector,IdSistemaParejaUCS,IdNucleoParejaUCS,IdParejaUCS,SectorSimple,Tipo,TipoPosicion,PrioridadR2," +
                                                    "TipoHMI,NumSacta)" +
                            " VALUES ('" + IdSistema + "','" +
                                         IdNucleo + "','" +
                                         IdSector + "'," +
                                         ((IdSistemaParejaUCS == null) ? "null," : ("'" + IdSistemaParejaUCS + "',")) +
                                         ((IdNucleoParejaUCS == null) ? "null," : ("'" + IdNucleoParejaUCS + "',")) +
                                         ((IdParejaUCS == null) ? "null," : ("'" + IdParejaUCS + "',")) +
                                         SectorSimple + ",'" +
                                         Tipo + "','" +
                                         TipoPosicion + "'," +
                                         PrioridadR2 + "," +
                                         TipoHMI + "," + NumSacta.ToString() + ")");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Sectores");
			return consulta;
        }

		public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];
			
			Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE Sectores SET IdSector='" + IdSector + "'," +
                                            "IdSistema='" + IdSistema + "'," +
                                            "IdNucleo='" + IdNucleo + "'," +
                                            "IdSistemaParejaUCS=" + ((IdSistemaParejaUCS == null) ? "null," : ("'" + IdSistemaParejaUCS + "',")) +
                                            "IdNucleoParejaUCS=" + ((IdNucleoParejaUCS == null) ? "null," : ("'" + IdNucleoParejaUCS + "',")) +
                                            "IdParejaUCS=" + ((IdParejaUCS == null) ? "null," : ("'" + IdParejaUCS + "',")) +
                                            "SectorSimple=" + SectorSimple + "," +
                                            "Tipo='" + Tipo + "'," +
                                            "TipoPosicion='" + TipoPosicion + "'," +
                                            "PrioridadR2=" + PrioridadR2 + "," +
                                            "TipoHMI=" + TipoHMI + ", NumSacta=" + NumSacta.ToString() + " " +
                                            "WHERE IdSector='" + IdSector + "' AND " + "IdSistema='" + IdSistema + "' AND IdNucleo='" + IdNucleo + "'"
                                            );

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Sectores");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdSector != null && IdNucleo != null)
                Consulta.Append("DELETE FROM Sectores WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "'");
            else if (IdSistema != null && SectorSimple && IdNucleo != null)
                Consulta.Append("DELETE FROM Sectores WHERE IdSistema='" + IdSistema + "' AND SectorSimple=true" + " AND IdNucleo='" + IdNucleo + "'");
            else if (IdSistema != null && IdSector != null)
                Consulta.Append("DELETE FROM Sectores WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "'");
            else if (IdSector != null && IdNucleo != null)
                Consulta.Append("DELETE FROM Sectores WHERE IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "'");
            else
                Consulta.Append("DELETE FROM Sectores");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Sectores");
			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM Sectores WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
    }
}
