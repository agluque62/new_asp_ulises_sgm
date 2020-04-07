using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using CD40.BD.Entidades;


namespace SICCIP.Entidades
{
	/*
    public class ParametrosTOP
    {
        private int _ModoLogin;
        private string _IdUsuario;
        private string _IdHw;



        #region Propiedades
        public string IdUsuario
        {
            get { return _IdUsuario; }
            set { _IdUsuario = value; }
        }

        public int ModoLogin
        {
            get { return _ModoLogin; }
            set { _ModoLogin = value; }
        }

        public string IdHw
        {
            get { return _IdHw; }
            set { _IdHw = value; }
        }
        #endregion

        //Constructor
        public ParametrosTOP()
        { }
    }

	public class RecursoTIFX
    {
        private int _Posicion;
        private string _Nombre;

        #region Propiedades
        public string Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }

        public int Posicion
        {
            get { return _Posicion; }
            set { _Posicion = value; }
        }
        #endregion

        //Constructor
        public RecursoTIFX()
        { }

        public RecursoTIFX(int po, string id)
        {
            Posicion = po;
            Nombre = id;
        }
    }

	public class ParametrosTIFX 
	{
        private int _ModoLogin;
        private string _IdHw;
        public List<RecursoTIFX> Recursos;

        #region Propiedades
        
        public int ModoLogin
        {
            get { return _ModoLogin; }
            set { _ModoLogin = value; }
        }

        public string IdHw
        {
            get { return _IdHw; }
            set { _IdHw = value; }
        }
        #endregion
        
        //Constructor
        public ParametrosTIFX()
        { }

        public ParametrosTIFX(int modo, string id)
        {
            ModoLogin = modo;
            IdHw = id;
        }
    }

    public class CfgRecurso
    {
        public string[] StrTipo ={ "Audio Rx", "Audio Tx", "Audio RxTx", "Datos Rx", "Datos Tx", "Datos RxTx" };
        
        private string _NombreRecurso;
        private int _Tipo;
        private string _RecursoEnlace;
        private string _NombreEquipo;
        private int _Interface;

		//[StructLayout(LayoutKind.Explicit)]
		//public struct param
		//{
		//    [FieldOffset(0)]
		//    public ParametrosDatos ParamDatos;
		//    [FieldOffset(0)]
		//    public ParametrosRadio ParamRadio;
		//};
        //public param parametros;

        #region Propiedades

        public string NombreRecurso
        {
            get { return _NombreRecurso; }
            set { _NombreRecurso = value; }
        }

        public string RecursoEnlace
        {
            get { return _RecursoEnlace; }
            set { _RecursoEnlace = value; }
        }

        public string NombreEquipo
        {
            get { return _NombreEquipo; }
            set { _NombreEquipo = value; }
        }

        public int Tipo
        {
            get { return _Tipo; }
            set { _Tipo = value; }
        }

        public int Interface
        {
            get { return _Interface; }
            set { _Interface = value; }
        }
        #endregion
        
        //Constructor
        public CfgRecurso()
        { }
    };

    public class PosicionUsuario
    {
        private int _Posicion;
        private string _Nombre;
        private int _Estado;
        private int _ControlCifrador;
        private int _Prioridad;
        private string _Recurso;
        private int _Tipo;

        #region Propiedades

        public int Posicion
        {
            get { return _Posicion; }
            set { _Posicion = value; }
        }
                
        public string Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }

        public int Estado
        {
            get { return _Estado; }
            set { _Estado = value; }
        }

        public int ControlCifrador
        {
            get { return _ControlCifrador; }
            set { _ControlCifrador = value; }
        }

        public int Prioridad
        {
            get { return _Prioridad; }
            set { _Prioridad = value; }
        }

        public string Recurso
        {
            get { return _Recurso; }
            set { _Recurso = value; }
        }

        public int Tipo
        {
            get { return _Tipo; }
            set { _Tipo = value; }
        }
        #endregion

        //Constructor
        public PosicionUsuario()
        { }       
    }

    public class CfgUsuario
    {
        private string _IdUsuario;
        private int _NumLinTacticas;
        private int _NumComInternas;
        public List<PosicionUsuario> Posiciones;

        #region Propiedades
        public string IdUsuario
        {
            get { return _IdUsuario; }
            set { _IdUsuario = value; }
        }
        
        public int NumLinTacticas
        {
            get { return _NumLinTacticas; }
            set { _NumLinTacticas = value; }
        }

        public int NumComInternas
        {
            get { return _NumComInternas; }
            set { _NumComInternas = value; }
        }
        #endregion

        //Constructor
        public CfgUsuario()
        { }
    }

    public class RecursoComPlan
    {
        private string _Nombre;
        private string _NombreEnlace;
        private string _IdEquipo;
        private int _IdInterfaz;

        #region Propiedades
        public string Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }

        public string NombreEnlace
        {
            get { return _NombreEnlace; }
            set { _NombreEnlace = value; }
        }

        public string IdEquipo
        {
            get { return _IdEquipo; }
            set { _IdEquipo = value; }
        }

        public int IdInterfaz
        {
            get { return _IdInterfaz; }
            set { _IdInterfaz = value; }
        }
        #endregion

        //Constructor
        public RecursoComPlan()
        { }
    }

    public class LineaCompPlan
    {
        private string _IdLinea;
		private uint _Estado;
        public List<RecursoComPlan> Recursos;

        #region Propiedades
		public string IdLinea
		{
			get { return _IdLinea; }
			set { _IdLinea = value; }
		}
		public uint Estado
		{
			get { return _Estado; }
			set { _Estado = value; }
		}
		#endregion

        //Cosntructor
        public LineaCompPlan()
        {
            Recursos = new List<RecursoComPlan>();
        }
    }

    public class UsuComPlan
    {
        private string _IdUsuario;
        private int _TipoUsu; //0: Voz    1: Datos    2: Audio
        private string _IdHw;
        public List<LineaCompPlan> Lineas;

        #region Propiedades
        public string IdUsuario
        {
            get { return _IdUsuario; }
            set { _IdUsuario = value; }
        }

        public int TipoUsu
        {
            get { return _TipoUsu; }
            set { _TipoUsu = value; }
        }

        public string IdHw
        {
            get { return _IdHw; }
            set { _IdHw = value; }
        }

        #endregion

        //Constructor
        public UsuComPlan()
        {
            Lineas = new List<LineaCompPlan>();
        }

        public bool Busca(UsuComPlan u)
        {
            if (String.Compare(IdUsuario, u.IdUsuario) == 0)
                return true;
            else
                return false;
        }

    }

    public class EnlaceLineaTactica
    {
        private string _Id;
        private int _Tipo; //   0:Recurso      1:Equipo
        private int _ControlRemoto;

        #region Propiedades
        public string Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public int Tipo
        {
            get { return _Tipo; }
            set { _Tipo = value; }
        }

        public int ControlRemoto
        {
            get { return _ControlRemoto; }
            set { _ControlRemoto = value; }
        }
        #endregion

        //Constructor
        public EnlaceLineaTactica()
        { }
    }

	public class RecursosDatos : ParametrosRecursosRadioKASiccip
	{
		public enum tipo { DatosRx, AudioRx, DatosTx, AudioTx, DatosRxTx, AudioRxTx };

		public const int Modo_Datos = 0;
		public const int Modo_Audio = 1;
		public const int Tipo_Rx = 0;
		public const int Tipo_Tx = 1;
		public const int Tipo_RxTx = 2;

		private string _Sistema;
		private string _Id;
		private int _Tipo;
		private int _PortSip;

		#region Propiedades
		public string Sistema
		{
			get { return _Sistema; }
			set { _Sistema = value; }
		}

		public string Id
		{
			get { return _Id; }
			set { _Id = value; }
		}

		public int Tipo
		{
			get { return _Tipo; }
			set { _Tipo = value; }
		}

		public int PortSip
		{
			get { return _PortSip; }
			set { _PortSip = value; }
		}

		#endregion

		static CD40.BD.AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

		public RecursosDatos()
			: base()
		{
			TipoRecurso = (uint)Tipos.Tipo_Recurso.TR_DATOS;

			if (ServiceAccesoABaseDeDatos == null)
				ServiceAccesoABaseDeDatos = new CD40.BD.AccesoABaseDeDatos();
		}

		#region Metodos SQL
		public new DataSet DataSetSelectSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			if (Sistema != "" && Id != "")
				Consulta.Append("SELECT * FROM RecursosDatos WHERE Sistema='" + Sistema + "' AND " +
											"Id='" + Id + "'");
			else if (Sistema != "")
				Consulta.Append("SELECT * FROM RecursosDatos WHERE Sistema='" + Sistema + "'");
			else
				Consulta.Append("SELECT * FROM RecursosDatos");

			return ServiceAccesoABaseDeDatos.GetDataSet(Consulta.ToString());
		}

		public override System.Collections.Generic.List<Tablas> ListSelectSQL()
		{
			ListaResultado.Clear();

			DataSetResultado = this.DataSetSelectSQL();
			if (DataSetResultado != null)
			{
				foreach (System.Data.DataRow dr in DataSetResultado.Tables[0].Rows)
				{
					RecursosDatos r = new RecursosDatos();

					Sistema = r.Sistema = (string)dr["Sistema"];
					Id = r.IdRecursoSCV = r.Id = (string)dr["Id"];
					Tipo = r.Tipo = (int)(uint)dr["Tipo"];
					if (dr["Puerto"] != System.DBNull.Value)
						PortSip = r.PortSip = (int)(uint)dr["Puerto"];

					ListaResultado.Add(r);
				}
			}
			return ListaResultado;
		}

		public override int InsertSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("INSERT INTO RecursosDatos VALUES ('"
					+ this.Sistema + "','"
					+ this.Id + "',"
					+ this.Tipo + "," + this.PortSip + ")");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int UpdateSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("UPDATE RecursosDatos SET Id='" + this.Id
					+ "',Tipo=" + this.Tipo + ",PortSip=" + this.PortSip
					+ " WHERE Sistema='" + this.Sistema + "' AND Id='" + this.Id + "'");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int DeleteSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			if (Sistema != "" && Id != "")
				Consulta.Append("DELETE FROM RecursosDatos WHERE Sistema='" + Sistema + "' AND " +
											"Id='" + Id + "'");
			else if (Sistema != "")
				Consulta.Append("DELETE FROM RecursosDatos WHERE Sistema='" + Sistema + "'");
			else
				Consulta.Append("DELETE FROM RecursosDatos");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int SelectCountSQL(string where)
		{
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("SELECT COUNT(*) FROM RecursosDatos WHERE " + where);

			return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		}
		#endregion
	}

	public class HwUsu : Tablas
    {
        private string _IdHw;
        private string _IdUsuario;

        #region Propiedades

        public string IdHw
        {
            get { return _IdHw; }
            set { _IdHw = value; }
        }
        public string IdUsuario
        {
            get { return _IdUsuario; }
            set { _IdUsuario = value; }
        }
        
        #endregion

        static CD40.BD.AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

		public HwUsu()
        {
            if (ServiceAccesoABaseDeDatos == null)
                ServiceAccesoABaseDeDatos = new  CD40.BD.AccesoABaseDeDatos();
        }

        #region Metodos SQL
		public override DataSet DataSetSelectSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			if (IdHw != null)
				Consulta.Append("SELECT * FROM HwUsu WHERE IdHw='" + IdHw + "'");
			else if (IdUsuario != null)
				Consulta.Append("SELECT * FROM HwUsu WHERE IdUsuario='" + IdUsuario + "'");
			else
				Consulta.Append("SELECT * FROM HwUsu");

			return ServiceAccesoABaseDeDatos.GetDataSet(Consulta.ToString());
		}

		public override System.Collections.Generic.List<Tablas> ListSelectSQL()
		{
			ListaResultado.Clear();

			DataSetResultado = this.DataSetSelectSQL();
			if (DataSetResultado != null)
			{
				foreach (System.Data.DataRow dr in DataSetResultado.Tables[0].Rows)
				{
					HwUsu r = new HwUsu();

					r.IdHw = (string)dr["IdHw"];
					r.IdUsuario = (string)dr["IdUsuario"];

					ListaResultado.Add(r);
				}
			}
			return ListaResultado;
		}

        public override int InsertSQL()
        {
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("INSERT INTO HwUsu VALUES ('"
				+ IdHw + "','" + IdUsuario + "')");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int UpdateSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("UPDATE HwUsu SET IdUsuario='"
                    + IdUsuario + "' WHERE IdHw='" + IdHw + "'");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int DeleteSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			if (IdHw != null)
				Consulta.Append("DELETE FROM HwUsu WHERE IdHw='" + IdHw + "'");
			else if (IdUsuario != null)
				Consulta.Append("DELETE FROM HwUsu WHERE IdUsuario='" + IdUsuario + "'");
			else
				Consulta.Append("DELETE FROM HwUsu");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int SelectCountSQL(string where)
		{
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("SELECT COUNT(*) FROM HwUsu WHERE " + where);

			return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		}
		#endregion
    }

	public class Enlaces : Tablas
    {
		private string _IdLinea;
		private string _IdRecurso1;
        private string _IdRecurso2;
        private string _IdElementoHW1;
        private string _IdElementoHW2;

        #region Propiedades
        public string IdLinea
        {
            get { return _IdLinea; }
            set { _IdLinea = value; }
        }

        public string IdRecurso1
        {
            get { return _IdRecurso1; }
            set { _IdRecurso1 = value; }
        }

        public string IdRecurso2
        {
            get { return _IdRecurso2; }
            set { _IdRecurso2 = value; }
        }

        public string IdElementoHW1
        {
            get { return _IdElementoHW1; }
            set { _IdElementoHW1 = value; }
        }
        public string IdElementoHW2
        {
            get { return _IdElementoHW2; }
            set { _IdElementoHW2 = value; }
        }
        #endregion

        static CD40.BD.AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

		public Enlaces()
        {
            if (ServiceAccesoABaseDeDatos == null)
                ServiceAccesoABaseDeDatos = new  CD40.BD.AccesoABaseDeDatos();
        }

        #region Metodos SQL
		public override DataSet DataSetSelectSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			if (IdLinea != null && IdLinea != "")
				Consulta.Append("SELECT * FROM Enlaces WHERE IdLinea='" + this.IdLinea + "'");
			else
				Consulta.Append("SELECT * FROM Enlaces'");

			return ServiceAccesoABaseDeDatos.GetDataSet(Consulta.ToString());
		}

		public override System.Collections.Generic.List<Tablas> ListSelectSQL()
		{
			ListaResultado.Clear();

			DataSetResultado = this.DataSetSelectSQL();
			if (DataSetResultado != null)
			{
				foreach (System.Data.DataRow dr in DataSetResultado.Tables[0].Rows)
				{
					Enlaces r = new Enlaces();

					r.IdLinea = (string)dr["IdLinea"];
					r.IdElementoHW1 = (string)dr["IdElementoHW1"];
					r.IdElementoHW2 = (string)dr["IdElementoHW2"];
					r.IdRecurso1 = (string)dr["IdRecurso1"];
					r.IdRecurso2 = (string)dr["IdRecurso2"];

					ListaResultado.Add(r);
				}
			}
			return ListaResultado;
		}

        public override int InsertSQL()
        {
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("INSERT INTO Enlaces VALUES ('" + this.IdLinea +
                    "','" + this.IdRecurso1 + "','" + this.IdRecurso2 + "','" + this.IdElementoHW1 + "','" + this.IdElementoHW2 + "')");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int UpdateSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("UPDATE Enlaces SET IdLinea='" + this.IdLinea + "',IdRecurso1='" + this.IdRecurso1 +
						"',IdRecurso2='" + this.IdRecurso2 + "',IdElementoHW1='" + this.IdElementoHW1 + "',IdElementoHW2='" + this.IdElementoHW2 +
						"' WHERE IdLinea='" + this.IdLinea + "'");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int DeleteSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			if (IdLinea != null && IdLinea != "")
				Consulta.Append("DELETE FROM Enlaces WHERE IdLinea='" + this.IdLinea + "'");
			else
				Consulta.Append("DELETE FROM Enlaces");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int SelectCountSQL(string where)
		{
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("SELECT COUNT(*) FROM Enlaces WHERE " + where);

			return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		}
		#endregion
    }

    public class Equipos : Tablas
    {
        private string _Id;
        private int _Tipo;
        private string _Descripcion;
        private bool _ControlRemoto;
		private string _Banda;

        #region Propiedades
        public string Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public int Tipo
        {
            get { return _Tipo; }
            set { _Tipo = value; }
        }

        public string Descripcion
        {
            get { return _Descripcion; }
            set { _Descripcion = value; }
        }

        public bool ControlRemoto
        {
            get { return _ControlRemoto; }
            set { _ControlRemoto = value; }
        }

		public string Banda
		{
			get { return _Banda; }
			set { _Banda = value; }
		}
		
		#endregion

        static CD40.BD.AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

		public Equipos()
        {
            if (ServiceAccesoABaseDeDatos == null)
                ServiceAccesoABaseDeDatos = new  CD40.BD.AccesoABaseDeDatos();
        }

        #region Metodos SQL
		public override DataSet DataSetSelectSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			if (Id != null)
			{
				if (Tipo != 0)
					Consulta.Append("SELECT * FROM Equipos WHERE Id='" + this.Id + "' AND Tipo=" + this.Tipo);
				else
					Consulta.Append("SELECT * FROM Equipos WHERE Id='" + this.Id + "'");
			}
			else if (Tipo != 0 && Banda != "")
				Consulta.Append("SELECT * FROM Equipos WHERE Tipo=" + this.Tipo + " AND Banda='" + Banda + "'");
			else if (Tipo != 0)
				Consulta.Append("SELECT * FROM Equipos WHERE Tipo=" + this.Tipo);
			else
				Consulta.Append("SELECT * FROM Equipos");

			return ServiceAccesoABaseDeDatos.GetDataSet(Consulta.ToString());
		}

		public override System.Collections.Generic.List<Tablas> ListSelectSQL()
		{
			ListaResultado.Clear();

			DataSetResultado = this.DataSetSelectSQL();
			if (DataSetResultado != null)
			{
				foreach (System.Data.DataRow dr in DataSetResultado.Tables[0].Rows)
				{
					Equipos r = new Equipos();

					r.Id = (string)dr["Id"];
					r.Tipo = (int)(uint)dr["Tipo"];
					r.Descripcion = (string)dr["Descripcion"];
					r.ControlRemoto = (sbyte)dr["ControlRemoto"] != 0;
					if (dr["Banda"]!=System.DBNull.Value)
						r.Banda = (string)dr["Banda"];

					ListaResultado.Add(r);
				}
			}
			return ListaResultado;
		}

        public override int InsertSQL()
        {
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("INSERT INTO Equipos VALUES ('"
                    + this.Id + "'," + this.Tipo + ",'" + this.Descripcion + 
                    "'," + (ControlRemoto ? "1" : "0") + "," +
					((Banda == null) ? "null" : ("'" + Banda + "'")) +
					")");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int UpdateSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("UPDATE Equipos SET Tipo="
					+ this.Tipo + ",Descripcion='" + this.Descripcion
					+ "',ControlRemoto=" + (ControlRemoto ? "1" : "0")
					+ ",Banda=" + ((Banda == null) ? "null" : ("'" + Banda + "'")) +
					" WHERE Id='" + this.Id + "'");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int DeleteSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			if (Id != null && Id != "")
				Consulta.Append("DELETE FROM Equipos WHERE Id='" + this.Id + "'");
			else
				Consulta.Append("DELETE FROM Equipos");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int SelectCountSQL(string where)
		{
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("SELECT COUNT(*) FROM Equipos WHERE " + where);

			return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		}
		#endregion
    }

    public class Interfaces : Tablas
    {
        private string _IdEquipo;
        private int _Id;
        private bool _Cifrado;

        #region Propiedades
        public string IdEquipo
        {
            get { return _IdEquipo; }
            set { _IdEquipo = value; }
        }

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public bool Cifrado
        {
            get { return _Cifrado; }
            set { _Cifrado = value; }
        }

        #endregion

        static CD40.BD.AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

		public Interfaces()
        {
            if (ServiceAccesoABaseDeDatos == null)
                ServiceAccesoABaseDeDatos = new  CD40.BD.AccesoABaseDeDatos();
        }

        #region Metodos SQL
		public override DataSet DataSetSelectSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			if (IdEquipo != "" && Id != 0)
				Consulta.Append("SELECT * FROM Interfaces WHERE IdEquipo='" + IdEquipo + "' AND Id=" + Id);
			else if (IdEquipo != "")
				Consulta.Append("SELECT * FROM Interfaces WHERE IdEquipo='" + IdEquipo + "'");
			else
				Consulta.Append("SELECT * FROM Interfaces");

			return ServiceAccesoABaseDeDatos.GetDataSet(Consulta.ToString());
		}

		public override System.Collections.Generic.List<Tablas> ListSelectSQL()
		{
			ListaResultado.Clear();

			DataSetResultado = this.DataSetSelectSQL();
			if (DataSetResultado != null)
			{
				foreach (System.Data.DataRow dr in DataSetResultado.Tables[0].Rows)
				{
					Interfaces r = new Interfaces();

					r.Id = (int)(uint)dr["Id"];
					r.IdEquipo = (string)dr["IdEquipo"];
					r.Cifrado = (sbyte)dr["Cifrado"] != 0;

					ListaResultado.Add(r);
				}
			}
			return ListaResultado;
		}

        public override int InsertSQL()
        {
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("INSERT INTO Interfaces VALUES ('"
					+ this.IdEquipo + "'," + this.Id + "," + (Cifrado ? "1" : "0") + ")");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int UpdateSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("UPDATE Interfaces SET IdEquipo='"
					+ this.IdEquipo + ",Id=" + this.Id + ",Cifrado=" + (Cifrado ? "1" : "0") + " WHERE IdEquipo='"
					+ this.IdEquipo + "'");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int DeleteSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			if (IdEquipo != "" && Id != 0)
				Consulta.Append("DELETE FROM Interfaces WHERE IdEquipo='" + IdEquipo + "' AND Id=" + Id);
			else if (IdEquipo != "")
				Consulta.Append("DELETE FROM Interfaces WHERE IdEquipo='" + IdEquipo + "'");
			else
				Consulta.Append("DELETE FROM Interfaces");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int SelectCountSQL(string where)
		{
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("SELECT COUNT(*) FROM Interfaces WHERE " + where);

			return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		}
		#endregion
    }

    public class RecursosInterface : Tablas
    {
        private string _IdEquipo;
        private int _IdInterface;
        private string _IdElementoHW;
        private string _IdRecurso;

        #region Propiedades
        public string IdEquipo
        {
            get { return _IdEquipo; }
            set { _IdEquipo = value; }
        }

        public int IdInterface
        {
            get { return _IdInterface; }
            set { _IdInterface = value; }
        }

        public string IdElementoHW
        {
            get { return _IdElementoHW; }
            set { _IdElementoHW = value; }
        }

        public string IdRecurso
        {
            get { return _IdRecurso; }
            set { _IdRecurso = value; }
        }
        #endregion
        static CD40.BD.AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

		public RecursosInterface()
        {
            if (ServiceAccesoABaseDeDatos == null)
                ServiceAccesoABaseDeDatos = new  CD40.BD.AccesoABaseDeDatos();
        }

        #region Metodos SQL
		public override DataSet DataSetSelectSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			if (IdEquipo != null && IdEquipo != "" && IdRecurso != null && IdRecurso != "")
				Consulta.Append("SELECT * FROM RecursosInterface WHERE IdEquipo='" + this.IdEquipo + "' AND " +
										"IdRecurso='" + this.IdRecurso + "'");
			else if (IdEquipo != null && IdEquipo != "")
				Consulta.Append("SELECT * FROM RecursosInterface WHERE IdEquipo='" + this.IdEquipo + "'");
			else if (IdRecurso != null && IdRecurso != "")
				Consulta.Append("SELECT * FROM RecursosInterface WHERE IdRecurso='" + this.IdRecurso + "'");

			return ServiceAccesoABaseDeDatos.GetDataSet(Consulta.ToString());
		}

		public override System.Collections.Generic.List<Tablas> ListSelectSQL()
		{
			ListaResultado.Clear();

			DataSetResultado = this.DataSetSelectSQL();
			if (DataSetResultado != null)
			{
				foreach (System.Data.DataRow dr in DataSetResultado.Tables[0].Rows)
				{
					RecursosInterface r = new RecursosInterface();

					r.IdEquipo = (string)dr["IdEquipo"];
					r.IdInterface = (int)(uint)dr["IdInterface"];
					r.IdElementoHW = (string)dr["IdElementoHW"];
					r.IdRecurso = (string)dr["IdRecurso"];

					ListaResultado.Add(r);
				}
			}
			return ListaResultado;
		}

        public override int InsertSQL()
        {
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("INSERT INTO RecursosInterface VALUES ('"
					+ this.IdEquipo + "'," + this.IdInterface + ",'"
					+ this.IdElementoHW + "','" + this.IdRecurso + "')");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int UpdateSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("UPDATE RecursosInterface SET IdEquipo='" + this.IdEquipo + "',IdInterface=" + this.IdInterface
					+ ",IdelementoHW='" + this.IdElementoHW + "',IdRecurso='" + this.IdRecurso + "' WHERE IdEquipo='"
					+ this.IdEquipo + "'");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int DeleteSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			if (IdEquipo != null && IdEquipo != "" && IdRecurso != null && IdRecurso != "")
				Consulta.Append("DELETE FROM RecursosInterface WHERE IdEquipo='" + this.IdEquipo + "' AND " +
										"IdRecurso='" + this.IdRecurso + "'");
			else if (IdEquipo != null && IdEquipo != "")
				Consulta.Append("DELETE FROM RecursosInterface WHERE IdEquipo='" + this.IdEquipo + "'");
			else if (IdRecurso != null && IdRecurso != "")
				Consulta.Append("DELETE FROM RecursosInterface WHERE IdRecurso='" + this.IdRecurso + "'");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int SelectCountSQL(string where)
		{
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("SELECT COUNT(*) FROM RecursosInterface WHERE " + where);

			return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		}
		#endregion
    }

    public class RecursosLinea : Tablas
    {
        private string _IdLinea;
        private string _IdRecurso;
        private string _IdElementoHW;

        #region Propiedades
        public string IdLinea
        {
            get { return _IdLinea; }
            set { _IdLinea = value; }
        }

        public string IdRecurso
        {
            get { return _IdRecurso; }
            set { _IdRecurso = value; }
        }

        public string IdElementoHW
        {
            get { return _IdElementoHW; }
            set { _IdElementoHW = value; }
        }
        #endregion

        static CD40.BD.AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

		public RecursosLinea()
        {
            if (ServiceAccesoABaseDeDatos == null)
                ServiceAccesoABaseDeDatos = new  CD40.BD.AccesoABaseDeDatos();
        }

        #region Metodos SQL
		public override DataSet DataSetSelectSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			if (IdLinea != null && IdLinea != "" && IdRecurso != null && IdRecurso != "")
				Consulta.Append("SELECT * FROM RecursosLinea WHERE IdLinea='" + IdLinea + "' AND IdRecurso='"
					+ IdRecurso + "'");
			else if (IdLinea != null && IdLinea != "")
				Consulta.Append("SELECT * FROM RecursosLinea WHERE IdLinea='" + IdLinea + "'");
			else
				Consulta.Append("SELECT * FROM RecursosLinea");

			return ServiceAccesoABaseDeDatos.GetDataSet(Consulta.ToString());
		}

		public override System.Collections.Generic.List<Tablas> ListSelectSQL()
		{
			ListaResultado.Clear();

			DataSetResultado = this.DataSetSelectSQL();
			if (DataSetResultado != null)
			{
				foreach (System.Data.DataRow dr in DataSetResultado.Tables[0].Rows)
				{
					RecursosLinea r = new RecursosLinea();

					r.IdLinea = (string)dr["IdLinea"];
					r.IdElementoHW = (string)dr["IdElementoHW"];
					r.IdRecurso = (string)dr["IdRecurso"];

					ListaResultado.Add(r);
				}
			}
			return ListaResultado;
		}

        public override int InsertSQL()
        {
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("INSERT INTO RecursosLinea VALUES ('"
					+ IdLinea + "','" + IdRecurso + "','" + IdElementoHW + "')");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int UpdateSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("UPDATE RecursosLinea SET IdLinea='" + IdLinea + "',IdRecurso='" + IdRecurso
					+ "',IdElementoHW='" + IdElementoHW + "' WHERE IdLinea='" + IdLinea + "'");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int DeleteSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			if (IdLinea != null && IdLinea != "" && IdRecurso != null && IdRecurso != "")
				Consulta.Append("DELETE FROM RecursosLinea WHERE IdLinea='" + IdLinea + "' AND IdRecurso='"
					+ IdRecurso + "'");
			else if (IdLinea != null && IdLinea != "")
				Consulta.Append("DELETE FROM RecursosLinea WHERE IdLinea='" + IdLinea + "'");
			else
				Consulta.Append("DELETE FROM RecursosLinea");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int SelectCountSQL(string where)
		{
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("SELECT COUNT(*) FROM RecursosLinea WHERE " + where);

			return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		}
		#endregion
    }

    public class Planes : Tablas
    {
        private string _Id;
        private bool _Activo;

        #region Propiedades
        public string Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public bool Activo
        {
            get { return _Activo; }
            set { _Activo = value; }
        }
		
		private DateTime _FechaActivacion;
		public DateTime FechaActivacion
		{
			get { return _FechaActivacion; }
			set { _FechaActivacion = value; }
		}
		#endregion

        static CD40.BD.AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

		public Planes()
        {
            if (ServiceAccesoABaseDeDatos == null)
                ServiceAccesoABaseDeDatos = new  CD40.BD.AccesoABaseDeDatos();
        }

        #region Metodos SQL
		public override DataSet DataSetSelectSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			if (Id != null)
				Consulta.Append("SELECT * FROM Planes WHERE Id='" + this.Id + "'");
			else
				Consulta.Append("SELECT * FROM Planes");

			return ServiceAccesoABaseDeDatos.GetDataSet(Consulta.ToString());
		}

		public override System.Collections.Generic.List<Tablas> ListSelectSQL()
		{
			ListaResultado.Clear();

			DataSetResultado = this.DataSetSelectSQL();
			if (DataSetResultado != null)
			{
				foreach (System.Data.DataRow dr in DataSetResultado.Tables[0].Rows)
				{
					Planes r = new Planes();

					r.Id = (string)dr["Id"];
					if (dr["Activo"] != System.DBNull.Value)
						r.Activo = (sbyte)dr["Activo"] != 0;
					if (dr["FechaActivacion"] != System.DBNull.Value)
						r.FechaActivacion = (DateTime)dr["FechaActivacion"];

					ListaResultado.Add(r);
				}
			}
			return ListaResultado;
		}

        public override int InsertSQL()
        {
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("INSERT INTO Planes (Id,Activo) VALUES ('" + this.Id +
										"'," + Activo +
										"DATE_FORMAT('" + FechaActivacion + "','%d/%m/%Y %H:%i:%s')" + 
										")");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int UpdateSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("UPDATE Planes SET Id='" + this.Id + "',Activo=" + Activo +
					" WHERE Id='" + Id + "'");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int DeleteSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			if (Id != null)
				Consulta.Append("DELETE FROM Planes WHERE Id='" + this.Id + "'");
			else
				Consulta.Append("DELETE FROM Planes");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int SelectCountSQL(string where)
		{
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("SELECT COUNT(*) FROM Planes WHERE " + where);

			return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		}
		#endregion
    }

    public class Lineas : Tablas
    {
        private string _Id;
        private string _IdComplementaria;
        private int _Estado;
		private string _FrecuenciaTX;
		private string _FrecuenciaRX;
		private string _Modo;
		private int _Tipo;

        #region Propiedades
        public string Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public string IdComplementaria
        {
            get { return _IdComplementaria; }
            set { _IdComplementaria = value; }
        }

        public int Estado
        {
            get { return _Estado; }
            set { _Estado = value; }
        }

		public string FrecuenciaTX
		{
			get { return _FrecuenciaTX; }
			set { _FrecuenciaTX = value; }
		}

		public string FrecuenciaRX
		{
			get { return _FrecuenciaRX; }
			set { _FrecuenciaRX = value; }
		}

		public string Modo
		{
			get { return _Modo; }
			set { _Modo = value; }
		}

		public int Tipo
		{
			get { return _Tipo; }
			set { _Tipo = value; }
		}

		#endregion

        static CD40.BD.AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

		public Lineas()
        {
            if (ServiceAccesoABaseDeDatos == null)
                ServiceAccesoABaseDeDatos = new  CD40.BD.AccesoABaseDeDatos();
        }

        #region Metodos SQL
		public override DataSet DataSetSelectSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			if (Id != null)
				Consulta.Append("SELECT * FROM Lineas WHERE Id='" + Id + "'");
			else
				Consulta.Append("SELECT * FROM Lineas");

			return ServiceAccesoABaseDeDatos.GetDataSet(Consulta.ToString());
		}

		public override System.Collections.Generic.List<Tablas> ListSelectSQL()
		{
			ListaResultado.Clear();

			DataSetResultado = this.DataSetSelectSQL();
			if (DataSetResultado != null)
			{
				foreach (System.Data.DataRow dr in DataSetResultado.Tables[0].Rows)
				{
					Lineas r = new Lineas();

					r.Id = (string)dr["Id"];
					if (dr["IdComplementaria"] != System.DBNull.Value)
						r.IdComplementaria = (string)dr["IdComplementaria"];
					if (dr["Estado"] != System.DBNull.Value)
						r.Estado = (int)(uint)dr["Estado"];
					if (dr["FrecuenciaTx"] != System.DBNull.Value)
						r.FrecuenciaTX = (string)dr["FrecuenciaTx"];
					if (dr["FrecuenciaRx"] != System.DBNull.Value)
						r.FrecuenciaRX = (string)dr["FrecuenciaRx"];
					if (dr["Modo"] != System.DBNull.Value)
						r.Modo = (string)dr["Modo"];
					if (dr["Tipo"] != System.DBNull.Value)
						r.Tipo = (int)dr["Tipo"];

					ListaResultado.Add(r);
				}
			}
			return ListaResultado;
		}

        public override int InsertSQL()
        {
			Consulta.Remove(0, Consulta.Length);
			if (IdComplementaria != null)
				Consulta.Append("INSERT INTO Lineas VALUES ('"
					+ this.Id + "','" + this.IdComplementaria + "'," + Convert.ToInt16(this.Estado) +
					",'" + FrecuenciaTX + "','" + FrecuenciaRX + "','" + Modo + "'," +
					Tipo +
					")");
			else
				Consulta.Append("INSERT INTO Lineas VALUES ('"
					+ this.Id + "',NULL," + Convert.ToInt16(this.Estado) +
					",'" + FrecuenciaTX + "','" + FrecuenciaRX + "','" + Modo + "'," +
					Tipo +
					")");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int UpdateSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			if (IdComplementaria == null)
				Consulta.Append("UPDATE Lineas SET Id='" + this.Id +
						"',Estado=" + Convert.ToInt16(this.Estado) + "," +
						"FrecuenciaTx='" + FrecuenciaTX + "'," +
						"FrecuenciaRx='" + FrecuenciaRX + "'," +
						"Modo='" + Modo + "'," +
						"Tipo=" + Tipo +
						" WHERE Id='" + this.Id + "'");
			else
				Consulta.Append("UPDATE Lineas SET Id='" + this.Id +
						"', IdComplementaria='" + this.IdComplementaria + "',Estado=" + Convert.ToInt16(this.Estado) + "," +
						"FrecuenciaTx='" + FrecuenciaTX + "'," +
						"FrecuenciaRx='" + FrecuenciaRX + "'," +
						"Modo='" + Modo + "'," +
						"Tipo=" + Tipo +
						" WHERE Id='" + this.Id + "'");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int DeleteSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			if (Id != null)
				Consulta.Append("DELETE FROM Lineas WHERE Id='" + this.Id + "'");
			else
				Consulta.Append("DELETE FROM Lineas");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int SelectCountSQL(string where)
		{
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("SELECT COUNT(*) FROM Lineas WHERE " + where);

			return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		}
		#endregion
    }

    public class LineasPlan : Tablas
    {
        private string _IdPlan;
        private string _IdLinea;

        #region Propiedades
        public string IdPlan
        {
            get { return _IdPlan; }
            set { _IdPlan = value; }
        }

        public string IdLinea
        {
            get { return _IdLinea; }
            set { _IdLinea = value; }
        }

        #endregion

        static CD40.BD.AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

		public LineasPlan()
        {
            if (ServiceAccesoABaseDeDatos == null)
                ServiceAccesoABaseDeDatos = new  CD40.BD.AccesoABaseDeDatos();
        }

        #region Metodos SQL
		public override DataSet DataSetSelectSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			if (IdPlan != null && IdLinea != null)
				Consulta.Append("SELECT l.* FROM LineasPlan lp, Lineas l WHERE IdPlan='" + this.IdPlan + "' AND lp.IdLinea='" +
                    this.IdLinea + "' AND lp.IdLinea=l.Id");
			else if (IdPlan != null)
				Consulta.Append("SELECT l.* FROM LineasPlan lp, Lineas l WHERE IdPlan='" + this.IdPlan + "' AND lp.IdLinea=l.Id");
			else if (IdLinea != null)
				Consulta.Append("SELECT l.* FROM LineasPlan lp, Lineas l WHERE IdLinea='" + this.IdLinea + "' AND lp.IdLinea=l.Id");

			return ServiceAccesoABaseDeDatos.GetDataSet(Consulta.ToString());
		}

		public override System.Collections.Generic.List<Tablas> ListSelectSQL()
		{
			ListaResultado.Clear();

			DataSetResultado = this.DataSetSelectSQL();
			if (DataSetResultado != null)
			{
				foreach (System.Data.DataRow dr in DataSetResultado.Tables[0].Rows)
				{
					Lineas linea = new Lineas();

					linea.Id = (string)dr["Id"];
					if (dr["IdComplementaria"] != System.DBNull.Value)
						linea.IdComplementaria = (string)dr["IdComplementaria"];
					if (dr["Estado"] != System.DBNull.Value)
						linea.Estado = (int)(uint)dr["Estado"];

					ListaResultado.Add(linea);
				}
			}
			return ListaResultado;
		}

        public override int InsertSQL()
        {
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("INSERT INTO LineasPlan VALUES ('"
					+ this.IdPlan + "','" + this.IdLinea + "')");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int UpdateSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("UPDATE LineasPlan SET IdPlan='" + this.IdPlan +
						"',IdLinea='" + this.IdLinea + "' WHERE IdPlan='" + this.IdPlan + "' AND IdLinea='" + this.IdLinea + "'");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int DeleteSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			if (IdPlan != null && IdLinea != null)
				Consulta.Append("DELETE FROM LineasPlan WHERE IdPlan='" + this.IdPlan + "' AND IdLinea='" +
					this.IdLinea + "'");
			else if (IdPlan != null)
				Consulta.Append("DELETE FROM LineasPlan WHERE IdPlan='" + this.IdPlan + "'");
			else if (IdLinea != null)
				Consulta.Append("DELETE FROM LineasPlan WHERE IdLinea='" + this.IdLinea + "'");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int SelectCountSQL(string where)
		{
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("SELECT COUNT(*) FROM LineasPlan WHERE " + where);

			return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		}
		#endregion
    }

    public class Usuarios : Tablas
    {
        public enum tipo { Voz, Datos, Audio, NoDatos };

        private string _Id;
        private int _Tipo;

        #region Propiedades
        public string Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public int Tipo
        {
            get { return _Tipo; }
            set { _Tipo = value; }
        }
        #endregion
        static CD40.BD.AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

		public Usuarios()
        {
            if (ServiceAccesoABaseDeDatos == null)
                ServiceAccesoABaseDeDatos = new  CD40.BD.AccesoABaseDeDatos();
        }

        #region Metodos SQL
		public override DataSet DataSetSelectSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			if (Id != null && Id != "")
				Consulta.Append("SELECT * FROM Usuarios WHERE Id='" + this.Id + "'");
			else if (Tipo == (int)tipo.NoDatos)
				Consulta.Append("SELECT * FROM Usuarios WHERE Tipo IN (0,2)");
			else if (Tipo != -1)
				Consulta.Append("SELECT * FROM Usuarios WHERE Tipo=" + this.Tipo);
			else
				Consulta.Append("SELECT * FROM Usuarios");

			return ServiceAccesoABaseDeDatos.GetDataSet(Consulta.ToString());
		}

		public override System.Collections.Generic.List<Tablas> ListSelectSQL()
		{
			ListaResultado.Clear();

			DataSetResultado = this.DataSetSelectSQL();
			if (DataSetResultado != null)
			{
				foreach (System.Data.DataRow dr in DataSetResultado.Tables[0].Rows)
				{
					Usuarios usr = new Usuarios();

					usr.Id = (string)dr["Id"];
					usr.Tipo = (int)(uint)dr["Tipo"];

					ListaResultado.Add(usr);
				}
			}
			return ListaResultado;
		}

        public override int InsertSQL()
        {
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("INSERT INTO Usuarios VALUES ('"
					+ this.Id + "'," + this.Tipo + ")");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int UpdateSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("UPDATE Usuarios SET Id='"
					+ this.Id + "',Tipo=" + this.Tipo + " WHERE Id='"
					+ this.Id + "'");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int DeleteSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			if (Id != null && Id != "")
				Consulta.Append("DELETE FROM Usuarios WHERE Id='" + this.Id + "'");
			else if (Tipo == (int)tipo.NoDatos)
				Consulta.Append("DELETE FROM Usuarios WHERE Tipo IN (0,2)");
			else if (Tipo != -1)
				Consulta.Append("DELETE FROM Usuarios WHERE Tipo=" + this.Tipo);
			else
				Consulta.Append("DELETE FROM Usuarios");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int SelectCountSQL(string where)
		{
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("SELECT COUNT(*) FROM Usuarios WHERE " + where);

			return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		}
		#endregion
    }

    public class LineasUsu : Tablas
    {
        private int _Posicion;
        private string _IdUsuario;
        private string _IdLinea;
        private int _ControlCifrador;
        private int _PrioridadPtt;

        #region Propiedades
        public int Posicion
        {
            get { return _Posicion; }
            set { _Posicion = value; }
        }

        public string IdUsuario
        {
            get { return _IdUsuario; }
            set { _IdUsuario = value; }
        }

        public string IdLinea
        {
            get { return _IdLinea; }
            set { _IdLinea = value; }
        }

        public int ControlCifrador
        {
            get { return _ControlCifrador; }
            set { _ControlCifrador = value; }
        }

        public int PrioridadPtt
        {
            get { return _PrioridadPtt; }
            set { _PrioridadPtt = value; }
        }
        #endregion

        static CD40.BD.AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

		public LineasUsu()
        {
            if (ServiceAccesoABaseDeDatos == null)
                ServiceAccesoABaseDeDatos = new  CD40.BD.AccesoABaseDeDatos();
        }

        #region Metodos SQL
		public override DataSet DataSetSelectSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			if (Posicion != 0 && IdUsuario != null && IdLinea != null)
				Consulta.Append("SELECT * FROM LineasUsu WHERE Posicion=" + this.Posicion +
						" AND IdUsuario='" + this.IdUsuario + "' AND IdLinea='" + this.IdLinea + "'");
			else if (IdUsuario != null)
				Consulta.Append("SELECT * FROM LineasUsu WHERE IdUsuario='" + this.IdUsuario + "'");
			else if (IdLinea != null)
				Consulta.Append("SELECT * FROM LineasUsu WHERE IdLinea='" + this.IdLinea + "'");

			return ServiceAccesoABaseDeDatos.GetDataSet(Consulta.ToString());
		}

		public override System.Collections.Generic.List<Tablas> ListSelectSQL()
		{
			ListaResultado.Clear();

			DataSetResultado = this.DataSetSelectSQL();
			if (DataSetResultado != null)
			{
				foreach (System.Data.DataRow dr in DataSetResultado.Tables[0].Rows)
				{
					LineasUsu usr = new LineasUsu();

					usr.Posicion = (int)(uint)dr["Posicion"];
					usr.IdUsuario = (string)dr["IdUsuario"];
					usr.IdLinea = (string)dr["IdLinea"];
					usr.ControlCifrador = (int)(uint)dr["ControlCifrador"];
					usr.PrioridadPtt = (int)(uint)dr["PrioridadPtt"];

					ListaResultado.Add(usr);
				}
			}
			return ListaResultado;
		}

        public override int InsertSQL()
        {
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("INSERT INTO LineasUsu VALUES (" + this.Posicion +
					",'" + this.IdUsuario + "','" + this.IdLinea + "'," + this.ControlCifrador + "," + this.PrioridadPtt + ")");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int UpdateSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("UPDATE LineasUsu SET Posicion=" + this.Posicion + ",IdUsuario='" + this.IdUsuario +
						"',IdLinea='" + this.IdLinea + "',ControlCifrador=" + this.ControlCifrador + ",PrioridadPTT=" + this.PrioridadPtt +
						" WHERE Posicion=" + this.Posicion + " AND IdUsuario='" + this.IdUsuario + "'");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int DeleteSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			if (Posicion != 0 && IdUsuario != null && IdLinea != null)
				Consulta.Append("DELETE FROM LineasUsu WHERE Posicion=" + this.Posicion +
						" AND IdUsuario='" + this.IdUsuario + "' AND IdLinea='" + this.IdLinea + "'");
			else if (IdUsuario != null)
				Consulta.Append("DELETE FROM LineasUsu WHERE IdUsuario='" + this.IdUsuario + "'");
			else if (IdLinea != null)
				Consulta.Append("DELETE FROM LineasUsu WHERE IdLinea='" + this.IdLinea + "'");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int SelectCountSQL(string where)
		{
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("SELECT COUNT(*) FROM LineasUsu WHERE " + where);

			return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		}
		#endregion
    }
    
    public class Operadores : Tablas
    {
        private string _IdOperador;
        private string _Clave;
        private int _Grupo;
        private string _Descripcion;

        #region Propiedades
        public string IdOperador
        {
            get { return _IdOperador; }
            set { _IdOperador = value; }
        }

        public string Clave
        {
            get { return _Clave; }
            set { _Clave = value; }
        }

        public int Grupo
        {
            get { return _Grupo; }
            set { _Grupo = value; }
        }

        public string Descripcion
        {
            get { return _Descripcion; }
            set { _Descripcion = value; }
        }
        #endregion

        static CD40.BD.AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

		public Operadores()
        {
            if (ServiceAccesoABaseDeDatos == null)
                ServiceAccesoABaseDeDatos = new  CD40.BD.AccesoABaseDeDatos();
        }

        #region Metodos SQL
		public override DataSet DataSetSelectSQL()
		{
			Consulta.Remove(0, Consulta.Length);
            if (IdOperador != null)
                Consulta.Append("SELECT * FROM Operadores WHERE Id='" + this.IdOperador + "'");
            else
                Consulta.Append("SELECT * FROM Operadores");

			return ServiceAccesoABaseDeDatos.GetDataSet(Consulta.ToString());
		}

		public override System.Collections.Generic.List<Tablas> ListSelectSQL()
		{
			ListaResultado.Clear();

			DataSetResultado = this.DataSetSelectSQL();
			if (DataSetResultado != null)
			{
				foreach (System.Data.DataRow dr in DataSetResultado.Tables[0].Rows)
				{
					Operadores usr = new Operadores();

					usr.IdOperador = (string)dr["Id"];
					usr.Clave = (string)dr["Clave"];
					usr.Grupo = (int)(uint)dr["Grupo"];
					usr.Descripcion = (string)dr["Info"];

					ListaResultado.Add(usr);
				}
			}
			return ListaResultado;
		}

        public override int InsertSQL()
        {
			Consulta.Remove(0, Consulta.Length);
			if (this.Descripcion != null)
				Consulta.Append("INSERT INTO Operadores VALUES ('"
										+ this.IdOperador + "','" + this.Clave + "'," + this.Grupo.ToString() +
										",'" + this.Descripcion + "')");
			else
				Consulta.Append("INSERT INTO Operadores VALUES ('"
										+ this.IdOperador + "','" + this.Clave + "'," + this.Grupo.ToString() +
										",'')");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int UpdateSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			if (this.Descripcion != null && this.Descripcion.Length > 0)
				Consulta.Append("UPDATE Operadores SET Info='" + this.Descripcion +
						"', Grupo=" + this.Grupo.ToString() + " WHERE Id='" + this.IdOperador + "'");
			else
				Consulta.Append("UPDATE Operadores SET Grupo=" + this.Grupo.ToString() + " WHERE Id='" + this.IdOperador + "'");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int DeleteSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			if (IdOperador != null)
				Consulta.Append("DELETE FROM Operadores WHERE Id='" + this.IdOperador + "'");
			else
				Consulta.Append("DELETE FROM Operadores");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int SelectCountSQL(string where)
		{
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("SELECT COUNT(*) FROM Operadores WHERE " + where);

			return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		}
		#endregion
	}

	public class RecursosUsuario : Tablas
	{
		private string _IdUsuario;
		private string _IdElementoHW;
		private string _IdRecurso;

		#region Propiedades
		public string IdUsuario
		{
			get { return _IdUsuario; }
			set { _IdUsuario = value; }
		}

		public string IdElementoHW
		{
			get { return _IdElementoHW; }
			set { _IdElementoHW = value; }
		}

		public string IdRecurso
		{
			get { return _IdRecurso; }
			set { _IdRecurso = value; }
		}
		#endregion
        static CD40.BD.AccesoABaseDeDatos ServiceAccesoABaseDeDatos;

		public RecursosUsuario()
        {
            if (ServiceAccesoABaseDeDatos == null)
                ServiceAccesoABaseDeDatos = new  CD40.BD.AccesoABaseDeDatos();
        }

        #region Metodos SQL
		public override DataSet DataSetSelectSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			if (IdUsuario != null && IdUsuario != "" && IdRecurso != null && IdRecurso != "")
				Consulta.Append("SELECT * FROM RecursosUsuario WHERE IdUsuario='" + this.IdUsuario + "' AND " +
										"IdRecurso='" + this.IdRecurso + "'");
			else if (IdUsuario != null && IdUsuario != "")
				Consulta.Append("SELECT * FROM RecursosUsuario WHERE IdUsuario='" + this.IdUsuario + "'");
			else if (IdRecurso != null && IdRecurso != "")
				Consulta.Append("SELECT * FROM RecursosUsuario WHERE IdRecurso='" + this.IdRecurso + "'");

			return ServiceAccesoABaseDeDatos.GetDataSet(Consulta.ToString());
		}

		public override System.Collections.Generic.List<Tablas> ListSelectSQL()
		{
			ListaResultado.Clear();

			DataSetResultado = this.DataSetSelectSQL();
			if (DataSetResultado != null)
			{
				foreach (System.Data.DataRow dr in DataSetResultado.Tables[0].Rows)
				{
					RecursosUsuario usr = new RecursosUsuario();

					usr.IdUsuario = (string)dr["IdUsuario"];
					usr.IdElementoHW = (string)dr["IdElementoHW"];
					usr.IdRecurso = (string)dr["IdRecurso"];

					ListaResultado.Add(usr);
				}
			}
			return ListaResultado;
		}

        public override int InsertSQL()
        {
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("INSERT INTO RecursosUsuario VALUES ('"
					+ this.IdUsuario + "','" + this.IdElementoHW + "','" + this.IdRecurso + "')");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int UpdateSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("UPDATE RecursosUsuario SET IdUsuario='" + this.IdUsuario +
						"',IdelementoHW='" + this.IdElementoHW + "',IdRecurso='" + this.IdRecurso + "' WHERE IdUsuario='"
					+ this.IdUsuario + "'");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int DeleteSQL()
		{
			Consulta.Remove(0, Consulta.Length);
			if (IdUsuario != null && IdUsuario != "" && IdRecurso != null && IdRecurso != "")
				Consulta.Append("DELETE FROM RecursosUsuario WHERE IdUsuario='" + this.IdUsuario + "' AND " +
										"IdRecurso='" + this.IdRecurso + "'");
			else if (IdUsuario != null && IdUsuario != "")
				Consulta.Append("DELETE FROM RecursosUsuario WHERE IdUsuario='" + this.IdUsuario + "'");
			else if (IdRecurso != null && IdRecurso != "")
				Consulta.Append("DELETE FROM RecursosUsuario WHERE IdRecurso='" + this.IdRecurso + "'");

			return CD40.BD.AccesoABaseDeDatos.ExecuteNonQuery(Consulta.ToString());
		}

		public override int SelectCountSQL(string where)
		{
			Consulta.Remove(0, Consulta.Length);
			Consulta.Append("SELECT COUNT(*) FROM RecursosUsuario WHERE " + where);

			return Convert.ToInt32(ServiceAccesoABaseDeDatos.ExecuteScalar(Consulta.ToString()));
		}
		#endregion
	}
	 */
}
