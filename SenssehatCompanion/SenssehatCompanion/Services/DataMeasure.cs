using Newtonsoft.Json;
using SenssehatCompanion.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SenssehatCompanion.Services
{
    class DataMeasure : IDataMeasure
    {
        HttpClient client;
        private readonly IConfig config;

        public DataMeasure(IConfig conf)
        {
            config = conf;
            client = new HttpClient();
            client.BaseAddress = new Uri($"{config.GetURL()}/");
        }

        bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;

        public async Task<Temperatura> GetTemperatureAsync(string unit)
        {
            if (unit != null && IsConnected)
            {
                var json = await client.GetStringAsync($"api/measurement/GetTemperature/{unit}");
                return await Task.Run(() => JsonConvert.DeserializeObject<Temperatura>(json));
            }

            return null;
        }
    }
}
