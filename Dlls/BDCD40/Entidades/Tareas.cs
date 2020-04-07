using System;
using System.Collections.Generic;
using System.Text;
using System.Data;


namespace CD40.BD.Entidades
{
	public class Tareas : Tablas
	{
		public enum _Enum_Periodicidad { D = 'D', M = 'M', S = 'S' };

        #region Propiedades de Tareas

        private string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }

		private string _IdTarea;
		public string IdTarea
		{
			get { return _IdTarea; }
			set { _IdTarea = value; }
		}

		private string _Programa;
		public string Programa
		{
			get { return _Programa; }
			set { _Programa = value; }
		}

		private string _Argumentos;
		public string Argumentos
		{
			get { return _Argumentos; }
			set { _Argumentos = value; }
		}

		private string _Hora;
		public string Hora
		{
			get { return _Hora; }
			set { _Hora = value; }
		}

		private _Enum_Periodicidad _Periodicidad;
		public _Enum_Periodicidad Periodicidad
		{
			get { return _Periodicidad; }
			set { _Periodicidad = value; }
		}

		private int _Intervalo;
		public int Intervalo 
		{
			get { return _Intervalo; }
			set { _Intervalo = value; }
		}

		private string _Comentario;
		public string Comentario
		{
			get { return _Comentario; }
			set { _Comentario = value; }
		}

		#endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public Tareas()
        {
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
			if (IdSistema != null && IdTarea != null)
				Consulta.Append("SELECT * FROM Tareas WHERE IdSistema='" + IdSistema + "' AND IdTarea='" + IdTarea + "'");
            else if (IdSistema != null)
                Consulta.Append("SELECT * FROM Tareas WHERE IdSistema='" + IdSistema + "'");
            else
                Consulta.Append("SELECT * FROM Tareas");

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
                    Tareas s = new Tareas();

                    s.IdSistema = (string)dr["IdSistema"];
                    s.IdTarea = (string)dr["IdTarea"];
					s.Programa = (string)dr["Programa"];
					if (dr["Argumentos"] != System.DBNull.Value)
						s.Argumentos = (string)dr["Argumentos"];
					else
						s.Argumentos = "";
					s.Hora = ((TimeSpan)dr["Hora"]).ToString();
					switch (dr["Periodicidad"].ToString())
					{
						case "S":
							s.Periodicidad = _Enum_Periodicidad.S;
							break;
						case "D":
							s.Periodicidad = _Enum_Periodicidad.D;
							break;
						default:
							s.Periodicidad = _Enum_Periodicidad.M;
							break;
					}
					if (dr["Intervalo"] != System.DBNull.Value)
						s.Intervalo = (int)dr["Intervalo"];
					else
						s.Intervalo = 1;

					if (dr["Comentario"] != System.DBNull.Value)
						s.Comentario = (string)dr["Comentario"];
					else
						s.Comentario = "";

                    ListaResultado.Add(s);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
			string[] consulta = new string[1];

            Consulta.Remove(0, Consulta.Length);
			Consulta.Append("INSERT INTO Tareas (IdSistema,IdTarea,Programa,Argumentos,Hora,Periodicidad,Intervalo,Comentario)" +
							" VALUES ('" + IdSistema + "','" +
										IdTarea + "','" +
										Programa + "'," +
										((Argumentos == null) ? "null" : ("'" + Argumentos + "','")) +
										Hora + "','" +
										Periodicidad.ToString() + "'," +
										Intervalo + ",'" +
										Comentario + "')");

			consulta[0] = Consulta.ToString();
            return consulta;
        }

		public override string[] UpdateSQL()
        {
			string[] consulta = new string[1];
			
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("UPDATE Tareas SET Periodicidad='" + Periodicidad.ToString() + "'," +
											"Hora='" + Hora + "',"+
											"Argumentos='" + Argumentos + "'," +
											"Intervalo=" + Intervalo + "," +
											"Comentario='" + Comentario + "' " +
											"WHERE IdSistema='" + IdSistema + "' AND IdTarea='" + IdTarea + "'");

			consulta[0] = Consulta.ToString();
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[1];

            Consulta.Remove(0, Consulta.Length);
			if (IdSistema != null && IdTarea != null)
				Consulta.Append("DELETE FROM Tareas WHERE IdSistema='" + IdSistema + "' AND IdTarea='" + IdTarea + "'");
			else if (IdSistema != null)
				Consulta.Append("DELETE FROM Tareas WHERE IdSistema='" + IdSistema + "'");
			else
				Consulta.Append("DELETE FROM Tareas");

			consulta[0] = Consulta.ToString();
			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM Tareas WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
	}
}
