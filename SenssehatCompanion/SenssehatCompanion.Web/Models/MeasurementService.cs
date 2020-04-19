using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SenssehatCompanion.Models
{
    public class MeasurementService : IMeasurementService
    {

        public MeasurementService() { }
        public Temperatura GetActualTemperature(char unit)
        {
            var random = new Random();
            var temp = new Temperatura() { Unit = 'C', Value = random.Next(10, 25) };
            temp.ChangeUnit(unit);
            return temp;
        }
    }
}
