
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
            client.Timeout = TimeSpan.FromSeconds(3);
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
                    var json =  await client.GetStringAsync($"api/led.php");
                    int[] data = await Task.Run(() => JsonConvert.DeserializeObject<int[]>(json));
                    return data;
                }
                catch (WebException e)
                {
                    DependencyService.Get<IMessage>().LongAlert(e.Message);
                }
                catch (JsonException e)
                {
                    DependencyService.Get<IMessage>().LongAlert(e.Message);
                }
                catch (HttpRequestException e)
                {
                    DependencyService.Get<IMessage>().LongAlert("Przekroczono limit połączenia!");
                }
                catch (Exception e)
                {
                    DependencyService.Get<IMessage>().LongAlert("Nieznany wyjątek!"+e.Message);
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
                    //var json = await client.PostAsync($"api/led.php",new StringContent("leds="+Newtonsoft.Json.JsonConvert.SerializeObject(leds)));
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(leds);
                    var resoult = await client.PostAsync($"api/led.php", new FormUrlEncodedContent( 
                        new Dictionary<string, string>()
                        {
                            {"leds",json}
                        }
                    ));
                    if(!resoult.IsSuccessStatusCode)
                        DependencyService.Get<IMessage>().LongAlert("Nie udało się wysłać panelu led.");
                    return await Task.Run(() => JsonConvert.DeserializeObject<bool>("true"));
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
