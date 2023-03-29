using MediatR;
using Microsoft.EntityFrameworkCore;
using WeatherService.Contexts;

namespace WeatherService.Queries.GetWeatherForecast
{
    public class GetWeatherForecastQueryHandler : IRequestHandler<GetWeatherForecastQuery, IEnumerable<WeatherForecast>>
    {
        private readonly ForecastContext _context;

        public GetWeatherForecastQueryHandler(ForecastContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WeatherForecast>> Handle(GetWeatherForecastQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Forecasts.ToListAsync();
            result = result.OrderBy(result => result.Date).ToList();
            return result;
        }
    }
}
