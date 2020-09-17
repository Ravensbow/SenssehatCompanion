using SenssehatCompanion.Models;
using SenssehatCompanion.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UltimateXF.Widget.Charts.Models;
using UltimateXF.Widget.Charts.Models.LineChart;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SenssehatCompanion.Views.PrzebiegiCzasoweTabItems
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Katy : ContentPage
    {
        private CancellationTokenSource source;
        private CancellationToken cts;
        private readonly Settings settings;
        private readonly IDataMeasure dataMeasure;


        LineDataSetXF dataSet;
        LineChartData data;
        List<EntryChart> entries = new List<EntryChart>();

        public Katy()
        {
            dataMeasure = DependencyService.Get<IDataMeasure>();
            settings = DependencyService.Get<IConfig>().GetConfig().Result;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            await UpdateJoystickAsync();
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            source.Cancel();
            base.OnDisappearing();
        }

        private async Task UpdateJoystickAsync()
        {
            while (true)
            {
                if (cts.IsCancellationRequested)
                    return;

                var measureData = await dataMeasure.GetMeasureAsync();
                if (measureData == null)
                    continue;

                var ys = measureData.Where(v => v.Name == "jx"||v.Name=="jy").OrderBy(v=>v.Name).ToArray();
                if (ys != null&&ys.Length==2)
                    Update(ys);
                await Task.Delay(100);
            }
        }

        private void Update(MeasureValues[] ys)
        {
            float value = (float)ys[0].Value;
            float time = (float)ys[1].Value;
         
            if (entries.Count > 1)
            {
                entries.RemoveRange(0, 1);
            }

            entries.Add(new EntryChart(time, value));
            dataSet = new LineDataSetXF(entries, Title)
            {
                Colors = new List<Color>{
                Color.Green
            },
                CircleHoleColor = Color.Blue,
                CircleColors = new List<Color>{
                Color.Blue
            },
                CircleRadius = 4,
                DrawValues = false,
            };

            data = new LineChartData(new List<ILineDataSetXF>() { dataSet });
            //chart.ChartData = data;
        }
    }
}