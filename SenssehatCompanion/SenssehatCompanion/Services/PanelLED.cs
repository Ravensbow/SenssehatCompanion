
using Newtonsoft.Json;
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
    class PanelLED : IPanelLED
    {
        public int[] LEDs { get; set; } = new int[64];

        HttpClient client;
        private IConfig config;

        bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;

        public PanelLED()
        {
            config = DependencyService.Get<IConfig>();
            client = new HttpClient();
            client.BaseAddress = new Uri($"{"http://" + config.GetURL()}/");
        }

        public bool DrawSymbol()
        {
            throw new NotImplementedException();
        }

        public async Task<int[]> GetLEDsState()
        {
            if (IsConnected)
            {
                try
                {
                    var json = await client.GetStringAsync($"api/LED/GetLEDs");
                    return await Task.Run(() => JsonConvert.DeserializeObject<int[]>(json));
                }
                catch (WebException e)
                {
                    DependencyService.Get<IMessage>().LongAlert(e.Message);
                }
                catch (JsonException e)
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

        public async Task<bool> SetLEDs(int[] leds)
        {
            if (IsConnected)
            {
                try
                {
                    var json = await client.GetStringAsync($"api/LED/SetLEDs?data={Newtonsoft.Json.JsonConvert.SerializeObject(leds)}");
                    return await Task.Run(() => JsonConvert.DeserializeObject<bool>(json));
                }
                catch (WebException e)
                {
                    DependencyService.Get<IMessage>().LongAlert(e.Message);
                }
                catch (JsonException e)
                {
                    DependencyService.Get<IMessage>().LongAlert(e.Message);
                }
                catch
                {
                    DependencyService.Get<IMessage>().LongAlert("Nieznany wyjątek!");
                }

            }

            return false;
        }
    }
}
