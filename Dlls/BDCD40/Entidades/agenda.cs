using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
	public class Agenda : Tablas
	{
		#region Propiedades de Agenda

		protected string _idSistema;
		public string IdSistema
		{
			get { return _idSistema; }
			set { _idSistema = value; }
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

		protected uint _Prefijo;
		public uint Prefijo
		{
			get { return _Prefijo; }
			set { _Prefijo = value; }
		}

		protected string _Numero;
		public string Numero
		{
			get { return _Numero; }
			set { _Numero = value; }
		}

		protected string _Nombre;
		public string Nombre
		{
			get { return _Nombre; }
			set { _Nombre = value; }
		}


		#endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

		public Agenda()
		{
			Prefijo = uint.MaxValue;
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
		}

		public override string DataSetSelectSQL()
		{
			Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdSector != null && IdNucleo != null && Prefijo!=uint.MaxValue && Nombre != null)
                Consulta.Append("SELECT * FROM Agenda WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "' AND Prefijo=" + Prefijo + 
                                " AND Nombre='" + Nombre + "' ORDER BY Nombre");
            else if (IdSistema != null && IdSector != null && IdNucleo != null)
                Consulta.Append("SELECT * FROM Agenda WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "' ORDER BY Nombre");
            else
				Consulta.Append("SELECT * FROM Agenda");

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
					Agenda r = new Agenda();

					if (dr["IdSistema"] != System.DBNull.Value)
						r.IdSistema = (string)dr["IdSistema"];
					if (dr["IdNucleo"] != System.DBNull.Value)
						r.IdNucleo = (string)dr["IdNucleo"];
					if (dr["IdSector"] != System.DBNull.Value)
						r.IdSector = (string)dr["IdSector"];
					if (dr["Prefijo"] != System.DBNull.Value)
						r.Prefijo = (uint)dr["Prefijo"];
					if (dr["Numero"] != System.DBNull.Value)
						r.Numero = (string)dr["Numero"];
					if (dr["Nombre"] != System.DBNull.Value)
						r.Nombre = (string)dr["Nombre"];

					ListaResultado.Add(r);
				}
			}
			return ListaResultado;
		}

		public override string[] InsertSQL()
		{
			string[] consulta = new string[2];

			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("INSERT INTO Agenda (IdSistema,IdNucleo,IdSector,Prefijo,Numero,Nombre)" +
							" VALUES ('" + IdSistema + "','" +
										 IdNucleo + "','" +
										 IdSector + "'," +
										 Prefijo + ",'" +
										 Numero + "','" +
										 Nombre + "')");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Agenda");
			return consulta;
		}

		public override string[] UpdateSQL()
		{
			string[] consulta = new string[2];

			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("UPDATE Agenda SET IdSistema='" + IdSistema + "'," +
											"IdNucleo='" + IdNucleo + "'," +
											"IdSector='" + IdSector + "'," +
											"Prefijo=" + Prefijo + "," +
											"Numero='" + Numero + "'," +
											"Nombre='" + Nombre + "' " +
											"WHERE IdSector='" + IdSector + "' AND IdSistema='" + IdSistema + "' AND IdNucleo='" + IdNucleo + "'"
											);

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Agenda");
	
			return consulta;
		}

		public override string[] DeleteSQL()
		{
			string[] consulta = new string[2];

			Consulta.Remove(0, Consulta.Length);
			if (IdSistema != null && IdSector != null && IdNucleo != null)
				Consulta.Append("DELETE FROM Agenda WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "'");
			else
				Consulta.Append("DELETE FROM Agenda");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Agenda");

			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM Agenda WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
	}
}
