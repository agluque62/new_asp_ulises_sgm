using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

using CD40.BD;
using CD40.BD.Entidades;


namespace ConfiguracionElementosHw
{
    /// <summary>
    /// Summary description for ConfiguracionElementosHw
    /// </summary>
    /// 

    //struct MetodosBssDelRecurso
    //{
    //    public int idMetodo;
    //    public string nombreMetodo;

    //    public MetodosBssDelRecurso(int id, string n)
    //    {
    //        idMetodo = id;
    //        nombreMetodo = n;
    //    }
    //}


    #region Parametros Multicast
    public class ParametrosMulticast
    {
        // Dirección IP del grupo multicast para configuración
        public string GrupoMulticastConfiguracion;
        // Puerto UDP para recepción de las notificaciones de configuración
        public uint PuertoMulticastConfiguracion;
    }
    #endregion

    #region Configuracion del sistema
    
    public class AsignacionUsuariosTV
    {
        public string IdUsuario;        // Nombre del usuario
        public string IdHost;           // Identificador del terminal de voz en la red
        public string IpGrabador1;      // Dirección IP del grabador 1 ED-137
        public string IpGrabador2;      // Dirección IP del grabador 2 ED-137
        public int RtspPort;             // Puerto RTSP
    }

    public class AsignacionRecursosGW
    {
        public string IdRecurso;        // Nombre del recurso
        public string IdHost;           // Identificador de la pasarela en la red
        public uint SipPort;            // Puerto SIP
    }

    public class DireccionamientoSIP
    {
        public struct StrNumeroAbonado
        {
            // Nombre de la agrupacion a la que pertenece IdUsuario (Podría pertenecer a una agrupación)
            public string IdAgrupacion;
            // Prefijo del número de abonado
            public uint Prefijo;
            // Numero de abonado al que atiende el usuario
            public string NumeroAbonado;
        }

        // Nombre del usuario
        public string IdUsuario;
        public System.Collections.Generic.List<StrNumeroAbonado> NumerosAbonadoQueAtiende;
    }

    public class PlanRecursos
    {
        public string IdRecurso;    // Nombre del recurso
        public Tipos.TipoInterface Tipo;  // Tipo del recurso
    }

    public class ListaTroncales
    {
        public string IdTroncal;                // Nombre del troncal
        public string NumeroTest;               // Número de la red ATS para efectuar llamada de test
        public PlanRecursos[] ListaRecursos;    // Lista de recursos que componen el troncal
    }

    public class ListaRedes
    {
        public string IdRed;                    // Nombre de la red
        public uint Prefijo;                  // Prefijo para acceder a la red
        public PlanRecursos[] ListaRecursos;    // Lista de recursos que componen la red
    }

    public class PlanRutas
    {
        public string TipoRuta;             // Tipo de la ruta. D: Directa; A: Alternativa
        public string[] ListaTroncales;     // Lista de identificadores de troncales de componen la ruta
    }

    public class NumeracionATS
    {
        public string Central;                   //Nombre de la Central ATS
        public bool CentralPropia;               // Distinguir si la central que se describe es la propia del CD40 u otra externa
        public bool Throwswitching;              // Indica si la central tiene capacidad de hacer throwswitching
        public RangosSCV[] RangosOperador;       // Lista de rangos de números del plan de numeración ATS para operadores
        public RangosSCV[] RangosPrivilegiados;    // Lista de rangos de números del plan de numeración ATS para operadores provilegiados
        public PlanRutas[] ListaRutas;          // Lista de rutas para el establecimiento de llamadas por la red ATS
		public string NumTest;					// Número de test que la central propia debe aceptar
												// sólo aplicable para la central propia. En el resto irá vacío.
	}

    public class DireccionamientoIP
    {
        public string IdHost;                   // Nombre con el que se identifica el elemento hardware en la red
        public string IpRed1;                   // Dirección IP para la red 1
        public string IpRed2;                   // Dirección IP para la red 2
        public string IpRed3;                   // Dirección IP para la red 3 (Proxy 3)
        public Tipos.Tipo_Elemento_HW TipoHost;       // Tipo del elemento hardware 0: TOP 1: TIFX
        public bool Interno;
        public long Min;
        public long Max;
        public bool EsCentralIP;
        public string SrvPresenciaIpRed1;       // Dirección IP:puerto del servidor 1 de presencia
        public string SrvPresenciaIpRed2;       // Dirección IP:puerto del servidor 2 de presencia
        public string SrvPresenciaIpRed3;       // Dirección IP:puerto del servidor 3 de presencia
    }

