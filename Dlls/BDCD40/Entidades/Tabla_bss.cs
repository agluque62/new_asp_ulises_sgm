using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CD40.BD.Entidades
{
    public class Tabla_bss : Tablas
    {
        #region Propiedades de Tabla_bss

        private string _IdSistema;

        private int _IdTabla_bss;
        public int IdTabla_bss
        {
            get { return _IdTabla_bss; } 
            set { _IdTabla_bss = value;}
         }

        private string _Name;
        public string Name 
        { 
            get { return _Name; }
            set { _Name = value; }
        }
        
        private string _Desc;
        public string Description 
        { 
            get { return _Desc; }
            set { _Desc = value; }
        }


        #endregion

        public Tabla_bss(string idSistema)
        {
            _IdSistema = idSistema;
        }
        public Tabla_bss()
        {
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
            if (_IdTabla_bss > 0)
                Consulta.Append("SELECT * FROM tabla_bss WHERE idtabla_bss=" + _IdTabla_bss);
            else if (this._Name != null)
            {   //La busqueda por nombre se hace distinguiendo mayúsculas y minúsculas
                Consulta.Append("SELECT * FROM tabla_bss WHERE binary name='" + _Name + "'");
            }
            else
                Consulta.Append("SELECT * FROM tabla_bss");

            return Consulta.ToString();
        }

        public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds)
        {
            ListaResultado.Clear();

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    Tabla_bss r = new Tabla_bss("");

                    r.IdTabla_bss = (int)dr["idtabla_bss"];
					if (dr["name"] != System.DBNull.Value)
						r.Name = (string)dr["name"];
					if (dr["description"] != System.DBNull.Value)
						r.Description = (string)dr["description"];

                    ListaResultado.Add(r);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
			string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
			Consulta.Append("INSERT INTO tabla_bss (name,description)" +
                            " VALUES ('" + Name + "','" + Description + "')");

			consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(_IdSistema, "tabla_bss");
			return consulta;
        }

        public override string[] UpdateSQL()
        {
			string[] consulta = new string[2];

			Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE tabla_bss SET name='" + Name + "'," +
                                            "description='" + Description + "' " +
                                            "WHERE idtabla_bss=" + _IdTabla_bss);

			consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(_IdSistema, "tabla_bss");

			return consulta;
		}

        public override string[] DeleteSQL()
        {
			string[] consulta = new string[2];

			Consulta.Remove(0, Consulta.Length);
            if (_IdTabla_bss >= 0)
                Consulta.Append("DELETE FROM tabla_bss WHERE idtabla_bss=" + IdTabla_bss);
            else
                Consulta.Append("DELETE FROM tabla_bss");

			consulta[0] = Consulta.ToString();
			consulta[1] = ReplaceSQL(_IdSistema, "tabla_bss");
			return consulta;
		}
    }
}
