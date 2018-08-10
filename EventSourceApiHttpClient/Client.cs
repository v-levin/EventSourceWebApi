using Serilog.Core;
using System.IO;

namespace EventSourceApiHttpClient
{
    public static class Client 
    {
        private static readonly Logger log = EventSourceLogger.InitializeLogger();
        private static readonly string path = Path.GetFullPath(@"..\..\..\..\");
        private static readonly string appConfigPath = @"EventSourceApiHttpClient\App.config";
        private static ConfigFileAppSettingResolver _config;

        public static BaseHttpClient InitializeClient(string path)
        {
            var configFilePath = $"{path}\\bin\\EventSourceApiHttpClient.dll.config";

            _config = new ConfigFileAppSettingResolver(configFilePath);

            var baseUrl = _config.GetSetting("BaseUrl");
            var mediaType = _config.GetSetting("MediaType");
            var authenticationScheme = _config.GetSetting("AuthenticationScheme");
            var authenticationToken = _config.GetSetting("AuthenticationToken");
            var timeout = _config.GetSetting("Timeout");

            return new BaseHttpClient(baseUrl, mediaType, timeout, authenticationScheme, authenticationToken);
        }

    }
}
