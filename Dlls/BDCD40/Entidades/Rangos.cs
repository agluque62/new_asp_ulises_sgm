using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class Rangos : RangosSCV
    {
        #region Propiedades de Rangos

        private string _idSistema;      // Identificador del sistema
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }

        private string _Central;        // Nombre de la central
        public string Central
        {
            get { return _Central; }
            set { _Central = value; }
        }

        private string _Tipo;           // Tipo de Rango O: Operador; P: Privilegiado
        public string Tipo
        {
            get { return _Tipo; }
            set { _Tipo = value; }
        }
        #endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public Rangos()
        {
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
        }

        public override string DataSetSelectSQL()
        {

            StringBuilder strConsulta = new StringBuilder();

            if (IdSistema != null && Central != null && Tipo != null)
                strConsulta.AppendFormat("SELECT * FROM Rangos WHERE IdSistema='{0}' AND Central='{1}' AND Tipo='{2}' ORDER BY Inicial", IdSistema, Central, Tipo);
            else if (IdSistema != null && Central != null)
                strConsulta.AppendFormat("SELECT * FROM Rangos WHERE IdSistema='{0}' AND Central='{1}' ORDER BY Inicial,Tipo", IdSistema, Central);
            else if (IdSistema != null)
                strConsulta.AppendFormat("SELECT * FROM Rangos WHERE IdSistema='{0}' ORDER BY Inicial,Tipo", IdSistema);
            else
                strConsulta.Append("SELECT * FROM Rangos");

            return strConsulta.ToString();
        }

        public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds)
        {
            ListaResultado.Clear();

			//DataSetResultado = this.DataSetSelectSQL();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    Rangos r = new Rangos();

                    r.IdSistema = (string)dr["IdSistema"];
                    r.Central = (string)dr["Central"];
                    r.Tipo = (string)dr["Tipo"];
                    if (dr["IdAbonado"] != System.DBNull.Value)
                        r.IdAbonado = (string)dr["IdAbonado"];
                    if (dr["IdPrefijo"] != System.DBNull.Value)
                        r.IdPrefijo = (uint)dr["IdPrefijo"];
                    if (dr["Inicial"] != System.DBNull.Value)
                        r.Inicial = (string)dr["Inicial"];
                    if (dr["Final"] != System.DBNull.Value)
                        r.Final = (string)dr["Final"];

                    ListaResultado.Add(r);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO Rangos (IdSistema,Tipo,Central,IdAbonado,IdPrefijo,Inicial,Final)" +
                            " VALUES ('" + IdSistema + "','" +
                                         Tipo + "','" +
                                         Central + "','" +
                                         IdAbonado + "'," +
                                         IdPrefijo + "," +
                                         ((Inicial == null) ? "null," : ("'" + Inicial + "',")) +
                                         ((Final == null) ? "null" : ("'" + Final + "'")) +
                                         ")");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Rangos");
			return consulta;
        }

		public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE Rangos SET IdSistema='" + IdSistema + "'," +
                                            "Central='" + Central + "'," +
                                            "Tipo='" + Tipo + "'," +
                                            "IdAbonado='" + IdAbonado + "'," +
                                            "IdPrefijo=" + IdPrefijo + "," +
                                            "Inicial=" + ((Inicial == null) ? "null, " : ("'" + Inicial + "', ")) +
                                            "Final=" + ((Final == null) ? "null " : ("'" + Final + "' ")) +
                                            "WHERE Central='" + Central + "' AND " + "IdSistema='" + IdSistema + "' AND Tipo='" + Tipo + "'");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Rangos");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && Central != null && Tipo != null)
                Consulta.Append("DELETE FROM Rangos WHERE IdSistema='" + IdSistema + "' AND Central='" + Central + "' AND Tipo='" + Tipo + "'");
            else if (IdSistema != null && Central != null && Inicial != null)
                Consulta.Append("DELETE FROM Rangos WHERE IdSistema='" + IdSistema + "' AND Central='" + Central + "' AND Inicial='" + Inicial + "'");
            else if (IdSistema != null && Central != null)
                Consulta.Append("DELETE FROM Rangos WHERE IdSistema='" + IdSistema + "' AND Central='" + Central + "'");
            else if (IdSistema != null)
                Consulta.Append("DELETE FROM Rangos WHERE IdSistema='" + IdSistema + "'");
            else
                Consulta.Append("DELETE FROM Rangos");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Rangos");
			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM Rangos WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}

		//public bool ValorDentroDelRangoSQL(string idSistema, string central, string rango, out string inicial, out string final)
		//{
		//	bool retorno = false;
		//	inicial = final = string.Empty;
			/*
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("SELECT COUNT(*) FROM Rangos " + 
							"WHERE IdSistema='" + idSistema + "' AND " +
							"Central='" + central + "' AND " +
							"CAST(Inicial AS UNSIGNED)<= CAST('" + rango + "' AS UNSIGNED) AND " + 
							"CAST(Final AS UNSIGNED)>= CAST('" + rango +	"' AS UNSIGNED)" );

			DataSet dsCuantos = ServiceAccesoABaseDeDatos.GetDataSet(Consulta.ToString());
			retorno = dsCuantos.Tables.Count > 0 && dsCuantos.Tables[0].Rows.Count > 0 && Convert.ToUInt16(dsCuantos.Tables[0].Rows[0][0]) > 0;

			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("SELECT * FROM Rangos " +
								"WHERE IdSistema='" + idSistema + "' AND " +
								"Central='" + central + "'");

			DataSet ds = ServiceAccesoABaseDeDatos.GetDataSet(Consulta.ToString());
			if (ds != null && ds.Tables[0].Rows.Count > 0)
			{
				System.Data.DataRow dr = ds.Tables[0].Rows[0];
				if (dr["Inicial"] != System.DBNull.Value)
					inicial = (string)dr["Inicial"];
				if (dr["Final"] != System.DBNull.Value)
					final = (string)dr["Final"];
			}
			*/

		//	return retorno;
		//}
    }
}
