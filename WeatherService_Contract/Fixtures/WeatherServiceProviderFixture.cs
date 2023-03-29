

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WeatherService;

namespace WeatherService_Contract.Fixtures
{
    public  class WeatherServiceProviderFixture
    {
        private readonly IHost _host;
        public string PactVersion { get; set; }
        public readonly Uri ServerUri;
        public readonly Uri BrokerBaseUri;
        public readonly string? PactBrokerToken;
        private readonly ILogger<WeatherServiceProviderFixture> _output;

        public WeatherServiceProviderFixture() 
        {
            using var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
                    .SetMinimumLevel(LogLevel.Trace));

            _output = loggerFactory.CreateLogger<WeatherServiceProviderFixture>();

            var basePath = Path.GetDirectoryName(typeof(WeatherServiceProviderFixture).Assembly.Location);
            var configs = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(basePath!, "appsettings.json"), false)
                .Build();



            ServerUri = new Uri(configs["ServerUri"]);
            //BrokerBaseUri = new Uri(configs["BrokerBaseUri"]);
            //PactBrokerToken = configs["PactBrokerToken"];
            PactVersion = configs["PROVIDER_VERSION"];



            _host = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls(ServerUri.ToString());
                    webBuilder.UseStartup<TestStartup>()
                        .UseSetting(WebHostDefaults.ApplicationKey, typeof(Startup).Assembly.FullName);
                }).Build();
            _host.Start();
        }

    }
}
