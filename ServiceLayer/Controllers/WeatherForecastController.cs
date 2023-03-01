using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace ServiceLayer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        public IConfiguration Configuration { get; set; }


        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration)
        {
            _logger = logger;
            Configuration = configuration;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<WeatherForecast[]> Get()
        {

            var client = new HttpClient();
            client.BaseAddress = new Uri(Configuration["WeatherServiceURL"]);

            var response = await client.GetAsync("weatherforecast");
            var content = await response.Content.ReadFromJsonAsync<WeatherForecast[]>();

            return content;
        }
    }
}