using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class LlamadasEntrantesSector : Tablas
    {
        #region Propiedades Llamadas Entrantes Sector

        // Nombre del sistema
        private string _idSistema;
        public string IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }

        // Núcleo al que pertenece el sector
        private string _IdNucleo;
        public string IdNucleo
        {
            get { return _IdNucleo; }
            set { _IdNucleo = value; }
        }

        // Nombre del sector
        private string _IdSector;
        public string IdSector
        {
            get { return _IdSector; }
            set { _IdSector = value; }
        }
        // Nombre de la red
        private string _idRed;
        public string IdRed
        {
            get { return _idRed; }
            set { _idRed = value; }
        }

        // Tipo de elemento de la lista: R (rango) o A (Abonado)
        private string _Tipo;
        public string Tipo
        {
            get { return _Tipo; }
            set { _Tipo = value; }
        }

        // Si tipo=R  -->Numero de Abonado inicial del rango
        // Si tipo=A  --> Número de abonado
        private string _NumAbonadoInicial;
        public string NumAbonadoInicial
        {
            get { return _NumAbonadoInicial; }
            set { _NumAbonadoInicial = value; }
        }

        private string _NumAbonadoFinal;
        public string NumAbonadoFinal
        {
            get { return _NumAbonadoFinal; }
            set { _NumAbonadoFinal = value; }
        }

        #endregion


        public LlamadasEntrantesSector()
        {
            IdSistema = string.Empty;
            IdNucleo = string.Empty;
            IdSector = string.Empty;
            IdRed = string.Empty;
            Tipo = string.Empty;
            NumAbonadoInicial = string.Empty;
            NumAbonadoFinal = string.Empty;
        }

        public override string DataSetSelectSQL()
        {

            StringBuilder strWhere = new StringBuilder();
            StringBuilder strConsulta = new StringBuilder();
            bool bWhere=false;

            strConsulta.Append("SELECT IdSistema,IdNucleo,IdSector,IdRed,Tipo,NumAbonadoInicial,NumAbonadoFinal FROM llamadas_entrantes_sector");

            if (IdSistema!=null)
            {
                strWhere.AppendFormat(" IdSistema='{0}' ", IdSistema);
                bWhere=true;
            }

            if (IdNucleo!=null)
            {
                if (bWhere)
                    strWhere.AppendFormat(" AND IdNucleo='{0}' ", IdNucleo);
                else
                {
                    strWhere.AppendFormat(" IdNucleo='{0}' ", IdNucleo);
                    bWhere = true;
                }
            }

            if (IdSector!=null)
            {
                if (bWhere)
                    strWhere.AppendFormat(" AND IdSector='{0}' ", IdSector);
                else
                {
                    strWhere.AppendFormat(" IdSector='{0}' ", IdSector);
                    bWhere = true;
                }
            }

            if (!string.IsNullOrEmpty(IdRed))
            {
                if (bWhere)
                    strWhere.AppendFormat(" AND IdRed='{0}' ", IdRed);
                else
                {
                    strWhere.AppendFormat(" IdRed='{0}' ", IdRed);
                    bWhere = true;
                }
            }

            if (!string.IsNullOrEmpty(Tipo))
            {
                if (bWhere)
                    strWhere.AppendFormat(" AND Tipo='{0}' ", Tipo);
                else
                {
                    strWhere.AppendFormat(" Tipo='{0}' ", Tipo);
                    bWhere = true;
                }
            }

            if (!string.IsNullOrEmpty(NumAbonadoInicial))
            {
                if (bWhere)
                    strWhere.AppendFormat(" AND NumAbonadoInicial='{0}' ", NumAbonadoInicial);
                else
                {
                    strWhere.AppendFormat(" NumAbonadoInicial='{0}' ", NumAbonadoInicial);
                    bWhere = true;
                }
            }

            if (bWhere)
                strConsulta.AppendFormat(" WHERE {0}", strWhere);

            strConsulta.AppendFormat(" ORDER BY IdSistema,IdNucleo,IdSector,IdRed,Tipo,NumAbonadoInicial ", strWhere);
            strWhere.Clear();

            return strConsulta.ToString();
        }

        public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds)
        {
            ListaResultado.Clear();

            // Se obtiene los registros resultado de la consulta

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    LlamadasEntrantesSector r = new LlamadasEntrantesSector();

                    if (dr["IdSistema"] != System.DBNull.Value)
                        r.IdSistema = (string)dr["IdSistema"];

                    if (dr["IdNucleo"] != System.DBNull.Value)
                        r.IdNucleo = (string)dr["IdNucleo"];

                    if (dr["IdSector"] != System.DBNull.Value)
                        r.IdSector = (string)dr["IdSector"];

                    if (dr["IdRed"] != System.DBNull.Value)
                        r.IdRed = (string)dr["IdRed"];

                    if (dr["Tipo"]!= System.DBNull.Value)
                        r.Tipo = (string)dr["Tipo"];

                    if (dr["NumAbonadoInicial"] != System.DBNull.Value)
                        r.NumAbonadoInicial = (string)dr["NumAbonadoInicial"];

                    if (dr["NumAbonadoFinal"] != System.DBNull.Value)
                        r.NumAbonadoFinal = (string)dr["NumAbonadoFinal"];

                    ListaResultado.Add(r);
                }
            }

            return ListaResultado;
        }

        public override string[] InsertSQL()
        {
            string[] consulta = new string[2];
            StringBuilder strConsulta = new StringBuilder();

            //Realiza el insert sobre la tabla llamadas_entrantes_sector
            strConsulta.AppendFormat("INSERT INTO llamadas_entrantes_sector (IdSistema,IdNucleo,IdSector,IdRed,Tipo,NumAbonadoInicial,NumAbonadoFinal) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}',{6})",
                                   IdSistema, IdNucleo, IdSector, IdRed, Tipo, NumAbonadoInicial, string.IsNullOrEmpty(NumAbonadoFinal) ? "null" : string.Format("'{0}'", NumAbonadoFinal));

            consulta[0] = strConsulta.ToString();
            consulta[1] = ReplaceSQL(IdSistema, "llamadas_entrantes_sector");

            strConsulta.Clear();

            return consulta;
        }

        public override string[] UpdateSQL()
        {
            string[] consulta = new string[2];
            StringBuilder strConsulta = new StringBuilder();

            //Realiza el update  sobre la tabla llamadas_entrantes_sector, desde la aplicación solo tienen sentido actualizar el NumAbonadoInicial y final

            strConsulta.AppendFormat("UPDATE llamadas_entrantes_sector SET NumAbonadoInicial='{0}',NumAbonadoFinal={1} WHERE  IdSistema='{2}' AND IdNucleo='{3}' AND IdSector='{4}' AND IdRed='{5}' AND Tipo='{6}' ",
                                   string.IsNullOrEmpty(NumAbonadoFinal) ? "null" : string.Format("'{0}'", NumAbonadoFinal),IdSistema, IdNucleo, IdSector, IdRed, Tipo);

            consulta[0] = strConsulta.ToString();
            consulta[1] = ReplaceSQL(IdSistema, "llamadas_entrantes_sector");

            strConsulta.Clear();

            return consulta;
        }

        public override string[] DeleteSQL()
        {
            string[] consulta = new string[2];
            StringBuilder strConsulta = new StringBuilder();

            if (!string.IsNullOrEmpty(IdSistema ) && !string.IsNullOrEmpty(IdNucleo) &&  !string.IsNullOrEmpty(IdSector) && !string.IsNullOrEmpty(IdRed))
            {
                strConsulta.AppendFormat("DELETE FROM llamadas_entrantes_sector WHERE IdSistema='{0}'AND IdNucleo='{1}' AND IdSector='{2}' AND IdRed='{3}'", IdSistema, IdNucleo, IdSector, IdRed);

                if (!string.IsNullOrEmpty(Tipo))
                {
                    Consulta.AppendFormat(" AND Tipo='{0}'", Tipo);
                }

                if (!string.IsNullOrEmpty(NumAbonadoInicial))
                {
                    Consulta.AppendFormat(" AND NumAbonadoInicial='{0}'", NumAbonadoInicial);
                }

                if (!string.IsNullOrEmpty(NumAbonadoFinal))
                {
                    Consulta.AppendFormat(" AND NumAbonadoFinal='{0}'", NumAbonadoFinal);
                }
            }
            else if (!string.IsNullOrEmpty(IdSistema) && !string.IsNullOrEmpty(IdNucleo) && !string.IsNullOrEmpty(IdSector))
            {
                strConsulta.AppendFormat("DELETE FROM llamadas_entrantes_sector WHERE IdSistema='{0}'AND IdNucleo='{1}' AND IdSector='{2}'", IdSistema, IdNucleo, IdSector);
            }
            else if (!string.IsNullOrEmpty(IdRed))
            {
                strConsulta.AppendFormat("DELETE FROM llamadas_entrantes_sector WHERE IdRed='{0}'", IdRed);
            }

            if (strConsulta.Length > 0)
            {
                consulta[0] = strConsulta.ToString();
                consulta[1] = ReplaceSQL(IdSistema, "llamadas_entrantes_sector");
                Consulta.Clear();
            }

            return consulta;
        }


    }
}
