using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CD40.BD.Entidades
{
	public class ParametrosSectorSCVKeepAlive : ParametrosSectorSCV
	{
		// KeepAlivePeriod
		private uint _KeepAlivePeriod;
		public uint KeepAlivePeriod
		{
			get { return _KeepAlivePeriod; }
			set { _KeepAlivePeriod = value; }
		}
		// KeepAliveMultiplier.
		private uint _KeepAliveMultiplier;
		public uint KeepAliveMultiplier
		{
			get { return _KeepAliveMultiplier; }
			set { _KeepAliveMultiplier = value; }
		}

		public override string DataSetSelectSQL() { return null; }
		public override System.Collections.Generic.List<Tablas> ListSelectSQL(DataSet ds) { return null; }
		public override string[] InsertSQL() { return null; }
		public override string[] UpdateSQL() { return null; }
		public override string[] DeleteSQL() { return null; }
		//public override int SelectCountSQL(string where) { return 0; }
	}
}
