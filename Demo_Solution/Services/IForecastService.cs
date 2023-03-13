namespace Demo_Solution.Services
{
	public interface IForecastService
	{
		Task DeleteForecast(Guid id);
		Task<IEnumerable<WeatherForecast>> GetWeatherForecasts();
		Task PostWeatherForecast(WeatherForecast forecast);
	}
}