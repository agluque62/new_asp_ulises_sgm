using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class TifX : Tablas
    {
        #region Propiedades de TifX

        private string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }

        private string _idTifX;
        public string IdTifx
        {
            get { return _idTifX; }
            set { _idTifX = value; }
        }

        private string _ModoArranque;
        public string ModoArranque
        {
            get { return _ModoArranque; }
            set { _ModoArranque = value; }
        }

        private uint _ModoSincronizacion;
        public uint ModoSincronizacion
        {
            get { return _ModoSincronizacion; }
            set { _ModoSincronizacion = value; }
        }

        private string _Master;
        public string Master
        {
            get { return _Master; }
            set { _Master = value; }
        }

        private uint _SNMPPortLocal;
        public uint SNMPPortLocal
        {
            get { return _SNMPPortLocal; }
            set { _SNMPPortLocal = value; }
        }

        private uint _SNMPPortRemoto;
        public uint SNMPPortRemoto
        {
            get { return _SNMPPortRemoto; }
            set { _SNMPPortRemoto = value; }
        }

        private uint _SNMPTraps;
        public uint SNMPTraps
        {
            get { return _SNMPTraps; }
            set { _SNMPTraps = value; }
        }

        private uint _SIPPortLocal;
        public uint SIPPortLocal
        {
            get { return _SIPPortLocal; }
            set { _SIPPortLocal = value; }
        }

        private uint _TimeSupervision;
        public uint TimeSupervision
        {
            get { return _TimeSupervision; }
            set { _TimeSupervision = value; }
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

        private string _Grabador1;
        public string Grabador1
        {
            get { return _Grabador1; }
            set { _Grabador1 = value; }
        }
        private string _Grabador2;
        public string Grabador2
        {
            get { return _Grabador2; }
            set { _Grabador2 = value; }
        }
        private byte _iSupervLanGW;
        public byte iSupervLanGW
        {
            get { return _iSupervLanGW; }
            set { _iSupervLanGW = value; }
        }
        private byte _itmmaxSupervLanGW;
        public byte itmmaxSupervLanGW
        {
            get { return _itmmaxSupervLanGW; }
            set { _itmmaxSupervLanGW = value; }
        }
        #endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public TifX()
        {
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdTifx != null)
                Consulta.Append("SELECT T.*,eu1.IpRed1 as IpRecorder1,eu2.IpRed1 as IpRecorder2 FROM Tifx T " +
                            "LEFT JOIN equiposeu eu1 ON (eu1.IdEquipos=T.Grabador1) " +
                            "LEFT JOIN equiposeu eu2 ON (eu2.IdEquipos=T.Grabador2) " +
                            "WHERE T.IdSistema='" + IdSistema + "' AND IdTifX='" + IdTifx + "'");
            else if (IdSistema != null && IpRed1 != null)
                Consulta.Append("SELECT T.*,eu1.IpRed1 as IpRecorder1,eu2.IpRed1 as IpRecorder2 FROM Tifx T " +
                            "LEFT JOIN equiposeu eu1 ON (eu1.IdEquipos=T.Grabador1) " +
                            "LEFT JOIN equiposeu eu2 ON (eu2.IdEquipos=T.Grabador2) " +
                            "WHERE T.IdSistema='" + IdSistema + "' AND IpRed1='" + IpRed1 + "'");
            else if (IdSistema != null && Grabador1 != null)
                Consulta.Append("SELECT T.*,eu1.IpRed1 as IpRecorder1,eu2.IpRed1 as IpRecorder2 FROM Tifx T " +
                            "LEFT JOIN equiposeu eu1 ON (eu1.IdEquipos=T.Grabador1) " +
                            "LEFT JOIN equiposeu eu2 ON (eu2.IdEquipos=T.Grabador2) " +
                            "WHERE T.IdSistema='" + IdSistema + "' AND (Grabador1='" + Grabador1 + "' OR Grabador2='" + Grabador1 + "')");
            else if (IdSistema != null)
                Consulta.Append("SELECT T.*,eu1.IpRed1 as IpRecorder1,eu2.IpRed1 as IpRecorder2 FROM Tifx T " +
                            "LEFT JOIN equiposeu eu1 ON (eu1.IdEquipos=T.Grabador1) " +
                            "LEFT JOIN equiposeu eu2 ON (eu2.IdEquipos=T.Grabador2) " +
                            "WHERE T.IdSistema='" + IdSistema + "' ORDER BY IdTifx");
			else if (IdTifx != null)
                Consulta.Append("SELECT T.*,eu1.IpRed1 as IpRecorder1,eu2.IpRed1 as IpRecorder2 FROM Tifx T " +
                            "LEFT JOIN equiposeu eu1 ON (eu1.IdEquipos=T.Grabador1) " +
                            "LEFT JOIN equiposeu eu2 ON (eu2.IdEquipos=T.Grabador2) " +
                            "WHERE IdTifX='" + IdTifx + "'");
			else
                Consulta.Append("SELECT T.*,eu1.IpRed1 as IpRecorder1,eu2.IpRed1 as IpRecorder2 FROM Tifx T " +
                            "LEFT JOIN equiposeu eu1 ON (eu1.IdEquipos=T.Grabador1) " +
                            "LEFT JOIN equiposeu eu2 ON (eu2.IdEquipos=T.Grabador2) " +
                            "ORDER BY IdTifx");

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
                    TifX tifx = new TifX();

                    tifx.IdSistema = (string)dr["IdSistema"];
                    tifx.IdTifx = (string)dr["IdTIFX"];
                    if (dr["ModoArranque"] != System.DBNull.Value)
                        tifx.ModoArranque = (string)dr["ModoArranque"];
                    if (dr["ModoSincronizacion"] != System.DBNull.Value)
                        tifx.ModoSincronizacion = (uint)dr["ModoSincronizacion"];
                    if (dr["Master"] != System.DBNull.Value)
                        tifx.Master = (string)dr["Master"];
                    if (dr["SNMPPortLocal"] != System.DBNull.Value)
                        tifx.SNMPPortLocal = (uint)dr["SNMPPortLocal"];
                    if (dr["SNMPPortRemoto"] != System.DBNull.Value)
                        tifx.SNMPPortRemoto = (uint)dr["SNMPPortRemoto"];
                    if (dr["SNMPTraps"] != System.DBNull.Value)
                        tifx.SNMPTraps = (uint)dr["SNMPTraps"];
                    if (dr["SIPPortLocal"] != System.DBNull.Value)
                        tifx.SIPPortLocal = (uint)dr["SIPPortLocal"];
                    if (dr["TimeSupervision"] != System.DBNull.Value)
                        tifx.TimeSupervision = (uint)dr["TimeSupervision"];
                    if (dr["IpRed1"] != System.DBNull.Value)
                        tifx.IpRed1 = (string)dr["IpRed1"];
                    if (dr["IpRed2"] != System.DBNull.Value)
                        tifx.IpRed2 = (string)dr["IpRed2"];
                    if (dr["Grabador1"] != System.DBNull.Value)
                        tifx.Grabador1 = (string)dr["Grabador1"];
                    else
                        tifx.Grabador1 = null;
                    if (dr["Grabador2"] != System.DBNull.Value)
                        tifx.Grabador2 = (string)dr["Grabador2"];
                    else
                        tifx.Grabador2 = null;
                    if (dr["iSupervLanGW"] != System.DBNull.Value)
                        tifx.iSupervLanGW = Convert.ToByte(dr["iSupervLanGW"]);
                    if (dr["itmmaxSupervLanGW"] != System.DBNull.Value)
                        tifx.itmmaxSupervLanGW = Convert.ToByte(dr["itmmaxSupervLanGW"]);

                    ListaResultado.Add(tifx);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO Tifx (IdSistema,IdTIFX,ModoArranque,ModoSincronizacion,Master," +
                            "SNMPPortLocal,SNMPPortRemoto,SNMPTraps,SIPPortLocal,TimeSupervision,IpRed1,IpRed2,Grabador1,Grabador2,iSupervLanGW,itmmaxSupervLanGW) " +
                            "VALUES ('" + IdSistema + "','" +
                                         IdTifx + "','" +
                                         ModoArranque + "'," +
                                         ModoSincronizacion + ",'" +
                                         Master + "'," +
                                         SNMPPortLocal + "," +
                                         SNMPPortRemoto + "," +
                                         SNMPTraps + "," +
                                         SIPPortLocal + "," +
                                         TimeSupervision + ",'" +
                                         IpRed1 + "','" +
                                         IpRed2 + "','" +
                                         Grabador1 + "','" +
                                         Grabador2 + "','" +
                                         iSupervLanGW + "','" +
                                         itmmaxSupervLanGW + "')");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Tifx");

            return consulta;
        }

		public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];
			
			Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE Tifx SET IdTIFX='" + IdTifx + "'," +
                                            "idSistema='" + IdSistema + "'," +
                                            "ModoArranque='" + ModoArranque + "'," +
                                            "ModoSincronizacion=" + ModoSincronizacion + "," +
                                            "Master='" + Master + "'," +
                                            "SNMPPortLocal=" + SNMPPortLocal + "," +
                                            "SNMPPortRemoto=" + SNMPPortRemoto + "," +
                                            "SNMPTraps=" + SNMPTraps + "," +
                                            "SIPPortLocal=" + SIPPortLocal + "," +
                                            "TimeSupervision=" + TimeSupervision + "," +
                                            "IpRed1='" + IpRed1 + "'," +
                                            "IpRed2='" + IpRed2 + "'," +
                                            "Grabador1='" + Grabador1 + "'," +
                                            "Grabador2='" + Grabador2 + "', " +
                                            "iSupervLanGW='" + iSupervLanGW + "'," +
                                            "itmmaxSupervLanGW='" + itmmaxSupervLanGW + "' " +
                                            "WHERE IdTifx='" + IdTifx + "' AND " + "IdSistema='" + IdSistema + "'"
                                            );

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Tifx");
			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdTifx != null)
                Consulta.Append("DELETE FROM Tifx WHERE IdSistema='" + IdSistema + "' AND IdTifX='" + IdTifx + "'");
            else if (IdSistema != null)
                Consulta.Append("DELETE FROM Tifx WHERE IdSistema='" + IdSistema + "'");
            else if (IdTifx != null)
                Consulta.Append("DELETE FROM Tifx WHERE IdTifX='" + IdTifx + "'");
            else
                Consulta.Append("DELETE FROM Tifx");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(IdSistema, "Tifx");
			return consulta;
		}
    }

    public class GwActivas : Tablas
    {
        #region Propiedades de GwActivas

        private string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }

        private string _idTifX;
        public string IdTifx
        {
            get { return _idTifX; }
            set { _idTifX = value; }
        }

        private string _IpRed;
        public string IpRed
        {
            get { return _IpRed; }
            set { _IpRed = value; }
        }
        #endregion

        public GwActivas()
        { }

        public GwActivas(string idSistema, string idTifx, string ip)
        {
            IdSistema = IdSistema;
            IdTifx = idTifx;
            IpRed = ip;
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdTifx != null)
                Consulta.Append("SELECT * FROM GwActivas WHERE IdSistema='" + IdSistema + "' AND IdTifX='" + IdTifx + "'");
			else if (IdSistema != null)
                Consulta.Append("SELECT * FROM GwActivas WHERE IdSistema='" + IdSistema + "' ORDER BY IdTifx");
			else
				Consulta.Append("SELECT * FROM GwActivas ORDER BY IdTifx");

            return Consulta.ToString();
        }

        public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds)
        {
            ListaResultado.Clear();

            if (ds != null)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    GwActivas tifx = new GwActivas();

                    tifx.IdSistema = (string)dr["IdSistema"];
                    tifx.IdTifx = (string)dr["IdTIFX"];
                    if (dr["IpRed"] != System.DBNull.Value)
                        tifx.IpRed = (string)dr["IpRed"];

                    ListaResultado.Add(tifx);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO GwActivas (IdSistema,IdTIFX,IpRed) " +
                            "VALUES ('" + IdSistema + "','" +
                                         IdTifx + "','" +
                                         IpRed + "')");

			consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(IdSistema, "GwActivas");

            return consulta;
        }

		public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];
			
			Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE GwActivas SET IdTIFX='" + IdTifx + "'," +
                                            "idSistema='" + IdSistema + "'," +
                                            "IpRed='" + IpRed + "' " +
                                            "WHERE IdTifx='" + IdTifx + "' AND " + "IdSistema='" + IdSistema + "'"
                                            );

			consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(IdSistema, "GwActivas");

            return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            if (IdSistema != null && IdTifx != null)
                Consulta.Append("DELETE FROM GwActivas WHERE IdSistema='" + IdSistema + "' AND IdTifX='" + IdTifx + "'");
            else if (IdSistema != null)
                Consulta.Append("DELETE FROM GwActivas WHERE IdSistema='" + IdSistema + "'");
            else
                Consulta.Append("DELETE FROM GwActivas");

			consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(IdSistema, "GwActivas");

            return consulta;
		}
    }
}
