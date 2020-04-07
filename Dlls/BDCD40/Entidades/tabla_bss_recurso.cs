using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CD40.BD.Entidades
{
    public class Tabla_bss_recurso : Tablas
    {
        #region Propiedades de Tabla_bss_recurso

        private string _IdSistema;

        private int _Id_Tabla_bss_recurso;
        public int Id_Tabla_bss_recurso
        {
            get { return _Id_Tabla_bss_recurso; }
            set { _Id_Tabla_bss_recurso = value; }
        }

        private int _Tabla_bss_idtabla_bss;
        public int Tabla_bss_idtabla_bss
        {
            get { return _Tabla_bss_idtabla_bss; }
            set { _Tabla_bss_idtabla_bss = value; }
        }

        #endregion

        //static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public Tabla_bss_recurso(string idSistema)
        {
            _IdSistema = idSistema;
        }
        public Tabla_bss_recurso()
        {
        
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
            if (_Id_Tabla_bss_recurso > 0)
                Consulta.Append("SELECT * FROM tabla_bss_recurso WHERE id_tabla_bss_recurso=" + _Id_Tabla_bss_recurso);
            else if (this._Tabla_bss_idtabla_bss >  0)
                Consulta.Append("SELECT * FROM tabla_bss_recurso WHERE tabla_bss_idtabla_bss =" + _Tabla_bss_idtabla_bss);
            else
                Consulta.Append("SELECT * FROM tabla_bss_recurso");

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
                    Tabla_bss_recurso r = new Tabla_bss_recurso("");

                    //r.Id_Tabla_bss_recurso = (int)dr["idtabla_bss_recurso"];// Comantado por MAF
                    
                    if (dr["id_tabla_bss_recurso"] != System.DBNull.Value)          // Añadido por MAF
                        r.Id_Tabla_bss_recurso = (int)dr["id_tabla_bss_recurso"];
                    
                    
                    if (dr["tabla_bss_idtabla_bss"] != System.DBNull.Value)
                        r.Tabla_bss_idtabla_bss = (int)dr["tabla_bss_idtabla_bss"];
                    
                    ListaResultado.Add(r);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
            string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO Tabla_bss_recurso (tabla_bss_idtabla_bss)" +
                            " VALUES (" + _Tabla_bss_idtabla_bss + ")");

            consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(_IdSistema, "Tabla_bss_recurso");
            return consulta;
        }

        public override string[] UpdateSQL()
        {
            string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE Tabla_bss_recurso SET tabla_bss_idtabla_bss=" + _Tabla_bss_idtabla_bss + " " +
                                            "WHERE idtabla_bss_recurso=" + _Id_Tabla_bss_recurso);

            consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(_IdSistema, "tabla_bss_idtabla_bss");

            return consulta;
        }

        public override string[] DeleteSQL()
        {
            string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            if (_Id_Tabla_bss_recurso >= 0)
                Consulta.Append("DELETE FROM Tabla_bss_recurso WHERE idtabla_bss_recurso=" + _Tabla_bss_idtabla_bss);
            else
                Consulta.Append("DELETE FROM Tabla_bss_recurso");

            consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(_IdSistema, "Tabla_bss_recurso");
            return consulta;
        }

    }
}
