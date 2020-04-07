using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class Recursos : RecursosSCV
    {
		public const int Modo_Datos = 0;
		public const int Modo_Audio = 1;
		public const int Tipo_Rx = 0;
		public const int Tipo_Tx = 1;
		public const int Tipo_RxTx = 2;
		
		#region Propiedades de Recursos
        // Nombre del sistema
        private string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }
        // Tipo de recurso interno para configuración
        private uint _TipoRecurso;
        public uint TipoRecurso
        {
            get { return _TipoRecurso; }
            set { _TipoRecurso = value; }
        }
		// Nombre del equipo externo que alberga al recurso si no va en una pasarela
		private string _IdEquipo;
		public string IdEquipo
		{
			get { return _IdEquipo; }
			set { _IdEquipo = value; }
		}
		// Nombre de la pasarela que alberga al recurso
		private string _IdTifX;
		public string IdTifX
		{
			get { return _IdTifX; }
			set { _IdTifX = value; }
		}
        // Puerto SIP
        private uint _PuertoSip;
        public uint PuertoSip
        {
            get { return _PuertoSip; }
            set { _PuertoSip = value; }
        }
        private string _IpRed1;//Devolver la Ip del/los encaminamientos
        public string IpRed1
        {
            get { return _IpRed1; }
            set { _IpRed1 = value; }
        }
        
        #endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public Recursos()
        {
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
        }

        public override string DataSetSelectSQL()
        {
            StringBuilder strConsulta = new StringBuilder();

            if (IdSistema != null && IdRecurso != null && TipoRecurso == (uint)Tipos.Tipo_Recurso.TR_DONT_CARE)
                strConsulta.Append("SELECT r.*,e.sipport as SipPort, e.IpRed1 as IpRed1 FROM recursos r " +
                                    "LEFT JOIN equiposeu e ON e.idEquipos=r.idEquipos " +
                                    "WHERE r.idequipos IS NOT NULL AND " +
                                            "r.IdSistema='" + IdSistema + "' AND IdRecurso='" + IdRecurso + "' " +
                                "UNION " +
                                "SELECT r.*,e.sipportlocal as SipPort, e.IpRed1 as IpRed1 FROM recursos r " +
                                    "LEFT JOIN TIFX e ON e.idTifx=r.idTifx " +
                                    "WHERE r.idTifx IS NOT NULL AND " +
                                    "r.IdSistema='" + IdSistema + "' AND IdRecurso='" + IdRecurso + "'");
            else if (IdSistema != null && IdRecurso != null)
                strConsulta.Append("SELECT r.*,e.sipport as SipPort, e.IpRed1 as IpRed1 FROM recursos r " +
                                    "LEFT JOIN equiposeu e ON e.idEquipos=r.idEquipos " +
                                    "WHERE r.idequipos IS NOT NULL AND r.IdSistema='" + IdSistema + "' AND IdRecurso='" + IdRecurso + "' AND TipoRecurso=" + TipoRecurso +
                                " UNION " +
                                "SELECT r.*,e.sipportlocal as SipPort, e.IpRed1 as IpRed1 FROM recursos r " +
                                    "LEFT JOIN TIFX e ON e.idTifx=r.idTifx " +
                                    "WHERE r.idTifx IS NOT NULL AND r.IdSistema='" + IdSistema + "' AND IdRecurso='" + IdRecurso + "' AND TipoRecurso=" + TipoRecurso);
            else if (IdSistema != null && IdTifX != null)
                strConsulta.Append("SELECT r.*,e.sipportlocal as SipPort, e.IpRed1 as IpRed1 FROM recursos r " +
                                    "LEFT JOIN TIFX e ON e.idTifx=r.idTifx " +
                                    "WHERE r.IdSistema='" + IdSistema + "' AND r.IdTIFX='" + IdTifX + "' ORDER BY SlotPasarela,NumDispositivoSlot");
            else if (IdSistema != null && IdEquipo != null)
                strConsulta.Append("SELECT r.*,e.sipport as SipPort, e.IpRed1 as IpRed1 FROM recursos r " +
                                    "LEFT JOIN equiposeu e ON e.idEquipos=r.idEquipos " +
                                    "WHERE r.IdSistema='" + IdSistema + "' AND r.IdEquipos='" + IdEquipo + "'");
            else if (IdSistema != null && TipoRecurso != (uint)Tipos.Tipo_Recurso.TR_DONT_CARE) // Se incluye el TipoRecurso para poder buscar los recursos del tipo. No quitar del WHERE
                strConsulta.Append("SELECT r.*,e.sipport as SipPort, e.IpRed1 as IpRed1 FROM recursos r " +
                                    "LEFT JOIN equiposeu e ON e.idEquipos=r.idEquipos " +
                                    "WHERE r.idequipos IS NOT NULL AND r.IdSistema='" + IdSistema + "' AND TipoRecurso=" + TipoRecurso +
                                " UNION " +
                                "SELECT r.*,e.sipportlocal as SipPort, e.IpRed1 as IpRed1 FROM recursos r " +
                                    "LEFT JOIN TIFX e ON e.idTifx=r.idTifx " +
                                    "WHERE r.idTifx IS NOT NULL AND r.IdSistema='" + IdSistema + "' AND TipoRecurso=" + TipoRecurso +"  ORDER BY IdRecurso");
            else if (IdSistema != null && TipoRecurso == (uint)Tipos.Tipo_Recurso.TR_DONT_CARE) // Se incluye el TipoRecurso para poder buscar los recursos del tipo. No quitar del WHERE
                strConsulta.Append("SELECT r.*,e.sipport as SipPort, e.IpRed1 as IpRed1 FROM recursos r " +
                                    "LEFT JOIN equiposeu e ON e.idEquipos=r.idEquipos " +
                                    "WHERE r.idequipos IS NOT NULL AND r.IdSistema='" + IdSistema + "'" +
                                " UNION " +
                                "SELECT r.*,e.sipportlocal as SipPort, e.IpRed1 as IpRed1 FROM recursos r " +
                                    "LEFT JOIN TIFX e ON e.idTifx=r.idTifx " +
                                    "WHERE r.idTifx IS NOT NULL AND r.IdSistema='" + IdSistema + "'  ORDER BY IdRecurso");
            else if (IdRecurso != null)
                strConsulta.Append("SELECT r.*,e.sipport as SipPort, e.IpRed1 as IpRed1 FROM recursos r " +
                                    "LEFT JOIN equiposeu e ON e.idEquipos=r.idEquipos " +
                                    "WHERE r.idequipos IS NOT NULL AND IdRecurso='" + IdRecurso + "'" +
                                " UNION " +
                                "SELECT r.*,e.sipportlocal as SipPort, e.IpRed1 as IpRed1 FROM recursos r " +
                                    "LEFT JOIN TIFX e ON e.idTifx=r.idTifx " +
                                    "WHERE r.idTifx IS NOT NULL AND IdRecurso='" + IdRecurso + "'");
            else
                strConsulta.Append("SELECT * FROM Recursos");

            return strConsulta.ToString();
        }

        public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds)
        {
            ListaResultado.Clear();

			//DataSetResultado = this.DataSetSelectSQL();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    Recursos r = new Recursos();

                    r.IdRecurso = (string)dr["IdRecurso"];
                    r.IdSistema = (string)dr["IdSistema"];
                    r.TipoRecurso = (uint)dr["TipoRecurso"];
                    if (dr["IdEquipos"] != System.DBNull.Value)
                        r.IdEquipo = (string)dr["IdEquipos"];
                    if (dr["IdTIFX"] != System.DBNull.Value)
                        r.IdTifX = (string)dr["IdTIFX"];
                    if (dr["Tipo"] != System.DBNull.Value)
                        r.Tipo = (uint)dr["Tipo"];
                    if (dr["Interface"] != System.DBNull.Value)
                        r.Interface = (Tipos.TipoInterface)(uint)dr["Interface"];
                    if (dr["SlotPasarela"] != System.DBNull.Value)
                        r.SlotPasarela = (int)dr["SlotPasarela"];
                    if (dr["NumDispositivoSlot"] != System.DBNull.Value)
                        r.NumDispositivoSlot = (int)dr["NumDispositivoSlot"];
                    if (dr["ServidorSIP"] != System.DBNull.Value)
                        r.ServidorSIP = (string)dr["ServidorSIP"];
                    if (dr["Diffserv"] != System.DBNull.Value)
                        r.Diffserv = Convert.ToBoolean(dr["Diffserv"]); 
                    if (dr["SipPort"] != System.DBNull.Value)
                        r.PuertoSip = Convert.ToUInt32(dr["SipPort"]); 
                    if (dr["IpRed1"] != System.DBNull.Value)
                        r.IpRed1 = (string)(dr["IpRed1"]);

                    ListaResultado.Add(r);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
			Consulta.Append("INSERT INTO Recursos (IdSistema,IdRecurso,TipoRecurso,IdEquipos,IdTIFX,Tipo," +
                            "Interface,SlotPasarela,NumDispositivoSlot,ServidorSIP,Diffserv)" +
                            " VALUES ('" + IdSistema + "','" +
                                         IdRecurso + "'," +
                                         TipoRecurso + "," +
										 ((IdEquipo == null) ? "null," : ("'" + IdEquipo + "',")) +
										 ((IdTifX == null) ? "null," : ("'" + IdTifX + "',")) +
										 Tipo + "," +
                                         (int)Interface + "," +
                                         SlotPasarela + "," +
                                         NumDispositivoSlot + "," +
                                         ((ServidorSIP == null) ? "null," : ("'" + ServidorSIP + "',")) +
                                         Diffserv + 
                                         ")");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Recursos");
			return consulta;
        }

		public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE Recursos SET IdRecurso='" + IdRecurso + "'," +
                                            "IdSistema='" + IdSistema + "'," +
                                            "TipoRecurso=" + TipoRecurso + "," +
											"IdEquipos=" + ((IdEquipo == null) ? "null, " : ("'" + IdEquipo + "',")) +
											"IdTifX=" + ((IdTifX == null) ? "null, " : ("'" + IdTifX + "',")) +
											"Tipo=" + Tipo + "," +
                                            "Interface=" + (int)Interface + "," +
                                            "SlotPasarela=" + SlotPasarela + "," +
                                            "NumDispositivoSlot=" + NumDispositivoSlot + "," +
                                            "ServidorSIP=" + ((ServidorSIP == null) ? "null, " : ("'" + ServidorSIP + "',")) +
                                            "Diffserv=" + Diffserv +
                                            // Quitamos TipoRecurso del WHERE porque puede que haya cambiado el tipo de interfaz
                                            // en un recurso de telefonía de LCEN a otro de telefonía o vv. en cuyo caso, el update no lo encontraría
                                            " WHERE IdRecurso='" + IdRecurso + "' AND IdSistema='" + IdSistema + "'"    
                                            );

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Recursos");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];
			
			Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdRecurso != null)
                Consulta.Append("DELETE FROM Recursos WHERE IdSistema='" + IdSistema + "' AND IdRecurso='" + IdRecurso + "' AND TipoRecurso=" + TipoRecurso);
            else if (IdSistema != null)
                Consulta.Append("DELETE FROM Recursos WHERE IdSistema='" + IdSistema + "'");
            else if (IdRecurso != null)
                Consulta.Append("DELETE FROM Recursos WHERE IdRecurso='" + IdRecurso + "'");
            else
                Consulta.Append("DELETE FROM Recursos");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Recursos");
			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM Recursos WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
    }
}
