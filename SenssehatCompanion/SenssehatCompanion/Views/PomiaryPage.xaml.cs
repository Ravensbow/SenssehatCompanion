using SenssehatCompanion.Models;
using SenssehatCompanion.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SenssehatCompanion.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PomiaryPage : ContentPage
    {
        private MeasureValues _temp;
        private CancellationTokenSource source;
        private CancellationToken cts;
        public MeasureValues Temp { 
            get {
                return _temp;
            } set    {
                _temp = value;
                OnPropertyChanged(nameof(MeasureValues));
            } }
        public PomiaryPage()
        {
            InitializeComponent();

            // string s = Connect("192.168.1.120", "t\r");
            //s = s.Substring(0, s.Length - 1);
        }
        protected override async void OnAppearing()
        {
            source = new CancellationTokenSource();
            cts = source.Token;
            await GetData();
            
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            source.Cancel();
            base.OnDisappearing();
        }
        public async Task GetData()
        {
            while (true)
            {
                if (cts.IsCancellationRequested)
                    return;
                Temp = await DependencyService.Get<DataMeasure>(DependencyFetchTarget.NewInstance).GetMeasureAsync();
                BindingContext = Temp;
                await Task.Delay(1000); 
            }
        }
        public void clik(object sender, EventArgs e) { Temp = new MeasureValues { temperature = 12 }; }
        static string Connect(String server, String message)
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