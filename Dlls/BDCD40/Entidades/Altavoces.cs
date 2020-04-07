using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class Altavoces : Tablas
    {
        #region Propiedades de Altavoces

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

        private uint _TipoDestino;
        public uint TipoDestino
        {
            get { return _TipoDestino; }
            set { _TipoDestino = value; }
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

        private uint _PosHMI;
        public uint PosHMI
        {
            get { return _PosHMI; }
            set { _PosHMI = value; }
        }

        private uint _NumAltavoz;
        public uint NumAltavoz
        {
            get { return _NumAltavoz; }
            set { _NumAltavoz = value; }
        }

        private string _Estado;
        public string Estado
        {
            get { return _Estado; }
            set { _Estado = value; }
        }

        #endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public Altavoces()
        {
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
			if (IdSistema != null && IdDestino != null && IdSector != null && IdNucleo != null && PosHMI != 0)
				Consulta.Append("SELECT * FROM Altavoces WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "' AND IdSector='" + IdSector + "' AND " +
									"IdNucleo='" + IdNucleo + "' AND PosHMI=" + PosHMI + " " +
									"ORDER BY NumAltavoz");
			else if (IdSistema != null && IdDestino != null && IdSector != null)
				Consulta.Append("SELECT * FROM Altavoces WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "' AND IdSector='" + IdSector + "' " +
									"ORDER BY NumAltavoz");
			else
				Consulta.Append("SELECT * FROM Altavoces");

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
                    Altavoces r = new Altavoces();

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
                    if (dr["NumAltavoz"] != System.DBNull.Value)
                        r.NumAltavoz = (uint)dr["NumAltavoz"];
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
            Consulta.Append("INSERT INTO Altavoces VALUES ('" + IdSistema + "','" +
                                                         IdDestino + "'," +
                                                         TipoDestino + ",'" +
                                                         IdNucleo + "','" +
                                                         IdSector + "'," +
                                                         PosHMI + "," +
                                                         NumAltavoz + ",'" +
                                                         Estado + "')");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Altavoces");
			return consulta;
        }

        public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE Altavoces SET Estado='" + Estado + "' " +
											"WHERE IdSistema='" + IdSistema + "' AND " +
                                            "IdDestino='" + IdDestino + "' AND " +
                                            "TipoDestino=" + TipoDestino + " AND " +
                                            "IdNucleo='" + IdNucleo + "' AND " +
                                            "IdSector='" + IdSector + "' AND " +
                                            "PosHMI=" + PosHMI + " AND " +
                                            "NumAltavoz=" + NumAltavoz);

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Altavoces");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];
			
			Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdDestino != null && IdSector != null)
                Consulta.Append("DELETE FROM Altavoces WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "' AND IdSector='" + IdSector + "'");
            else
                Consulta.Append("DELETE FROM Altavoces");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Altavoces");
			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM Altavoces WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
    }
}
