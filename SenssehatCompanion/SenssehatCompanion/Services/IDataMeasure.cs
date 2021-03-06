﻿using SenssehatCompanion.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SenssehatCompanion.Services
{
    public interface IDataMeasure
    {
        Task<List<MeasureValues>> GetMeasureAsync();
        Task<Joystick> GetJoystickAsync();
        MeasureValues GetMeasureFake();
    }
}