    public class ParametrosGeneralesSistema
    {
        public uint TiempoMaximoPTT;            // Tiempo máximo en PTT
        public uint TiempoSinJack1; 
        public uint TiempoSinJack2;
        public uint TamLiteralEnlExt;           // Tamaño de los literales en los enlaces externos
        public uint TamLiteralEnlDA;               // Tamaño de los literales en los enlades de acceso directo
        public uint TamLiteralEnlIA;               // Tamaño de los literales en los enlades de acceso inmediato
        public uint TamLiteralEnlAG;               // Tamaño de los literales en los enlades de la agenda
        public uint TamLiteralEmplazamiento;    // Tamaño de los literales en los emplazamientos
    }

    //VMG 12/12/2018
    public class CEncaminaAbonados
    {
        //usuarios_scv[]
        //redes_alternativas[]
    }
    public class CTelefonosSeguridad
    {
        public string TelefonosSeguridad;
    }
    
    public class CParametrosProxy
    {
        public CEncaminaAbonados encamina_abonados;
        public CTelefonosSeguridad[] telefonos_seguridad;
    }


    public class ConfiguracionSistema
    {
        // Estructura con los parámetros generales del CD40
        public ParametrosGeneralesSistema ParametrosGenerales;
        // Plan de numeración ATS
        public NumeracionATS[] PlanNumeracionATS;
        // Plan de direccionamiento IP
        public DireccionamientoIP[] PlanDireccionamientoIP;
        // Plan de troncales
        public ListaTroncales[] PlanTroncales;
        // Lista de redes
        public ListaRedes[] PlanRedes;
        // Tabla de asignación de usuarios a terminales de voz
        public AsignacionUsuariosTV[] PlanAsignacionUsuarios;
        // Tabla de asignación de recursos a pasarelas
        public AsignacionRecursosGW[] PlanAsignacionRecursos;
        // Plan de direccionamiento SIP
        public System.Collections.Generic.List<DireccionamientoSIP> PlanDireccionamientoSIP;
    }

    #endregion

    #region Configuracion de la TIFX
    public class CfgPasarela
    {
        // Nombre de la pasarela con el que se identifica en la red
        public string Nombre;
        // Numero de recursos que aloja
        public int NumRecursos;
        // Modo de sincronización 0: Ninguna; 1: IEE1588; 2: NTP; 3: Protocolo propietario
        public int ModoSincronizacion;
        // Dirección IP:Puerto de master de sincronización.
        public string MasterSincronizacion;
        // Puerto local SNMP
        public uint PuertoLocalSNMP;
        // Puerto remoto SNMP
        public uint PuertoRemotoSNMP;
        // Puerto remoto traps SNMP
        public uint PuertoRemotoTrapsSNMP;
        // Puerto local SIP
        public uint PuertoLocalSIP;
        // Periodo de supervisión de sesiones SIP. 0: no se supervisan
        public uint PeriodoSupervisionSIP;
        // Dirección IP grabador 1.
        public string IpGrabador1;
        // Dirección IP grabador 2.
        public string IpGrabador2;
        //VMG 18/02/2019
        // Supervisa conexion GW
        public byte iSupervLanGW;
        // Tiempo maximo de espera a la conexion con la GW
        public byte itmmaxSupervLanGW;
        // Lista de recursos alojados en la pasarela
        public RecursosSCV[] ListaRecursos;
    }

    #endregion

    #region Configuracion de la TOP
    public class NumerosAbonado
    {
        public uint Prefijo;
        public string Numero;
    }

    public class LoginTerminalVoz
    {
        public string ModoLogin;    // Modo de arranque de la TOP. 'A':Automático; 'M':Manual
        public string IdUsuario;    // Identificador del usuario configurado en la top solicitada.
    }

