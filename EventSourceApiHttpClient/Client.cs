using Serilog.Core;
using System.IO;

namespace EventSourceApiHttpClient
{
    public static class Client 
    {
        private static readonly Logger log = EventSourceLogger.InitializeLogger();
        private static readonly string path = Path.GetFullPath(@"..\..\..\..\");
        private static readonly string appConfigPath = @"EventSourceApiHttpClient\App.config";
        private static readonly ConfigFileAppSettingResolver config = new ConfigFileAppSettingResolver($@"{path}\{appConfigPath}");

        public static BaseHttpClient InitializeClient()
        {
            var baseUrl = config.GetSetting("BaseUrl");
            var mediaType = config.GetSetting("MediaType");
            var authenticationScheme = config.GetSetting("AuthenticationScheme");
            var authenticationToken = config.GetSetting("AuthenticationToken");
            var timeout = config.GetSetting("Timeout");

            return new BaseHttpClient(baseUrl, mediaType, timeout, authenticationScheme, authenticationToken);
        }

    }
}
