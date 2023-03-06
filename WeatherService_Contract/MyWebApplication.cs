using Microsoft.AspNetCore.Mvc.Testing;

namespace WeatherService_Contract
{
    class MyWebApplication : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            //other setup for service etc goes here



            return base.CreateHost(builder);
        }
    }
}