    public class CfgUsuario
    {
		public string Nombre;   // Nombre de la agrupación
		public string IdIdentificador;   // Nombre original del sector (identificador del usuario dominante)
        public List<NumerosAbonado> ListaAbonados;  // Lista de números de abonado a los que atiende el usuario
        public int NumeroEnlacesInternos;   // Número de enlaces internos
        public int NumeroEnlacesExternos;   // Número de enlaces externos
        public SectoresSCV Sector;             // Incluye Tipo CWP, Prioridad, TipoHMI y Pareja UCS
        public ParametrosSectorSCVKeepAlive ParametrosDelSector;    // Incluye dimensionamiento de páginas, llamadas, enlaces, intrusión y ser intruído.
        public PermisosRedesSCV[] PermisosRedDelSector;   // Lista con los permisos (saliente y entrante) sobre las redes
        public TeclasSectorSCV TeclasDelSector;    // Configuración de la disponibilidad de funciones
        public NivelesSCV NivelesDelSector;        // Niveles para protocolo Q-SIG
    }

    #endregion

    #region Configuracion Enlaces Externos

    public class CfgRecursoEnlaceExterno
    {
        // Identificador del recurso
        public string IdRecurso;
        // Tipo del Recurso. 0..6
        public uint Tipo;
        // Estado de asignación I,A,S
        public string Estado;
        // Modo confirmación PTT. 0,1,2
        public uint ModoConfPTT;
        // Tipo PTT. 0..n
        public uint NumFlujosAudio;
        // Nombre del emplazamiento
        public string IdEmplazamiento;
        // Zona a la que pertenece la covertura del recurso
        public string NombreZona;
        // Metodo BSS seleccionado
        public int MetodoBss;
        // Valor Metodo BSS seleccionado
        public string IdMetodoBss;
        // Tabla calidad audio (BSS)
        public string NameTablaBss;
        // Valores Tabla calidad audio (BSS)
        public int[] ValuesTablaBss;
        // Parametro GRS Delay
        public int GrsDelay;
        // Parametro OffSetFrequency
        public int OffSetFrequency;
        // Parametro EnableEventPttSq
        public bool EnableEventPttSq;
        //public string id del recursoPar de redundancia, vacio si no hay redundancia
        public string RedundanciaIdPareja;
        //Rol en 1+1, principal=P, reserva=R, vacio si ho hay redundancia
        public string RedundanciaRol;
    }

    public class CfgEnlaceExterno
    {
        // Literal del enlace, que para el equipo radio contiene el Id Destino
        public string Literal;

        // Alias del enlace
        public string AliasEnlace;

        // Lista de posiciones donde el enlace está configurado
        public List<uint> ListaPosicionesEnHmi;
        // Tipo de frecuencia
        public uint TipoFrecuencia;
        // Exclusividad en asignación de TxRx
        public bool ExclusividadTxRx;
        // Lista de recursos que atienden el enlace
        public List<CfgRecursoEnlaceExterno> ListaRecursos;
        // Estado de asignación
        public string EstadoAsignacion;
        // Lista de los posibles destinos de audio con su estado
        public List<string> DestinoAudio;
        // Prioridad del enlace
        public uint Prioridad;
        // Supervision de portadora para esta frecuencia
        public bool SupervisionPortadora;
        // Frecuencia Sintonizada en equipo HF (Solo HF).
        public int FrecuenciaSintonizada;

        // 0: Relativo
        // 1: Absoluto
        public int MetodoCalculoClimax;
        // 50..2000 msg.
        public int VentanaSeleccionBss;
        public bool SincronizaGrupoClimax;
        public bool AudioPrimerSqBss;
        public bool FrecuenciaNoDesasignable;
        // Ventana de Reposo para Zona TX por Defecto (Para emplazamientos múltiples, ahora no se utilzará)
        public int VentanaReposoZonaTxDefecto;
        // Nombre de la Zona Tx por defecto (No pertenece al modelo)
        public string NombreZonaTxDefecto;
        // Prioridad de la sesión SIP
        public int PrioridadSesionSIP;
        // Parametro CldSupervisionTime
        public int CldSupervisionTime;
        // Parametro Metodos BSS ofrecidos
        public int MetodosBssOfrecidos;
        // Lista de métodos disponibles en el recurso
        //public List<DestinosRadio.MetodosBssDelRecurso> MetodosBss;
        //Modo de transmisión para el modo FD:  R -->Último Receptor, C -->Climax, M -->Manual
        public string ModoTransmision;
        //VMG 18/02/2019
        //Emplazamiento por defecto de la frecuencia 1 --> Si
        public string EmplazamientoDefecto;
        //Tiempo de vuelta al empl por defecto
        public string TiempoVueltaADefecto;

