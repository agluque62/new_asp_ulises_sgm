using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class TeclasSectorSCV : Tablas
    {
        #region Propiedades TeclasSectorSCV
        // Disponibilidad de la función en concreto
        private bool _TransConConsultaPrev;
        public bool TransConConsultaPrev
        {
            get { return _TransConConsultaPrev; }
            set { _TransConConsultaPrev = value; }
        }

        private bool _TransDirecta;
        public bool TransDirecta
        {
            get { return _TransDirecta; }
            set { _TransDirecta = value; }
        }

        private bool _Conferencia;
        public bool Conferencia
        {
            get { return _Conferencia; }
            set { _Conferencia = value; }
        }

        private bool _Escucha;
        public bool Escucha
        {
            get { return _Escucha; }
            set { _Escucha = value; }
        }

        private bool _Retener;
        public bool Retener
        {
            get { return _Retener; }
            set { _Retener = value; }
        }

        private bool _Captura;
        public bool Captura
        {
            get { return _Captura; }
            set { _Captura = value; }
        }

        private bool _Redireccion;
        public bool Redireccion
        {
            get { return _Redireccion; }
            set { _Redireccion = value; }
        }

        private bool _RepeticionUltLlamada;
        public bool RepeticionUltLlamada
        {
            get { return _RepeticionUltLlamada; }
            set { _RepeticionUltLlamada = value; }
        }

        private bool _RellamadaAut;
        public bool RellamadaAut
        {
            get { return _RellamadaAut; }
            set { _RellamadaAut = value; }
        }

        private bool _TeclaPrioridad;
        public bool TeclaPrioridad
        {
            get { return _TeclaPrioridad; }
            set { _TeclaPrioridad = value; }
        }

        private bool _Tecla55mas1;
        public bool Tecla55mas1
        {
            get { return _Tecla55mas1; }
            set { _Tecla55mas1 = value; }
        }

        private bool _Monitoring;
        public bool Monitoring
        {
            get { return _Monitoring; }
            set { _Monitoring = value; }
        }

        private bool _CoordinadorTF;
        public bool CoordinadorTF
        {
            get { return _CoordinadorTF; }
            set { _CoordinadorTF = value; }
        }

        private bool _CoordinadorRD;
        public bool CoordinadorRD
        {
            get { return _CoordinadorRD; }
            set { _CoordinadorRD = value; }
        }

        private bool _IntegracionRDTF;
        public bool IntegracionRDTF
        {
            get { return _IntegracionRDTF; }
            set { _IntegracionRDTF = value; }
        }

        private bool _LlamadaSelectiva;
        public bool LlamadaSelectiva
        {
            get { return _LlamadaSelectiva; }
            set { _LlamadaSelectiva = value; }
        }

        private bool _GrupoBSS;
        public bool GrupoBSS
        {
            get { return _GrupoBSS; }
            set { _GrupoBSS = value; }
        }

        private bool _LTT;
        public bool LTT
        {
            get { return _LTT; }
            set { _LTT = value; }
        }

        private bool _SayAgain;
        public bool SayAgain
        {
            get { return _SayAgain; }
            set { _SayAgain = value; }
        }

        private bool _InhabilitacionRedirec;
        public bool InhabilitacionRedirec
        {
            get { return _InhabilitacionRedirec; }
            set { _InhabilitacionRedirec = value; }
        }

        private bool _Glp;
        public bool Glp
        {
            get { return _Glp; }
            set { _Glp = value; }
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
