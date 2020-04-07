using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
	public class EstadoRecursos : Tablas
	{
        #region Propiedades de EstadoRecursos

        private string _IdSistema;
        public string IdSistema
        {
            get { return _IdSistema; }
            set { _IdSistema = value; }
        }

        private string _IdDestino;
        public string IdDestino
        {
            get { return _IdDestino; }
            set { _IdDestino = value; }
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

		private string _IdSectorizacion;
		public string IdSectorizacion
		{
			get { return _IdSectorizacion; }
			set { _IdSectorizacion = value; }
		}

		private uint _PosHMI;
        public uint PosHMI
        {
            get { return _PosHMI; }
            set { _PosHMI = value; }
        }

        private string _IdRecurso;
		public string IdRecurso
        {
			get { return _IdRecurso; }
			set { _IdRecurso = value; }
        }

        private string _Estado;
        public string Estado
        {
            get { return _Estado; }
            set { _Estado = value; }
        }

        #endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public EstadoRecursos()
        {
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
			if (IdSistema != null && IdDestino != null && IdSector != null && IdNucleo != null && IdSectorizacion != null && IdRecurso != null)
				Consulta.Append("SELECT * FROM EstadoRecursos WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + 
									"' AND IdRecurso='" + IdRecurso + "' AND IdSectorizacion='" + IdSectorizacion + "'");
			else
				Consulta.Append("SELECT * FROM EstadoRecursos");

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
                    EstadoRecursos r = new EstadoRecursos();

                    if (dr["IdSistema"] != System.DBNull.Value)
                        r.IdSistema = (string)dr["IdSistema"];
                    if (dr["IdDestino"] != System.DBNull.Value)
                        r.IdDestino = (string)dr["IdDestino"];
                    if (dr["IdSectorizacion"] != System.DBNull.Value)
						r.IdSectorizacion = (string)dr["IdSectorizacion"];
                    if (dr["IdNucleo"] != System.DBNull.Value)
                        r.IdNucleo = (string)dr["IdNucleo"];
                    if (dr["IdSector"] != System.DBNull.Value)
                        r.IdSector = (string)dr["IdSector"];
                    if (dr["IdRecurso"] != System.DBNull.Value)
						r.IdRecurso = (string)dr["IdRecurso"];
                    if (dr["PosHMI"] != System.DBNull.Value)
                        r.PosHMI = (uint)dr["PosHMI"];
                    if (dr["Estado"] != System.DBNull.Value)
                        r.Estado = (string)dr["Estado"];

                    ListaResultado.Add(r);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
			Consulta.Append("INSERT INTO EstadoRecursos VALUES ('" + IdDestino + "'," +
														 PosHMI + ",'" +
														 IdSector + "','" +
														 IdNucleo + "','" +
														 IdSectorizacion + "','" +
														 IdSistema + "','" +
														 IdRecurso + "','" +
                                                         Estado + "')");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "EstadoRecursos");
            return consulta;
        }

        public override string[] UpdateSQL()
        {
            return null;
        }

        public override string[] DeleteSQL()
        {
			return null;
		}

		//public override string[] SelectCountSQL(string where)
		//{
		//    return null;
		//}
	}
}
