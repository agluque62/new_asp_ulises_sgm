using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class ParametrosTFSCV : ParametrosRecursoGeneral
    {
        #region Propiedades ParametrosTFSCV
		// Lado de la línea (Sólo para el caso de que el interface del recurso sea TipoInterface.TI_ATS_R2 (5)
		private string _Lado;
		public string Lado
		{
			get { return _Lado; }
			set { _Lado = value; }
		}
		// Modo de la línea (Sólo para el caso de que el interface del recurso sea TipoInterface.TI_ATS_QSIG (7)
		private string _Modo;
		public string Modo
		{
			get { return _Modo; }
			set { _Modo = value; }
		}
		#endregion
        public System.Collections.Generic.List<string> ListaEnlacesInternos;

        public override string DataSetSelectSQL() { return null; }
        public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds) { return null; }
        public override string[] InsertSQL() { return null; }
        public override string[] UpdateSQL() { return null; }
        public override string[] DeleteSQL() { return null; }
		//public override int SelectCountSQL(string where) { return 0; }
        public override string[] UpdateDestinoSQL() { return null; }
        public override string[] LiberaDestinoSQL() { return null; }
    }
}
