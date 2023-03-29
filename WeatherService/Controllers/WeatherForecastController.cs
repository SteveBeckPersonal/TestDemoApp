using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherService.Commands.AddWeatherForecastCommand;
using WeatherService.Contexts;
using WeatherService.Queries.GetWeatherForecast;

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
        private readonly IMediator _mediatR;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ForecastContext forecastContext, IMediator mediator)
        {
            _logger = logger;
            _context = forecastContext;
            _mediatR = mediator;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WeatherForecast>))]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> Get()
        {
            var result = await _mediatR.Send(new GetWeatherForecastQuery());
            return Ok(result);
        }

        [HttpPost(Name = "AddWeatherForecast")]
        public async Task<ActionResult> Post(WeatherForecast forecast)
        {
            await _mediatR.Send(new AddWeatherForecastCommand(forecast));
            return Ok();
        }

        [HttpDelete("{id}", Name = "RemoveWeatherForecast")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try 
            {
                await _mediatR.Send(new RemoveWeatherForecastCommand(id));
            }
            catch (NullReferenceException) 
            {
                return NotFound();
            }
            return Ok();
        }
    }
}