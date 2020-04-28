﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SenssehatCompanion.Services
{
    interface IPanelLED
    {
        Task<int[]> GetLEDsState();
        Task<bool> SetLEDs(int[] leds);
        bool DrawSymbol();
    }
}
