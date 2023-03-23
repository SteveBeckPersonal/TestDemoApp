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

[Consumer Tests](https://dev.azure.com/haefelesoftware/TestAutomation/_git/Automation_Application?path=/Demo_Solution_Contract/ConsumerTests.cs)

## Provider Tests

[Provider Tests](https://dev.azure.com/haefelesoftware/TestAutomation/_git/Automation_Application?path=/WeatherService_Contract/ProviderTests.cs)

Provider tests require a locally hosted instance of the AUT which allows the tester to inject or replace dependencies with mocks, as well as access the
databases or contexts of the application.

[Test Startup](https://dev.azure.com/haefelesoftware/TestAutomation/_git/Automation_Application?path=/WeatherService_Contract/TestStartup.cs)

The ConfigureServices function in the TestStartup is used for mocking services, and DB connections / contexts.
The Configure function is used for injecting Middleware such as the ProviderStateMiddleware or AuthServices.

All interactions are tested via a single [Test], the main source of effort with Provider tests is to ensure external services are adequately mocked, and the data
required for the test is present in application.

### Provider States

[Provider State Middleware](https://dev.azure.com/haefelesoftware/TestAutomation/_git/Automation_Application?path=/WeatherService_Contract/Middleware/ProviderStateMiddleware.cs)

Provider states are injected through the startup as middleware, and are executed by Pact.io before the interaction test runs. This is done by mapping the Given statements
from the Contract to an action which generates or injects the required data into the system for the test.
