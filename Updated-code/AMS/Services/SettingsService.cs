using System;
using System.IO;
using System.Xml.Serialization;
using AMS.Models;

namespace AMS.Services
{
    public class SettingsService
    {
        private static SettingsService _instance;
        public static SettingsService Instance => _instance ?? (_instance = new SettingsService());

        private static readonly string SettingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "AMS", "settings.xml");

        private CompanySettings _settings;
        public CompanySettings Settings
        {
            get => _settings ?? (_settings = Load());
            private set => _settings = value;
        }

        public string CompanyName => Settings.CompanyName;
        public double ExchangeRate
        {
            get => Settings.DefaultExchangeRate;
            set { Settings.DefaultExchangeRate = value; Save(); }
        }
        public string LastDatabasePath { get; set; }

        private SettingsService() { }

        private CompanySettings Load()
        {
            try
            {
                if (!File.Exists(SettingsPath)) return new CompanySettings();
                var xs = new XmlSerializer(typeof(CompanySettings));
                using (var fs = new FileStream(SettingsPath, FileMode.Open))
                    return (CompanySettings)xs.Deserialize(fs);
            }
            catch { return new CompanySettings(); }
        }

        public void Save()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath));
                var xs = new XmlSerializer(typeof(CompanySettings));
                using (var fs = new FileStream(SettingsPath, FileMode.Create))
                    xs.Serialize(fs, Settings);
            }
            catch { }
        }

        public void UpdateSettings(CompanySettings updated)
        {
            Settings = updated;
            Save();
        }
    }
}
