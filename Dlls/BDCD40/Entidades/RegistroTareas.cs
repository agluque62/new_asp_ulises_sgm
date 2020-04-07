using System;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
	public class RegistroTareas : Tablas
	{
	    #region Propiedades de RegistroTareas

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

		private DateTime _Ejecutada;
		public DateTime Ejecutada
		{
			get { return _Ejecutada; }
			set { _Ejecutada = value; }
		}

		private int _Resultado;
		public int Resultado
		{
			get { return _Resultado; }
			set { _Resultado = value; }
		}

		private string _Comentario;
		public string Comentario
		{
			get { return _Comentario; }
			set { _Comentario = value; }
		}
		#endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

		public RegistroTareas()
		{
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
		}

		public RegistroTareas(string cadenaConexion)
		{
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos(cadenaConexion);
		}

		public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
			if (IdSistema != null && IdTarea != null && Ejecutada != null)
				Consulta.Append("SELECT * FROM RegistroTareas WHERE IdSistema='" + IdSistema + " AND IdTarea='" + IdTarea + "' AND DATE_FORMAT(Ejecutada,'%d/%m/%Y')='" + Ejecutada.Date.ToString("dd/MM/yyyy") + "'");
			else if (IdSistema != null && IdTarea != null)
				Consulta.Append("SELECT * FROM RegistroTareas WHERE IdSistema='" + IdSistema + "' AND IdTarea='" + IdTarea + "'");
            else if (IdSistema != null)
                Consulta.Append("SELECT * FROM RegistroTareas WHERE IdSistema='" + IdSistema + "'");
            else
                Consulta.Append("SELECT * FROM RegistroTareas");

            return Consulta.ToString();
        }

        public override System.Collections.Generic.List<Tablas> ListSelectSQL(System.Data.DataSet ds)
        {
            ListaResultado.Clear();

			//DataSetResultado = this.DataSetSelectSQL();
            if (ds != null)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    RegistroTareas s = new RegistroTareas();

                    s.IdTarea = (string)dr["IdTarea"];
                    s.IdSistema = (string)dr["IdSistema"];
					s.Ejecutada = (DateTime)dr["Ejecutada"];
					s.Resultado = (int)dr["Resultado"];
					if (dr["Comentario"] != System.DBNull.Value)
						s.Comentario = (string)dr["Comentario"];

                    ListaResultado.Add(s);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
			string[] consulta = new string[1];

            Consulta.Remove(0, Consulta.Length);
			Consulta.Append("INSERT INTO RegistroTareas (IdSistema,IdTarea,Ejecutada,Resultado,Comentario)" +
							" VALUES ('" + IdSistema + "','" +
										IdTarea + "','" +
										Ejecutada.ToString("yyyy/MM/dd HH:mm:ss") + "'," +
										Resultado + ",'" +
										Comentario + "'" + ")");

			consulta[0] = Consulta.ToString();
            return consulta;
        }

		public override string[] UpdateSQL()
        {
			/*
            Consulta.Remove(0, Consulta.Length);
			Consulta.Append("UPDATE RegistroTareas SET Duracion=NOW() " +
											"WHERE IdTarea=" + IdTarea + " AND " + "IdSistema='" + IdSistema + "' AND IdHw='" + IdHw + "' AND TipoHw=" + (uint)TipoHw + " ORDER BY -Ejecutada LIMIT 1");

            return AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
			 */
			return null;
        }

		public override string[] DeleteSQL()
        {
			string[] consulta = new string[1];
			
			Consulta.Remove(0, Consulta.Length);
			if (IdSistema != null && IdTarea != null && Ejecutada != null)
				Consulta.Append("DELETE FROM RegistroTareas WHERE IdSistema='" + IdSistema + "' AND IdTarea='" + IdTarea + "' AND DATE_FORMAT(Ejecutada,'%d/%m/%Y')='" + Ejecutada.Date.ToString("dd/MM/yyyy") + "'");
			else if (IdSistema != null && IdTarea != null)
				Consulta.Append("DELETE FROM RegistroTareas WHERE IdSistema='" + IdSistema + "' AND IdTarea='" + IdTarea + "'");
			else if (IdSistema != null)
				Consulta.Append("DELETE FROM RegistroTareas WHERE IdSistema='" + IdSistema + "'");
			else
				Consulta.Append("DELETE FROM RegistroTareas");

			consulta[0] = Consulta.ToString();
			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM RegistroTareas WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
	}
}
