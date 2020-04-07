using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;


namespace UtilitiesCD40
{
    public static class GeneraIncidencias
    {
        //const string OID_TRAP_GENERA_INCIDENCIA = "1.1.600.1.";
        //const string IP_TRAP_GENERA_INCIDENCIA = "192.168.0.111";

        static string _IpTo;

        public static void StartSnmp(string ip, string toIp)
        {
            _IpTo = toIp;

            SnmpAgent.Init(ip);
            SnmpAgent.Start();
        }

        public static void EndSnmp()
        {
            SnmpAgent.Close();
        }

        public static void SendTrap(string ipTo, string idIncidencia, string value)
        {
            string newOid = SnmpAgent.OID_TRAP_GENERA_INCIDENCIA + idIncidencia;

            SnmpClient.TrapTo(ipTo, "public", newOid, value);
        }
    }
}
