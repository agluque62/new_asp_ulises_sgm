using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CD40.BD.Entidades
{

    public class RadioParam : Tablas
    {

        #region Propiedades de RadioParam
        // Identificador del recurso
        protected int _IdRadio_Param;
        public int IdRadio_Param
        {
            get { return _IdRadio_Param; }
            set { _IdRadio_Param = value; }
        }
        // Tipo de Destino al que está asociado.
        private int _GrsDelay;
        public int GrsDelay
        {
            get { return _GrsDelay; }
            set { _GrsDelay = value; }
        }
        // Identificador del emplazamineto al que está asociado.
        private int _OffSetFrequency;
        public int OffSetFrequency
        {
            get { return _OffSetFrequency; }
            set { _OffSetFrequency = value; }
        }

        // Frecuencia del equipo (Solo para principal).
        private bool _EnableEventPttSq;
        public bool EnableEventPttSq
        {
            get { return _EnableEventPttSq; }
            set { _EnableEventPttSq = value; }
        }

        /// <summary>
        /// Puerto
        /// </summary>
        private int _Metodos_bss_idmetodos_bss;
        public int Metodos_bss_idmetodos_bss
        {
            get { return _Metodos_bss_idmetodos_bss; }
            set { _Metodos_bss_idmetodos_bss = value; }
        }

        #endregion


        public RadioParam()
            : base()
        { }

        public RadioParam(int id)
            : base()
        {
            IdRadio_Param = id;
        }

        //public new string DataSetSelectSQL()
        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);

            if (IdRadio_Param != 0)
                Consulta.Append("SELECT r.* FROM Radio_Param r WHERE r.idradio_param=" + IdRadio_Param);
            else
                Consulta.Append("SELECT r.* FROM Radio_Param r");

            return Consulta.ToString();
        }

        public override System.Collections.Generic.List<Tablas> ListSelectSQL(System.Data.DataSet ds)
        {
            ListaResultado.Clear();

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    RadioParam pr = new RadioParam();

                    pr.IdRadio_Param = (int)dr["idradio_param"];

                    if (dr["GrsDelay"] != System.DBNull.Value)
                        pr.GrsDelay = (int)dr["GrsDelay"];
                    if (dr["OffSetFrequency"] != System.DBNull.Value)
                        pr.OffSetFrequency = (int)dr["OffSetFrequency"];
                    if (dr["EnableEventPttSq"] != System.DBNull.Value)
                        pr.EnableEventPttSq = (bool)dr["EnableEventPttSq"];
                    if (dr["metodos_bss_idmetodos_bss"] != System.DBNull.Value)
                        pr.Metodos_bss_idmetodos_bss = (int)dr["metodos_bss_idmetodos_bss"];

                    ListaResultado.Add(pr);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
            string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO Radio_Param (GrsDelay,OffSetFrequency,EnableEventPttSq,metodos_bss_idmetodos_bss)" +
                            " VALUES (" + GrsDelay + "," +
                                            OffSetFrequency + "," +
                                            EnableEventPttSq + "," +
                                            Metodos_bss_idmetodos_bss + ")");

            consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(null, "Radio_Param");

            return consulta;
        }

        public override string[] UpdateSQL()
        {
            string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE Radio_Param SET GrsDelay=" + GrsDelay + "," +
                                            "OffSetFrequency=" + OffSetFrequency + "," +
                                            "EnableEventPttSq=" + EnableEventPttSq + "," +
                                            "metodos_bss_idmetodos_bss=" + Metodos_bss_idmetodos_bss + " " +
                                            "WHERE idradio_param=" + IdRadio_Param);

            consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(null, "Radio_Param");

            return consulta;
        }

        public override string[] DeleteSQL()
        {
            string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            if (IdRadio_Param != 0)
                Consulta.Append("DELETE FROM Radio_Param WHERE idradio_param=" + IdRadio_Param);
            else
                Consulta.Append("DELETE FROM Radio_Param");

            consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(null, "Radio_Param");

            return consulta;
        }
    }
}
