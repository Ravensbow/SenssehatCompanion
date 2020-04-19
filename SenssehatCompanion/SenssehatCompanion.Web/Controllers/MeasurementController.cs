using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SenssehatCompanion.Models;

namespace SenssehatCompanion.Controllers
{
    [Route("api/measurement/[action]")]
    [ApiController]
    public class MeasurementController : ControllerBase
    {
        private readonly IMeasurementService measurementService;

        public MeasurementController(IMeasurementService MS)
        {
            measurementService = MS;
        }
        [HttpGet("{unit}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Temperatura> GetTemperature(char unit)
        {
            Temperatura t = measurementService.GetActualTemperature(unit);

            if (t == null)
                return NotFound();

            return t;
        }
    }
}