using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class DestinosInternos : DestinosTelefonia
    {
        #region Propiedades de DestinosInternos
        private string _IdSector;
        public string IdSector
        {
            get { return _IdSector; }
            set { _IdSector = value; }
        }

        private string _IdNucleo;
        public string IdNucleo
        {
            get { return _IdNucleo; }
            set { _IdNucleo = value; }
        }
        #endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public DestinosInternos()
            : base()
        {
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdGrupo != null)
                Consulta.Append("SELECT * FROM DestinosInternos WHERE IdSistema='" + IdSistema + "' AND IdGrupo='" + IdGrupo + "'");
            else if (IdSistema != null && IdDestino != null && IdPrefijo != 0)
                Consulta.Append("SELECT * FROM DestinosInternos WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "' AND IdPrefijo=" + IdPrefijo);
            else if (IdSistema != null && IdDestino != null)
                Consulta.Append("SELECT * FROM DestinosInternos WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "'");
            else
                Consulta.Append("SELECT * FROM DestinosInternos");

            return Consulta.ToString();
        }

        public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds)
        {
            //           base.ListSelectSQL();

            ListaResultado.Clear();

			//DataSetResultado = this.DataSetSelectSQL();
            if (ds != null)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    DestinosInternos r = new DestinosInternos();

                    if (dr["IdSistema"] != System.DBNull.Value)
                        r.IdSistema = (string)dr["IdSistema"];
                    if (dr["IdDestino"] != System.DBNull.Value)
                        r.IdDestino = (string)dr["IdDestino"];
                    if (dr["TipoDestino"] != System.DBNull.Value)
                        r.TipoDestino = (uint)dr["TipoDestino"];
                    if (dr["IdPrefijo"] != System.DBNull.Value)
                        r.IdPrefijo = (uint)dr["IdPrefijo"];
                    if (dr["IdSector"] != System.DBNull.Value)
                        r.IdSector = (string)dr["IdSector"];
                    if (dr["IdNucleo"] != System.DBNull.Value)
                        r.IdNucleo = (string)dr["IdNucleo"];

                    ListaResultado.Add(r);
                }
            }
            return ListaResultado;
        }

        // Al insertar DestinoInterno==>Trigger AltaDestinoInterno (da de alta un destino telefonia)
        public override string[] InsertSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO DestinosInternos VALUES ('" + IdSistema + "','" +
                                                         IdDestino + "'," +
                                                         TipoDestino + "," +
                                                         IdPrefijo + ",'" +
                                                         IdSector + "','" +
                                                         IdNucleo + "')");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "DestinosInternos");
			return consulta;
		}

        public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE DestinosInternos SET IdSistema='" + IdSistema + "'," +
                                            "IdDestino='" + IdDestino + "'," +
                                            "TipoDestino=" + TipoDestino + "," +
                                            "IdPrefijo=" + IdPrefijo + "," +
                                            "IdSector='" + IdSector + "'," +
                                            "IdNucleo='" + IdNucleo + "' " +
                                            "WHERE IdDestino='" + IdDestino + "' AND " + "IdSistema='" + IdSistema + "' AND TipoDestino=" + TipoDestino);

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "DestinosInternos");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdDestino != null)
                Consulta.Append("DELETE FROM DestinosInternos WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "'");
            else
                Consulta.Append("DELETE FROM DestinosInternos");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "DestinosInternos");
			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM DestinosInternos WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
    }
}
