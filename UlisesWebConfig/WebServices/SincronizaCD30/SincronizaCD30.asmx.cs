using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.Configuration;
using System.Configuration;
using SincroCD30;


namespace UlisesWebConfig.WebServices.SincronizaCD30
{
    /// <summary>
    /// Descripción breve de SincronizaCD30
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class SincronizaCD30 : System.Web.Services.WebService
    {

		//[WebMethod]
		//public string HelloWorld()
		//{
		//    return "Hola a todos";
		//}
		[WebMethod]
		public int AltaOperador(string idOperador, int nivelAcceso, string clave, string nombre, string apellidos, string telefono)
		{
			CD30BD s = new CD30BD();
			int retorno = s.AltaOperador(idOperador.Substring(0, idOperador.Length > 8 ? 8 : idOperador.Length), nivelAcceso, clave, nombre, apellidos, telefono);
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int BajaOperador(string idOperador)
		{
			CD30BD s = new CD30BD();
			int retorno = s.BajaOperador(idOperador.Substring(0, idOperador.Length > 8 ? 8 : idOperador.Length));
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int ModificaOperador(string idOperador, int nivelAcceso, string clave, string nombre, string apellidos, string telefono)
		{
			CD30BD s = new CD30BD();
			int retorno = s.ModificaOperador(idOperador.Substring(0, idOperador.Length > 8 ? 8 : idOperador.Length), nivelAcceso, clave, nombre, apellidos, telefono);
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int AsignarSectoresUCS(string idSectorizacion, List<string> sectores, int posDest)
		{
			CD30BD s = new CD30BD();
			int retorno = s.AsignarSectoresUCS(idSectorizacion.Substring(0, idSectorizacion.Length > 8 ? 8 : idSectorizacion.Length), sectores, posDest);
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int IntercambiarUCS(string idSectorizacion, List<string> origen, int topOrigen, List<string> destino, int topDestino)
		{
			CD30BD s = new CD30BD();
			int retorno = s.IntercambiarUCS(idSectorizacion.Substring(0, idSectorizacion.Length > 8 ? 8 : idSectorizacion.Length), origen, topOrigen, destino, topDestino);
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int LiberarUCS(string idSectorizacion, int pos)
		{
			CD30BD s = new CD30BD();
			int retorno = s.LiberarUCS(idSectorizacion.Substring(0, idSectorizacion.Length > 8 ? 8 : idSectorizacion.Length), pos);
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public void LiberarSectorUCS(string idSectorizacion, string sector)
		{
			Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
			KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
			if (Int32.Parse(sincronizar.Value) == 1)
			{
				CD30BD s = new CD30BD();
				s.LiberarSectorUCS(idSectorizacion.Substring(0, idSectorizacion.Length > 8 ? 8 : idSectorizacion.Length), sector);
				s.CloseDB();
			}
		}

		[WebMethod]
		public int MoverSectores(string idSectorizacion, List<string> sectoresOrigen, int posOrigen, List<string> sectoresDestino, int posDest)
		{
			CD30BD s = new CD30BD();
			int retorno = s.MoverSectores(idSectorizacion.Substring(0, idSectorizacion.Length > 8 ? 8 : idSectorizacion.Length), sectoresOrigen, posOrigen, sectoresDestino, posDest);
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int AltaAgrupacion(string idAgrupacion, List<string> sectores)
		{
			CD30BD s = new CD30BD();
			int retorno = s.AltaAgrupacion(idAgrupacion.Substring(0, idAgrupacion.Length > 16 ? 16 : idAgrupacion.Length), sectores);
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int BajaAgrupacion(string idAgrupacion)
		{
			CD30BD s = new CD30BD();
			int retorno = s.BajaAgrupacion(idAgrupacion.Substring(0, idAgrupacion.Length > 16 ? 16 : idAgrupacion.Length));
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int AltaUsuario(string idNucleo, string idSector, string tipoSector, int numSacta, int prioR2, List<string> numPub, List<string> numATS)
		{
			CD30BD s = new CD30BD();
			int retorno = s.AltaUsuario(idNucleo, idSector.Substring(0, idSector.Length > 8 ? 8 : idSector.Length), tipoSector, numSacta, prioR2, numPub, numATS);
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int ModificacionUsuario(string idNucleo, string idSector, string tipoSector, int numSacta, int prioR2, List<string> numPub, List<string> numATS)
		{
			CD30BD s = new CD30BD();
			int retorno = s.ModificacionUsuario(idNucleo, idSector.Substring(0, idSector.Length > 8 ? 8 : idSector.Length), tipoSector, numSacta, prioR2, numPub, numATS);
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int BajaUsuario(string idNucleo, string idSector)
		{
			CD30BD s = new CD30BD();
			int retorno = s.BajaUsuario(idNucleo, idSector.Substring(0, idSector.Length > 8 ? 8 : idSector.Length));
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int AltaEmplazamiento(string id)
		{
			CD30BD s = new CD30BD();
			int retorno = s.AltaEmplazamiento(id.Substring(0, id.Length > 8 ? 8 : id.Length));
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int BajaEmplazamiento(string id)
		{
			CD30BD s = new CD30BD();
			int retorno = s.BajaEmplazamiento(id.Substring(0, id.Length > 8 ? 8 : id.Length));
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int AltaNucleo(string idNucleo)
		{
			CD30BD s = new CD30BD();
			int retorno = s.AltaNucleo(idNucleo.Substring(0, idNucleo.Length > 8 ? 8 : idNucleo.Length));
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int BajaNucleo(string idNucleo)
		{
			CD30BD s = new CD30BD();
			int retorno = s.BajaNucleo(idNucleo.Substring(0, idNucleo.Length > 8 ? 8 : idNucleo.Length));
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int AltaSectorizacion(string idSect)
		{
			CD30BD s = new CD30BD();
			int retorno = s.AltaSectorizacion(idSect.Substring(0, idSect.Length > 8 ? 8 : idSect.Length));
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int BajaSectorizacion(string idSect)
		{
			CD30BD s = new CD30BD();
			int retorno = s.BajaSectorizacion(idSect.Substring(0, idSect.Length > 8 ? 8 : idSect.Length));
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int AltaTroncal(string id, string numTest)
		{
			CD30BD s = new CD30BD();
			int retorno = s.AltaTroncal(id.Substring(0, id.Length > 8 ? 8 : id.Length), numTest);
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int BajaTroncal(string id)
		{
			CD30BD s = new CD30BD();
			int retorno = s.BajaTroncal(id.Substring(0, id.Length > 8 ? 8 : id.Length));
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int ModificacionTroncal(string id, string numTest)
		{
			CD30BD s = new CD30BD();
			int retorno = s.ModificacionTroncal(id.Substring(0, id.Length > 8 ? 8 : id.Length), numTest);
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int AltaRed(int prefijo, string id)
		{
			CD30BD s = new CD30BD();
			int retorno = s.AltaRed(prefijo, id.Substring(0, id.Length > 25 ? 25 : id.Length));
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int BajaRed(int prefijo, string id)
		{
			CD30BD s = new CD30BD();
			int retorno = s.BajaRed(prefijo, id.Substring(0, id.Length > 25 ? 25 : id.Length));
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int AltaCanalRadio(string id, int exclusividad, char modo, char tipoPTT, char tipoSquelch, int maxPTT)
		{
			CD30BD s = new CD30BD();
			int retorno = s.AltaCanalRadio(id.Substring(0, id.Length > 8 ? 8 : id.Length), exclusividad, modo, tipoPTT, tipoSquelch, maxPTT);
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int ModificacionCanalRadio(string id, int exclusividad, char modo, char tipoPTT, char tipoSquelch, int maxPTT)
		{
			CD30BD s = new CD30BD();
			int retorno = s.ModificacionCanalRadio(id.Substring(0, id.Length > 8 ? 8 : id.Length), exclusividad, modo, tipoPTT, tipoSquelch, maxPTT);
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int BajaCanalRadio(string id)
		{
			CD30BD s = new CD30BD();
			int retorno = s.BajaCanalRadio(id.Substring(0, id.Length > 8 ? 8 : id.Length));
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int AltaFrecuencia(string emplazamiento, string idFrecuencia, int GrupoBSS, string idCanalRad)
		{
			CD30BD s = new CD30BD();
			int retorno = s.AltaFrecuencia(emplazamiento.Substring(0, emplazamiento.Length > 8 ? 8 : emplazamiento.Length), idFrecuencia.Substring(0, idFrecuencia.Length > 10 ? 10 : idFrecuencia.Length), GrupoBSS, idCanalRad);
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int ModificacionFrecuencia(string idFrecuencia, string idEmplazamiento, string idCanal, int GrupoBSS)
		{
			CD30BD s = new CD30BD();
			int retorno = s.ModificacionFrecuencia(idFrecuencia.Substring(0, idFrecuencia.Length > 10 ? 10 : idFrecuencia.Length), idEmplazamiento.Substring(0, idEmplazamiento.Length > 8 ? 8 : idEmplazamiento.Length), idCanal, GrupoBSS);
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int BajaFrecuencia(string idFrecuencia, bool eliminarColateralesRD)
		{
			CD30BD s = new CD30BD();
			int retorno = s.BajaFrecuencia(idFrecuencia.Substring(0, idFrecuencia.Length > 8 ? 8 : idFrecuencia.Length), eliminarColateralesRD);
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int AltaLineaTelefonia(int tipoRecurso, string id, int tipo, int tipoLinTroncal, int lado, int acceso, string idTroncal)
		{
			CD30BD s = new CD30BD();
			int retorno = s.AltaLineaTelefonia(tipoRecurso, id.Substring(0, id.Length > 8 ? 8 : id.Length), tipo, tipoLinTroncal, lado, acceso, idTroncal.Substring(0, idTroncal.Length > 8 ? 8 : idTroncal.Length));
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int ModificacionLineaTelefonia(int tipoRecurso, string id, int tipo, int tipoLinTroncal, int lado, int acceso, string idTroncal)
		{
			CD30BD s = new CD30BD();
			int retorno = s.ModificacionLineaTelefonia(tipoRecurso, id.Substring(0, id.Length > 8 ? 8 : id.Length), tipo, tipoLinTroncal, lado, acceso, idTroncal.Substring(0, idTroncal.Length > 8 ? 8 : idTroncal.Length));
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int BajaLineaTelefonia(string id, int tipo, int tipoLinTroncal, string idTroncal)
		{
			CD30BD s = new CD30BD();
			int retorno = s.BajaLineaTelefonia(id.Substring(0, id.Length > 8 ? 8 : id.Length), tipo, tipoLinTroncal, idTroncal.Substring(0, idTroncal.Length > 8 ? 8 : idTroncal.Length));
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int AltaDestino(string idDestino, int prefijoRed, string idRecurso, int grupo, string numero)
		{
			CD30BD s = new CD30BD();
			int retorno = s.AltaDestino(idDestino.Substring(0, idDestino.Length > 8 ? 8 : idDestino.Length), prefijoRed, idRecurso.Length > 0 ? (idRecurso.Substring(0, idRecurso.Length > 8 ? 8 : idRecurso.Length)) : "", grupo, numero);
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int ModificacionDestino(string idDestino, int prefijoRed, string idRecurso, int grupo, string numero)
		{
			CD30BD s = new CD30BD();
			int retorno = s.ModificacionDestino(idDestino.Substring(0, idDestino.Length > 8 ? 8 : idDestino.Length), prefijoRed, idRecurso.Length > 0 ? (idRecurso.Substring(0, idRecurso.Length > 8 ? 8 : idRecurso.Length)) : "", grupo, numero);
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int BajaDestino(string idDestino, int prefijoRed)
		{
			CD30BD s = new CD30BD();
			int retorno = s.BajaDestino(idDestino.Substring(0, idDestino.Length > 8 ? 8 : idDestino.Length), prefijoRed);
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int AltaCentralPropia(string idEncaminamiento, string numTest, bool ts)
		{
			CD30BD s = new CD30BD();
			int retorno = s.AltaCentralPropia(idEncaminamiento, numTest, ts);
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int AltaRangoR2(int tipo, string desde, string hasta)
		{
			CD30BD s = new CD30BD();
			int retorno = s.AltaRangoR2(tipo, desde, hasta);
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int AltaDestinoR2(string idDestinoR2, int tipo, string desde, string hasta)
		{
			CD30BD s = new CD30BD();
			int retorno = s.AltaDestinoR2(idDestinoR2.Substring(0, idDestinoR2.Length > 8 ? 8 : idDestinoR2.Length), tipo, desde, hasta);
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int AltaRuta(string idRuta, bool directa, int numero, string idRecurso)
		{
			CD30BD s = new CD30BD();
			int retorno = s.AltaRuta(idRuta.Substring(0, idRuta.Length > 8 ? 8 : idRuta.Length), directa, numero, idRecurso);
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int BajaEncaminamiento(string id)
		{
			CD30BD s = new CD30BD();
			int retorno = s.BajaEncaminamiento(id.Substring(0, id.Length > 8 ? 8 : id.Length));
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int AltaColateralRadio(string idNucleo, string idSector, uint posHMI, string literal, string idDestino, string idEmplazamiento)
		{
			CD30BD s = new CD30BD();
			int retorno = s.AltaColateralRadio(idNucleo, idSector, posHMI, literal, idDestino, idEmplazamiento);
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int ModificaColateralRadio(string idNucleo, string idSector, uint posHMI, string idDestino, string idEmplazamiento)
		{
			CD30BD s = new CD30BD();
			int retorno = s.ModificaColateralRadio(idNucleo, idSector, posHMI, idDestino, idEmplazamiento);
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int BajaColateralRadio(string idNucleo, uint posHMI, string idSector)
		{
			CD30BD s = new CD30BD();
			int retorno = s.BajaColateralRadio(idNucleo, posHMI, idSector);
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int AltaColateralTelefonia(string idNucleo, string idSector, uint posHMI, string literal, string idDestino, uint idPrefijo, string origenR2, uint prioridad)
		{
			CD30BD s = new CD30BD();
			int retorno = s.AltaColateralTelefonia(idNucleo, idSector, posHMI, literal, idDestino, idPrefijo, origenR2, prioridad);
			s.CloseDB();
			return retorno;
		}

		[WebMethod]
		public int ModificaColateralTelefonia(string idNucleo, string idSector, uint posHMI, string literal, uint prioridad)
		{
			CD30BD s = new CD30BD();
			int retorno = s.ModificaColateralTelefonia(idNucleo, idSector, posHMI, literal, prioridad);
			s.CloseDB();
			return retorno;
		}
		[WebMethod]
		public int BajaColateralTelefonia(string idNucleo, string idSector, uint posHMI)
		{
			CD30BD s = new CD30BD();
			int retorno = s.BajaColateralTelefonia(idNucleo, idSector, posHMI);
			s.CloseDB();
			return retorno;
		}
	}
}
