﻿using Newtonsoft.Json;
using SenssehatCompanion.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
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

        public Temperatura GetTemperatureTCP(string unit)
        {
            string s = ConnectTCP("192.168.1.120", "t\r");
            var Temp = new Temperatura() { Unit = 'C', Value = Convert.ToDouble(s, new NumberFormatInfo() { NumberDecimalSeparator = "." }) };
            return Temp;
        }

        static string ConnectTCP(String server, String message)
        {
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                Int32 port = 11000;
                TcpClient client = new TcpClient(server, port);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent: {0}", message);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                // Close everything.
                stream.Close();
                client.Close();
                return responseData;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            return "error";
        }
    }
}
