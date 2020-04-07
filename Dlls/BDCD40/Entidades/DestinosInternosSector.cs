using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class DestinosInternosSector : Tablas
    {
        #region Propiedades de DestinosInternosSector

        protected string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }

        protected string _IdDestino;
        public string IdDestino
        {
            get { return _IdDestino; }
            set { _IdDestino = value; }
        }

        protected uint _TipoDestino;
        public uint TipoDestino
        {
            get { return _TipoDestino; }
            set { _TipoDestino = value; }
        }

        protected string _IdNucleo;
        public string IdNucleo
        {
            get { return _IdNucleo; }
            set { _IdNucleo = value; }
        }

        protected string _IdSector;
        public string IdSector
        {
            get { return _IdSector; }
            set { _IdSector = value; }
        }

        protected uint _IdPrefijo;
        public uint IdPrefijo
        {
            get { return _IdPrefijo; }
            set { _IdPrefijo = value; }
        }

        protected uint _PosHMI;
        public uint PosHMI
        {
            get { return _PosHMI; }
            set { _PosHMI = value; }
        }

        protected uint _Prioridad;
        public uint Prioridad
        {
            get { return _Prioridad; }
            set { _Prioridad = value; }
        }

        protected string _OrigenR2;
        public string OrigenR2
        {
            get { return _OrigenR2; }
            set { _OrigenR2 = value; }
        }

        protected uint _PrioridadSIP;
        public uint PrioridadSIP
        {
            get { return _PrioridadSIP; }
            set { _PrioridadSIP = value; }
        }

        protected string _TipoAcceso;
        public string TipoAcceso
        {
            get { return _TipoAcceso; }
            set { _TipoAcceso = value; }
        }

        protected string _Literal;
        public string Literal
        {
            get { return _Literal; }
            set { _Literal = value; }
        }

        #endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public DestinosInternosSector()
        {
			IdPrefijo = uint.MaxValue;

			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
			if (IdSistema != null && IdSector != null && IdNucleo != null && PosHMI != 0 && IdPrefijo != uint.MaxValue)
				Consulta.Append("SELECT * FROM DestinosInternosSector WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "' AND PosHMI=" + PosHMI + " AND IdPrefijo=" + IdPrefijo);
			else if (IdSistema != null && IdSector != null && IdNucleo != null && PosHMI != 0)
				Consulta.Append("SELECT * FROM DestinosInternosSector WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "' AND PosHMI=" + PosHMI);
			else if (IdSistema != null && IdDestino != null && IdSector != null)
                Consulta.Append("SELECT * FROM DestinosInternosSector WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "' AND IdSector='" + IdSector + "'");
            else if (IdSistema != null && IdSector != null && PosHMI != 0)
                Consulta.Append("SELECT * FROM DestinosInternosSector WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND PosHMI=" + PosHMI);
            else if (IdSistema != null && IdSector != null)
                Consulta.Append("SELECT * FROM DestinosInternosSector WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "'");
            else
                Consulta.Append("SELECT * FROM DestinosInternosSector");

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
                    DestinosInternosSector r = new DestinosInternosSector();

                    if (dr["IdSistema"] != System.DBNull.Value)
                        r.IdSistema = (string)dr["IdSistema"];
                    if (dr["IdDestino"] != System.DBNull.Value)
                        r.IdDestino = (string)dr["IdDestino"];
                    if (dr["TipoDestino"] != System.DBNull.Value)
                        r.TipoDestino = (uint)dr["TipoDestino"];
                    if (dr["IdNucleo"] != System.DBNull.Value)
                        r.IdNucleo = (string)dr["IdNucleo"];
                    if (dr["IdSector"] != System.DBNull.Value)
                        r.IdSector = (string)dr["IdSector"];
                    if (dr["IdPrefijo"] != System.DBNull.Value)
                        r.IdPrefijo = (uint)dr["IdPrefijo"];
                    if (dr["PosHMI"] != System.DBNull.Value)
                        r.PosHMI = (uint)dr["PosHMI"];
                    if (dr["Prioridad"] != System.DBNull.Value)
                        r.Prioridad = (uint)dr["Prioridad"];
                    if (dr["OrigenR2"] != System.DBNull.Value)
                        r.OrigenR2 = (string)dr["OrigenR2"];
                    if (dr["PrioridadSIP"] != System.DBNull.Value)
                        r.PrioridadSIP = (uint)dr["PrioridadSIP"];
                    if (dr["TipoAcceso"] != System.DBNull.Value)
                        r.TipoAcceso = (string)dr["TipoAcceso"];
                    if (dr["Literal"] != System.DBNull.Value)
                        r.Literal = (string)dr["Literal"];

                    ListaResultado.Add(r);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO DestinosInternosSector (IdSistema,IdDestino,TipoDestino,IdNucleo,IdSector,IdPrefijo,PosHMI,Prioridad,OrigenR2,PrioridadSIP,TipoAcceso,Literal)" +
                            " VALUES ('" + IdSistema + "','" +
                                         IdDestino + "'," +
                                         TipoDestino + ",'" +
                                         IdNucleo + "','" +
                                         IdSector + "'," +
                                         IdPrefijo + "," +
                                         PosHMI + "," +
                                         Prioridad + ",'" +
                                         OrigenR2 + "'," +
                                         PrioridadSIP + ",'" +
                                         TipoAcceso + "','" +
                                         Literal + "')");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "DestinosInternosSector");
			return consulta;
        }

        public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];

			Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE DestinosInternosSector SET IdSistema='" + IdSistema + "'," +
                                            "IdDestino='" + IdDestino + "'," +
                                            "TipoDestino=" + TipoDestino + "," +
                                            "IdNucleo='" + IdNucleo + "'," +
                                            "IdSector='" + IdSector + "'," +
                                            "IdPrefijo=" + IdPrefijo + "," +
                                            "PosHMI=" + PosHMI + "," +
                                            "Prioridad=" + Prioridad + "," +
                                            "OrigenR2='" + OrigenR2 + "'," +
                                            "PrioridadSIP=" + PrioridadSIP + "," +
                                            "TipoAcceso='" + TipoAcceso + "'," +
                                            "Literal='" + Literal + "' " +
                                            "WHERE IdDestino='" + IdDestino + "' AND IdSector='" + IdSector + "' AND IdSistema='" + IdSistema + "' AND PosHMI=" + PosHMI + " AND IdPrefijo=" + IdPrefijo
                                            );

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "DestinosInternosSector");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];

			Consulta.Remove(0, Consulta.Length);
			if (IdSistema != null && IdSector != null && IdNucleo != null && PosHMI != 0 && IdPrefijo != uint.MaxValue)
				Consulta.Append("DELETE FROM DestinosInternosSector WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "' AND PosHMI=" + PosHMI + " AND IdPrefijo=" + IdPrefijo);
			else if (IdSistema != null && IdSector != null && IdNucleo != null && PosHMI != 0)
				Consulta.Append("DELETE FROM DestinosInternosSector WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "' AND PosHMI=" + PosHMI);
			else if (IdSistema != null && IdNucleo != null && IdDestino != null && IdSector != null && TipoAcceso != null)
				Consulta.Append("DELETE FROM DestinosInternosSector WHERE IdSistema='" + IdSistema + "' AND IdNucleo='" + IdNucleo + "' AND IdDestino='" + IdDestino + "' AND IdSector='" + IdSector + "' AND TipoAcceso='" + TipoAcceso + "'");
			else if (IdSistema != null && IdDestino != null && IdSector != null)
				Consulta.Append("DELETE FROM DestinosInternosSector WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "' AND IdSector='" + IdSector + "' AND TipoAcceso='" + TipoAcceso + "'");
			else if (IdSistema != null && IdSector != null)
				Consulta.Append("DELETE FROM DestinosInternosSector WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "'");
			else
				Consulta.Append("DELETE FROM DestinosInternosSector");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "DestinosInternosSector");
			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM DestinosInternosSector WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
    }
}
