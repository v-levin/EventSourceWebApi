using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace EventSourceApiHttpClient
{
    public static class Client 
    {
        private static Logger log = new EventSourceLogger().InitializeLogger();

        public static BaseHttpClient InitializeClient()
        {
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            var mediaType = ConfigurationManager.AppSettings["MediaType"];
            var authenticationScheme = ConfigurationManager.AppSettings["AuthenticationScheme"];
            var authenticationToken = ConfigurationManager.AppSettings["AuthenticationToken"];
            var timeout = ConfigurationManager.AppSettings["Timeout"];

            if (string.IsNullOrEmpty(baseUrl))
                log.Information("The Url configuration is missing.");

            if (string.IsNullOrEmpty(mediaType))
                log.Information("The MediaType configuration is missing.");

            if (string.IsNullOrEmpty(authenticationScheme))
                log.Information("The AuthenticationScheme configuration is missing.");

            if (string.IsNullOrEmpty(authenticationToken))
                log.Information("The AuthenticationToken configuration is missing.");

            if (string.IsNullOrEmpty(timeout))
                log.Information("The timeout configuration is missing.");

            return new BaseHttpClient(baseUrl, mediaType, timeout, authenticationScheme, authenticationToken);
        }

    }
}
