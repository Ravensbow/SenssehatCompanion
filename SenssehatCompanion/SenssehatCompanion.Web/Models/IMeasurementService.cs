using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SenssehatCompanion.Models
{
    public interface IMeasurementService
    {
        Temperatura GetActualTemperature(char unit);
    }
}
