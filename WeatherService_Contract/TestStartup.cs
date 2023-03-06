using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.ComponentModel.Design;
using WeatherService;
using WeatherService_Contract.Middleware;

namespace WeatherService_Contract
{
    public class TestStartup
    {
        private readonly Startup inner;
        private readonly IConfiguration Configuration;

        public TestStartup(IConfiguration configuration, IWebHostEnvironment environment)
        {

            var basePath = Path.GetDirectoryName(typeof(TestStartup).Assembly.Location);
            var configs = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(Path.Combine(basePath!, "appsettings.json"), false)
                .AddEnvironmentVariables()
                 .Build();

            //configs["CosmosDb:EndpointUrl"] = BookingProviderFixture.CosmosEndpoint;
            //configs["CosmosDb:PrivateKey"] = BookingProviderFixture.CosmosKey;

            this.inner = new Startup(configs);
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            this.inner.ConfigureServices(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseMiddleware<ProviderStateMiddleware>();
            this.inner.Configure(app, env);
        }
    }
}
