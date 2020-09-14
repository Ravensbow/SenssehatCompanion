using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = Microcharts.Entry;
using Microcharts.Forms;
using Microcharts;
using SkiaSharp;
using UltimateXF.Widget.Charts.Models;
using UltimateXF.Widget.Charts.Models.LineChart;
using UltimateXF.Widget.Charts.Models.Formatters;
using SenssehatCompanion.Services;
using SenssehatCompanion.Models;

namespace SenssehatCompanion.Views.PrzebiegiCzasoweTabItems
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TemperaturaPage : ContentPage
    {
        private System.Timers.Timer timer;
        private readonly Settings settings;
        private readonly IDataMeasure dataMeasure;
       

        List<string> labels = new List<string>();
        LineDataSetXF dataSet5;
        LineChartData data4;
        List<EntryChart> entries = new List<EntryChart>();
        private int time=0;
        private bool ploting = false;

        public TemperaturaPage()
        {
            InitializeComponent();
            settings = DependencyService.Get<IConfig>().GetConfig().Result;
            dataMeasure = DependencyService.Get<IDataMeasure>();
            setTimer();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            chart.AxisLeft.DrawGridLines = true;
            chart.AxisLeft.DrawAxisLine = true;
            chart.AxisLeft.Enabled = true;

            chart.AxisRight.DrawAxisLine = true;
            chart.AxisRight.DrawGridLines = true;
            chart.AxisRight.Enabled = true;

            chart.XAxis.XAXISPosition = XAXISPosition.BOTTOM;
            chart.XAxis.DrawGridLines = true;
            
        }


        private void setTimer()
        {
            timer = new System.Timers.Timer(settings.Interval);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = false;
            timer.Enabled = false;
        }
        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (!ploting)
                return;
            MeasureValues temp =  dataMeasure.GetMeasureAsync().Result;

            time += 1;
            if (temp != null)
                UpdateChart(temp);
            if (!ploting)
                return;
            timer.Enabled = true;
        }

        private void UpdateChart(MeasureValues temp)
        {
            float value = 0;
            switch(Title)
            {
                case "Temperatura" :
                    value = (float)temp.temperature;
                    break;
                case "Ciśnienie" :
                    value = (float)temp.pressure;
                    break;
                case "Wilgotność":
                    value = (float)temp.humidity;
                    break;
            }
            if (entries.Count > settings.NumSamples)
            {
                entries.RemoveRange(0,entries.Count-settings.NumSamples);
            }
                
            entries.Add(new EntryChart(time, value));
            dataSet5 = new LineDataSetXF(entries, Title)
            {
                Colors = new List<Color>{
                Color.Green
            },
                CircleHoleColor = Color.Blue,
                CircleColors = new List<Color>{
                Color.Blue
            },
                CircleRadius = 1,
                DrawValues = false,

            };

            data4 = new LineChartData(new List<ILineDataSetXF>() { dataSet5 });
            chart.ChartData = data4;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (ploting==true)
            {
                ploting = false;
                timer.Stop();
                timer.Dispose();
                (sender as Button).Text="Start";
            }
            
            else
            {
                ploting = true;
                setTimer();
                timer.Start();
                (sender as Button).Text = "Stop";
            } 
        }
    }
}