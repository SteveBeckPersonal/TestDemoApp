using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherService.Contexts;

namespace WeatherService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ForecastContext _context;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ForecastContext forecastContext)
        {
            _logger = logger;
            _context = forecastContext;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WeatherForecast[]))]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> Get()
        {
            return Ok(await _context.Forecasts.ToArrayAsync());
        }

        [HttpPost(Name = "AddWeatherForecast")]
        public async Task<ActionResult> Post(WeatherForecast forecast)
        {
            _context.Forecasts.Add(forecast);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}", Name = "RemoveWeatherForecast")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var forecast = _context.Forecasts.Where(x => x.Id.Equals(id)).FirstOrDefault();
            if (forecast == null) 
            {
                return NotFound();
            }

            _context.Forecasts.Remove(forecast);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}