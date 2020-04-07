using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class RecursosTF : ParametrosGrabacionTf
    {
        #region Propiedades de RecursosTF
        // Identificador del sistema al que pertenece el recurso
        private string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; _IdSistemaSCV = value; }
        }
        // Identificador del recurso
        private string _IdRecursos;
        public string IdRecurso
        {
            get { return _IdRecursos; }
            set { _IdRecursos = value; _IdRecursoSCV = value; }
        }
        // Prefijo del Destino asociado al recurso
        private uint _IdPrefijo;
        public uint IdPrefijo
        {
            get { return _IdPrefijo; }
            set { _IdPrefijo = value; }
        }
        // Tipo de Destino al que está asociado.
        private uint _TipoDestino;
        public uint TipoDestino
        {
            get { return _TipoDestino; }
            set { _TipoDestino = value; }
        }
        // Identificador del Destino al que está asociado.
        private string _IdDestino;
        public string IdDestino
        {
            get { return _IdDestino; }
            set { _IdDestino = value; }
        }
        // Nombre del troncal al que pertenece el recurso (si es que pertenece a un troncal)
        private string _IdTroncal;
        public string IdTroncal
        {
            get { return _IdTroncal; }
            set { _IdTroncal = value; }
        }
        // Nombre de la red a la que pertenece el recurso (si es que pertenece a una red)
        private string _IdRed;
        public string IdRed
        {
            get { return _IdRed; }
            set { _IdRed = value; }
        }
        //VMG 23/10/2018
        //Tiempo teniendo información acústica antes de pasar el interfaz a reposo 
        private uint _TReleaseBL;
        public uint TReleaseBL
        {
            get { return _TReleaseBL; }
            set { _TReleaseBL = value; }
        }
        //Detección de Caller-Id en llamadas entrantes
        private byte _iDetCallerId;
        public byte iDetCallerId
        {
            get { return _iDetCallerId; }
            set { _iDetCallerId = value; }
        }
        //Tiempo para detectar CallerId en unidades de 100 ms
        private uint _iTmCallerId;
        public uint iTmCallerId
        {
            get { return _iTmCallerId; }
            set { _iTmCallerId = value; }
        }
        //Tiene en cuenta la inversión de polaridad en el comportamiento de la línea de AB
        private byte _iDetInversionPol;
        public byte iDetInversionPol
        {
            get { return _iDetInversionPol; }
            set { _iDetInversionPol = value; }
        }
        //Tiempo maximo llamada entrante en lineas tlf
        private uint _iTmLlamEntrante;
        public uint iTmLlamEntrante
        {
            get { return _iTmLlamEntrante; }
            set { _iTmLlamEntrante = value; }
        }
        //Tiempo para detectar fin de llamada desde que llega ringoff para lineas de AB
        private uint _iTmDetFinLlamada;
        public uint iTmDetFinLlamada
        {
            get { return _iTmDetFinLlamada; }
            set { _iTmDetFinLlamada = value; }
        }
        //Periodo para supervisar señal ring en llamadas entrantes
        private uint _iPeriodoSpvRing;
        public uint iPeriodoSpvRing
        {
            get { return _iPeriodoSpvRing; }
            set { _iPeriodoSpvRing = value; }
        }
        //Número de veces que hay que supervisar para dar por válido un valor
        private uint _iFiltroSpvRing;
        public uint iFiltroSpvRing
        {
            get { return _iFiltroSpvRing; }
            set { _iFiltroSpvRing = value; }
        }
        //Deteccion dtmf para líneas BC
        private byte _iDetDtmf;
        public byte iDetDtmf
        {
            get { return _iDetDtmf; }
            set { _iDetDtmf = value; }
        }


        #endregion

        public RecursosTF()
            : base()
        {
            TipoRecurso = (uint)Tipos.Tipo_Recurso.TR_TELEFONIA;
        }

        //public new string DataSetSelectSQL()
        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdDestino != null)
                Consulta.Append("SELECT r.*,pr.* FROM RecursosTF r,ParametrosRecurso pr WHERE r.IdSistema='" + IdSistema + "' AND r.IdDestino='" + IdDestino + "' AND r.IdPrefijo=" + IdPrefijo +
                                    " AND r.IdSistema=pr.IdSistema AND r.IdRecurso=pr.IdRecurso AND r.TipoRecurso=pr.TipoRecurso");
            else if (IdSistema != null && IdRed != null)
                Consulta.Append("SELECT r.*,pr.* FROM RecursosTF r,ParametrosRecurso pr WHERE r.IdSistema='" + IdSistema + "' AND r.IdRed='" + IdRed + "'" +
                                    " AND r.IdSistema=pr.IdSistema AND r.IdRecurso=pr.IdRecurso AND r.TipoRecurso=pr.TipoRecurso");
            else if (IdSistema != null && IdTroncal != null)//VMG 11/12/2018
                Consulta.Append("SELECT * FROM (SELECT r.*,pr.GananciaAGCRX, pr.GananciaAGCRXdBm, pr.GananciaAGCTX, pr.GananciaAGCTXdBm, pr.SupresionSilencio, pr.TamRTP, pr.Codec, pr.TipoEM, pr.GrabacionEd137,"+
                                "pr.TReleaseBL, pr.iDetCallerId, pr.iTmCallerId, pr.iDetInversionPol, pr.iTmLlamEntrante, pr.iTmDetFinLlamada, pr.iPeriodoSpvRing, pr.ifiltroSpvRing, pr.iDetDtmf " +
                                                "FROM RecursosTF r,ParametrosRecurso pr WHERE r.IdSistema='" + IdSistema + "' AND r.IdTroncal='" + IdTroncal + "'	AND r.IdSistema=pr.IdSistema AND r.IdRecurso=pr.IdRecurso AND r.TipoRecurso=pr.TipoRecurso AND r.lado='0' ORDER BY r.IdRecurso ASC) AS A " +
                                "UNION ALL " +
                                "SELECT * FROM (SELECT r.*,pr.GananciaAGCRX, pr.GananciaAGCRXdBm,pr.GananciaAGCTX,pr.GananciaAGCTXdBm,pr.SupresionSilencio,pr.TamRTP,pr.Codec,pr.TipoEM,pr.GrabacionEd137,"+
                                "pr.TReleaseBL, pr.iDetCallerId, pr.iTmCallerId, pr.iDetInversionPol, pr.iTmLlamEntrante, pr.iTmDetFinLlamada, pr.iPeriodoSpvRing, pr.ifiltroSpvRing, pr.iDetDtmf " +
                                "FROM RecursosTF r,ParametrosRecurso pr WHERE r.IdSistema='" + IdSistema + "' AND r.IdTroncal='" + IdTroncal + "' AND r.IdSistema=pr.IdSistema AND r.IdRecurso=pr.IdRecurso AND r.TipoRecurso=pr.TipoRecurso AND r.lado='1' ORDER BY r.IdRecurso DESC) AS B");
            else if (IdSistema != null && IdRecurso != null)
                Consulta.Append("SELECT r.*,pr.* FROM RecursosTF r,ParametrosRecurso pr WHERE r.IdSistema='" + IdSistema + "' AND r.IdRecurso='" + IdRecurso + "'" +
                                    " AND r.IdSistema=pr.IdSistema AND r.IdRecurso=pr.IdRecurso AND r.TipoRecurso=pr.TipoRecurso");
            else if (IdSistema != null)
                Consulta.Append("SELECT r.*,pr.* FROM RecursosTF r,ParametrosRecurso pr WHERE r.IdSistema='" + IdSistema + "'" +
                                    " AND r.IdSistema=pr.IdSistema AND r.IdRecurso=pr.IdRecurso AND r.TipoRecurso=pr.TipoRecurso ORDER BY r.IdRecurso");
            else
                Consulta.Append("SELECT r.*,pr.* FROM RecursosTF r,ParametrosRecurso pr WHERE" +
                                    " r.IdSistema=pr.IdSistema AND r.IdRecurso=pr.IdRecurso AND r.TipoRecurso=pr.TipoRecurso");

            return Consulta.ToString();
        }

        public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds)
        {
            //base.ListSelectSQL();

            ListaResultado.Clear();

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    RecursosTF pr = new RecursosTF();

                    IdRecurso = pr.IdRecurso = (string)dr["IdRecurso"];
                    IdSistema = pr.IdSistema = (string)dr["IdSistema"];
                    TipoRecurso = pr.TipoRecurso = (uint)dr["TipoRecurso"];

                    // Puede que haya que poner r. y pr. antes de cada campo para identificarlo
                    if (dr["IdPrefijo"] != System.DBNull.Value)
                        pr.IdPrefijo = (uint)dr["IdPrefijo"];
                    if (dr["TipoDestino"] != System.DBNull.Value)
                        pr.TipoDestino = (uint)dr["TipoDestino"];
                    if (dr["IdDestino"] != System.DBNull.Value)
                        pr.IdDestino = (string)dr["IdDestino"];
                    if (dr["IdTroncal"] != System.DBNull.Value)
                        pr.IdTroncal = (string)dr["IdTroncal"];
                    if (dr["IdRed"] != System.DBNull.Value)
                        pr.IdRed = (string)dr["IdRed"];
                    if (dr["Lado"] != System.DBNull.Value)
                        pr.Lado = (string)dr["Lado"];
                    if (dr["Modo"] != System.DBNull.Value)
                        pr.Modo = (string)dr["Modo"];
                    if (dr["GananciaAGCTX"] != System.DBNull.Value)
                        pr.GananciaAGCTX = (uint)dr["GananciaAGCTX"];
                    if (dr["GananciaAGCTXdBm"] != System.DBNull.Value)
                        pr.GananciaAGCTXdBm = (float)dr["GananciaAGCTXdBm"];
                    if (dr["GananciaAGCRX"] != System.DBNull.Value)
                        pr.GananciaAGCRX = (uint)dr["GananciaAGCRX"];
                    if (dr["GananciaAGCRXdBm"] != System.DBNull.Value)
                        pr.GananciaAGCRXdBm = (float)dr["GananciaAGCRXdBm"];
                    if (dr["SupresionSilencio"] != System.DBNull.Value)
                        pr.SupresionSilencio = Convert.ToBoolean(dr["SupresionSilencio"]);   //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0;
                    if (dr["TamRTP"] != System.DBNull.Value)
                        pr.TamRTP = (uint)dr["TamRTP"];
                    if (dr["Codec"] != System.DBNull.Value)
                        pr.Codec = (uint)dr["Codec"];
                    if (dr["TipoEM"] != System.DBNull.Value)
                        pr.TipoEM = (Tipos.Tipo_EM)(int)dr["TipoEM"];
                    if (dr["GrabacionEd137"] != System.DBNull.Value)
                        pr.GrabacionEd137 = Convert.ToBoolean(dr["GrabacionEd137"]);   //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0;
                    if (dr["TReleaseBL"] != System.DBNull.Value)
                        pr.TReleaseBL = Convert.ToUInt32(dr["TReleaseBL"]);
                    if (dr["iDetCallerId"] != System.DBNull.Value)
                        pr.iDetCallerId = Convert.ToByte(dr["iDetCallerId"]);
                    if (dr["iTmCallerId"] != System.DBNull.Value)
                        pr.iTmCallerId = Convert.ToUInt32(dr["iTmCallerId"]);
                    if (dr["iDetInversionPol"] != System.DBNull.Value)
                        pr.iDetInversionPol = Convert.ToByte(dr["iDetInversionPol"]);
                    if (dr["iTmLlamEntrante"] != System.DBNull.Value)
                        pr.iTmLlamEntrante = Convert.ToUInt32(dr["iTmLlamEntrante"]);
                    if (dr["iTmDetFinLlamada"] != System.DBNull.Value)
                        pr.iTmDetFinLlamada = Convert.ToUInt32(dr["iTmDetFinLlamada"]);
                    if (dr["iPeriodoSpvRing"] != System.DBNull.Value)
                        pr.iPeriodoSpvRing = Convert.ToUInt32(dr["iPeriodoSpvRing"]);
                    if (dr["iFiltroSpvRing"] != System.DBNull.Value)
                        pr.iFiltroSpvRing = Convert.ToUInt32(dr["iFiltroSpvRing"]);
                    if (dr["iDetDtmf"] != System.DBNull.Value)
                        pr.iDetDtmf = Convert.ToByte(dr["iDetDtmf"]);

                    ListaResultado.Add(pr);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
            string[] consulta = new string[4];

            Array.Copy(base.InsertSQL(), consulta, 2);

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO RecursosTF (IdSistema,IdRecurso,TipoRecurso,IdPrefijo,TipoDestino,IdDestino,IdTroncal,IdRed,Lado,Modo)" +
                            " VALUES ('" + IdSistema + "','" +
                                        IdRecurso + "'," +
                                        TipoRecurso + "," +
                                        IdPrefijo + "," +
                                        TipoDestino + "," +
                                        ((IdDestino == null) ? "null," : ("'" + IdDestino + "',")) +
                                        ((IdTroncal == null) ? "null," : ("'" + IdTroncal + "',")) +
                                        ((IdRed == null) ? "null, " : ("'" + IdRed + "',")) +
                                        ((Lado == null) ? "null, " : ("'" + Lado + "', ")) +
                                        ((Modo == null) ? "null " : ("'" + Modo + "'")) + ")");

            consulta[2] = Consulta.ToString();
            consulta[3] = ReplaceSQL(IdSistema, "RecursosTF");

            return consulta;
        }

        public override string[] UpdateSQL()
        {
            string[] consulta = new string[5];

            Array.Copy(base.UpdateSQL(), consulta, 3);

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE RecursosTF SET IdRecurso='" + IdRecurso + "'," +
                                            "IdSistema='" + IdSistema + "'," +
                                            "TipoRecurso=" + TipoRecurso + "," +
                                            "TipoDestino=" + TipoDestino + "," +
                                            "IdPrefijo=" + IdPrefijo + "," +
                                            "IdDestino=" + ((IdDestino == null) ? "null, " : ("'" + IdDestino + "',")) +
                                            "IdTroncal=" + ((IdTroncal == null) ? "null, " : ("'" + IdTroncal + "', ")) +
                                            "IdRed=" + ((IdRed == null) ? "null, " : ("'" + IdRed + "', ")) +
                                            "Lado=" + ((Lado == null) ? "null, " : ("'" + Lado + "', ")) +
                                            "Modo=" + ((Modo == null) ? "null " : ("'" + Modo + "' ")) +
                                            "WHERE IdRecurso='" + IdRecurso + "' AND IdSistema='" + IdSistema + "'"
                                            );

            consulta[3] = Consulta.ToString();
            consulta[4] = ReplaceSQL(IdSistema, "RecursosTF");

            return consulta;
        }

        public override string[] DeleteSQL()
        {
            string[] consulta = new string[4];

            Array.Copy(base.InsertSQL(), consulta, 2);

            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdRecurso != null)
                Consulta.Append("DELETE FROM RecursosTF WHERE IdSistema='" + IdSistema + "' AND IdRecurso='" + IdRecurso + "'");
            else if (IdSistema != null)
                Consulta.Append("DELETE FROM RecursosTF WHERE IdSistema='" + IdSistema + "'");
            else
                Consulta.Append("DELETE FROM RecursosTF");

            consulta[2] = Consulta.ToString();
            consulta[3] = ReplaceSQL(IdSistema, "RecursosTF");

            return consulta;
        }

        //public override int SelectCountSQL(string where)
        //{
        //    Consulta.Remove(0, Consulta.Length);
        //    Consulta.Append("SELECT COUNT(*) FROM RecursosTF WHERE " + where);

        //    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
        //}

        public override string[] UpdateDestinoSQL()
        {
            string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE RecursosTF SET IdDestino='" + IdDestino + "'," +
                                            "IdPrefijo=" + IdPrefijo + "," +
                                            "TipoDestino=" + TipoDestino + " " +
                                            "WHERE IdRecurso='" + IdRecurso + "' AND " + "IdSistema='" + IdSistema + "'"
                                            );
            consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(IdSistema, "RecursosTF");

            return consulta;
        }

        public override string[] LiberaDestinoSQL()
        {
            string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE RecursosTF SET IdDestino=null,TipoDestino=null,IdPrefijo=null " +
                                            "WHERE IdDestino='" + IdDestino + "' AND " + "IdSistema='" + IdSistema + "' AND TipoDestino=" + TipoDestino
                                            );

            consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(IdSistema, "RecursosTF");

            return consulta;
        }

    }
}
