using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class NivelesSCV : Tablas
    {
        #region Propiedades NivelesSCV
        private uint _CICL;
        public uint CICL
        {
            get { return _CICL; }
            set { _CICL = value; }
        }

        private uint _CIPL;
        public uint CIPL
        {
            get { return _CIPL; }
            set { _CIPL = value; }
        }

        private uint _CPICL;
        public uint CPICL
        {
            get { return _CPICL; }
            set { _CPICL = value; }
        }

        private uint _CPIPL;
        public uint CPIPL
        {
            get { return _CPIPL; }
            set { _CPIPL = value; }
        }


        #endregion

        public override string DataSetSelectSQL() { return null; }
        public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds) { return null; }
        public override string[] InsertSQL() { return null; }
		public override string[] UpdateSQL() { return null; }
		public override string[] DeleteSQL() { return null; }
		//public override int SelectCountSQL(string where) { return 0; }
    }
}
