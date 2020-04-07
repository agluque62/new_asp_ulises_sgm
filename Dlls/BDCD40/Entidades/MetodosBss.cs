using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class MetodosBss : Tablas
    {
        #region Propiedades de MetodosBss

        private int _IdMetodos_Bss;
        public int IdMetodos_Bss
        {
            get { return _IdMetodos_Bss; }
            set { _IdMetodos_Bss = value; }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        #endregion

        public MetodosBss()
            : base()
        {
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);

            Consulta.Append("SELECT * FROM metodos_bss");

            return Consulta.ToString();
        }

        public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds)
        {
            ListaResultado.Clear();

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    MetodosBss r = new MetodosBss();

                    if (dr["idmetodos_bss"] != System.DBNull.Value)
                        r.IdMetodos_Bss = (int)dr["idmetodos_bss"];
                    if (dr["name"] != System.DBNull.Value)
                        r.Name = (string)dr["name"];

                    ListaResultado.Add(r);
                }
            }
            return ListaResultado;
        }

        // Al insertar DestinoRadio==>Trigger AltaDestinoRadio (da de alta un destino)
        public override string[] InsertSQL()
        {
            return new string[0]; 
        }

        public override string[] UpdateSQL()
        {
            return new string[0];
        }

        public override string[] DeleteSQL()
        {
            return new string[0];
        }
    }
}
