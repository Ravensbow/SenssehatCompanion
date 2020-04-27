using System;
using System.Collections.Generic;
using System.Text;

namespace SenssehatCompanion.Services
{
    class PanelLED : IPanelLED
    {
        public int[,] LEDs { get; set; } = new int[8,8];

        public PanelLED()
        {
        }

        public bool DrawSymbol()
        {
            throw new NotImplementedException();
        }

        public int[,] GetLEDsState()
        {
            return LEDs;
        }

        public bool SetLEDs(int[,] leds)
        {
            LEDs = leds;
            return true;
        }
    }
}
