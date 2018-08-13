namespace EventSourceApiHttpClient
{
    public static class Client 
    {
        private static ConfigFileAppSettingResolver _config;

        private const string BaseUrlSettingName = "BaseUrl";

        public static BaseHttpClient InitializeClient(string path)
        {
            var configFilePath = $"{path}\\bin\\EventSourceApiHttpClient.dll.config";

            _config = new ConfigFileAppSettingResolver(configFilePath);

            var baseUrl = _config.GetSetting(BaseUrlSettingName);
            var mediaType = _config.GetSetting("MediaType");
            var authenticationScheme = _config.GetSetting("AuthenticationScheme");
            var authenticationToken = _config.GetSetting("AuthenticationToken");
            var timeout = _config.GetSetting("Timeout");

            return new BaseHttpClient(baseUrl, mediaType, timeout, authenticationScheme, authenticationToken);
        }

    }
}
