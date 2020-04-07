using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class ParametrosLCENSCV : ParametrosRecursoGeneral
    {
        #region Propiedades ParametrosLCENSCV
        private uint _T1;
        public uint T1
        {
            get { return _T1; }
            set { _T1 = value; }
        }

        private uint _T1Max;
        public uint T1Max
        {
            get { return _T1Max; }
            set { _T1Max = value; }
        }

        private uint _T2;
        public uint T2
        {
            get { return _T2; }
            set { _T2 = value; }
        }

        private uint _T2Max;
        public uint T2Max
        {
            get { return _T2Max; }
            set { _T2Max = value; }
        }

        private uint _T3;
        public uint T3
        {
            get { return _T3; }
            set { _T3 = value; }
        }

        private uint _T4;
        public uint T4
        {
            get { return _T4; }
            set { _T4 = value; }
        }

        private uint _T4Max;
        public uint T4Max
        {
            get { return _T4Max; }
            set { _T4Max = value; }
        }

        private uint _T5;
        public uint T5
        {
            get { return _T5; }
            set { _T5 = value; }
        }

        private uint _T5Max;
        public uint T5Max
        {
            get { return _T5Max; }
            set { _T5Max = value; }
        }

        private uint _T6;
        public uint T6
        {
            get { return _T6; }
            set { _T6 = value; }
        }

        private uint _T6Max;
        public uint T6Max
        {
            get { return _T6Max; }
            set { _T6Max = value; }
        }

        private uint _T8;
        public uint T8
        {
            get { return _T8; }
            set { _T8 = value; }
        }

        private uint _T8Max;
        public uint T8Max
        {
            get { return _T8Max; }
            set { _T8Max = value; }
        }

        private uint _T9;
        public uint T9
        {
            get { return _T9; }
            set { _T9 = value; }
        }

        private uint _T9Max;
        public uint T9Max
        {
            get { return _T9Max; }
            set { _T9Max = value; }
        }

        private uint _T10;
        public uint T10
        {
            get { return _T10; }
            set { _T10 = value; }
        }

        private uint _T10Max;
        public uint T10Max
        {
            get { return _T10Max; }
            set { _T10Max = value; }
        }

        private uint _T11;
        public uint T11
        {
            get { return _T11; }
            set { _T11 = value; }
        }

        private uint _T11Max;
        public uint T11Max
        {
            get { return _T11Max; }
            set { _T11Max = value; }
        }

        private uint _T12;
        public uint T12
        {
            get { return _T12; }
            set { _T12 = value; }
        }
        // Frecuencia tono Rx
        private uint _FrqTonoSQ;
        public uint FrqTonoSQ
        {
            get { return _FrqTonoSQ; }
            set { _FrqTonoSQ = value; }
        }
        // Umbral detección Rx
        private int _UmbralTonoSQ;
        public int UmbralTonoSQ
        {
            get { return _UmbralTonoSQ; }
            set { _UmbralTonoSQ = value; }
        }
        // Frecuencia tono Tx
        private uint _FrqTonoPTT;
        public uint FrqTonoPTT
        {
            get { return _FrqTonoPTT; }
            set { _FrqTonoPTT = value; }
        }
        // Umbral detección Tx
        private int _UmbralTonoPTT;
        public int UmbralTonoPTT
        {
            get { return _UmbralTonoPTT; }
            set { _UmbralTonoPTT = value; }
        }
        // Periodo de refrescos de estados. En msg.
        private uint _RefrescoEstados;
        public uint RefrescoEstados
        {
            get { return _RefrescoEstados; }
            set { _RefrescoEstados = value; }
        }
        // Timeout de refrescos de estados
        private uint _Timeout;
        public uint Timeout
        {
            get { return _Timeout; }
            set { _Timeout = value; }
        }
        // Longitud de ráfagas en transiciones de estados
        private uint _LongRafagas;
        public uint LongRafagas
        {
            get { return _LongRafagas; }
            set { _LongRafagas = value; }
        }


        #endregion

        public System.Collections.Generic.List<string> ListaEnlacesInternos;

        public override string DataSetSelectSQL() { return null; }
		public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds) { return null; }
        public override string[] InsertSQL() { return null; }
        public override string[] UpdateSQL() { return null; }
        public override string[] DeleteSQL() { return null; }
        public override string[] UpdateDestinoSQL() { return null; }
        public override string[] LiberaDestinoSQL() { return null; }
    }
}
