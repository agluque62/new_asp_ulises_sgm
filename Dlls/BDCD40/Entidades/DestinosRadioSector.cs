using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class DestinosRadioSector : Tablas
    {
        #region Propiedades de DestinosRadioSector
        protected string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }

        protected string _IdSector;
        public string IdSector
        {
            get { return _IdSector; }
            set { _IdSector = value; }
        }

        protected string _IdNucleo;
        public string IdNucleo
        {
            get { return _IdNucleo; }
            set { _IdNucleo = value; }
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

        protected uint _PrioridadSIP;
        public uint PrioridadSIP
        {
            get { return _PrioridadSIP; }
            set { _PrioridadSIP = value; }
        }

        private string _ModoOperacion;
        public string ModoOperacion
        {
            get { return _ModoOperacion; }
            set { _ModoOperacion = value; }
        }

        private string _Cascos;
        public string Cascos
        {
            get { return _Cascos; }
            set { _Cascos = value; }
        }

        private string _Literal;
        public string Literal
        {
            get { return _Literal; }
            set { _Literal = value; }
        }

        private bool _SupervisionPortadora;
        public bool SupervisionPortadora
        {
            get { return _SupervisionPortadora; }
            set { _SupervisionPortadora = value; }
        }
        #endregion

        //static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public DestinosRadioSector()
        {
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdDestino != null && IdSector != null)
                Consulta.Append("SELECT * FROM DestinosRadioSector WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "' AND IdSector='" + IdSector + "'");
            else if (IdSistema != null && IdSector != null && PosHMI != 0)
                Consulta.Append("SELECT * FROM DestinosRadioSector WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND PosHMI=" + PosHMI);
            else if (IdSistema != null && IdSector != null)
                Consulta.Append("SELECT * FROM DestinosRadioSector WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "'");
			else if (IdSistema != null && IdDestino != null)
				Consulta.Append("SELECT * FROM DestinosRadioSector WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "'");
			else if (IdSistema != null)
				Consulta.Append("SELECT * FROM DestinosRadioSector WHERE IdSistema='" + IdSistema + "'");
			else
                Consulta.Append("SELECT * FROM DestinosRadioSector");

			return Consulta.ToString();
        }

		public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds)
		{
			ListaResultado.Clear();

            if (ds != null && ds.Tables.Count > 0)
			{
				foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
				{
					DestinosRadioSector r = new DestinosRadioSector();

					if (dr["IdDestino"] != System.DBNull.Value)
						r.IdDestino = (string)dr["IdDestino"];
					if (dr["IdSistema"] != System.DBNull.Value)
						r.IdSistema = (string)dr["IdSistema"];
					if (dr["IdSector"] != System.DBNull.Value)
						r.IdSector = (string)dr["IdSector"];
					if (dr["IdNucleo"] != System.DBNull.Value)
						r.IdNucleo = (string)dr["IdNucleo"];
					if (dr["TipoDestino"] != System.DBNull.Value)
						r.TipoDestino = (uint)dr["TipoDestino"];
					if (dr["PosHMI"] != System.DBNull.Value)
						r.PosHMI = (uint)dr["PosHMI"];
					if (dr["Prioridad"] != System.DBNull.Value)
						r.Prioridad = (uint)dr["Prioridad"];
					if (dr["PrioridadSIP"] != System.DBNull.Value)
						r.PrioridadSIP = (uint)dr["PrioridadSIP"];
					if (dr["ModoOperacion"] != System.DBNull.Value)
						r.ModoOperacion = (string)dr["ModoOperacion"];
					if (dr["Cascos"] != System.DBNull.Value)
						r.Cascos = (string)dr["Cascos"];
					if (dr["Literal"] != System.DBNull.Value)
						r.Literal = (string)dr["Literal"];
                    if (dr["SupervisionPortadora"] != System.DBNull.Value)
                        r.SupervisionPortadora = (bool)dr["SupervisionPortadora"];  //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0;

					ListaResultado.Add(r);
				}
			}
			return ListaResultado;
		}

        public override string[] InsertSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO DestinosRadioSector (IdSistema,IdDestino,TipoDestino,IdNucleo,IdSector,PosHMI,Prioridad,PrioridadSIP,ModoOperacion,Cascos,Literal,SupervisionPortadora)" +
                            " VALUES ('" + IdSistema + "','" +
                                         IdDestino + "'," +
                                         TipoDestino + ",'" +
                                         IdNucleo + "','" +
                                         IdSector + "'," +
                                         PosHMI + "," +
                                         Prioridad + "," +
                                         PrioridadSIP + ",'" +
                                         ModoOperacion + "','" +
                                         Cascos + "','" +
                                         Literal + "'," + 
                                         SupervisionPortadora + ")");


			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "DestinosRadioSector");
			return consulta;
		}

        public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            if (PosHMI> 0)
                Consulta.Append("UPDATE DestinosRadioSector SET IdSistema='" + IdSistema + "'," +
                                            "IdSector='" + IdSector + "'," +
                                            "IdNucleo='" + IdNucleo + "'," +
                                            "IdDestino='" + IdDestino + "'," +
                                            "TipoDestino=" + TipoDestino + "," +
                                            "PosHMI=" + PosHMI + "," +
                                            "Prioridad=" + Prioridad + "," +
                                            "PrioridadSIP=" + PrioridadSIP + "," +
                                            "ModoOperacion='" + ModoOperacion + "'," +
                                            "Cascos='" + Cascos + "'," +
                                            "Literal='" + Literal + "'," +
                                            "SupervisionPortadora=" + SupervisionPortadora +
                                            " WHERE IdDestino='" + IdDestino + "' AND " + "IdSector='" + IdSector + "' AND " + "IdSistema='" + IdSistema + "' AND" +
                                                    " PosHMI=" + PosHMI + " AND IdNucleo='" + IdNucleo + "' AND TipoDestino=" + TipoDestino
                                            );
            else if(IdSector==null)
                Consulta.Append("UPDATE DestinosRadioSector SET Literal='" + Literal + "' " +
                                            " WHERE IdDestino='" + IdDestino + "' AND " + "IdSistema='" + IdSistema + "' AND" +
                                            " IdNucleo='" + IdNucleo +"'"
                                            );
            else  // Sólo para la supervisión de portadora
                Consulta.Append("UPDATE DestinosRadioSector SET IdSistema='" + IdSistema + "'," +
                                            "IdSector='" + IdSector + "'," +
                                            "IdNucleo='" + IdNucleo + "'," +
                                            "IdDestino='" + IdDestino + "'," +
                                            "TipoDestino=" + TipoDestino + "," +
                                            "SupervisionPortadora=" + SupervisionPortadora +
                                            " WHERE IdDestino='" + IdDestino + "' AND " + "IdSector='" + IdSector + "' AND " + "IdSistema='" + IdSistema + "' AND" +
                                                    " IdNucleo='" + IdNucleo + "' AND TipoDestino=" + TipoDestino
                                            );


			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "DestinosRadioSector");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
			if (IdSistema != null && IdDestino != null && IdSector != null && PosHMI != 0)
				Consulta.Append("DELETE FROM DestinosRadioSector WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "' AND IdSector='" + IdSector + "' AND PosHMI=" + PosHMI);
			else if (IdSistema != null && IdDestino != null && IdSector != null)
				Consulta.Append("DELETE FROM DestinosRadioSector WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "' AND IdSector='" + IdSector + "'");
			else if (IdSistema != null && IdSector != null)
				Consulta.Append("DELETE FROM DestinosRadioSector WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "'");
			else if (IdSistema != null && IdDestino != null)
				Consulta.Append("DELETE FROM DestinosRadioSector WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "'");
			else
				Consulta.Append("DELETE FROM DestinosRadioSector");


			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "DestinosRadioSector");
			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM DestinosRadioSector WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}

        public string SelectCountDistinctDestinoSQL()
        {
            // Cuenta los enlaces radio distintos de un sector que tengan configurado un recurso
            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("SELECT COUNT(DISTINCT a.IdDestino) FROM DestinosRadioSector a, RecursosRadio b WHERE " +
                                "a.IdSistema='" + IdSistema + "' AND a.IdSector='" + IdSector + "' AND a.IdDestino=b.IdDestino");
            return Consulta.ToString();
        }
    }
}
