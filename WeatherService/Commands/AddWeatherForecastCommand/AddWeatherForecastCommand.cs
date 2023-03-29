using MediatR;

namespace WeatherService.Commands.AddWeatherForecastCommand
{
    public class AddWeatherForecastCommand : IRequest
    {
        public WeatherForecast Forecast { get; set; }

        public AddWeatherForecastCommand(WeatherForecast forecast)
        {
            Forecast = forecast;
        }
    }
}
