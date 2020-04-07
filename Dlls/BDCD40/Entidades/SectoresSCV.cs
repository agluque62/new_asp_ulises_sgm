using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class SectoresSCV : Tablas
    {
        #region Propiedades UsuariosSCV
        // Identificador del sector/usuario que compone la pareja (Controlador/Planificador)
        private string _ParejaUCS;
        public string IdParejaUCS
        {
            get { return _ParejaUCS; }
            set { _ParejaUCS = value; }
        }
        // C: Controlador; P: Planificador
        private string _TipoPosicion;
        public string TipoPosicion
        {
            get { return _TipoPosicion; }
            set { _TipoPosicion = value; }
        }
        // Prioridad R2
        private uint _PrioridadR2;
        public uint PrioridadR2
        {
            get { return _PrioridadR2; }
            set { _PrioridadR2 = value; }
        }
        // Tipo HMI. Análogo a cliente.
        private uint _TipoHMI;
        public uint TipoHMI
        {
            get { return _TipoHMI; }
            set { _TipoHMI = value; }
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
