using System;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class Tipos
    {
        public enum TipoInterface
        { TI_Radio, TI_LCEN, TI_BC, TI_BL, TI_AB, TI_ATS_R2, TI_ATS_N5, TI_ATS_QSIG, TI_ISDN_2BD, TI_ISDN_30BD, TI_I_O, TI_DATOS,
            TI_RRC, TI_EM_PP = 50, TI_EM_MARC = 51,TI_NOT_FOUND=255}
        public enum Tipo_Elemento_HW { TEH_TOP, TEH_TIFX, TEH_EXTERNO_RADIO, TEH_EXTERNO_TELEFONIA, TEH_SISTEMA, TEH_GRABADOR }
        public enum Tipo_Enlace { TE_EXTERNO, TE_INTERNO, TE_LCEN }
        public enum Tipo_Recurso { TR_RADIO, TR_TELEFONIA, TR_LC, TR_DATOS, TR_NM, TR_DONT_CARE = 255 }
		public enum tipo { DatosRx, AudioRx, DatosTx, AudioTx, DatosRxTx, AudioRxTx };
        public enum Tipo_EM { Type_I, Type_II, Type_III, Type_IV, Type_V };
        public enum Modo_EM { W2, W4 };
        public enum Lado_EM { Lado_EM, Lado_PLR };
 
        public enum Tipo_Sector { TS_MANTENIMIENTO = 'M', TS_REAL = 'R', TS_VIRTUAL = 'V' }
        public enum Tipo_Posicion { TP_CONTROLADOR = 'C', TP_PLANIFICADOR = 'P' }
        public enum Nivel_Prioridad_R2 { NVPR2_ALTA = 2, NVPR2_MEDIA = 3, NVPR2_BAJA = 4 }
        public enum Tipo_HMI { TH_ACC, TH_TWR }
        //public enum EstadoNodes { DESCONOCIDO, NO_USADO, PRINCIPAL, RESERVA}
        public enum PresenciaNodes { NO_PRESENTE, PRESENTE }

        public enum Tipo_Panel { PANEL_NULO=0, PANEL_LC=1, PANEL_TELEFONIA=2, PANEL_RADIO=3 }


        public class ExportaTipoEnumerados
        {
            public TipoInterface TI;
            public Tipo_Elemento_HW TEH;
            public Tipo_Enlace TE;
            public Tipo_Recurso TR;
        }
    }
}
