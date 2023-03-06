using System.ComponentModel;
using System.Formats.Asn1;
using System.Net;
using System.Text;
using System.Text.Json;
using PactNet;
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
                                
            };
        }

        public async Task SetForecastData(IDictionary<string, string> parameters, string state) 
        {
            var t = "";
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

                var providerState = JsonSerializer.Deserialize<ProviderState>(jsonRequestBody, Options);

                //A null or empty provider state key must be handled
                if (!IsNullOrEmpty(providerState?.State))
                {
                    await _providerStates[providerState.State](providerState.Params, providerState.State);
                }

                await context.Response.WriteAsync(String.Empty);
            }
        }
    }
}
