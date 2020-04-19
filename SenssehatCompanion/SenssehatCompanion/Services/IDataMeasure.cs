using SenssehatCompanion.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SenssehatCompanion.Services
{
    interface IDataMeasure
    {
        Task<Temperatura> GetTemperatureAsync(string unit);
    }
}
