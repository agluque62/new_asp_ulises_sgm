using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace CD40.BD.Entidades
{
	public class HistoricoIncidencias : Tablas
	{
        #region Propiedades de HistoricoIncidencias

        private uint _IdIncidencia;
		public uint IdIncidencia
        {
			get { return _IdIncidencia; }
			set { _IdIncidencia = value; }
        }

        private string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }

		private int _scv;
		public int Scv
		{
			get { return _scv; }
			set { _scv = value; }
		}

		private string _IdHw;
		public string IdHw
		{
			get { return _IdHw; }
			set { _IdHw = value; }
		}

		private Tipos.Tipo_Elemento_HW _TipoHw;
		public Tipos.Tipo_Elemento_HW TipoHw
		{
			get { return _TipoHw; }
			set { _TipoHw = value; }
		}

		private DateTime _FechaHora;
		public DateTime FechaHora
		{
			get { return _FechaHora; }
			set { _FechaHora = value; }
		}

		private DateTime _Reconocida;
		public DateTime Reconocida
		{
			get { return _Reconocida; }
			set { _Reconocida = value; }
		}

		private string _Descripcion;
		public string Descripcion
		{
			get { return _Descripcion; }
			set { _Descripcion = value; }
		}


		#endregion

        public HistoricoIncidencias()
        {
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
			if (IdSistema != null && IdHw != null && Scv != 0 && FechaHora != null)
				Consulta.Append("SELECT * FROM HistoricoIncidencias WHERE IdSistema='" + IdSistema + "' AND IdHw='" + IdHw + "' AND Scv=" + Scv + " AND FechaHora='" + FechaHora.ToString() + "'");
			else if (IdSistema != null && IdHw != null && Scv != 0)
				Consulta.Append("SELECT * FROM HistoricoIncidencias WHERE IdSistema='" + IdSistema + "' AND IdHw='" + IdHw + "' AND Scv=" + Scv);
			else if (IdSistema != null && Scv!=0)
				Consulta.Append("SELECT * FROM HistoricoIncidencias WHERE IdSistema='" + IdSistema + "' AND Scv=" + Scv);
			else if (IdSistema != null)
				Consulta.Append("SELECT * FROM HistoricoIncidencias WHERE IdSistema='" + IdSistema + "'");
			else
				Consulta.Append("SELECT * FROM HistoricoIncidencias");

            return Consulta.ToString();
        }

		public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds)
        {
            ListaResultado.Clear();

            if (ds != null)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    HistoricoIncidencias s = new HistoricoIncidencias();

                    s.IdIncidencia = (uint)dr["IdIncidencia"];
                    s.IdSistema = (string)dr["IdSistema"];
                    s.IdHw = (string)dr["IdHw"];
					s.Scv = (int)dr["Scv"];
					s.TipoHw = (Tipos.Tipo_Elemento_HW)dr["TipoHw"];
					s.FechaHora = (DateTime)dr["FechaHora"];
					s.Reconocida = (DateTime)dr["Reconocida"];
					if (dr["Descripcion"] != System.DBNull.Value)
						s.Descripcion = (string)dr["Descripcion"];

                    ListaResultado.Add(s);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
			string[] consulta = new string[1];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO HistoricoIncidencias SET " + 
								"IdSistema='" + IdSistema + "'," +
								"Scv=" + Scv + "," +
								"IdHw='" + IdHw + "'," +
								"TipoHw=" + (uint)TipoHw + "," +
								"IdIncidencia='" + IdIncidencia + "'," +
								"FechaHora=NOW() " + "," +
								"Descripcion='" + Descripcion + "'");

			consulta[0] = Consulta.ToString();
            return consulta;
        }

        public override string[] UpdateSQL()
        {
			string[] consulta = new string[1];

			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("UPDATE HistoricoIncidencias SET Reconocida=NOW() " +
											"WHERE IdIncidencia=" + IdIncidencia + " AND " + "IdSistema='" + IdSistema + "' AND IdHw='" + IdHw + "' AND TipoHw=" + (uint)TipoHw + " ORDER BY -FechaHora LIMIT 1");

			consulta[0] = Consulta.ToString();
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[1];

			Consulta.Remove(0, Consulta.Length);
			if (IdSistema != null && IdHw != null && Scv != 0 && FechaHora != null)
				Consulta.Append("DELETE FROM HistoricoIncidencias WHERE IdSistema='" + IdSistema + "' AND IdHw='" + IdHw + "' AND Scv=" + Scv + " AND FechaHora='" + FechaHora.ToString() + "'");
			else if (IdSistema != null && IdHw != null && Scv != 0)
				Consulta.Append("DELETE FROM HistoricoIncidencias WHERE IdSistema='" + IdSistema + "' AND IdHw='" + IdHw + "' AND Scv=" + Scv);
			else if (IdSistema != null && Scv != 0)
				Consulta.Append("DELETE FROM HistoricoIncidencias WHERE IdSistema='" + IdSistema + "' AND Scv=" + Scv);
			else if (IdSistema != null)
				Consulta.Append("DELETE FROM HistoricoIncidencias WHERE IdSistema='" + IdSistema + "'");
			else
				Consulta.Append("DELETE FROM HistoricoIncidencias");

			consulta[0] = Consulta.ToString();
			return consulta;
		}

        public override string DataSetComponentsSQL(DateTime desde, DateTime hasta)
        {
            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("SELECT DISTINCT(IdHw),TipoHw FROM HistoricoIncidencias WHERE DATE_FORMAT(FechaHora,'%Y/%m/%d')>='" + desde.GetDateTimeFormats()[48].ToString() + "' AND " +
                                                                                        "DATE_FORMAT(FechaHora,'%Y/%m/%d')<'" + hasta.GetDateTimeFormats()[48].ToString() + "'");

            return Consulta.ToString();
        }

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM HistoricoIncidencias WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
	}
}
