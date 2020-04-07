using System;
using System.Text;
using System.Globalization;
using System.Collections.Generic;

using NLog;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Pipeline;
using Lextm.SharpSnmpLib.Messaging;

namespace UtilitiesCD40
{
    class SnmpLogger : Lextm.SharpSnmpLib.Pipeline.ILogger
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private const string Empty = "-";

        public SnmpLogger()
        {
        }

        public void Log(ISnmpContext context)
        {
            if (_logger.IsInfoEnabled)
            {
                _logger.Info(GetLogEntry(context));
            }
        }

        private static string GetLogEntry(ISnmpContext context)
        {
            return string.Format(
                 CultureInfo.InvariantCulture,
                 "{0} {1} {2} {3} {4} {5} {6} {7} {8} {9}",
                 DateTime.UtcNow,
                 context.Binding.Endpoint.Address,
                 context.Request.TypeCode() == SnmpType.Unknown ? Empty : context.Request.TypeCode().ToString(),
                 GetStem(context.Request.Pdu().Variables),
                 context.Binding.Endpoint.Port,
                 context.Request.Parameters.UserName,
                 context.Sender.Address,
                 (context.Response == null) ? Empty : context.Response.Pdu().ErrorStatus.ToErrorCode().ToString(),
                 context.Request.Version,
                 DateTime.Now.Subtract(context.CreatedTime).TotalMilliseconds);
        }

        private static string GetStem(ICollection<Variable> variables)
        {
            if (variables.Count == 0)
            {
                return Empty;
            }

            StringBuilder result = new StringBuilder();
            foreach (Variable v in variables)
            {
                result.AppendFormat("{0};", v.Id);
            }

            if (result.Length > 0)
            {
                result.Length--;
            }

            return result.ToString();
        }
    }
}

