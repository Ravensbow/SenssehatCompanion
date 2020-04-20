using Newtonsoft.Json;
using SenssehatCompanion.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SenssehatCompanion.Services
{
    class DataMeasure : IDataMeasure
    {
        HttpClient client;
        private IConfig config;

        public DataMeasure()
        {
            config = DependencyService.Get<IConfig>();
            client = new HttpClient();
            client.BaseAddress = new Uri($"{"http://"+config.GetURL()}/");
        }

        bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;

        public async Task<Temperatura> GetTemperatureAsync(string unit)
        {
            if (unit != null && IsConnected)
            {
                try
                {
                    var json = await client.GetStringAsync($"api/measurement/GetTemperature/{unit}");
                    return await Task.Run(() => JsonConvert.DeserializeObject<Temperatura>(json));
                }
                catch(WebException e)
                {
                    DependencyService.Get<IMessage>().LongAlert(e.Message);
                }
                catch(JsonException e)
                {
                    DependencyService.Get<IMessage>().LongAlert(e.Message);
                }
                catch
                {
                    DependencyService.Get<IMessage>().LongAlert("Nieznany wyjątek!");
                }
                
            }

            return null;
        }

        public Temperatura GetTemperaturaFake(string unit)
        {
            Random random = new Random();
            return new Temperatura() { Value = random.Next(10, 25), Unit = unit[0] };
        }
    }
}
