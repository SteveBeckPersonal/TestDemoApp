using PactNet.Infrastructure.Outputters;
using PactNet.Verifier;
using WeatherService_Contract.Fixtures;
using Xunit.Abstractions;

namespace ServiceLayer_Contract
{
    public class ProviderTests : IClassFixture<WeatherServiceProviderFixture>
    {
        private ITestOutputHelper OutputHelper { get; }
        private readonly WeatherServiceProviderFixture _fixture;

        public ProviderTests(WeatherServiceProviderFixture fixture, ITestOutputHelper outputHelper) 
        {
            _fixture = fixture;
            OutputHelper = outputHelper;
        }

        [Fact]
        public void VerifyContract()
        {
            var config = new PactVerifierConfig
            {
                Outputters = new List<IOutput> { new XUnitOutput(OutputHelper) },
                LogLevel = PactNet.PactLogLevel.Debug
            };

            FileInfo fileInfo = new FileInfo(@"pacts/Demo_Solution-ServiceLayer.json");
            
            IPactVerifier pactVerifier = new PactVerifier(config);
            pactVerifier.ServiceProvider("WeatherService", _fixture.ServerUri)
                .WithPactBrokerSource(_fixture.BrokerBaseUri, options => {
                    options.TokenAuthentication(_fixture.PactBrokerToken);
                    options.PublishResults(_fixture.PactVersion);
                })
                .WithProviderStateUrl(new Uri(_fixture.ServerUri,"/provider-states"))
                .WithRequestTimeout(TimeSpan.FromMinutes(5))
                .Verify();

        }
    }
}