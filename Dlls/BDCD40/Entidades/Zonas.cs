using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CD40.BD.Entidades
{
    public class Zonas : Tablas
    {
        #region Propiedades de Zonas

        private int _IdZonas;
        public int IdZonas
        {
            get { return _IdZonas; }
            set { _IdZonas = value; }
        }

        private string _IdSistema;
        public string IdSistema
        {
            get { return _IdSistema; }
            set { _IdSistema = value; }
        }

        private string _Nombre;
        public string Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }

        #endregion

        //static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public Zonas(string idSistema)
        {
            _IdSistema = idSistema;
        }
        public Zonas()
        {
        }

        public override string DataSetSelectSQL()
        {
            Consulta.Remove(0, Consulta.Length);
            if (_IdSistema != null && _IdZonas > 0)
                Consulta.Append("SELECT * FROM Zonas WHERE IdSistema='" + _IdSistema + "' AND idZonas=" + _IdZonas);
            else if (_IdSistema != null)
                Consulta.Append("SELECT * FROM Zonas WHERE IdSistema='" + _IdSistema + "'");
            else
                Consulta.Append("SELECT * FROM Zonas");

            return Consulta.ToString();
        }

        public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds)
        {
            ListaResultado.Clear();

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    Zonas r = new Zonas("");

                    r.IdZonas = (int)dr["idZonas"];
                    if (dr["IdSistema"] != System.DBNull.Value)
                        r.IdSistema = (string)dr["IdSistema"];
                    if (dr["Nombre"] != System.DBNull.Value)
                        r.Nombre = (string)dr["Nombre"];

                    ListaResultado.Add(r);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
            string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("INSERT INTO Zonas (Idsistema,Nombre)" +
                            " VALUES ('" + IdSistema + "','" + Nombre + "')");

            consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(_IdSistema, "Zonas");
            return consulta;
        }

        public override string[] UpdateSQL()
        {
            string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            Consulta.Append("UPDATE Zonas SET Nombre='" + Nombre + "' " +
                                            "WHERE idZonas=" + _IdZonas);
            consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(_IdSistema, "Zonas");

            return consulta;
        }

        public override string[] DeleteSQL()
        {
            string[] consulta = new string[2];

            Consulta.Remove(0, Consulta.Length);
            if (_IdZonas >= 0)
                Consulta.Append("DELETE FROM Zonas WHERE idZonas=" + _IdZonas);
            else
                Consulta.Append("DELETE FROM Zonas");

            consulta[0] = Consulta.ToString();
            consulta[1] = ReplaceSQL(_IdSistema, "Zonas");
            return consulta;
        }

    }
}
