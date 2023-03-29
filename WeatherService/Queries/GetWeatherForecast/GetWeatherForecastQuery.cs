using MediatR;

namespace WeatherService.Queries.GetWeatherForecast
{
    public class GetWeatherForecastQuery : IRequest<IEnumerable<WeatherForecast>>
    {
    }
}