        //Porcentaje que no es RSSI
        public string PorcentajeRSSI;
    }
    #endregion

    #region Configuracion Enlaces Internos

    public class CfgRecursoEnlaceInterno
    {
        // Prefijo del destino
        public uint Prefijo;
        // Nombre del recurso que da servicio al destino o nombre del usuario interno
        public string NombreRecurso;
        // Número de abonado del destino
        public string NumeroAbonado;
    }

	public class CfgRecursoEnlaceInternoConInterface : CfgRecursoEnlaceInterno
	{
		// Tipo de interfaz del recurso
		public Tipos.TipoInterface Interface;

        public string NombreMostrar;
	}

    public class CfgEnlaceInterno
    {
        // Literal del enlace
        public string Literal;
        // Posición donde el enlace está configurado
        public uint PosicionHMI;
        // Tipo del enlace : DA, AI, AG, MD-->Multi Destino VMG
        public string TipoEnlaceInterno;
        // Lista de recursos que atienden al enlace
        public List<CfgRecursoEnlaceInternoConInterface> ListaRecursos;
        // Dependencia dentro de la cual se ubica el destino
        public string Dependencia;
        // Prioridad del enlace
        public uint Prioridad;
        // Identificador del usuario origen para hacer la llamada R2
        public string OrigenR2;
        //VMG 13/12/2018
        //Identificador del dominio.
        public string Dominio;
        
        
    }
    #endregion

    #region Configuración Frecuencias HF

    public class HfRangoFrecuencias
    {
        public UInt32 FMin;
        public UInt32 FMax;
    }

    public class PoolHfElement
    {
        public string Id;
        public string SipUri;
        public string IpGestor;
        public string Oid;
        public HfRangoFrecuencias[] Frecs;
    }

    #endregion

    #region Configuración N+M
    public enum Tipo_Frecuencia
    {
        Basica, HF, VHF, UHF
    }

    public enum Tipo_Canal
    {
        Monocanal, Multicanal, Otros
    }

    public enum Tipo_Formato_Trabajo
    {
        Principal, Reserva, Ambos
    }

    public enum Tipo_Formato_Frecuencia 
    {
        MHz = 0,
	    Hz = 1
    }

    public enum GearChannelSpacings
    {
	    ChannelSpacingsDefault = 0,
        kHz_8_33 = 1, 
        kHz_12_5 = 2,
        kHz_25_00 = 3
    }

    public enum GearCarrierOffStatus
    {
        Off = 0,
        kHz_7_5 = 1,
        kHz_5_0 = 2,
        kHz_2_5 = 3,
        Hz_0_0 = 4,
        kHz_minus_2_5 = 5,
        kHz_minus_5_0 = 6,
        kHz_minus_7_5 = 7,
        kHz_8 = 8,
        kHz_4 = 9,
        kHz_minus_4 = 10,
        kHz_minus_8 = 11,
        kHz_7_3 = 12,
        kHz_minus_7_3 = 13
    }

    public enum GearModulations
    {
        AM = 0,
        Reserved = 1,
        ACARS = 2,
        VDL2 = 3,
        AM_CT = 4
    }

    public enum GearPowerLevels
    {
	    PowerLevelsDefault = 0,
        Low = 1,
        Normal = 2
    }

    public class Node : PoolHfElement
    {
        public bool EsReceptor;
        public bool EsTransmisor;
        public Tipo_Frecuencia TipoDeFrecuencia;
        public string FrecuenciaPrincipal;
        public Tipo_Canal TipoDeCanal;
        public Tipo_Formato_Trabajo FormaDeTrabajo;
        public int Prioridad;
        public uint Puerto;
        public GearCarrierOffStatus Offset;
        public GearChannelSpacings Canalizacion;
        public GearModulations Modulacion;
        public GearPowerLevels NivelDePotencia;
        public Tipo_Formato_Frecuencia FormatoFrecuenciaPrincipal;
        public int ModeloEquipo;
        public string IdEmplazamiento;
    }

    #endregion
}
