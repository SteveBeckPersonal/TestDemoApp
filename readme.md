# Automation Testing Application

## Purpose

This repo exists to provide an application to demonstrate Contract Testing, and Blazor Component testing.
It is a simple Blazor demo application, with a asp.net 6.0 backend which uses SQL Server and Entity Framework.

## Requirements

- Sql Server - or container
- Visual Studio

## Setup

Ensure that the SQL Connection string in the appsettings files is set correctly to your instance of SQL.
Otherwise the application should launch simply, and contract testing will work if the appsettings files are updated accordingly.

# Contract Testing

Contract testing is a technique for testing an integration point by checking each application in isolation to ensure the messages it sends or receives conform to a shared understanding that is documented in a "contract".

For applications that communicate via HTTP, these "messages" would be the HTTP request and response, and for an application that used queues, this would be the message that goes on the queue.

In practice, a common way of implementing contract tests (and the way Pact does it) is to check that all the calls to your test doubles return the same results as a call to the real application would.

## Useful Links

- Pact.io https://docs.pact.io/
- Pact.Net https://docs.pact.io/implementation_guides/net/readme
- Contract testing workshop https://github.com/DiUS/pact-workshop-dotnet-core-v3/

## Terminology

- Consumer - the service or application which calls the integration request.
- Provider - the service which processes the request and returns a response.
- Contract - A collection of interactions performed by the consumer, containing: endpoint, request, and expected response information to test against the provider.
- Broker - A middleman tool which manages the contracts and can share the contract with the provider, as well as receive the results from the provider tests.

## Workflow

1. Application / Service team (Consumer Team) create consumer tests which replicate the use cases for the integration under test.
2. Contract is uploaded to the Broker
3. Provider Team uses the Contract from the Broker to create the setup (Provider State) for each interaction
4. Provider tests can now be executed pre-deployment to verify that the Schema between Consumer and Provider is correct, and the use cases are also valid.

## Consumer Tests

A Consumer Test is made up of a builder pattern which defines the endpoint, the state of the Integration Under Test (Given) and the expected response.
A local endpoint is then used to mock the service, and the Consumer Application function which calls the integration is then executed.
Pact.io will use return the resposne specified in the builder and will then verify the schema matches, and finally generate the Contract.
After the contract has been created it must be shared with the broker via a REST request to the broker.

```csharp
public ApiTest(ITestOutputHelper output)
        {
            products = new List<object>()
            {
                new { id = 9, type = "CREDIT_CARD", name = "GEM Visa", version = "v2" },
                new { id = 10, type = "CREDIT_CARD", name = "28 Degrees", version = "v1" }
            };

            var Config = new PactConfig
            {
                PactDir = Path.Join("..", "..", "..", "..", "..", "pacts"),
                LogDir = "pact_logs",
                Outputters = new[] { new XUnitOutput(output) },
                DefaultJsonSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            };

            pact = Pact.V3("ApiClient", "ProductService", Config).UsingNativeBackend(port);
            ApiClient = new ApiClient(new System.Uri($"http://localhost:{port}"));
        }

        [Fact]
        public async void GetAllProducts()
        {
            // Arange
            pact.UponReceiving("A valid request for all products")
                    .Given("There is data")
                    .WithRequest(HttpMethod.Get, "/api/products")
                .WillRespond()
                    .WithStatus(HttpStatusCode.OK)
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithJsonBody(new TypeMatcher(products));

            await pact.VerifyAsync(async ctx => {
                var response = await ApiClient.GetAllProducts();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            });
        }

```
