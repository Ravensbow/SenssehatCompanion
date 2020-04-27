using SenssehatCompanion.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SenssehatCompanion.Services
{
    public interface IDataMeasure
    {
        Task<Temperatura> GetTemperatureAsync(string unit);
        Temperatura GetTemperaturaFake(string unit);
        Temperatura GetTemperatureTCP(string unit);
    }
}
