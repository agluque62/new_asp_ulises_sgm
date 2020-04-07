using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace CD40.BD.Entidades
{
	public class ParametrosRecursosRadioKASiccip : ParametrosRecursosRadioKeepAlive
	{
		private string _UriRecursoDestino;
		public string UriRecursoDestino
		{
			get { return _UriRecursoDestino; }
			set { _UriRecursoDestino = value; }
		}

		public ParametrosRecursosRadioKASiccip()
            : base()
        {
        }

		public /* new */ override string DataSetSelectSQL()
		{
			return base.DataSetSelectSQL();
		}

		public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds)
		{
			return base.ListSelectSQL(ds);
		}

		public override string[] InsertSQL()
		{
			return base.InsertSQL();
		}

		public override string[] UpdateSQL()
		{
			return base.UpdateSQL();
		}

		public override string[] DeleteSQL()
		{
			return base.DeleteSQL();
		}

		//public override int SelectCountSQL(string where)
		//{
		//    return base.SelectCountSQL(where);
		//}

		public override string[] UpdateDestinoSQL() { return null; }

		public override string[] LiberaDestinoSQL() { return null; }

		public string GetUriRecursoDestino()
		{
				return Tablas.SelectView("ViewGetUriEnlaceLineasPlanActivo", new string[] { IdRecursoSCV });
				/*
				DataSet dsRecursos = Procedimientos.GetUriRecursoDestino(IdRecursoSCV);
				if (dsRecursos != null && dsRecursos.Tables[0].Rows.Count > 0)
				{
					System.Data.DataRow drEnlace = dsRecursos.Tables[0].Rows[0];
					if (IdRecursoSCV == (string)drEnlace["IdRecurso1"])
						this.UriRecursoDestino = this.TipoRecurso == 0 ? (string)drEnlace["IdRecurso2"] + "@" + (string)drEnlace["IdElementoHw2"]
										: (string)drEnlace["IdElementoHw2"];	// Los recursos de datos añadirán en el servicio web ":<Port>"
					else if (IdRecursoSCV == (string)drEnlace["IdRecurso2"])
						this.UriRecursoDestino = this.TipoRecurso == 0 ? (string)drEnlace["IdRecurso1"] + "@" + (string)drEnlace["IdElementoHw1"]
							: (string)drEnlace["IdElementoHw1"];	// Los recursos de datos añadirán en el servicio web ":<Port>"

					return true;
				}

				return false;
				 */
		}
	}
}
