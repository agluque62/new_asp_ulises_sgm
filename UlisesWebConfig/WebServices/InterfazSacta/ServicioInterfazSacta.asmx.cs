using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Configuration;
using System.Configuration;

using MySql.Data;
using MySql.Data.MySqlClient;

using Newtonsoft.Json;

using Sacta;
using CD40.BD;

using Utilities;

namespace UlisesWebConfig.WebServices.InterfazSacta
{
    /// <summary>
    /// Descripción breve de ServicioInterfazSacta
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class ServicioInterfazSacta : System.Web.Services.WebService
    {
        static SactaModule sModule = null;
        static string IdSistema = default;
        static byte EstadoSacta = (byte)0;
        static MySqlConnection MySqlConnectionToCd40 = null;

		static void sModule_SactaActivityChanged(object sender, Dictionary<string, object> msg)
		{
			EstadoSacta = (byte)msg["SactaActivity"];
		}
        static string Sistema
        {
            get
            {
                if (IdSistema == default)
                {
                    IdSistema = WebConfigurationManager.OpenWebConfiguration("~")
                        .AppSettings.Settings["Sistema"]?.Value;
                }
                return IdSistema ?? "departamento";
            }
        }
        static MySqlConnection Connection
        {
            get
            {
                if (MySqlConnectionToCd40 == null)
                {
                    var CadenaConexion = WebConfigurationManager.OpenWebConfiguration("~")
                        .ConnectionStrings.ConnectionStrings["ConexionBaseDatosCD40"]?.ToString();
                    MySqlConnectionToCd40 = new MySqlConnection(CadenaConexion ?? "");
                }
                return MySqlConnectionToCd40;
            }
        }
        static SactaModule SModule
        {
            get
            {
                if (sModule == null)
                {
                    sModule = new Sacta.SactaModule(Sistema, Connection);
                    sModule.SactaActivityChanged += 
                        new Sacta.Utilities.GenericEventHandler<Dictionary<string, object>>(sModule_SactaActivityChanged);
                }
                return sModule;
            }
        }

        [WebMethod]
        public byte GetEstadoSacta()
        {
            if (SModule != null)
            {
                EstadoSacta = (byte)((byte)(SModule.State << 4) | (EstadoSacta));
                return EstadoSacta;
            }
            return 0;
        }

        [WebMethod]
        public void StartSacta()
        {
            SModule.Start();
        }

        [WebMethod]
        public void EndSacta()
        {
            SModule.Stop();
            EstadoSacta = (byte)0;
            sModule = null;
        }

        [WebMethod]
        public string SactaConfGet()
        {
            return SactaModule.SactaCfgGet();
        }

        [WebMethod]
        public bool SactaConfSet(string jcfg)
        {
            return SactaModule.SactaCfgSet(jcfg);
        }

        /** 20200320. Metodos para el nuevo Servicio SACTA localizado en MTTO. */
        [WebMethod]
        public string SectorizeFromSacta(uint Version, string dataSect)
        {
            var info = new SactaInfo();
            var FechaActivacion = DateTime.Now;
            var util = new CD40.BD.Utilidades(MySqlConnectionToCd40);
            int Result = 0;
            string Cause = default(string);
            object sectorizacion = null;
            Exception exception = default(Exception);

            util.EventResultSectorizacion += new CD40.BD.SectorizacionEventHandler<CD40.BD.SactaInfo>((resinfo) =>
            {
                Result = (int)resinfo["Resultado"];
                Cause = resinfo.ContainsKey("ErrorCause") ? (string)resinfo["ErrorCause"] : null;
            });

            info["Version"] = Version;
            info["IdSistema"] = "departamento";
            info["SectName"] = "SACTA";
            info["SectData"] = dataSect;
            try
            {
                sectorizacion = util.GeneraSectorizacion(info, FechaActivacion);
            }
            catch (Exception x)
            {
                Result = 1;
                Cause = String.Format("Exception {0}", x.Message);
                exception = x;
            }
            return JsonConvert.SerializeObject(new
            {
                Executed = sectorizacion != null,
                FechaActivacion,
                Version,
                Result,
                Cause,
                DataIn = new
                {
                    Version,
                    dataSect
                },
                exception
            }); ;
        }

        //[WebMethod]
        //public string HelloWorld()
        //{
        //    var webConfiguracion = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
        //    var  cnx = webConfiguracion.ConnectionStrings.ConnectionStrings["ConexionBaseDatosCD40"].ToString();
        //    return "Hola a todos";
        //}
    }
}
