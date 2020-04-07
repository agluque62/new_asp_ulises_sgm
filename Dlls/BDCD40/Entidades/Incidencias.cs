using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace CD40.BD.Entidades
{
	public class Incidencias : Tablas
	{
        #region Propiedades de Incidencias

		private uint _IdIncidencia;
		public uint IdIncidencia
		{
			get { return _IdIncidencia; }
			set { _IdIncidencia = value; }
		}

		private uint _IdIncidenciaCorrectora;
		public uint IdIncidenciaCorrectora
		{
			get { return _IdIncidenciaCorrectora; }
			set { _IdIncidenciaCorrectora = value; }
		}

		private string _Incidencia;
		public string Incidencia
		{
			get { return _Incidencia; }
			set { _Incidencia = value; }
		}

		private string _Descripcion;
		public string Descripcion
		{
			get { return _Descripcion; }
			set { _Descripcion = value; }
		}

		private bool _GeneraError;
		public bool GeneraError
		{
			get { return _GeneraError; }
			set { _GeneraError = value; }
		}

        private string _Oid;
        public string Oid
        {
            get { return _Oid; }
            set { _Oid = value; }
        }

        #endregion

		private string TablaIncidencias;

        public Incidencias()
        {
//            if (ServiceAccesoABaseDeDatos == null)
//                ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
			TablaIncidencias = "Incidencias";
        }

		public Incidencias(string idioma)
		{
            switch (idioma)
            {
                case "es":
                    TablaIncidencias = "Incidencias";
                    break;
                case "en":
                     TablaIncidencias= "Incidencias_Ingles";
                     break;
                case "fr":
                     TablaIncidencias = "Incidencias_Frances";
                     break;
            }
		}


        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
			if (IdIncidencia != 0)
                Consulta.Append("SELECT * FROM " + TablaIncidencias + " WHERE IdIncidencia=" + IdIncidencia);
            else
                Consulta.Append("SELECT * FROM " + TablaIncidencias);

            return Consulta.ToString();
        }

        public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds)
        {
            ListaResultado.Clear();

            if (ds != null)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    Incidencias a = new Incidencias();

                    a.IdIncidencia = (uint)dr["IdIncidencia"];
					if (dr["IdIncidenciaCorrectora"]!=System.DBNull.Value)
						a.IdIncidenciaCorrectora = (uint)dr["IdIncidenciaCorrectora"];
					if (dr["Incidencia"] != System.DBNull.Value)
						a.Incidencia = (string)dr["Incidencia"];
					if (dr["Descripcion"] != System.DBNull.Value)
						a.Descripcion = (string)dr["Descripcion"];
					if (dr["GeneraError"] != System.DBNull.Value)
                        a.GeneraError = (bool)dr["GeneraError"];    //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0;
                    if (dr["OID"] != System.DBNull.Value)
                        a.Oid = (string)dr["OID"];

                    ListaResultado.Add(a);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
			string[] consulta = new string[1];

            Consulta.Remove(0, Consulta.Length);
			Consulta.Append("INSERT INTO " + TablaIncidencias + " SET SET IdIncidencia=" + IdIncidencia + "," +
															"IdIncidenciaCorrectora=" + IdIncidenciaCorrectora + "," +
															"Incidencia='" + Incidencia + "', " +
															"Descripcion='" + Descripcion + "', " +
															"GeneraError=" + GeneraError);

			consulta[0] = Consulta.ToString();
            return consulta;
        }

        public override string[] UpdateSQL()
        {
			string[] consulta = new string[1];

			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("UPDATE " + TablaIncidencias + " SET IdIncidencia=" + IdIncidencia + "," +
												"IdIncidenciaCorrectora=" + IdIncidenciaCorrectora + "," +
												"Incidencia='" + Incidencia + "', " +
												"Descripcion='" + Descripcion + "', " +
												"GeneraError=" + GeneraError + " " +
												"WHERE IdIncidencia=" + IdIncidencia);

			consulta[0] = Consulta.ToString();
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[1];

			Consulta.Remove(0, Consulta.Length);
			if (Descripcion != null)
				Consulta.Append("DELETE FROM " + TablaIncidencias + " WHERE Descripcion='" + Descripcion + "'");
            else
				Consulta.Append("DELETE FROM  " + TablaIncidencias);

			consulta[0] = Consulta.ToString();
			return consulta;
		}

        public override string DataSetComponentsSQL(DateTime d, DateTime f)
        {
            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("SELECT IdIncidencia,Incidencia FROM " + TablaIncidencias + " AS i WHERE i.GeneraError=1 AND i.IdIncidenciaCorrectora is not null");

            return Consulta.ToString();
        }


		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM Incidencias WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
	}
}
