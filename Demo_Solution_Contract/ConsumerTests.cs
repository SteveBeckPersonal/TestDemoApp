using System.Net.Mail;
using Demo_Solution.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json.Linq;
using PactNet;
using Xunit.Abstractions;

namespace Demo_Solution_Contract
{
    public class ConsumerTests
    {

        private readonly IPactBuilderV3 _pactBuilder;

        public ConsumerTests(ITestOutputHelper output)
        {
            var config = new PactConfig
            {
                PactDir = Path.Join("..", "..", "..", "pacts"),
                LogLevel = PactLogLevel.Debug,
                Outputters = new[] { new XUnitOutput(output) }

            };

            var pact = PactNet.Pact.V3("Demo_Solution", "ServiceLayer", config);
            _pactBuilder = pact.UsingNativeBackend();
        }

        [Fact]
        public async void GetWeatherForecasts()
        {
            WeatherForecast weatherForecast = new WeatherForecast()
            {
                Id = Guid.Parse("4831b360-e00e-4e99-990a-5f9bd8a94eb7"),
                TemperatureC = 15,
                Summary = "Cool",
                Date = DateTime.Parse("2023-01-01")
            };

            WeatherForecast weatherForecast2 = new WeatherForecast()
            {
                Id = Guid.Parse("cd29b446-2570-45ed-b4f8-1909277b3a4c"),
                TemperatureC = 25,
                Summary = "Warm",
                Date = DateTime.Parse("2023-02-02")
            };

            WeatherForecast[] weatherForecasts = new WeatherForecast[] { weatherForecast, weatherForecast2 };

            _pactBuilder.UponReceiving("A GET that returns all weather forecasts")
                .Given($"Two weather requests exist with ids: [{weatherForecast.Id},{weatherForecast2.Id}]")
                .WithRequest(HttpMethod.Get, $"/weatherforecast")
                .WillRespond()
                .WithJsonBody(weatherForecasts);

       
            var configs = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json")
                            .Build();

            


            await _pactBuilder.VerifyAsync(async context =>
            {
                configs["WeatherService"] = context.MockServerUri.ToString();
                var forecastService = new ForecastService(configs);
                var foreacts = await forecastService.GetWeatherForecasts();
                Assert.True(foreacts.Any());
            });
        }
    }
}