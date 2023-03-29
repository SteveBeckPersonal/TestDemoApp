using MediatR;
using WeatherService.Contexts;

namespace WeatherService.Commands.AddWeatherForecastCommand
{
    public class AddWeatherForecastCommandHandler : IRequestHandler<AddWeatherForecastCommand>
    {
        private readonly ForecastContext _context;

        public AddWeatherForecastCommandHandler(ForecastContext context)
        {
            _context = context;
        }

        public async Task Handle(AddWeatherForecastCommand request, CancellationToken cancellationToken)
        {
            _context.Forecasts.Add(request.Forecast);
            await _context.SaveChangesAsync();           
        }
    }
}
