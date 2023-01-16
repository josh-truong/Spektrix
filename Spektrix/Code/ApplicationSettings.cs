using System;
using System.Configuration;

namespace Spektrix.Code
{
    public static class ApplicationSettingsManager
    {
        // ##############################################################################
        // Wraapper to ConfigurationManager for getting/settings app settings
        public static bool ContainsKey(string key)
        {
            return ConfigurationManager.AppSettings[key] != null;
        }

        public static void Set(string key, string value)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            KeyValueConfigurationCollection settings = configuration.AppSettings.Settings;
            if (!ContainsKey(key)) { settings.Add(key, value); }
            else { settings[key].Value = value; }
            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configuration.AppSettings.SectionInformation.Name);
        }

        public static string Get(string key, string value = "")
        {
            if (!ContainsKey(key)) { Set(key, value); }
            return ConfigurationManager.AppSettings[key];
        }


        // ##############################################################################
        // Extension methods to ApplicationSettingsBase for getting/settings user settings
        // Usage Examples
        // Set default icons for group tab
        //Properties.Settings.Default.Set($"T0", "hexagons.png");
        //string background = Properties.Settings.Default.Get(key + "Background", "");

        [Obsolete("ApplicationSettingsBase.ContainsKey does not work. Use Configuration.ContainsKey.", true)]
        public static bool ContainsKey(this ApplicationSettingsBase settings, string key)
        {
            return settings.Properties[key] != null;
        }

        [Obsolete("ApplicationSettingsBase.Set does not work. Use Configuration.Set.", true)]
        public static void Set<T>(this ApplicationSettingsBase settings, string key, T value)
        {
            if (!settings.ContainsKey(key))
            {
                // If settings does not exists
                SettingsProperty prop = new SettingsProperty(key)
                {
                    IsReadOnly = false,
                    PropertyType = typeof(T),
                    SerializeAs = SettingsSerializeAs.Xml,
                    Provider = settings.Providers["LocalFileSettingsProvider"],
                };
                prop.Attributes.Add(typeof(UserScopedSettingAttribute), new UserScopedSettingAttribute());
                settings.Properties.Add(prop);
                // Load Settings
                settings.Save();
            }
            // Set and save value
            settings.Properties[key].DefaultValue = value;
            settings.Save();
        }

        [Obsolete("ApplicationSettingsBase.Get does not work. Use Configuration.Get.", true)]
        public static T Get<T>(this ApplicationSettingsBase settings, string key, T value)
        {
            if (!settings.ContainsKey(key))
                settings.Set(key, value);
            return (T)settings.Properties[key].DefaultValue;
        }
        // ##############################################################################
    }
}