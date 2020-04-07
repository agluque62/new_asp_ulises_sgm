using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class ParametrosSectorSCV : Tablas
    {
        #region Propiedades ParametrosSectorSCV
        private uint _NumLlamadasEntrantesIda;
        public uint NumLlamadasEntrantesIda
        {
            get { return _NumLlamadasEntrantesIda; }
            set { _NumLlamadasEntrantesIda = value; }
        }

        private uint _NumLlamadasEnIda;
        public uint NumLlamadasEnIda
        {
            get { return _NumLlamadasEnIda; }
            set { _NumLlamadasEnIda = value; }
        }

        private uint _NumFrecPagina;
        public uint NumFrecPagina
        {
            get { return _NumFrecPagina; }
            set { _NumFrecPagina = value; }
        }

        private uint _NumPagFrec;
        public uint NumPagFrec
        {
            get { return _NumPagFrec; }
            set { _NumPagFrec = value; }
        }

        protected uint _NumEnlacesInternosPag;
        public uint NumEnlacesInternosPag
        {
            get { return _NumEnlacesInternosPag; }
            set { _NumEnlacesInternosPag = value; }
        }

        protected uint _NumPagEnlacesInt;
        public uint NumPagEnlacesInt
        {
            get { return _NumPagEnlacesInt; }
            set { _NumPagEnlacesInt = value; }
        }

        private uint _NumEnlacesAI;
        public uint NumEnlacesAI
        {
            get { return _NumEnlacesAI; }
            set { _NumEnlacesAI = value; }
        }

        // Permiso para intruír
        private bool _Intrusion;
        public bool Intrusion
        {
            get { return _Intrusion; }
            set { _Intrusion = value; }
        }
        // Permiso para ser intruído
        private bool _Intruido;
        public bool Intruido
        {
            get { return _Intruido; }
            set { _Intruido = value; }
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
