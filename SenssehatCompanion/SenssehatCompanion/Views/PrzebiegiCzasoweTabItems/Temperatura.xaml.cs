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

namespace SenssehatCompanion.Views.PrzebiegiCzasoweTabItems
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Temperatura : ContentPage
    {
        public Temperatura()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var entries = new List<EntryChart>();
            var labels = new List<string>();

            var random = new Random();
            for (int i = 0; i < 10; i++)
            {
                entries.Add(new EntryChart(i, random.Next(10, 25)));
                labels.Add(i.ToString());
            }

            var dataSet5 = new LineDataSetXF(entries, "Temperatura")
            {
                Colors = new List<Color>{
                Color.Green
            },
                CircleHoleColor = Color.Blue,
                CircleColors = new List<Color>{
                Color.Blue
            },
                CircleRadius = 3,
                DrawValues = false,

            };
            
            var data4 = new LineChartData(new List<ILineDataSetXF>() { dataSet5 });

            chart.ChartData = data4;
            chart.AxisLeft.DrawGridLines = true;
            chart.AxisLeft.DrawAxisLine = true;
            chart.AxisLeft.Enabled = true;

            chart.AxisRight.DrawAxisLine = true;
            chart.AxisRight.DrawGridLines = true;
            chart.AxisRight.Enabled = true;

            chart.XAxis.XAXISPosition = XAXISPosition.BOTTOM;
            chart.XAxis.DrawGridLines = true;
            chart.XAxis.AxisValueFormatter = new TextByIndexXAxisFormatter(labels);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}