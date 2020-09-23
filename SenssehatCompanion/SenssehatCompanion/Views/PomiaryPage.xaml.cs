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
        private List<MeasureValues> _temp;
        private CancellationTokenSource source;
        private CancellationToken cts;
        private Dictionary<string, Label> value_labels = new Dictionary<string, Label>();
        private Dictionary<string, Label> datelabel_labels = new Dictionary<string, Label>();
        public List<MeasureValues> Temp { 
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
                if (Temp == null)
                    continue;
                foreach(var v in Temp)
                {
                    if(!MeasureStack.Children.Any(c=>c.ClassId==v.Name))
                    {
                        MeasureStack.Children.Add(SetMeasureView(v));
                    }
                    else
                    {
                        if(value_labels.ContainsKey(v.Name))
                            value_labels[v.Name].Text = v.Value.ToString() + " " + v.Unit;
                        if (datelabel_labels.ContainsKey(v.Name))
                            datelabel_labels[v.Name].Text = DateTime.Now.ToString("HH:mm:ss dd.MM.y");
                    }

                }
                await Task.Delay(1000); 
            }
        }
        private StackLayout SetMeasureView(MeasureValues v)
        {
            var S = new StackLayout() { ClassId=v.Name};

            var s = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                ClassId = v.Name
            };
            s.Children.Add(new Label()
            {
                Text = v.Name,
                TextColor= Color.Black,
                FontSize= 20
            });
            var dtlabel = new Label()
            {
                Text = DateTime.Now.ToString("HH:mm:ss dd.MM.y"),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.EndAndExpand
            };
            s.Children.Add(dtlabel);
            var vlabel = new Label()
            {
                Text = v.Value.ToString() + " " + v.Unit,
                TextColor = Color.Black,
                FontSize = 30,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(0, 0, 20, 0)
            };
            if(!value_labels.ContainsKey(v.Name))
                value_labels.Add(v.Name, vlabel);
            if (!datelabel_labels.ContainsKey(v.Name))
                datelabel_labels.Add(v.Name, dtlabel);
            S.Children.Add(s);
            S.Children.Add(vlabel);

            return S;
        }
    }
}