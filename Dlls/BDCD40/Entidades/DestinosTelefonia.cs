using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class DestinosTelefonia : Destinos
    {
        #region Propiedades de DestinosTelefonia

        private uint _IdPrefijo;
        public uint IdPrefijo
        {
            get { return _IdPrefijo; }
            set { _IdPrefijo = value; }
        }

        private string _IdGrupo;
        public string IdGrupo
        {
            get { return _IdGrupo; }
            set { _IdGrupo = value; }
        }

        #endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public DestinosTelefonia()
            : base()
        {
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
        }

        public override string DataSetSelectSQL()
        {
            //            base.DataSetSelectSQL();

            Consulta.Remove(0, Consulta.Length);
			if (IdSistema != null && IdGrupo != null)
				Consulta.Append("SELECT * FROM DestinosTelefonia WHERE IdSistema='" + IdSistema + "' AND IdGrupo='" + IdGrupo + "'");
			else if (IdSistema != null && IdDestino != null)
				Consulta.Append("SELECT * FROM DestinosTelefonia WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "' AND TipoDestino=" + TipoDestino);
			else if (IdSistema != null && IdDestino != null)
				Consulta.Append("SELECT * FROM DestinosTelefonia WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "'");
			else if (IdSistema != null)
				Consulta.Append("SELECT * FROM DestinosTelefonia WHERE IdSistema='" + IdSistema + "'");
			else
				Consulta.Append("SELECT * FROM DestinosTelefonia");

            return Consulta.ToString();
        }

        public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds)
        {
            //           base.ListSelectSQL();

            ListaResultado.Clear();

			//DataSetResultado = this.DataSetSelectSQL();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    DestinosTelefonia r = new DestinosTelefonia();

                    if (dr["IdSistema"] != System.DBNull.Value)
                        r.IdSistema = (string)dr["IdSistema"];
                    if (dr["IdDestino"] != System.DBNull.Value)
                        r.IdDestino = (string)dr["IdDestino"];
                    if (dr["TipoDestino"] != System.DBNull.Value)
                        r.TipoDestino = (uint)dr["TipoDestino"];
                    if (dr["IdPrefijo"] != System.DBNull.Value)
                        r.IdPrefijo = (uint)dr["IdPrefijo"];
                    if (dr["IdGrupo"] != System.DBNull.Value)
                        r.IdGrupo = (string)dr["IdGrupo"];

                    ListaResultado.Add(r);
                }
            }
            return ListaResultado;
        }

        // Al insertar DestinoTelefonia==>Trigger AltaDestinoTelefonia (da de alta un destino)
        public override string[] InsertSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO DestinosTelefonia VALUES ('" + IdSistema + "','" +
                                                         IdDestino + "'," +
                                                         TipoDestino + "," +
                                                         IdPrefijo + "," +
                                                         ((IdGrupo == null) ? "null" : ("'" + IdGrupo + "'")) +
                                                         ")");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "DestinosTelefonia");
			return consulta;
        }

        public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE DestinosTelefonia SET IdSistema='" + IdSistema + "'," +
                                            "IdDestino='" + IdDestino + "'," +
                                            "TipoDestino=" + TipoDestino + "," +
                                            "IdPrefijo=" + IdPrefijo + "," +
                                            "IdGrupo=" + ((IdGrupo == null) ? "null " : ("'" + IdGrupo + "' ")) +
                                            "WHERE IdDestino='" + IdDestino + "' AND " + "IdSistema='" + IdSistema + "'");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "DestinosTelefonia");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdDestino != null)
                Consulta.Append("DELETE FROM DestinosTelefonia WHERE IdSistema='" + IdSistema + "' AND IdDestino='" + IdDestino + "'");
            else
                Consulta.Append("DELETE FROM DestinosTelefonia");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "DestinosTelefonia");
			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM DestinosTelefonia WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
    }
}
