using EventSourceApiHttpClient.Interfaces;
using System.Configuration;

namespace EventSourceApiHttpClient
{
    public class ConfigFileAppSettingResolver : IAppSettingResolver
    {
        private readonly System.Configuration.Configuration _config;

        public ConfigFileAppSettingResolver(string path)
        {
            var map = new ExeConfigurationFileMap() { ExeConfigFilename = path };
            _config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
        }

        public string GetSetting(string name)
        {
            return _config.AppSettings.Settings[name] != null ? _config.AppSettings.Settings[name].Value : null;
        }
    }
}
