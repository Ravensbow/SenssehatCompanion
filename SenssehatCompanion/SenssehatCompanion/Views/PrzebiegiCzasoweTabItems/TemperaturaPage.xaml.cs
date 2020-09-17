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
using System.Threading;

namespace SenssehatCompanion.Views.PrzebiegiCzasoweTabItems
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TemperaturaPage : ContentPage
    {
        private System.Timers.Timer timer;
        private readonly Settings settings;
        private readonly IDataMeasure dataMeasure;
        private Thread updateChartThread;
        private CancellationTokenSource source;
        private CancellationToken cts;

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
            dataMeasure = DependencyService.Get<DataMeasure>(DependencyFetchTarget.NewInstance);
            setTimer();
        }

        protected override async void OnAppearing()
        {
            source = new CancellationTokenSource();
            cts = source.Token;
            
            chart.AxisLeft.DrawGridLines = true;
            chart.AxisLeft.DrawAxisLine = true;
            chart.AxisLeft.Enabled = true;
            chart.MinimumHeightRequest = 400;
            chart.AutoScaleMinMaxEnabled = false;
            chart.AxisLeft.AxisMinimum = -35;
            chart.AxisLeft.AxisMaximum= 110;
            chart.AxisRight.Enabled = false;
            chart.AxisRight.DrawAxisLine = true;
            chart.AxisRight.DrawGridLines = true;
            chart.AxisRight.Enabled = true;
            chart.XAxis.XAXISPosition = XAXISPosition.BOTTOM;
            chart.XAxis.DrawGridLines = true;
            await UpdateChartAsync();

            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            source.Cancel();
            base.OnDisappearing();
        }

        private async Task UpdateChartAsync()
        {
            updateChartThread = Thread.CurrentThread;
            while(true)
            {
                if (cts.IsCancellationRequested)
                    return;
                if (!ploting)
                {
                    await Task.Delay(settings.Interval);
                    continue;
                }

                var measureData = await dataMeasure.GetMeasureAsync();
                if (measureData == null)
                    continue;

                var md = measureData.Find(v => v.Name == Title);
                if(md!=null)
                    UpdateChart(md);

                await Task.Delay(settings.Interval);
            }
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
            MeasureValues temp =  dataMeasure.GetMeasureAsync().Result.Find(mv=> mv.Name==Title);

            if (temp != null)
                UpdateChart(temp);
            if (!ploting)
                return;
            timer.Enabled = true;
        }

        private void UpdateChart(MeasureValues temp)
        {
            float value = 0;
            time += 1;
            value = (float)temp.Value;
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
            if(ploting)
            {
                ploting = false;
                (sender as Button).Text = "Start";
            }
            else
            {
                ploting = true;
                (sender as Button).Text = "Stop";
            }
            //if (ploting==true)
            //{
            //    ploting = false;
            //    timer.Stop();
            //    timer.Dispose();
            //    (sender as Button).Text="Start";
            //}
            
            //else
            //{
            //    ploting = true;
            //    setTimer();
            //    timer.Start();
            //    (sender as Button).Text = "Stop";
            //} 
        }
    }
}