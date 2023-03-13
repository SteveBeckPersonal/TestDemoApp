using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo_Solution.Services;

namespace Demo_Solution_Tests.Mocks
{
	public class MockForecastService : IForecastService
	{
		public Task DeleteForecast(Guid id)
		{
			Forecasts = Forecasts.Where(x => x.Id != id).ToArray();
			return Task.FromResult(() => { }); 
		}

		public static IEnumerable<WeatherForecast> Forecasts { get; set; }	
		public Task<IEnumerable<WeatherForecast>> GetWeatherForecasts()
		{
			return Task.FromResult(Forecasts);
		}

		public Task PostWeatherForecast(WeatherForecast forecast)
		{
			Forecasts = Forecasts.Add(forecast);
			Forecasts = Forecasts.OrderBy(x => x.Date);
			return Task.FromResult(() => { });
		}
	}
}
