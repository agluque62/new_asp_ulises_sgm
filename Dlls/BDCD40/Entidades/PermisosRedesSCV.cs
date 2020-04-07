using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class PermisosRedesSCV : Tablas
    {
        #region Propiedades PermisosRedesSCV
        // Nombre de la red
        private string _idRed;
        public string IdRed
        {
            get { return _idRed; }
            set { _idRed = value; }
        }

        // Permiso para llamar por la red en cuestión
        private bool _Llamar;
        public bool Llamar
        {
            get { return _Llamar; }
            set { _Llamar = value; }
        }
        // Permiso para recibir llamadas por la red en cuestión
        private bool _Recibir;
        public bool Recibir
        {
            get { return _Recibir; }
            set { _Recibir = value; }
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
