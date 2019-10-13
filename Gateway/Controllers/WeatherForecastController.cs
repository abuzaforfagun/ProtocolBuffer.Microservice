using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly MicroserviceConfiguration _configuration;
        private readonly IHttpService _httpService;

        public WeatherForecastController(MicroserviceConfiguration configuration, IHttpService httpService)
        {
            _configuration = configuration;
            _httpService = httpService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _httpService.GetAsync<IEnumerable<WeatherForecast>>(_configuration.WeatherService.GetAll);
            return Ok(data);
        }
    }
}
