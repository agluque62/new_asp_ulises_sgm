using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CD40.BD.Entidades
{
    public class ValoresTabla : Tablas
    {
        #region Propiedades de ValoresTabla

        private string _IdSistema;

        private int _IdValores_Tabla;
        public int IdValores_Tabla
        {
            get { return _IdValores_Tabla; }
            set { _IdValores_Tabla = value; }
        }

        private int _tabla_bss_idtabla_bss;
        public int Tabla_bss_idtabla_bss
        {
            get { return _tabla_bss_idtabla_bss; }
            set { _tabla_bss_idtabla_bss = value; }
        }

        private int _Valor_Prop;
        public int Valor_Prop
        {
            get { return _Valor_Prop; }
            set { _Valor_Prop = value; }
        }

        private int _Valor_rssi;
        public int Valor_rssi
        {
            get { return _Valor_rssi; }
            set { _Valor_rssi = value; }
        }


        #endregion

        //static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public ValoresTabla(string idSistema)
        {
            _IdSistema = idSistema;
        }
        public ValoresTabla()
        {
            
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
            if (_IdValores_Tabla > 0)
                Consulta.Append("SELECT * FROM Valores_Tabla WHERE idValores_Tabla=" + _IdValores_Tabla);
            else if (_tabla_bss_idtabla_bss > 0)
                Consulta.Append("SELECT * FROM Valores_Tabla WHERE tabla_bss_idtabla_bss=" + Tabla_bss_idtabla_bss);
            else
                Consulta.Append("SELECT * FROM Valores_Tabla");

            return Consulta.ToString();
        }


        public override string DataSetSelectSQL(string tableName)
        {
            Consulta.Remove(0, Consulta.Length);

            Consulta.Append("SELECT * FROM Valores_Tabla vt " +
                    "INNER JOIN tabla_bss tb ON tb.idtabla_bss = vt.tabla_bss_idtabla_bss " +
                    "WHERE tb.name='" + tableName + "'");

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
                    ValoresTabla r = new ValoresTabla("");

                    r.IdValores_Tabla = (int)dr["idValores_Tabla"];
                    if (dr["tabla_bss_idtabla_bss"] != System.DBNull.Value)
                        r.Tabla_bss_idtabla_bss = (int)dr["tabla_bss_idtabla_bss"];
                    if (dr["valor_prop"] != System.DBNull.Value)
                        r._Valor_Prop = (int)dr["valor_prop"];
                    if (dr["valor_rssi"] != System.DBNull.Value)
                        r.Valor_rssi = (int)dr["valor_rssi"];

                    ListaResultado.Add(r);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
            string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO Valores_Tabla (tabla_bss_idtabla_bss,valor_prop,valor_rssi)" +
                            " VALUES (" + _tabla_bss_idtabla_bss + "," + _Valor_Prop + "," + _Valor_rssi + ")");

            consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(_IdSistema, "Valores_Tabla");
            return consulta;
        }

        public override string[] UpdateSQL()
        {
            string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE Valores_Tabla SET tabla_bss_idtabla_bss=" + _tabla_bss_idtabla_bss + "," +
                                            "valor_prop=" + _Valor_Prop + "," +
                                            "valor_rssi=" + _Valor_rssi + " " +
                                            "WHERE idValores_Tabla=" + _IdValores_Tabla);

            consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(_IdSistema, "Valores_Tabla");

            return consulta;
        }

        public override string[] DeleteSQL()
        {
            string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            if (_IdValores_Tabla > 0)
                Consulta.Append("DELETE FROM Valores_Tabla WHERE idValores_Tabla=" + IdValores_Tabla);
            else
                Consulta.Append("DELETE FROM Valores_Tabla");

            consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(_IdSistema, "Valores_Tabla");
            return consulta;
        }

    }
}
