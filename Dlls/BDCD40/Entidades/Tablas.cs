using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace CD40.BD.Entidades
{
    public abstract class Tablas
    {
        protected System.Collections.Generic.List<Tablas> ListaResultado;
        //protected System.Data.DataSet DataSetResultado;
		protected static System.Text.StringBuilder Consulta = new System.Text.StringBuilder(125);

        private long _InsertedId;
        public long InsertedId
        {
            get { return _InsertedId; }
            set { _InsertedId = value; }
        }

        public Tablas()
        {
            //DataSetResultado = new System.Data.DataSet();
            ListaResultado = new System.Collections.Generic.List<Tablas>();

            if (Consulta == null)
                Consulta = new System.Text.StringBuilder();
        }

        protected string GeneraClave(string id)
        {
            System.Security.Cryptography.HashAlgorithm hash;
            hash = new System.Security.Cryptography.SHA1Managed();
            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(id);
            byte[] clave = hash.ComputeHash(plainTextBytes);
            return Convert.ToBase64String(clave);
        }

        public static string SelectView(string viewName, params string[] strWhere)
        {
            //AccesoABaseDeDatos Service = new AccesoABaseDeDatos();

            Consulta.Remove(0, Consulta.Length);
			switch (viewName)
			{
				case "ViewGetUriEnlaceLineasPlanActivo":
					if (strWhere.Length == 1)
						Consulta.Append("SELECT * FROM Interface_Sacim.ViewGetUriEnlaceLineasPlanActivo WHERE IdRecurso1='" + strWhere[0] + "'" +
										" OR IdRecurso2='" + strWhere[0] + "'");
					else
						Consulta.Append("SELECT * FROM ViewGetUriEnlaceLineasPlanActivo");
					return Consulta.ToString();
				case "ViewRecursosImplicadosRutas":
					if (strWhere.Length == 2 && strWhere[0] != null && strWhere[1] != null)
						Consulta.Append("SELECT * FROM ViewRecursosImplicadosRutas WHERE IdSistema='" + strWhere[0] + "' AND Idrecurso='" + strWhere[1] + "'");
					
					return Consulta.ToString();
				case "ViewIncidenciasMasAlarma":
					if (strWhere.Length == 1 && strWhere[0] != null)
						Consulta.Append("SELECT * FROM ViewIncidenciasMasAlarma WHERE IdSistema='" + strWhere[0] + "'");
					else
						Consulta.Append("SELECT * FROM ViewIncidenciasMasAlarma");

					return Consulta.ToString();
				case "ViewIncidenciasMasAlarma_ingles":
					if (strWhere.Length == 1 && strWhere[0] != null)
						Consulta.Append("SELECT * FROM ViewIncidenciasMasAlarma_ingles WHERE IdSistema='" + strWhere[0] + "'");
					else
						Consulta.Append("SELECT * FROM ViewIncidenciasMasAlarma_ingles");

					return Consulta.ToString();
				case "ViewDestinosTelefonia":
					if (strWhere.Length == 1 && strWhere[0] != null)
						Consulta.Append("SELECT * FROM ViewDestinosTelefonia WHERE IdSistema='" + strWhere[0] + "' ORDER BY IdDestino");
					else
                        Consulta.Append("SELECT * FROM ViewDestinosTelefonia  ORDER BY IdDestino");

					return Consulta.ToString();
				case "ViewSectoresEnTops":
					if (strWhere.Length == 2 && strWhere[0] != null && strWhere[1] != null)
						Consulta.Append("SELECT IdSistema,IdTop,IdNucleo,IdSector,tipo FROM ViewSectoresEnTops WHERE IdSistema='" + strWhere[0] + "' AND (IdSectorizacion='" + strWhere[1] + "' OR IdSectorizacion IS null) ORDER BY IdTop");
					else
						Consulta.Append("SELECT IdSistema,IdTop,IdNucleo,IdSector,tipo FROM ViewSectoresEnTops ORDER BY IdTop");

					return Consulta.ToString();
				case "SectoresEnTopsParaInformeXML":
					if (strWhere.Length == 2 && strWhere[0] != null && strWhere[1] != null)
						Consulta.Append("SELECT * FROM SectoresEnTopsParaInformeXML WHERE IdSistema='" + strWhere[0] + "' AND (IdSectorizacion='" + strWhere[1] + "' OR IdSectorizacion IS null) ORDER BY IdTop");
					else
						Consulta.Append("SELECT * FROM SectoresEnTopsParaInformeXML ORDER BY IdSistema,IdSectorizacion,IdTop");

					return Consulta.ToString();
			}

            return null;
        }

		protected string ReplaceSQL(string idSistema, string idTabla)
		{
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("REPLACE INTO TablasModificadas (IdTabla)" +
							" VALUES ('" + idTabla + "')");

			return Consulta.ToString();
		}

        public abstract string DataSetSelectSQL();
		public abstract List<Tablas> ListSelectSQL(DataSet ds);
        public abstract string[] InsertSQL();
        public abstract string[] UpdateSQL();
        public abstract string[] DeleteSQL();
        public virtual string DataSetSelectSQL(string tableName) { return null; }

        public virtual string DataSetComponentsSQL(DateTime desde, DateTime hasta) { return null; }
    }
}
