using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class Encaminamientos : Tablas
    {
        #region Propiedades de Encaminamientos

        private string _Central;
        public string Central
        {
            get { return _Central; }
            set { _Central = value; }
        }

        private string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }

        private bool _CentralPropia;
        public bool CentralPropia
        {
            get { return _CentralPropia; }
            set { _CentralPropia = value; }
        }

        private bool _Throwswitching;
        public bool Throwswitching
        {
            get { return _Throwswitching; }
            set { _Throwswitching = value; }
        }

		private string _NumTest;
		public string NumTest
		{
			get { return _NumTest; }
			set { _NumTest = value; }
		}

        private bool _CentralIp;
        public bool CentralIp
        {
            get { return _CentralIp; }
            set { _CentralIp = value; }
        }

        #endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public Encaminamientos()
        {
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && Central != null)
                Consulta.Append("SELECT * FROM Encaminamientos WHERE IdSistema='" + IdSistema + "' AND Central='" + Central + "'");
            else if (IdSistema != null)
                Consulta.Append("SELECT * FROM Encaminamientos WHERE IdSistema='" + IdSistema + "'");
            else
                Consulta.Append("SELECT * FROM Encaminamientos");

            return Consulta.ToString();
        }

        public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds)
        {
            ListaResultado.Clear();

			//DataSetResultado = this.DataSetSelectSQL();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    Encaminamientos r = new Encaminamientos();

                    r.Central = (string)dr["Central"];
                    r.IdSistema = (string)dr["IdSistema"];
                    if (dr["CentralPropia"] != System.DBNull.Value)
                        r.CentralPropia = (bool)dr["CentralPropia"];    //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0;
					if (dr["Throwswitching"] != System.DBNull.Value)
                        r.Throwswitching = (bool)dr["Throwswitching"];  //Incompatibilidad MySql Server 5.6.11 y 5.0  != 0;
					if (dr["NumTest"] != System.DBNull.Value)
						r.NumTest = (string)dr["NumTest"];
                    if (dr["CentralIp"] != System.DBNull.Value)
                        r.CentralIp = (bool)dr["CentralIp"];    

                    ListaResultado.Add(r);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
			string[] consulta=new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO Encaminamientos (IdSistema,Central,CentralPropia,Throwswitching,NumTest,CentralIp)" +
                            " VALUES ('" + IdSistema + "','" +
                                         Central + "'," +
                                         CentralPropia + "," +
                                         Throwswitching + ",'" +
										 NumTest + "'," +
                                         CentralIp + ")");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Encaminamientos");
			return consulta;
        }

        public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];

			Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE Encaminamientos SET " +
                                            "IdSistema='" + IdSistema + "'," +
                                            "Central='" + Central + "'," +
                                            "CentralPropia=" + CentralPropia + "," +
											"Throwswitching=" + Throwswitching + "," +
											"NumTest='" + NumTest + "'," +
                                            "CentralIp=" + CentralIp + " " +
                                            "WHERE Central='" + Central + "' AND " + "IdSistema='" + IdSistema + "'"
                                            );

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Encaminamientos");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];
			
			Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && Central != null)
                Consulta.Append("DELETE FROM Encaminamientos WHERE IdSistema='" + IdSistema + "' AND Central='" + Central + "'");
            else if (IdSistema != null)
                Consulta.Append("DELETE FROM Encaminamientos WHERE IdSistema='" + IdSistema + "'");
            else
                Consulta.Append("DELETE FROM Encaminamientos");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Encaminamientos");
			return consulta;
		}

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM Encaminamientos WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}

		public string CentralPropiaSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("SELECT Central FROM Encaminamientos WHERE IdSistema='" + IdSistema + "' AND CentralPropia=true");

			return Consulta.ToString();

			//string retorno = Convert.ToString(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));

			//if (retorno != null && retorno != "")
			//    return retorno;

			//return null;
		}
    }
}
