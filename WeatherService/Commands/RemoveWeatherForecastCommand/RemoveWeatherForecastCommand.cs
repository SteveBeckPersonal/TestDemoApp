using MediatR;

namespace WeatherService.Commands.AddWeatherForecastCommand
{
    public class RemoveWeatherForecastCommand : IRequest
    {
        public Guid Id { get; set; }

        public RemoveWeatherForecastCommand(Guid id)
        {
            Id = id;
        }
    }
}
