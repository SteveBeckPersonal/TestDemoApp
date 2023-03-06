using System.ComponentModel;
using System.Formats.Asn1;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using PactNet;
using WeatherService;
using WeatherService.Contexts;
using static System.String;

namespace WeatherService_Contract.Middleware
{
    public class ProviderStateMiddleware
    {
        private readonly IDictionary<string, Func<IDictionary<string, string>, string, Task>> _providerStates;
        private readonly RequestDelegate _next;
        private ForecastContext _forecastContext;

        private static readonly JsonSerializerOptions Options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public ProviderStateMiddleware(RequestDelegate next)
        {
            _next = next;

            _providerStates = new Dictionary<string, Func<IDictionary<string, string>, string, Task>>
            {
                {
                    "Two weather requests exist with ids 4831b360-e00e-4e99-990a-5f9bd8a94eb7,cd29b446-2570-45ed-b4f8-1909277b3a4c",
                    SetForecastData
                }
            };
        }

        public async Task SetForecastData(IDictionary<string, string> parameters, string state) 
        {
            _forecastContext.RemoveRange(_forecastContext.Forecasts);
            _forecastContext.SaveChanges();
            var forecasts = await GetTestData<WeatherForecast[]>(state);
            _forecastContext.AddRange(forecasts);
            _forecastContext.SaveChanges();

        }

        public async Task InvokeAsync(HttpContext context, ForecastContext forecastContext)
        {
            _forecastContext = forecastContext;

            if (!(context.Request.Path.Value?.StartsWith("/provider-states") ?? false))
            {
                await _next.Invoke(context);
                return;
            }

            context.Response.StatusCode = (int)HttpStatusCode.OK;

            if (context.Request.Method == HttpMethod.Post.ToString())
            {
                string jsonRequestBody;
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                {
                    jsonRequestBody = await reader.ReadToEndAsync();
                }

                var providerState = System.Text.Json.JsonSerializer.Deserialize<ProviderState>(jsonRequestBody, Options);

                //A null or empty provider state key must be handled
                if (!IsNullOrEmpty(providerState?.State))
                {
                    await _providerStates[providerState.State](providerState.Params, providerState.State);
                }

                await context.Response.WriteAsync(String.Empty);
            }
        }

        private async Task<T?> GetTestData<T>(string state) where T : class
        {
            var basePath = Path.GetDirectoryName(typeof(ProviderStateMiddleware).Assembly.Location); ;
            var dataFilePath = Path.Combine(basePath!, "data", $"{state}.json");
            var fileData = File.Exists(dataFilePath) ? await File.ReadAllTextAsync(dataFilePath) : null;
            var searchData = IsNullOrEmpty(fileData) ? null : JsonConvert.DeserializeObject<T>(fileData);

            return searchData;
        }
    }
}
