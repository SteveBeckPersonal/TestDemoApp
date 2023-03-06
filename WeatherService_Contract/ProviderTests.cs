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

            IPactVerifier pactVerifier = new PactVerifier(config);
        }
    }
}