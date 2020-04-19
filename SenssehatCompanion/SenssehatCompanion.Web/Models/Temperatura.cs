using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SenssehatCompanion.Models
{
    public class Temperatura
    {
        public double Value { get; set; }
        public char Unit { get; set; }

        public void ChangeUnit(char unit)
        {
            if (unit == 'C' && Unit == 'K')
                Value = Value - 273.15;
            else if(unit == 'C' && Unit == 'F')
                Value =(5 / 9)*(Value - 32);
            else if (unit == 'F' && Unit == 'K')
                Value = (1.8 * (Value-275.15)) + 32;
            else if (unit == 'F' && Unit == 'C')
                Value =(1.8*Value) + 32;
            else if (unit == 'K' && Unit == 'C')
                Value = Value + 273.15;
            else if (unit == 'K' && Unit == 'F')
                Value = (5 / 9) * (Value - 32)+275.15;
            if (unit =='K'|| unit == 'F'|| unit == 'C')
            Unit = unit;
        }
    }
}
