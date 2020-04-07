using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class RangosSCV : Tablas
    {
        private uint _IdPrefijo;     // Prefijo de la red del número de abonado como alternativa a la llamada por la red ATS
        public uint IdPrefijo
        {
            get { return _IdPrefijo; }
            set { _IdPrefijo = value; }
        }

        private string _IdAbonado;     // Número de abonado de una red como alternativa a la llamada por la red ATS
        public string IdAbonado
        {
            get { return _IdAbonado; }
            set { _IdAbonado = value; }
        }

        private string _Inicial;        // Número inicial del rango en el plan de numeración ATS
        public string Inicial
        {
            get { return _Inicial; }
            set { _Inicial = value; }
        }

        private string _Final;          // Número final del rango en el plan de numeración ATS
        public string Final
        {
            get { return _Final; }
            set { _Final = value; }
        }

        public override string DataSetSelectSQL() { return null; }
        public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds) { return null; }
        public override string[] InsertSQL() { return null; }
		public override string[] UpdateSQL() { return null; }
		public override string[] DeleteSQL() { return null; }
		//public override string[] SelectCountSQL(string where) { return 0; }
    }
}
