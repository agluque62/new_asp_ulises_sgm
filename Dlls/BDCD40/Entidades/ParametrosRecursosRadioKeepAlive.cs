using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace CD40.BD.Entidades
{
	public class ParametrosRecursosRadioKeepAlive : ParametrosRecursoRadio
	{
		private uint _KeepAlivePeriod;
		public uint KeepAlivePeriod
		{
			get { return _KeepAlivePeriod; }
			set { _KeepAlivePeriod = value; }
		}

		private uint _KeepAliveMultiplier;
		public uint KeepAliveMultiplier
		{
			get { return _KeepAliveMultiplier; }
			set { _KeepAliveMultiplier = value; }
		}

		public ParametrosRecursosRadioKeepAlive()
            : base()
        {
        }

		public /* new */ override string DataSetSelectSQL()
		{
			return base.DataSetSelectSQL();
		}

		public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds)
		{
			return base.ListSelectSQL(ds);
		}

		public override string[] InsertSQL()
		{
			return base.InsertSQL();
		}

		public override string[] UpdateSQL()
		{
			return base.UpdateSQL();
		}

		public override string[] DeleteSQL()
		{
			return base.DeleteSQL();
		}

		//public override int SelectCountSQL(string where)
		//{
		//    return base.SelectCountSQL(where);
		//}

		public override string[] UpdateDestinoSQL() { return null; }

		public override string[] LiberaDestinoSQL() { return null; }
	}
}
