using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Demo_Solution.Services
{
	public class ForecastService
	{
        private IConfiguration Configuration { get; set; }
		private HttpClient ForecastClient { get; set; }
        public ForecastService(IConfiguration configuration) 
		{
			Configuration = configuration;
			ForecastClient = new HttpClient();
			ForecastClient.BaseAddress = new Uri(Configuration["WeatherService"]);
		}

		public async Task<WeatherForecast[]> GetWeatherForecasts() 
		{
			var response = await ForecastClient.GetAsync("weatherforecast");
			var forecasts = await response.Content.ReadFromJsonAsync<WeatherForecast[]>();
			return forecasts.OrderBy(forecast => forecast.Date).ToArray();
		}

		public async Task PostWeatherForecast(WeatherForecast forecast)
		{
            var json = JsonConvert.SerializeObject(forecast);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await ForecastClient.PostAsync("weatherforecast", data);
        }

		public async Task DeleteForecast(Guid id) 
		{
            var response = await ForecastClient.DeleteAsync($"weatherforecast/{id}");
        }

	}

	public class WeatherForecast
	{
        public Guid Id { get; set; }
        public DateTime Date { get; set; }

		public int TemperatureC { get; set; }

		public string? Summary { get; set; }

		public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
	}
}
