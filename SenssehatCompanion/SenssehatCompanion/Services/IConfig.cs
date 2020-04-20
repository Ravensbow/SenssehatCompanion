using SenssehatCompanion.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SenssehatCompanion.Services
{
    public interface IConfig
    {
        Task<bool> ChangeConfig(Settings s);
        Task<Settings> GetConfig();
        string GetURL();
    }
}
