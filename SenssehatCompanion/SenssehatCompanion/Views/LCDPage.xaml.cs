using App.Controls.Behaviors;
using SenssehatCompanion.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SenssehatCompanion.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LCDPage : ContentPage
    {
        List<Button> chosenLeds = new List<Button>();
        Dictionary<Tuple<int, int>, Button> panel = new Dictionary<Tuple<int, int>, Button>();
        public string Gowno = "elo";
        public LCDPage()
        {
            InitializeComponent();
            //for(int i =0;i<8;i++)
            //{
            //    var row = new FlexLayout() { Padding = new Thickness(10,0,10,0) };
            //    for(int j=0;j<8;j++)
            //    {
            //        var but = new Button()
            //        {
            //            BackgroundColor = Color.Black,
            //            Margin = new Thickness(5, 0, 0, 0)
            //        };
            //        but.Clicked += Button_Clicked;
            //        var beh = new LongPressBehavior() { Obiekt = but };
            //        beh.LongPressed += LongPressBehavior_LongPressed;
            //        but.Behaviors.Add(beh);
            //        panel.Add(Tuple.Create(i,j),but);
            //        row.Children.Add(but);
            //    }
            //    MainStack.Children.Add(row);
            //}
            InitializePanelLED();
        }

        private void InitializePanelLED()
        {
            int[] sensLEDs = DependencyService.Get<PanelLED>().GetLEDsState().Result;

            for (int i = 0; i < 8; i++)
            {
                var row = new FlexLayout() { Padding = new Thickness(10, 0, 10, 0) };
                for (int j = 0; j < 8; j++)
                {
                    string test = "#" + sensLEDs[i * 8 + j].ToString("X6");
                    var but = new Button()
                    {
                        BackgroundColor = Color.FromHex("#"+sensLEDs[i * 8 + j].ToString("X6")),
                        Margin = new Thickness(5, 0, 0, 0)
                    };
                    but.Clicked += Button_Clicked;
                    var beh = new LongPressBehavior() { Obiekt = but };
                    beh.LongPressed += LongPressBehavior_LongPressed;
                    but.Behaviors.Add(beh);
                    panel.Add(Tuple.Create(i, j), but);
                    row.Children.Add(but);
                }
                MainStack.Children.Add(row);
            }
        }


        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            var entry = sender as Entry;
            if (entry.CursorPosition!=0)
            {
                if (entry.Text == null || entry.Text == "" || !int.TryParse(entry.Text, out _) || int.Parse(entry.Text) < 0)
                    entry.Text = "";
                else if (int.Parse(entry.Text) > 255)
                    entry.Text = "255";

                int r = (entryRed.Text != null&& int.TryParse(entryRed.Text, out _)) ? int.Parse(entryRed.Text) : 0;
                int g = (entryGreen.Text != null&& int.TryParse(entryGreen.Text, out _)) ? int.Parse(entryGreen.Text) : 0;
                int b = (entryBlue.Text != null &&int.TryParse(entryBlue.Text, out _)) ? int.Parse(entryBlue.Text) : 0;
                colorInfo.Color = Color.FromRgb(r, g, b); 
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var but = sender as Button;
            if (chosenLeds.Count==0|| chosenLeds.Last()!=but)
            {
                clearChosen();
                but.BorderWidth = 5;
                but.BorderColor = Color.LightSkyBlue;
                chosenLeds.Add(but);
                colorInfo.Color = but.BackgroundColor;
                entryRed.Text = (255 * colorInfo.Color.R).ToString();
                entryGreen.Text = (255 * colorInfo.Color.G).ToString();
                entryBlue.Text = (255 * colorInfo.Color.B).ToString(); 
            }
        }
        private void clearChosen()
        {
            chosenLeds.ForEach(b => b.BorderWidth = 0);
            chosenLeds.Clear();
            entryRed.Text = "";
            entryGreen.Text = "";
            entryBlue.Text = "";
        }

        private void LongPressBehavior_LongPressed(object sender, EventArgs e)
        {
            Xamarin.Essentials.Vibration.Vibrate(TimeSpan.FromMilliseconds(100));
            var beh = sender as LongPressBehavior;
            var but = beh.Obiekt as Button;
            but.BorderWidth = 5;
            but.BorderColor = Color.LightSkyBlue;
            chosenLeds.Add(but);
            colorInfo.Color = but.BackgroundColor;
            entryRed.Text = (255 * colorInfo.Color.R).ToString();
            entryGreen.Text = (255 * colorInfo.Color.G).ToString();
            entryBlue.Text = (255 * colorInfo.Color.B).ToString();
        }

        private async Task Ustaw_Clicked(object sender, EventArgs e)
        {
            chosenLeds.ForEach(b => b.BackgroundColor = colorInfo.Color);
            await DependencyService.Get<PanelLED>().SetLEDs(buttonToArray());
        }

        private int[] buttonToArray()
        {
            int[] temp = new int[64];
            for(int i =0;i<8; i++)
            {
                for (int j = 0; j< 8; j++)
                {
                    Button b;
                    if (panel.TryGetValue(Tuple.Create(i, j), out b))
                    {
                        string s = b.BackgroundColor.ToHex().Replace("#","").Substring(2);
                        temp[i * 8 + j] = int.Parse(s, System.Globalization.NumberStyles.HexNumber);
                    }
                }
            }
            return temp;
        }
    }
}