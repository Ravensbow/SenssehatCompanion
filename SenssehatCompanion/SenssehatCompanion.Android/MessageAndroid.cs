using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;


[assembly: Xamarin.Forms.Dependency(typeof(SenssehatCompanion.Droid.MessageAndroid))]
namespace SenssehatCompanion.Droid
{
    
    public class MessageAndroid : SenssehatCompanion.Services.IMessage
    {
        private List<Toast> toasts = new List<Toast>();
        public void LongAlert(string message)
        {
            var t = Toast.MakeText(Application.Context, message, ToastLength.Long);
            toasts.Add(t);
            t.Show();
        }
        public void Clear()
        {
            toasts.ForEach(t => t.Cancel());
            toasts.Clear();
        }
        public void ShortAlert(string message)
        {
            var t = Toast.MakeText(Application.Context, message, ToastLength.Short);
            toasts.Add(t);
            t.Show();
        }
    }
}