using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public abstract class ParametrosRecursoGeneral : Tablas
    {
        public abstract override string DataSetSelectSQL();
		public abstract override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds);
        public abstract override string[] InsertSQL();
        public abstract override string[] UpdateSQL();
        public abstract override string[] DeleteSQL();
        //public abstract override int SelectCountSQL(string where);
        public abstract string[] UpdateDestinoSQL();
        public abstract string[] LiberaDestinoSQL();
    }
}
