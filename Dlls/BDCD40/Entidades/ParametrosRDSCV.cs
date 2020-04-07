using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class ParametrosRDSCV : ParametrosRecursoGeneral
    {
        #region Propiedades ParametrosRDSCV
        private bool _EM;
        public bool EM
        {
            get { return _EM; }
            set { _EM = value; }
        }
        // Tipo squelch: h: HW; s: SW; v: VAD
        private string _SQ;
        public string SQ
        {
            get { return _SQ; }
            set { _SQ = value; }
        }
        // Tipo PTT. h:HW; s:SW; m:HW+SW
        private string _PTT;
        public string PTT
        {
            get { return _PTT; }
            set { _PTT = value; }
        }
        // Frecuencia tono E
        private uint _FrqTonoE;
        public uint FrqTonoE
        {
            get { return _FrqTonoE; }
            set { _FrqTonoE = value; }
        }
        // Umbral tono E
        private int _UmbralTonoE;
        public int UmbralTonoE
        {
            get { return _UmbralTonoE; }
            set { _UmbralTonoE = value; }
        }
        // Frecuencia tono M
        private uint _FrqTonoM;
        public uint FrqTonoM
        {
            get { return _FrqTonoM; }
            set { _FrqTonoM = value; }
        }
        // Umbral tono E
        private int _UmbralTonoM;
        public int UmbralTonoM
        {
            get { return _UmbralTonoM; }
            set { _UmbralTonoM = value; }
        }
        // Frecuencia tono SQ
        private uint _FrqTonoSQ;
        public uint FrqTonoSQ
        {
            get { return _FrqTonoSQ; }
            set { _FrqTonoSQ = value; }
        }
        // Umbral detección SQ
        private int _UmbralTonoSQ;
        public int UmbralTonoSQ
        {
            get { return _UmbralTonoSQ; }
            set { _UmbralTonoSQ = value; }
        }
        // Frecuencia tono PTT
        private uint _FrqTonoPTT;
        public uint FrqTonoPTT
        {
            get { return _FrqTonoPTT; }
            set { _FrqTonoPTT = value; }
        }
        // Umbral detección PTT
        private int _UmbralTonoPTT;
        public int UmbralTonoPTT
        {
            get { return _UmbralTonoPTT; }
            set { _UmbralTonoPTT = value; }
        }
        // BSS: Cálculo param. Q. S: Acivo; N: Inactivo
        private bool _BSS;
        public bool BSS
        {
            get { return _BSS; }
            set { _BSS = value; }
        }
        // NTZ SI/NO
        private bool _NTZ;
        public bool NTZ
        {
            get { return _NTZ; }
            set { _NTZ = value; }
        }
        // Tipo NTZ. 0: Cierre contactos; 1: Señal 0v; 2: Señal 5v.
        private uint _TipoNTZ;
        public uint TipoNTZ
        {
            get { return _TipoNTZ; }
            set { _TipoNTZ = value; }
        }
        // Cifrado SI/NO
        private bool _Cifrado;
        public bool Cifrado
        {
            get { return _Cifrado; }
            set { _Cifrado = value; }
        }
        // Supervisión portadora Tx SI/NO
        private bool _SupervPortadoraTx;
        public bool SupervPortadoraTx
        {
            get { return _SupervPortadoraTx; }
            set { _SupervPortadoraTx = value; }
        }
        // Supervisión moduladora Tx SI/NO
        private bool _SupervModuladoraTx;
        public bool SupervModuladoraTx
        {
            get { return _SupervModuladoraTx; }
            set { _SupervModuladoraTx = value; }
        }
        // Modo confirmación PTT. 0: No confirma; 1: A todas las sesiones RTP; 2: Sólo a la sesión RTP de la que transmite audio
        private uint _ModoConfPTT;
        public uint ModoConfPTT
        {
            get { return _ModoConfPTT; }
            set { _ModoConfPTT = value; }
        }
        // Periodo repetición  estados SQ y BSS
        private uint _RepSQyBSS;
        public uint RepSQyBSS
        {
            get { return _RepSQyBSS; }
            set { _RepSQyBSS = value; }
        }
        // Número de paquetes de desactivación de SQ
        private uint _DesactivacionSQ;
        public uint DesactivacionSQ
        {
            get { return _DesactivacionSQ; }
            set { _DesactivacionSQ = value; }
        }
        // Timeout de activación de PTT. En msg.
        private uint _TimeoutPTT;
        public uint TimeoutPTT
        {
            get { return _TimeoutPTT; }
            set { _TimeoutPTT = value; }
        }
        // Umbral detección VAD
        private int _UmbralVAD;
        public int UmbralVAD
        {
            get { return _UmbralVAD; }
            set { _UmbralVAD = value; }
        }
        // Tiempo máximo en PTT. 0:Infinito
        private uint _TiempoPTT;
        public uint TiempoPTT
        {
            get { return _TiempoPTT; }
            set { _TiempoPTT = value; }
        }
        // Número máximo de flujos de audio a transmitir. 0,1: PTT Lockout; >=2 PTT Summarization, indicando máximo.
        private uint _NumFlujosAudio;
        public uint NumFlujosAudio
        {
            get { return _NumFlujosAudio; }
            set { _NumFlujosAudio = value; }
        }

        #endregion

        public struct RecursosExternos
        {
            public string Nombre;
            public string Tipo;
            public uint ModoConfirmacionPtt;
            public uint TipoPtt;
        }

        public System.Collections.Generic.List<RecursosExternos> ListaEnlacesRecursosExternos;

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
