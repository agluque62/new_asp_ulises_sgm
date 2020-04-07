using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class DestinosExternosSector : Tablas
    {
        #region Propiedades de DestinosExternosSector

        protected string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
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

        //Prefijo del Destino que puede ser nulo
        protected Nullable<uint> _IdPrefijoDestinoLCEN;
        public Nullable<uint> IdPrefijoDestinoLCEN
        {
            get { return _IdPrefijoDestinoLCEN; }
            set { _IdPrefijoDestinoLCEN = value; }
        }

        protected string _IdDestinoLCEN;
        public string IdDestinoLCEN
        {
            get { return _IdDestinoLCEN; }
            set { _IdDestinoLCEN = value; }
        }

        #endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public DestinosExternosSector()
        {
			IdPrefijo = uint.MaxValue;
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
        }

        public override string DataSetSelectSQL()
        {
            bool bWhere = true;

            Consulta.Clear();
			if (IdSistema != null && IdSector != null && IdNucleo != null && PosHMI != 0 && IdPrefijo != uint.MaxValue)
				Consulta.Append("SELECT * FROM DestinosExternosSector WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "' AND PosHMI=" + PosHMI + " AND IdPrefijo=" + IdPrefijo);
			else if (IdSistema != null && IdSector != null && IdNucleo != null && PosHMI != 0)
				Consulta.Append("SELECT * FROM DestinosExternosSector WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "' AND PosHMI=" + PosHMI);
			else if (IdSistema != null && IdDestino != null && IdSector != null)
				Consulta.Append("SELECT * FROM DestinosExternosSector WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "' AND IdSector='" + IdSector + "'");
			else if (IdSistema != null && IdSector != null && PosHMI != 0)
				Consulta.Append("SELECT * FROM DestinosExternosSector WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND PosHMI=" + PosHMI);
            else if (IdSistema != null && IdSector != null)
                Consulta.Append("SELECT * FROM DestinosExternosSector WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "'");
            else if (IdSistema != null && IdDestino != null)
                Consulta.Append("SELECT * FROM DestinosExternosSector WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "'");
            else if (IdSistema != null && IdDestinoLCEN != null && IdPrefijoDestinoLCEN!=null)
                Consulta.AppendFormat("SELECT * FROM DestinosExternosSector WHERE IdSistema='{0}' AND IdDestinoLCEN='{1}' AND IdPrefijoDestinoLCEN={2}", IdSistema, IdDestinoLCEN, IdPrefijoDestinoLCEN);
            else
            {
                Consulta.Append("SELECT * FROM DestinosExternosSector");
                bWhere = false;
            }

            //Añadimos a la clausula Where el tipo de panel desde el que se accede IA ->Panel de LC, DA -> Panel de telefonía
            if (!string.IsNullOrEmpty(TipoAcceso))
            {
                if (bWhere)
                  Consulta.AppendFormat(" AND TipoAcceso='{0}' ",TipoAcceso);
                else
                  Consulta.AppendFormat(" WHERE TipoAcceso='{0}' ", TipoAcceso);
            }


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
                    DestinosExternosSector r = new DestinosExternosSector();

                    if (dr["IdSistema"] != System.DBNull.Value)
                        r.IdSistema = (string)dr["IdSistema"];
                    if (dr["IdDestino"] != System.DBNull.Value)
                        r.IdDestino = (string)dr["IdDestino"];
                    if (dr["TipoDestino"] != System.DBNull.Value)
                        r.TipoDestino = (uint)dr["TipoDestino"];
                    if (dr["IdNucleo"] != System.DBNull.Value)
                        r.IdNucleo = (string)dr["IdNucleo"];
                    if (dr["IdSector"] != System.DBNull.Value)
                        r.IdSector = (string)dr["IdSector"];
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

                    //Se recupera el prefijo y el Identificador del Destino LCEN asociado al destino externo
                    //Esta situación sólo se puede dar si el destino es ATS (Prefijo == 3)y se encuentra definido dentro del panel de LC (TipoAcceso='IA')

                    if (r.TipoAcceso=="IA" && r.IdPrefijo == 3)
                    {
                        if (dr["IdPrefijoDestinoLCEN"] != System.DBNull.Value)
                            r.IdPrefijoDestinoLCEN = (uint)dr["IdPrefijoDestinoLCEN"];

                        if (dr["IdDestinoLCEN"] != System.DBNull.Value)
                            r.Literal = (string)dr["IdDestinoLCEN"];
                    }

                    ListaResultado.Add(r);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
            string[] consulta = new string[2];
            
            Consulta.Clear();

            /*
            Consulta.Append("INSERT INTO DestinosExternosSector (IdSistema,IdDestino,TipoDestino,IdNucleo,IdSector,IdPrefijo,PosHMI,Prioridad,OrigenR2,PrioridadSIP,TipoAcceso,Literal)" +
                            " VALUES ('" + IdSistema + "','" +
                                         IdDestino + "'," +
                                         TipoDestino + ",'" +
                                         IdNucleo + "','" +
                                         IdSector + "'," +
                                         IdPrefijo + "," +
                                         PosHMI + "," +
                                         Prioridad + ",'" +
                                         OrigenR2 + "'," +
                                         PrioridadSIP + ",'" +
                                         TipoAcceso + "','" +
                                         Literal + "')");
            */

            Consulta.Append("INSERT INTO DestinosExternosSector (IdSistema,IdDestino,TipoDestino,IdNucleo,IdSector,IdPrefijo,PosHMI,Prioridad,OrigenR2,PrioridadSIP,TipoAcceso,Literal,IdPrefijoDestinoLCEN,IdDestinoLCEN)");
            Consulta.AppendFormat(" VALUES ('{0}','{1}',{2},'{3}','{4}',{5},{6},{7},'{8}',{9},'{10}','{11}'", IdSistema, IdDestino, TipoDestino, IdNucleo, IdSector, IdPrefijo, PosHMI, Prioridad, OrigenR2, PrioridadSIP,TipoAcceso, Literal);

            if (string.Compare(TipoAcceso, "IA") == 0)
            {
                if (null != IdPrefijoDestinoLCEN)
                    Consulta.AppendFormat(",{0},", IdPrefijoDestinoLCEN);
                else
                    Consulta.Append(",null,");

                if (null != IdDestinoLCEN)
                    Consulta.AppendFormat("'{0}')", IdDestinoLCEN);
                else
                    Consulta.Append("null)");
            }
            else
            {
                //Se inserta los campos IdPrefijoDestinoLCEN e IdDestinoLCEN con valor null
                //Porque sólo se utilizan en el panel de LC
                Consulta.Append(",null,null)");
            }

            consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(IdSistema, "DestinosExternosSector");

            Consulta.Clear();

            return consulta;
        }

        public override string[] UpdateSQL()
        {
            string[] consulta = new string[2];

            Consulta.Clear();
            /*
            Consulta.Append("UPDATE DestinosExternosSector SET IdSistema='" + IdSistema + "'," +
                                            "IdDestino='" + IdDestino + "'," +
                                            "TipoDestino=" + TipoDestino + "," +
                                            "IdNucleo='" + IdNucleo + "'," +
                                            "IdSector='" + IdSector + "'," +
                                            "IdPrefijo=" + IdPrefijo + "," +
                                            "PosHMI=" + PosHMI + "," +
                                            "Prioridad=" + Prioridad + "," +
                                            "OrigenR2='" + OrigenR2 + "'," +
                                            "PrioridadSIP=" + PrioridadSIP + "," +
                                            "TipoAcceso='" + TipoAcceso + "'," +
                                            "Literal='" + Literal + "' " +
                                            "WHERE IdDestino='" + IdDestino + "' AND IdSector='" + IdSector + "' AND IdSistema='" + IdSistema + "' AND PosHMI=" + PosHMI + " AND IdPrefijo=" + IdPrefijo
                                            );
            */
            Consulta.AppendFormat("UPDATE DestinosExternosSector SET IdSistema='{0}',IdDestino='{1}',TipoDestino={2},IdNucleo='{3}',IdSector='{4}',IdPrefijo={5},", IdSistema, IdDestino, TipoDestino, IdNucleo, IdSector, IdPrefijo);
            Consulta.AppendFormat("PosHMI={0},Prioridad={1},OrigenR2='{2}',PrioridadSIP={3},TipoAcceso='{4}',Literal='{5}',", PosHMI, Prioridad, OrigenR2, PrioridadSIP, TipoAcceso, Literal);

            //Los campos IdPrefijoDestinoLCEN y IdDestinoLCEN sólo tienen sentido para los destinos configurados desde el panel de Línea Caliente (tipoAcceso='IA')
            if (string.Compare(TipoAcceso, "IA") == 0 && null != IdPrefijoDestinoLCEN)
                Consulta.AppendFormat("IdPrefijoDestinoLCEN={0},", IdPrefijoDestinoLCEN);
            else
                Consulta.Append("IdPrefijoDestinoLCEN=null,");

            if (string.Compare(TipoAcceso, "IA") == 0 && null != IdDestinoLCEN)
                Consulta.AppendFormat("IdDestinoLCEN='{0}' ", IdDestinoLCEN);
            else
                Consulta.Append("IdDestinoLCEN=null ");

            Consulta.AppendFormat(" WHERE IdSistema='{0}' AND IdNucleo='{1}' AND IdSector='{2}' AND IdDestino='{3}' AND TipoDestino={4} AND IdPrefijo={5} AND TipoAcceso='{6}'", IdSistema, IdNucleo, IdSector, IdDestino, TipoDestino, IdPrefijo, TipoAcceso);

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "DestinosExternosSector");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];
            bool bWhere = true;

			Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdSector != null && IdNucleo != null && PosHMI != 0 && IdPrefijo != uint.MaxValue)
            {
                //Consulta.Append("DELETE FROM DestinosExternosSector WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "' AND PosHMI=" + PosHMI + " AND IdPrefijo=" + IdPrefijo);
                Consulta.AppendFormat("DELETE FROM DestinosExternosSector WHERE IdSistema='{0}' AND IdNucleo='{1}' AND IdSector='{2}' AND PosHMI={3} AND IdPrefijo={4}", IdSistema, IdNucleo, IdSector, PosHMI, IdPrefijo);
            }
            else if (IdSistema != null && IdSector != null && IdNucleo != null && PosHMI != 0)
                Consulta.Append("DELETE FROM DestinosExternosSector WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "' AND IdNucleo='" + IdNucleo + "' AND PosHMI=" + PosHMI);
            else if (IdSistema != null && IdDestino != null && IdSector != null)
                Consulta.Append("DELETE FROM DestinosExternosSector WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "' AND IdSector='" + IdSector + "'");
            else if (IdSistema != null && IdSector != null)
                Consulta.Append("DELETE FROM DestinosExternosSector WHERE IdSistema='" + IdSistema + "' AND IdSector='" + IdSector + "'");
            else if (IdSistema != null && IdDestino != null)
                Consulta.Append("DELETE FROM DestinosExternosSector WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "'");
            else
            {
                Consulta.Append("DELETE FROM DestinosExternosSector");
                bWhere = false;
            }

            //Añadimos a la clausula Where el tipo de panel desde el que se accede IA ->Panel de LC, DA -> Panel de telefonía
            if (!string.IsNullOrEmpty(TipoAcceso))
            {
                if (bWhere)
                    Consulta.AppendFormat(" AND TipoAcceso='{0}' ", TipoAcceso);
                else
                    Consulta.AppendFormat(" WHERE TipoAcceso='{0}' ", TipoAcceso);
            }

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "DestinosExternosSector");
			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM DestinosExternosSector WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
    }
}
