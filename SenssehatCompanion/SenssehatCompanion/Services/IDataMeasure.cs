using SenssehatCompanion.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SenssehatCompanion.Services
{
    public interface IDataMeasure
    {
        Task<MeasureValues> GetMeasureAsync();
        MeasureValues GetMeasureFake();
        //Temperatura GetTemperatureTCP(string unit);
    }
}
