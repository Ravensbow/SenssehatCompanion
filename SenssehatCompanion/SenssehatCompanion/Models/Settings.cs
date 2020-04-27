using System;
using System.Collections.Generic;
using System.Text;

namespace SenssehatCompanion.Models
{
    public class Settings
    {
        public string ServerIP { get; set; }
        public int Port { get; set; }
        public int Interval { get; set; }
        public int NumSamples { get; set; }
        public Settings(Settings s)
        {
            ServerIP = s.ServerIP;
            Port = s.Port;
            Interval = s.Interval;
            NumSamples = s.NumSamples;
        }
        public Settings() { }

    }
}
