using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class EstadosRecursos : Tablas
    {
        #region Propiedades de EstadosRecursos
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

        protected uint _PosHMI;
        public uint PosHMI
        {
            get { return _PosHMI; }
            set { _PosHMI = value; }
        }

        protected string _IdRecurso;
        public string IdRecurso
        {
            get { return _IdRecurso; }
            set { _IdRecurso = value; }
        }

        protected uint _TipoRecurso;
        public uint TipoRecurso
        {
            get { return _TipoRecurso; }
            set { _TipoRecurso = value; }
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

        private string _Estado;
        public string Estado
        {
            get { return _Estado; }
            set { _Estado = value; }
        }
        #endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public EstadosRecursos()
        {
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
			if (IdSistema != null && IdDestino != null && IdSector != null && IdNucleo != null && PosHMI != 0 && IdRecurso != null)
				Consulta.Append("SELECT * FROM EstadosRecursos WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "' AND PosHMI=" + PosHMI + " AND IdRecurso='" + IdRecurso + "'");
			else if (IdSistema != null && IdDestino != null && IdSector != null && IdNucleo != null && PosHMI != 0)
				Consulta.Append("SELECT * FROM EstadosRecursos WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "' AND PosHMI=" + PosHMI);
			else if (IdSistema != null && IdDestino != null && IdRecurso != null)
				Consulta.Append("SELECT * FROM EstadosRecursos WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "' AND IdRecurso='" + IdRecurso + "'");
			else
				Consulta.Append("SELECT * FROM EstadosRecursos");

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
                    EstadosRecursos r = new EstadosRecursos();

                    if (dr["IdSistema"] != System.DBNull.Value)
                        r.IdSistema = (string)dr["IdSistema"];
                    if (dr["IdSector"] != System.DBNull.Value)
                        r.IdSector = (string)dr["IdSector"];
                    if (dr["IdNucleo"] != System.DBNull.Value)
                        r.IdNucleo = (string)dr["IdNucleo"];
                    if (dr["PosHMI"] != System.DBNull.Value)
                        r.PosHMI = (uint)dr["PosHMI"];
                    if (dr["IdRecurso"] != System.DBNull.Value)
                        r.IdRecurso = (string)dr["IdRecurso"];
                    if (dr["TipoRecurso"] != System.DBNull.Value)
                        r.TipoRecurso = (uint)dr["TipoRecurso"];
                    if (dr["IdDestino"] != System.DBNull.Value)
                        r.IdDestino = (string)dr["IdDestino"];
                    if (dr["TipoDestino"] != System.DBNull.Value)
                        r.TipoDestino = (uint)dr["TipoDestino"];
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
            Consulta.Append("INSERT INTO EstadosRecursos (IdSistema,IdNucleo,IdSector,PosHMI,IdRecurso,TipoRecurso,IdDestino,TipoDestino,Estado)" +
                            " VALUES ('" + IdSistema + "','" +
                                         IdNucleo + "','" +
                                         IdSector + "'," +
                                         PosHMI + ",'" +
                                         IdRecurso + "'," +
                                         TipoRecurso + ",'" +
                                         IdDestino + "'," +
                                         TipoDestino + ",'" +
                                         Estado + "')");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "EstadosRecursos");
			return consulta;
        }

        public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];

			Consulta.Remove(0, Consulta.Length);

            if (IdSector != string.Empty && IdNucleo != string.Empty && PosHMI != 0)
                Consulta.Append("UPDATE EstadosRecursos SET IdSistema='" + IdSistema + "'," +
                                            "IdSector='" + IdSector + "'," +
                                            "IdNucleo='" + IdNucleo + "'," +
                                            //"PosHMI=" + PosHMI + "," +
                                            "IdRecurso='" + IdRecurso + "'," +
                                            "TipoRecurso=" + TipoRecurso + "," +
                                            "IdDestino='" + IdDestino + "'," +
                                            "TipoDestino=" + TipoDestino + "," +
                                            "Estado='" + Estado + "' " +
                                            "WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "' AND IdRecurso='" + IdRecurso + "'"
                                            );
            else
                Consulta.Append("UPDATE EstadosRecursos SET " +
                                            "Estado='" + Estado + "' " +
                                            "WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "' AND IdRecurso='" + IdRecurso + "'"
                                            );


			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "EstadosRecursos");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];
			
			Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdDestino != null && IdSector != null && IdNucleo != null && PosHMI != 0 && IdRecurso != null)
                Consulta.Append("DELETE FROM EstadosRecursos WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "' AND PosHMI=" + PosHMI + " AND IdRecurso='" + IdRecurso + "'");
			else if (IdSistema != null && IdDestino != null && IdRecurso != null)
				Consulta.Append("DELETE FROM EstadosRecursos WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "' AND IdRecurso='" + IdRecurso + "'");
			else if (IdSistema != null && IdDestino != null)
				Consulta.Append("DELETE FROM EstadosRecursos WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "'");
			else
                Consulta.Append("DELETE FROM EstadosRecursos");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "EstadosRecursos");
			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM EstadosRecursos WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
    }
}
