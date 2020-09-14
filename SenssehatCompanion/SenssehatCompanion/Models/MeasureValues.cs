using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SenssehatCompanion.Models
{
    public class MeasureValues : INotifyPropertyChanged
    {
        public double temperature { get; set; }
        public double pressure { get; set; }
        public double humidity { get; set; }
        public Gyroscope gyroscope { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class Gyroscope
    {
        public double Roll { get; set; }
        public double Yaw { get; set; }
        public double Pitch { get; set; }
    }
}
