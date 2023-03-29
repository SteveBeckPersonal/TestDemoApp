using MediatR;
using WeatherService.Contexts;

namespace WeatherService.Commands.AddWeatherForecastCommand
{
    public class RemoveWeatherForecastCommandHandler : IRequestHandler<RemoveWeatherForecastCommand>
    {
        private readonly ForecastContext _context;

        public RemoveWeatherForecastCommandHandler(ForecastContext context)
        {
            _context = context;
        }

        public async Task Handle(RemoveWeatherForecastCommand request, CancellationToken cancellationToken)
        {
            var forecast = _context.Forecasts.Where(x => x.Id.Equals(request.Id)).FirstOrDefault();
            if (forecast == null)
            {
                throw new NullReferenceException($"Weather Forecast not found with id - {request.Id}");
            }

            _context.Forecasts.Remove(forecast);
            await _context.SaveChangesAsync();
        }
    }
}
