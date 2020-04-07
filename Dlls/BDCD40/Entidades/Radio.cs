using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class Radio : Tablas
    {
        #region Propiedades de Radio

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

        protected uint _PosHMI;
        public uint PosHMI
        {
            get { return _PosHMI; }
            set { _PosHMI = value; }
        }

        protected string _IdDestino;
        public string IdDestino
        {
            get { return _IdDestino; }
            set { _IdDestino = value; }
        }

        protected uint _Prioridad;
        public uint Prioridad
        {
            get { return _Prioridad; }
            set { _Prioridad = value; }
        }

        protected uint _PrioridadSIP;
        public uint PrioridadSIP
        {
            get { return _PrioridadSIP; }
            set { _PrioridadSIP = value; }
        }

        protected string _ModoOperacion;
        public string ModoOperacion
        {
            get { return _ModoOperacion; }
            set { _ModoOperacion = value; }
        }

        protected string _Cascos;
        public string Cascos
        {
            get { return _Cascos; }
            set { _Cascos = value; }
        }

        protected string _Literal;
        public string Literal
        {
            get { return _Literal; }
            set { _Literal = value; }
        }

        private bool _SupervisionPortadora;
        public bool SupervisionPortadora
        {
            get { return _SupervisionPortadora; }
            set { _SupervisionPortadora = value; }
        }
        #endregion

        public Radio()
        {
        }

        public override string DataSetSelectSQL()
        {
            StringBuilder strConsulta = new StringBuilder();

            if (IdSistema != null && IdSectorizacion != null && IdSector != null && IdNucleo != null)
                strConsulta.Append("SELECT * FROM Radio WHERE IdSistema='" + IdSistema + "' AND IdSectorizacion='" + IdSectorizacion + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "' ORDER BY IdDestino,Literal,PosHMI");
            else if (IdSistema != null && IdSector != null && IdSectorizacion != null)
                strConsulta.Append("SELECT * FROM Radio WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdSectorizacion='" + IdSectorizacion + "' ORDER BY IdDestino,Literal,PosHMI");
            else if (IdSistema != null && IdSector != null && IdNucleo != null && PosHMI != 0)
                strConsulta.Append("SELECT * FROM Radio WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND PosHMI=" + PosHMI + " AND IdNucleo='" + IdNucleo + "' ORDER BY IdDestino,Literal,PosHMI");
            else if (IdSistema != null && IdSector != null && IdNucleo != null)
                strConsulta.Append("SELECT * FROM Radio WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "' ORDER BY IdDestino,Literal,PosHMI");
			else if (IdSistema != null && IdSectorizacion != null)
                strConsulta.Append("SELECT * FROM Radio WHERE IdSistema='" + IdSistema + "' AND IdSectorizacion='" + IdSectorizacion + "' ORDER BY IdDestino,Literal,PosHMI");
			else if (IdSistema != null)
                strConsulta.Append("SELECT * FROM Radio WHERE IdSistema='" + IdSistema + "' ORDER BY IdDestino,PosHMI ");
			else
                strConsulta.Append("SELECT * FROM Radio ORDER BY IdDestino,PosHMI ");

            return strConsulta.ToString();
        }

        public override List<Tablas> ListSelectSQL(DataSet ds)
        {
            ListaResultado.Clear();

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    Radio r = new Radio();

                    if (dr["IdSistema"] != System.DBNull.Value)
                        r.IdSistema = (string)dr["IdSistema"];
                    if (dr["IdSectorizacion"] != System.DBNull.Value)
                        r.IdSectorizacion = (string)dr["IdSectorizacion"];
                    if (dr["IdNucleo"] != System.DBNull.Value)
                        r.IdNucleo = (string)dr["IdNucleo"];
                    if (dr["IdSector"] != System.DBNull.Value)
                        r.IdSector = (string)dr["IdSector"];
                    if (dr["PosHMI"] != System.DBNull.Value)
                        r.PosHMI = (uint)dr["PosHMI"];
                    if (dr["IdDestino"] != System.DBNull.Value)
                        r.IdDestino = (string)dr["IdDestino"];
                    if (dr["Prioridad"] != System.DBNull.Value)
                        r.Prioridad = (uint)dr["Prioridad"];
                    if (dr["PrioridadSIP"] != System.DBNull.Value)
                        r.PrioridadSIP = (uint)dr["PrioridadSIP"];
                    if (dr["ModoOperacion"] != System.DBNull.Value)
                        r.ModoOperacion = (string)dr["ModoOperacion"];
                    if (dr["Cascos"] != System.DBNull.Value)
                        r.Cascos = (string)dr["Cascos"];
                    if (dr["Literal"] != System.DBNull.Value)
                        r.Literal = (string)dr["Literal"];
                    if (dr["SupervisionPortadora"] != System.DBNull.Value)
                        r.SupervisionPortadora = (bool)dr["SupervisionPortadora"];  //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0;

                    ListaResultado.Add(r);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO Radio (IdSistema,IdSectorizacion,IdNucleo,IdSector,PosHMI,IdDestino,Prioridad,PrioridadSIP,ModoOperacion,Cascos,Literal,SupervisionPortadora)" +
                            " VALUES ('" + IdSistema + "','" +
                                         IdSectorizacion + "'," +
                                         IdNucleo + "','" +
                                         IdSector + "'," +
                                         PosHMI + ",'" +
                                         IdDestino + "'," +
                                         Prioridad + ",'" +
                                         PrioridadSIP + ",'" +
                                         ModoOperacion + "','" +
                                         Cascos + "','" +
                                         Literal + "'," + 
                                         SupervisionPortadora + ")");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Radio");
			return consulta;
        }

        public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE Radio SET IdSistema='" + IdSistema + "'," +
                                            "IdSectorizacion='" + IdSectorizacion + "'," +
                                            "IdNucleo='" + IdNucleo + "'," +
                                            "IdSector='" + IdSector + "'," +
                                            "PosHMI=" + PosHMI + "," +
                                            "IdDestino='" + IdDestino + "'," +
                                            "Prioridad=" + Prioridad + "," +
                                            "PrioridadSIP=" + PrioridadSIP + "," +
                                            "ModoOperacion='" + ModoOperacion + "'," +
                                            "Cascos='" + Cascos + "'," +
                                            "Literal='" + Literal + "'," +
                                            "SupervisionPortadora=" + SupervisionPortadora +
                                            " WHERE IdSectorizacion='" + IdSectorizacion + "' AND IdSector='" + IdSector + "' AND IdSistema='" + IdSistema + "'"
                                            );

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Radio");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];
			
            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdSectorizacion != null && IdSector != null)
                Consulta.Append("DELETE FROM Radio WHERE IdSistema='" + IdSistema + "' AND IdSectorizacion='" + IdSectorizacion + "' AND IdSector='" + IdSector + "'");
            else if (IdSistema != null && IdSectorizacion != null)
                Consulta.Append("DELETE FROM Radio WHERE IdSistema='" + IdSistema + "' AND IdSectorizacion='" + IdSectorizacion + "'");
            else
                Consulta.Append("DELETE FROM Radio");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Radio");
			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM Radio WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}

        public string SelectCountDistinctDestinoSQL(bool bListaEnlacesExternos)
        {
            // Cuenta los enlaces radio de un sector que tengan configurado un recurso
            Consulta.Remove(0, Consulta.Length);

            if (bListaEnlacesExternos)
            {
                //Obtiene el número de destinos Radio con idDestino y literal diferente configurados en el sector
                Consulta.Append("SELECT COUNT(1) FROM (SELECT A.IdDestino,A.LITERAL FROM Radio A "); 
                Consulta.AppendFormat("WHERE A.IdSectorizacion='{0}' AND A.IdSistema='{1}'  AND A.IdSector='{2}' AND ",IdSectorizacion, IdSistema,IdSector);
                Consulta.Append("EXISTS (SELECT 1 FROM RecursosRadio B WHERE B.IdDestino=A.IdDestino AND B.IdSistema=A.IdSistema)group by A.iddestino,A.literal) T ");
            }
            else
            {
                //Obtiene el número de destinos Radio con distinto identificador
                Consulta.Append("SELECT COUNT(DISTINCT a.IdDestino) FROM Radio a, RecursosRadio b WHERE IdSectorizacion='" + IdSectorizacion + "' AND " +
                                  "a.IdSistema='" + IdSistema + "' AND a.IdSector='" + IdSector + "' AND a.IdDestino=b.IdDestino");
            }
            return Consulta.ToString();
        }
    }
}
