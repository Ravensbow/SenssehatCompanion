using System;
using System.Collections.Generic;
using System.Text;

namespace SenssehatCompanion.Services
{
    interface IPanelLED
    {
        int[,] GetLEDsState();
        bool SetLEDs(int[,] leds);
        bool DrawSymbol();
    }
}
