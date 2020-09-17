using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading;
using UltimateXF.Widget.Charts.Models;
using UltimateXF.Widget.Charts.Models.LineChart;
using SenssehatCompanion.Models;
using SenssehatCompanion.Services;
using SenssehatCompanion.Tools;

namespace SenssehatCompanion.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JoystickPage : ContentPage
    {
        private CancellationTokenSource source;
        private CancellationToken cts;
        private readonly Settings settings;
        private readonly IDataMeasure dataMeasure;
        private Joystick actualState;

        public JoystickPage()
        {
            dataMeasure = DependencyService.Get<DataMeasure>(DependencyFetchTarget.NewInstance);
            settings = DependencyService.Get<IConfig>().GetConfig().Result;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            source = new CancellationTokenSource();
            cts = source.Token;

            await UpdateJoystickAsync();
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            source.Cancel();
            source.Dispose();
            base.OnDisappearing();
        }

        private async Task UpdateJoystickAsync()
        {
            while (true)
            {
                if (cts.IsCancellationRequested)
                    return;

                var joyS = await dataMeasure.GetJoystickAsync();
                if (joyS == null)
                    continue;

                Update(joyS);
                await Task.Delay(30);
            }
        }

        private void Update(Joystick js)
        {
            if(actualState!=null&&js.direction!=actualState.direction)
            {
                switch (actualState.direction)
                {
                    case JoystickStatu.Down:
                        boxDown.BackgroundColor = Color.Gray;
                        break;
                    case JoystickStatu.Up:
                        boxUp.BackgroundColor = Color.Gray;
                        break;
                    case JoystickStatu.Right:
                        boxRight.BackgroundColor = Color.Gray;
                        break;
                    case JoystickStatu.Left:
                        boxLeft.BackgroundColor = Color.Gray;
                        break;
                    case JoystickStatu.Middle:
                        boxCenter.BackgroundColor = Color.Gray;
                        break;
                }
            }
            switch (js.direction)
            {
                case JoystickStatu.Down:
                    switch(js.action)
                    {
                        case JoystickEvent.Pressed:
                            boxDown.BackgroundColor = Color.LightGray;
                            break;
                        case JoystickEvent.Held:
                            boxDown.BackgroundColor = Color.WhiteSmoke;
                            break;
                        case JoystickEvent.Released:
                            boxDown.BackgroundColor = Color.Gray;
                            break;
                    }
                    break;
                case JoystickStatu.Up:
                    switch (js.action)
                    {
                        case JoystickEvent.Pressed:
                            boxUp.BackgroundColor = Color.LightGray;
                            break;
                        case JoystickEvent.Held:
                            boxUp.BackgroundColor = Color.WhiteSmoke;
                            break;
                        case JoystickEvent.Released:
                            boxUp.BackgroundColor = Color.Gray;
                            break;
                    }
                    break;
                case JoystickStatu.Right:
                    switch (js.action)
                    {
                        case JoystickEvent.Pressed:
                            boxRight.BackgroundColor = Color.LightGray;
                            break;
                        case JoystickEvent.Held:
                            boxRight.BackgroundColor = Color.WhiteSmoke;
                            break;
                        case JoystickEvent.Released:
                            boxRight.BackgroundColor = Color.Gray;
                            break;
                    }
                    break;
                case JoystickStatu.Left:
                    switch (js.action)
                    {
                        case JoystickEvent.Pressed:
                            boxLeft.BackgroundColor = Color.LightGray;
                            break;
                        case JoystickEvent.Held:
                            boxLeft.BackgroundColor = Color.WhiteSmoke;
                            break;
                        case JoystickEvent.Released:
                            boxLeft.BackgroundColor = Color.Gray;
                            break;
                    }
                    break;
                case JoystickStatu.Middle:
                    switch (js.action)
                    {
                        case JoystickEvent.Pressed:
                            boxCenter.BackgroundColor = Color.LightGray;
                            break;
                        case JoystickEvent.Held:
                            boxCenter.BackgroundColor = Color.WhiteSmoke;
                            break;
                        case JoystickEvent.Released:
                            boxCenter.BackgroundColor = Color.Gray;
                            break;
                    }
                    break;
            }
            actualState = js;

        }
    }
}