using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class Operadores : Tablas
    {
        #region Propiedades de Operadores

        private string _idOperador;
        public string IdOperador
        {
            get { return _idOperador; }
            set { _idOperador = value; }
        }

        private string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }

        private string _Clave;
        public string Clave
        {
            get { return _Clave; }
            set { _Clave = value; }
        }

        private uint _NivelAcceso;
        public uint NivelAcceso
        {
            get { return _NivelAcceso; }
            set { _NivelAcceso = value; }
        }

        private string _Nombre;
        public string Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }

        private string _Apellidos;
        public string Apellidos
        {
            get { return _Apellidos; }
            set { _Apellidos = value; }
        }

        private string _Telefono;
        public string Telefono
        {
            get { return _Telefono; }
            set { _Telefono = value; }
        }

        private DateTime _FechaUltimoAcceso;
        public DateTime FechaUltimoAcceso
        {
            get { return _FechaUltimoAcceso; }
            set { _FechaUltimoAcceso = value; }
        }

        private string _Comentarios;
        public string Comentarios
        {
            get { return _Comentarios; }
            set { _Comentarios = value; }
        }

        #endregion

		//static AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

        public Operadores()
        {
			//if (ServiceAccesoABaseDeDatos == null)
			//    ServiceAccesoABaseDeDatos = new AccesoABaseDeDatos();
        }

        public override string DataSetSelectSQL()
        {
            StringBuilder strConsulta = new StringBuilder();

            if (IdSistema != null && IdOperador != null && Clave != null)
                strConsulta.Append("SELECT * FROM Operadores WHERE IdSistema='" + IdSistema + "' AND IdOperador='" + IdOperador + "' AND Clave='" + Clave + "'");
            else if (IdSistema != null)
                strConsulta.AppendFormat("SELECT * FROM Operadores WHERE IdSistema='{0}' ORDER BY IdOperador", IdSistema);
            else if (IdOperador != null)
                strConsulta.AppendFormat("SELECT * FROM Operadores WHERE IdOperador='{0}' ", IdOperador);
            else
                strConsulta.Append("SELECT * FROM Operadores");

            return strConsulta.ToString();
        }

        public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds)
        {
            ListaResultado.Clear();

			//DataSetResultado = this.DataSetSelectSQL();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    Operadores top = new Operadores();

                    top.IdOperador = (string)dr["IdOperador"];
                    top.IdSistema = (string)dr["IdSistema"];
                    if (dr["Clave"] != System.DBNull.Value)
                        top.Clave = (string)dr["Clave"];
                    if (dr["NivelAcceso"] != System.DBNull.Value)
                        top.NivelAcceso = (uint)dr["NivelAcceso"];
                    if (dr["Nombre"] != System.DBNull.Value)
                        top.Nombre = (string)dr["Nombre"];
                    if (dr["Apellidos"] != System.DBNull.Value)
                        top.Apellidos = (string)dr["Apellidos"];
                    if (dr["Telefono"] != System.DBNull.Value)
                        top.Telefono = (string)dr["Telefono"];
                    if (dr["FechaUltAcceso"] != System.DBNull.Value)
                        top.FechaUltimoAcceso = (DateTime)dr["FechaUltAcceso"];
                    if (dr["Comentarios"] != System.DBNull.Value)
                        top.Comentarios = (string)dr["Comentarios"];

                    ListaResultado.Add(top);
                }
            }
            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
            string[] consulta = new string[2];
            StringBuilder strConsulta = new StringBuilder();

            strConsulta.Append("INSERT INTO Operadores (IdOperador,IdSistema,Clave,NivelAcceso,Nombre,Apellidos,Telefono,FechaUltAcceso,Comentarios) ");
            strConsulta.AppendFormat("VALUES ('{0}','{1}','{2}',{3},'{4}','{5}','{6}',STR_TO_DATE('{7}','%d/%m/%Y'),'{8}')", IdOperador, IdSistema, Clave, NivelAcceso, Nombre, Apellidos, Telefono, FechaUltimoAcceso.ToString("dd/MM/yyyy"), Comentarios);

            consulta[0] = strConsulta.ToString();
            consulta[1] = ReplaceSQL(IdSistema, "Operadores");

            strConsulta.Clear();
            return consulta;

        }

        public override string[] UpdateSQL()
        {
            string[] consulta = new string[2];

            StringBuilder strConsulta = new StringBuilder();

            strConsulta.AppendFormat("UPDATE Operadores SET IdOperador='{0}', IdSistema='{1}',Clave='{2}',NivelAcceso={3},Nombre='{4}',Apellidos='{5}',Telefono='{6}',", IdOperador, IdSistema, Clave, NivelAcceso, Nombre, Apellidos, Telefono);
            strConsulta.AppendFormat("FechaUltAcceso=STR_TO_DATE('{0}','%d/%m/%Y'),Comentarios='{1}' ", FechaUltimoAcceso.ToString("dd/MM/yyyy"), Comentarios);
            strConsulta.AppendFormat("WHERE IdOperador='{0}' AND IdSistema='{1}'", IdOperador, IdSistema);


            consulta[0] = strConsulta.ToString();
            consulta[1] = ReplaceSQL(IdSistema, "Operadores");

            strConsulta.Clear();
            return consulta;
        }

        public override string[] DeleteSQL()
        {
            string[] consulta = new string[2];
            StringBuilder strConsulta = new StringBuilder();

            if (IdSistema != null && IdOperador != null)
                strConsulta.Append("DELETE FROM Operadores WHERE IdSistema='" + IdSistema + "' AND IdOperador='" + IdOperador + "'");
            else if (IdSistema != null)
                strConsulta.Append("DELETE FROM Operadores WHERE IdSistema='" + IdSistema + "'");
            else if (IdOperador != null)
                strConsulta.Append("DELETE FROM Operadores WHERE IdOperador='" + IdOperador + "'");
            else
                strConsulta.Append("DELETE FROM Operadores");

            consulta[0] = strConsulta.ToString();
            consulta[1] = ReplaceSQL(IdSistema, "Operadores");

            strConsulta.Clear();

            return consulta;
        }

		//public override int SelectCountSQL(string where)
		//{
		//    Consulta.Remove(0, Consulta.Length);
		//    Consulta.Append("SELECT COUNT(*) FROM Operadores WHERE " + where);

		//    return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		//}
    }
}
