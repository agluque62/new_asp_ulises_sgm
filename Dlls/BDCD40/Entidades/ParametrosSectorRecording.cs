using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
    public class ParametrosSectorRecording : ParametrosSectorSCVKeepAlive
    {
        // Grabacion de acuerdo al ED-137
        private bool _GrabacionEd137;
        public bool GrabacionEd137
        {
            get { return _GrabacionEd137; }
            set { _GrabacionEd137 = value; }
        }

        public override string DataSetSelectSQL() { return null; }
        public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds) { return null; }
        public override string[] InsertSQL() { return null; }
        public override string[] UpdateSQL() { return null; }
        public override string[] DeleteSQL() { return null; }
        //public override int SelectCountSQL(string where) { return 0; }
    }
}
