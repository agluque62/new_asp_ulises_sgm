using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class MetodosbssDestinosradio : Tablas
    {
        #region Propiedades de ValoresTabla

        private int _Idmetodosbss_Destinosradio;
        public int Idmetodosbss_Destinosradio
        {
            get { return _Idmetodosbss_Destinosradio; }
            set { _Idmetodosbss_Destinosradio = value; }
        }

        private int _Metodos_bss_Idmetodos_bss;
        public int Metodos_bss_Idmetodos_bss
        {
            get { return _Metodos_bss_Idmetodos_bss; }
            set { _Metodos_bss_Idmetodos_bss = value; }
        }

        private int _Metodosbss_Destinosradio_fk;
        public int Metodosbss_Destinosradio_fk
        {
            get { return _Metodosbss_Destinosradio_fk; }
            set { _Metodosbss_Destinosradio_fk = value; }
        }


        #endregion

        //static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public MetodosbssDestinosradio()
        {
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
            if (_Idmetodosbss_Destinosradio > 0)
                Consulta.Append("SELECT * FROM metodosbss_destinosradio WHERE idmetodosbss_destinosradio=" + _Idmetodosbss_Destinosradio);
            else
                Consulta.Append("SELECT * FROM metodosbss_destinosradio");

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
                    MetodosbssDestinosradio r = new MetodosbssDestinosradio();

                    if (dr["idmetodosbss_destinosradio"] != System.DBNull.Value)
                        r.Idmetodosbss_Destinosradio = (int)dr["idmetodosbss_destinosradio"];
                    if (dr["metodos_bss_idmetodos_bss"] != System.DBNull.Value)
                        r.Metodos_bss_Idmetodos_bss = (int)dr["metodos_bss_idmetodos_bss"];
                    if (dr["metodosbss_destinosradio_fk"] != System.DBNull.Value)
                        r.Metodosbss_Destinosradio_fk = (int)dr["metodosbss_destinosradio_fk"];

                    ListaResultado.Add(r);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
            string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO metodosbss_destinosradio (metodos_bss_idmetodos_bss,metodosbss_destinosradio_fk)" +
                            " VALUES (" + Metodos_bss_Idmetodos_bss + "," + Metodosbss_Destinosradio_fk + ")");

            consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(null, "metodosbss_destinosradio");
            return consulta;
        }

        public override string[] UpdateSQL()
        {
            string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE metodosbss_destinosradio SET metodos_bss_idmetodos_bss=" + Metodos_bss_Idmetodos_bss + "," +
                                            " metodos_bss_idmetodos_bss=" + Metodos_bss_Idmetodos_bss + "," +
                                            " WHERE idmetodosbss_destinosradio=" + Idmetodosbss_Destinosradio);

            consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(null, "ValoresTabla");

            return consulta;
        }

        public override string[] DeleteSQL()
        {
            string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            if (_Idmetodosbss_Destinosradio > 0)
                Consulta.Append("DELETE FROM metodosbss_destinosradio WHERE idmetodosbss_destinosradio=" + Idmetodosbss_Destinosradio);
            else
                Consulta.Append("DELETE FROM metodosbss_destinosradio");

            consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(null, "metodosbss_destinosradio");
            return consulta;
        }

    }
}
