using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class RecursosSCV : Tablas
    {
        #region Propiedades de RecursosSCV
        // Nombre del recurso
        private string _IdRecursos;
        public string IdRecurso
        {
            get { return _IdRecursos; }
            set { _IdRecursos = value; }
        }
        // Tipo del recurso
        private uint _Tipo;
        public uint Tipo
        {
            get { return _Tipo; }
            set { _Tipo = value; }
        }
        // Tipo de Interfaz
        private Tipos.TipoInterface _Interface;
        public Tipos.TipoInterface Interface
        {
            get { return _Interface; }
            set { _Interface = value; }
        }
        // Slot de la pasarela. 0,1,2 ó 3
        private int _SlotPasarela;
        public int SlotPasarela
        {
            get { return _SlotPasarela; }
            set { _SlotPasarela = value; }
        }
        // Número de dispositivo o interfaz dentro del slot de la pasarela.  0,1,2 ó 3
        private int _NumDispositivoSlot;
        public int NumDispositivoSlot
        {
            get { return _NumDispositivoSlot; }
            set { _NumDispositivoSlot = value; }
        }
        // Dirección del servidor de registros SIP
        private string _ServidorSIP;
        public string ServidorSIP
        {
            get { return _ServidorSIP; }
            set { _ServidorSIP = value; }
        }
        // Usar protocolo DiffServ
        private bool _Diffserv;
        public bool Diffserv
        {
            get { return _Diffserv; }
            set { _Diffserv = value; }
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
