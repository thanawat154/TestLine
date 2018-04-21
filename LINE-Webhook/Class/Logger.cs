using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LINE_Webhook.Logging
{
    public static class Logger
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void LogError(string errorMessage)
        {
            log.Error(errorMessage);
        }

        public static void LogWarning(string errorMessage)
        {
            log.Warn(errorMessage);
        }

    }
}