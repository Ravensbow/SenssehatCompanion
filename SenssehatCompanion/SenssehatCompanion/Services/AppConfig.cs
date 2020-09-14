using SenssehatCompanion.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SenssehatCompanion.Services
{
    class AppConfig : IConfig
    {
        public Settings Settings { get; set; }

        public AppConfig()
        {
            Settings = new Settings();
            Settings.ServerIP = "192.168.0.116";
            Settings.Port = 80;
            Settings.NumSamples = 10;
            Settings.Interval = 100;
        }
        public async Task<bool> ChangeConfig(Settings s)
        {
            Settings = s;
            return await Task.FromResult(true);
        }

        public async Task<Settings> GetConfig()
        {
            return await Task.FromResult(Settings);
        }

        public string GetURL()
        {
            return Settings.ServerIP+":"+Settings.Port;
        }
    }
}
