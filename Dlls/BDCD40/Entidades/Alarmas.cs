using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace CD40.BD.Entidades
{
	public class Alarmas : Tablas
	{
        #region Propiedades de Alarmas

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

		private bool _Alarma;
        public bool Alarma
        {
            get { return _Alarma; }
            set { _Alarma = value; }
        }

        #endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public Alarmas()
        {
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdIncidencia != 0)
                Consulta.Append("SELECT * FROM Alarmas WHERE IdSistema='" + IdSistema + "' AND IdIncidencia=" + IdIncidencia);
            else if (IdSistema != null)
                Consulta.Append("SELECT * FROM Alarmas WHERE IdSistema='" + IdSistema + "'");
            else
                Consulta.Append("SELECT * FROM Alarmas");

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
                    Alarmas r = new Alarmas();

                    r.IdIncidencia = (uint)dr["IdIncidencia"];
                    r.IdSistema = (string)dr["IdSistema"];
                    r.Alarma = (bool)dr["Alarma"];  //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0;

                    ListaResultado.Add(r);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO Alarmas (IdSistema,IdIncidencia,Alarma)" +
                            " VALUES ('" + IdSistema + "'," +
                                         IdIncidencia + "," +
                                         Alarma + ")");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Alarmas");
			return consulta;
        }

        public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];

			Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE Alarmas SET IdIncidencia=" + IdIncidencia + "," +
                                            "IdSistema='" + IdSistema + "'," +
                                            "Alarma=" + Alarma + " " +
                                            "WHERE IdIncidencia=" + IdIncidencia + " AND " + "IdSistema='" + IdSistema + "'"
                                            );

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Alarmas");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];

			Consulta.Remove(0, Consulta.Length);
			if (IdSistema != null && IdIncidencia != 0)
				Consulta.Append("DELETE FROM Alarmas WHERE IdSistema='" + IdSistema + "' AND IdIncidencia=" + IdIncidencia);
			else if (IdSistema != null)
				Consulta.Append("DELETE FROM Alarmas WHERE IdSistema='" + IdSistema + "'");
			else
				Consulta.Append("DELETE FROM Alarmas");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Alarmas");
			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM Alarmas WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
	}
}
