using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
	public class EquiposEU : Tablas
	{
        #region Propiedades de EquiposEU

        private string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }

        private string _IdEquipos;
        public string IdEquipos
        {
            get { return _IdEquipos; }
            set { _IdEquipos = value; }
        }

        private string _IpRed1;
        public string IpRed1
        {
            get { return _IpRed1; }
            set { _IpRed1 = value; }
        }

        private string _IpRed2;
        public string IpRed2
        {
            get { return _IpRed2; }
            set { _IpRed2 = value; }
        }

		private uint _TipoEquipo;
		public uint TipoEquipo
		{
			get { return _TipoEquipo;}
			set { _TipoEquipo = value;}
		}

        private bool _Interno;
        public bool Interno
        {
            get { return _Interno; }
            set { _Interno = value; }
        }

        private int _Min;
        public int Min
        {
            get { return _Min; }
            set { _Min = value; }
        }

        private int _Max;
        public int Max
        {
            get { return _Max; }
            set { _Max = value; }
        }

        private int _SipPort;
        public int SipPort
        {
            get { return _SipPort; }
            set { _SipPort = value; }
        }

        //Dirección IP y puerto del proxy 3
        private string _IpRed3;
        public string IpRed3
        {
            get { return _IpRed3; }
            set { _IpRed3 = value; }
        }

        //Direccion IP y puerto del servidor de presencia 1
        private string _SrvPresenciaIpRed1;
        public string SrvPresenciaIpRed1
        {
            get { return _SrvPresenciaIpRed1; }
            set { _SrvPresenciaIpRed1 = value; }
        }

        //Direccion IP y puerto del servidor de presencia 2
        private string _SrvPresenciaIpRed2;
        public string SrvPresenciaIpRed2
        {
            get { return _SrvPresenciaIpRed2; }
            set { _SrvPresenciaIpRed2 = value; }
        }

        //Direccion IP:puerto del servidor de presencia 3
        private string _SrvPresenciaIpRed3;
        public string SrvPresenciaIpRed3
        {
            get { return _SrvPresenciaIpRed3; }
            set { _SrvPresenciaIpRed3 = value; }
        }

        private bool _bObtenerEquiposCentralIp;
        public bool bObtenerEquiposCentralIp
        {
            get { return _bObtenerEquiposCentralIp; }
            set { _bObtenerEquiposCentralIp = value; }
        }

        #endregion

        public EquiposEU()
        {
            bObtenerEquiposCentralIp = false;
        }

        public override string DataSetSelectSQL()
        {
            StringBuilder strConsulta=new StringBuilder();

            if (!bObtenerEquiposCentralIp)
            {
                if (IdSistema != null && IdEquipos != null)
                    strConsulta.AppendFormat("SELECT * FROM EquiposEU WHERE IdSistema='{0}' AND IdEquipos='{1}' ",IdSistema,IdEquipos);
                else if (IdSistema != null && TipoEquipo != 0)
                    strConsulta.AppendFormat("SELECT * FROM EquiposEU WHERE IdSistema='{0}' AND TipoEquipo='{1}' ", IdSistema, TipoEquipo);
                else if (IdSistema != null)
                    strConsulta.AppendFormat("SELECT * FROM EquiposEU WHERE IdSistema='{0}' " ,IdSistema);
                else
                    strConsulta.Append("SELECT * FROM EquiposEU ");
            }
            else
            {
                //Se obtienen los equipos externos asociados a las Centrales ATS IP definidas en el sistema
                strConsulta.AppendFormat("SELECT * FROM EquiposEU E WHERE IdSistema='{0}'AND  Max=-1 AND Min=-1 AND EXISTS ", IdSistema);
                strConsulta.Append("(SELECT 1 FROM encaminamientos C WHERE C.IdSistema=E.IdSistema AND C.Central=E.IdEquipos and C.CentralIP=1 )");
            }

            return strConsulta.ToString();
        }

        public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds)
        {
            ListaResultado.Clear();

			//DataSetResultado = this.DataSetSelectSQL();
            if (ds != null && ds.Tables.Count>0)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    EquiposEU r = new EquiposEU();

                    r.IdSistema = (string)dr["IdSistema"];
                    r.IdEquipos = (string)dr["IdEquipos"];
					if (dr["IpRed1"] != System.DBNull.Value)
						r.IpRed1 = (string)dr["IpRed1"];
					if (dr["IpRed2"] != System.DBNull.Value)
						r.IpRed2 = (string)dr["IpRed2"];
					if (dr["TipoEquipo"] != System.DBNull.Value)
						r.TipoEquipo = (uint)dr["TipoEquipo"];
                    if (dr["Interno"] != System.DBNull.Value)
                        r.Interno = (bool)dr["Interno"];  //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0;
                    if (dr["Min"] != System.DBNull.Value)
                        r.Min = ((int)dr["Min"]);
                    if (dr["Max"] != System.DBNull.Value)
                        r.Max = ((int)dr["Max"]);
                    if (dr["SipPort"] != System.DBNull.Value)
                        r.SipPort = ((int)dr["SipPort"]);

                    // Se obtiene la dirección IP:puerto del servidor proxy 3
                    if (dr["IpRed3"] != System.DBNull.Value)
                        r.IpRed3 = (string)dr["IpRed3"];

                    // Se obtiene la dirección IP:puerto del servidor 1 de presencia
                    if (dr["SrvPresenciaIpRed1"] != System.DBNull.Value)
                        r.SrvPresenciaIpRed1 = (string)dr["SrvPresenciaIpRed1"];

                    // Se obtiene la dirección IP:puerto del servidor 2 de presencia
                    if (dr["SrvPresenciaIpRed2"] != System.DBNull.Value)
                        r.SrvPresenciaIpRed2 = (string)dr["SrvPresenciaIpRed2"];

                    // Se obtiene la dirección IP:puerto del servidor 3 de presencia
                    if (dr["SrvPresenciaIpRed3"] != System.DBNull.Value)
                        r.SrvPresenciaIpRed3 = (string)dr["SrvPresenciaIpRed3"];

                    ListaResultado.Add(r);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO EquiposEU (IdSistema,IdEquipos,IpRed1,IpRed2,TipoEquipo,Interno,Min,Max,SipPort,IpRed3,SrvPresenciaIpRed1,SrvPresenciaIpRed2,SrvPresenciaIpRed3)");
            Consulta.AppendFormat(" VALUES ('{0}', '{1}','{2}','{3}',{4},{5},{6},{7},{8},", IdSistema, IdEquipos, IpRed1, IpRed2, TipoEquipo, Interno, Min, Max, SipPort);
            Consulta.AppendFormat("'{0}','{1}','{2}','{3}')", IpRed3, SrvPresenciaIpRed1, SrvPresenciaIpRed2, SrvPresenciaIpRed3);

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "EquiposEU");
			return consulta;
        }

        public override string[] UpdateSQL()
        {
            string[] consulta = new string[4];
            StringBuilder strCadena = new StringBuilder();


            /*
                 * JCAM: 03/01/2017. Incidencia #2508
                 * Min y Max no se actualizan: sólo se usan en el INSERT desde la
                 * página de encaminamientos al definir un encaminamiento como central IP
                                             
                "Min=" + Min + "," +
                "Max=" + Max + " " +*/

            strCadena.AppendFormat("UPDATE EquiposEU SET IdSistema='{0}',IdEquipos='{1}',IpRed1='{2}',IpRed2='{3}',TipoEquipo={4},", IdSistema, IdEquipos, IpRed1, IpRed2, TipoEquipo);
            strCadena.AppendFormat("Interno={0},SipPort={1},IpRed3='{2}',SrvPresenciaIpRed1='{3}',SrvPresenciaIpRed2='{4}',SrvPresenciaIpRed3='{5}' ", Interno, SipPort, IpRed3, SrvPresenciaIpRed1, SrvPresenciaIpRed2, SrvPresenciaIpRed3);
            strCadena.AppendFormat("WHERE  IdSistema='{0}' AND IdEquipos='{1}'", IdSistema, IdEquipos);

            consulta[0] = strCadena.ToString();
            consulta[1] = ReplaceSQL(IdSistema, "EquiposEU");

            strCadena.Clear();

            return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];

			Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdEquipos != null)
                Consulta.Append("DELETE FROM EquiposEU WHERE IdSistema='" + IdSistema + "' AND IdEquipos='" + IdEquipos + "'");
            else if (IdSistema != null)
                Consulta.Append("DELETE FROM EquiposEU WHERE IdSistema='" + IdSistema + "'");
            else
                Consulta.Append("DELETE FROM EquiposEU");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "EquiposEU");
			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM EquiposEU WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
	}
}
