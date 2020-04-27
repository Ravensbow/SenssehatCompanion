using SenssehatCompanion.Models;
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
    public partial class SettingsPage : ContentPage
    {
        public Settings Settings { get; set; }
        public SettingsPage()
        {
            InitializeComponent();
            Settings = new Settings(DependencyService.Get<IConfig>().GetConfig().Result);
            BindingContext = Settings;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if(await DependencyService.Get<IConfig>().ChangeConfig(Settings))
            {
                Navigation.RemovePage(this);
                DependencyService.Get<IMessage>().ShortAlert("Pomyślnie zapisano");
            }
        }
    }
}