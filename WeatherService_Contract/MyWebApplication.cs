using Microsoft.AspNetCore.Mvc.Testing;
using WeatherService_Contract.Fixtures;

namespace WeatherService_Contract
{
    class MyWebApplication : WebApplicationFactory<Program>
    {
      
        protected override IHost CreateHost(IHostBuilder builder)
        {
            var basePath = Path.GetDirectoryName(typeof(WeatherServiceProviderFixture).Assembly.Location);
            var _config = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(basePath!, "appsettings.json"), false)
                .Build();

            //other setup for service etc goes here
            builder.ConfigureWebHostDefaults(webBuilder => {
                webBuilder.UseUrls(_config["ServerUri"]);
            });


            return base.CreateHost(builder);
        }
    }
}
