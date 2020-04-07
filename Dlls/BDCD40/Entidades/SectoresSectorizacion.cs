using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class SectoresSectorizacion : Tablas
    {
        #region Propiedades de SectoresSectorizacion

        private string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }

        private string _IdSectorizacion;
        public string IdSectorizacion
        {
            get { return _IdSectorizacion; }
            set { _IdSectorizacion = value; }
        }

        private string _IdNucleo;
        public string IdNucleo
        {
            get { return _IdNucleo; }
            set { _IdNucleo = value; }
        }

        private string _IdSector;
        public string IdSector
        {
            get { return _IdSector; }
            set { _IdSector = value; }
        }

        private string _IdTop;
        public string IdTop
        {
            get { return _IdTop; }
            set { _IdTop = value; }
        }

        #endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public SectoresSectorizacion()
        {
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
			if (IdSistema != null && IdSector != null && IdNucleo != null && IdSectorizacion != null)
				Consulta.Append("SELECT * FROM SectoresSectorizacion WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "' AND IdSectorizacion='" + IdSectorizacion + "'");
            else if (IdSistema != null && IdSectorizacion != null && IdNucleo != null)
                Consulta.Append("SELECT * FROM SectoresSectorizacion WHERE IdSistema='" + IdSistema + "' AND IdSectorizacion='" + IdSectorizacion + "' AND IdNucleo='" + IdNucleo + "'");
            else if (IdSistema != null && IdSector != null && IdNucleo != null)
				Consulta.Append("SELECT * FROM SectoresSectorizacion WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "'");
            else if (IdSistema != null && IdSectorizacion != null)
                Consulta.Append("SELECT * FROM SectoresSectorizacion WHERE IdSistema='" + IdSistema + "' AND IdSectorizacion='" + IdSectorizacion + "'");
            else if (IdSistema != null)
				Consulta.Append("SELECT * FROM SectoresSectorizacion WHERE IdSistema='" + IdSistema + "'");
			else
				Consulta.Append("SELECT * FROM SectoresSectorizacion");

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
                    SectoresSectorizacion r = new SectoresSectorizacion();

                    r.IdSistema = (string)dr["IdSistema"];
                    r.IdSectorizacion = (string)dr["IdSectorizacion"];
                    r.IdNucleo = (string)dr["IdNucleo"];
                    r.IdSector = (string)dr["IdSector"];
                    if (dr["IdTOP"] != System.DBNull.Value)
                        r.IdTop = (string)dr["IdTOP"];

                    ListaResultado.Add(r);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO SectoresSectorizacion (IdSistema,IdSectorizacion,IdNucleo,IdSector,IdTOP)" +
                            " VALUES ('" + IdSistema + "','" +
                                         IdSectorizacion + "','" +
                                         IdNucleo + "','" +
                                         IdSector + "','" +
                                         IdTop + "')");

			consulta[0]=Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "SectoresSectorizacion");
			return consulta;
        }

		public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];

			Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE SectoresSectorizacion SET IdSistema='" + IdSistema + "'," +
                                            "IdNucleo='" + IdNucleo + "'," +
                                            "IdSectorizacion='" + IdSectorizacion + "'," +
                                            "IdSector='" + IdSector + "'," +
                                            "IdTOP='" + IdTop + "' " +
                                            "WHERE IdSistema='" + IdSistema + "' AND IdSectorizacion='" + IdSectorizacion + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "'"
                                            );

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "SectoresSectorizacion");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];
			
			Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdSector != null && IdSectorizacion != null && IdNucleo != null)
                Consulta.Append("DELETE FROM SectoresSectorizacion WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdSectorizacion='" + IdSectorizacion + "' AND IdNucleo='" + IdNucleo + "'");
            else if (IdSistema != null && IdSectorizacion != null && IdTop != null)
                Consulta.Append("DELETE FROM SectoresSectorizacion WHERE IdSistema='" + IdSistema + "' AND IdTop='" + IdTop + "' AND IdSectorizacion='" + IdSectorizacion + "'");
            else if (IdSistema != null && IdSectorizacion != null)
                Consulta.Append("DELETE FROM SectoresSectorizacion WHERE IdSistema='" + IdSistema + "' AND IdSectorizacion='" + IdSectorizacion + "'");
            else if (IdSistema != null)
                Consulta.Append("DELETE FROM SectoresSectorizacion WHERE IdSistema='" + IdSistema + "'");
            else
                Consulta.Append("DELETE FROM SectoresSectorizacion");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "SectoresSectorizacion");
			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM SectoresSectorizacion WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
    }
}
