using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class Internos : Tablas
    {
        #region Propiedades de Internos

        protected string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }

        protected string _IdSectorizacion;
        public string IdSectorizacion
        {
            get { return _IdSectorizacion; }
            set { _IdSectorizacion = value; }
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

        protected string _IdDestino;
        public string IdDestino
        {
            get { return _IdDestino; }
            set { _IdDestino = value; }
        }

        protected uint _IdPrefijo;
        public uint IdPrefijo
        {
            get { return _IdPrefijo; }
            set { _IdPrefijo = value; }
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

        protected string _OrigenR2;
        public string OrigenR2
        {
            get { return _OrigenR2; }
            set { _OrigenR2 = value; }
        }

        protected uint _PrioridadSIP;
        public uint PrioridadSIP
        {
            get { return _PrioridadSIP; }
            set { _PrioridadSIP = value; }
        }

        protected string _TipoAcceso;
        public string TipoAcceso
        {
            get { return _TipoAcceso; }
            set { _TipoAcceso = value; }
        }

        protected string _Literal;
        public string Literal
        {
            get { return _Literal; }
            set { _Literal = value; }
        }

        #endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public Internos()
        {
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdSectorizacion != null && IdSector != null && IdNucleo != null)
                Consulta.Append("SELECT * FROM Internos WHERE IdSistema='" + IdSistema + "' AND IdSectorizacion='" + IdSectorizacion + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "'");
            else if (IdSistema != null && IdSector != null && IdSectorizacion != null)
                Consulta.Append("SELECT * FROM Internos WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdSectorizacion='" + IdSectorizacion + "'");
            else if (IdSistema != null && IdSector != null && IdNucleo != null && PosHMI != 0)
                Consulta.Append("SELECT * FROM Internos WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND PosHMI=" + PosHMI + " AND IdNucleo='" + IdNucleo + "'");
            else if (IdSistema != null && IdSector != null && IdNucleo != null)
                Consulta.Append("SELECT * FROM Internos WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "'");
			else if (IdSistema != null && IdSectorizacion != null)
				Consulta.Append("SELECT * FROM Internos WHERE IdSistema='" + IdSistema + "' AND IdSectorizacion='" + IdSectorizacion + "'");
			else if (IdSistema != null)
				Consulta.Append("SELECT * FROM Internos WHERE IdSistema='" + IdSistema + "'");
			else
                Consulta.Append("SELECT * FROM Internos");

            return Consulta.ToString();
        }

        public override List<Tablas> ListSelectSQL(DataSet ds)
        {
            ListaResultado.Clear();

			//DataSetResultado = this.DataSetSelectSQL();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    Internos r = new Internos();

                    if (dr["IdSistema"] != System.DBNull.Value)
                        r.IdSistema = (string)dr["IdSistema"];
                    if (dr["IdSectorizacion"] != System.DBNull.Value)
                        r.IdSectorizacion = (string)dr["IdSectorizacion"];
                    if (dr["IdNucleo"] != System.DBNull.Value)
                        r.IdNucleo = (string)dr["IdNucleo"];
                    if (dr["IdSector"] != System.DBNull.Value)
                        r.IdSector = (string)dr["IdSector"];
                    if (dr["IdDestino"] != System.DBNull.Value)
                        r.IdDestino = (string)dr["IdDestino"];
                    if (dr["IdPrefijo"] != System.DBNull.Value)
                        r.IdPrefijo = (uint)dr["IdPrefijo"];
                    if (dr["PosHMI"] != System.DBNull.Value)
                        r.PosHMI = (uint)dr["PosHMI"];
                    if (dr["Prioridad"] != System.DBNull.Value)
                        r.Prioridad = (uint)dr["Prioridad"];
                    if (dr["OrigenR2"] != System.DBNull.Value)
                        r.OrigenR2 = (string)dr["OrigenR2"];
                    if (dr["PrioridadSIP"] != System.DBNull.Value)
                        r.PrioridadSIP = (uint)dr["PrioridadSIP"];
                    if (dr["TipoAcceso"] != System.DBNull.Value)
                        r.TipoAcceso = (string)dr["TipoAcceso"];
                    if (dr["Literal"] != System.DBNull.Value)
                        r.Literal = (string)dr["Literal"];

                    ListaResultado.Add(r);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO Internos (IdSistema,IdSectorizacion,IdNucleo,IdSector,IdDestino,IdPrefijo,PosHMI,Prioridad,OrigenR2,PrioridadSIP,TipoAcceso,Literal)" +
                            " VALUES ('" + IdSistema + "','" +
                                         IdSectorizacion + "'," +
                                         IdNucleo + "','" +
                                         IdSector + "','" +
                                         IdDestino + "'," +
                                         IdPrefijo + ",'" +
                                         TipoAcceso + "'," +
                                         PosHMI + "," +
                                         Prioridad + ",'" +
                                         OrigenR2 + "'," +
                                         PrioridadSIP + ",'" +
                                         Literal + "')");
			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Internos");
			return consulta;
        }

		public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE Internos SET IdSistema='" + IdSistema + "'," +
                                            "IdSectorizacion='" + IdSectorizacion + "'," +
                                            "IdNucleo='" + IdNucleo + "'," +
                                            "IdSector='" + IdSector + "'," +
                                            "IdDestino='" + IdDestino + "'," +
                                            "IdPrefijo=" + IdPrefijo + "," +
                                            "PosHMI=" + PosHMI + "," +
                                            "Prioridad=" + Prioridad + "," +
                                            "OrigenR2='" + OrigenR2 + "'," +
                                            "PrioridadSIP=" + PrioridadSIP + "," +
                                            "TipoAcceso='" + TipoAcceso + "'," +
                                            "Literal='" + Literal + "' " +
                                            "WHERE IdSectorizacion='" + IdSectorizacion + "' AND IdSector='" + IdSector + "' AND IdSistema='" + IdSistema + "'"
                                            );

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Internos");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];
			
			Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdSectorizacion != null && IdSector != null)
                Consulta.Append("DELETE FROM Internos WHERE IdSistema='" + IdSistema + "' AND IdSectorizacion='" + IdSectorizacion + "' AND IdSector='" + IdSector + "' AND TipoAcceso='" + TipoAcceso + "'");
            else if (IdSistema != null && IdSectorizacion != null)
                Consulta.Append("DELETE FROM Internos WHERE IdSistema='" + IdSistema + "' AND IdSectorizacion='" + IdSectorizacion + "'");
            else
                Consulta.Append("DELETE FROM Internos");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Internos");
			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM Internos WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
    }
}
