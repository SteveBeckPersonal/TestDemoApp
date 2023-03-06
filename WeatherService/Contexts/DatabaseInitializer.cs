using System.Diagnostics;

namespace WeatherService.Contexts
{
    public static class DbInitializer
    {
        public static void Initialize(ForecastContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Forecasts.Any())
            {
                return;   // DB has been seeded
            }

            var forecast = new WeatherForecast[]
            {
            new WeatherForecast{Date = DateTime.Now.AddDays(1),TemperatureC = 27,Summary = "Warm"},
            new WeatherForecast{Date = DateTime.Now.AddDays(2),TemperatureC = 26,Summary = "Warm"},
            new WeatherForecast{Date = DateTime.Now.AddDays(3),TemperatureC = 25,Summary = "Warm"},
            new WeatherForecast{Date = DateTime.Now.AddDays(4),TemperatureC = 16,Summary = "Rain"}
            };
            foreach (WeatherForecast s in forecast)
            {
                context.Forecasts.Add(s);
            }
            context.SaveChanges();

        }
    }
}
