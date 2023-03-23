using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using Demo_Solution.Pages;
using Demo_Solution.Services;
using Demo_Solution_Tests.Mocks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using RichardSzalay.MockHttp;

namespace Demo_Solution_Tests
{
	public class ForecastTests
	{
		[Fact]
		public void RendersTableWithExpectedData()
		{
			var weather = new WeatherForecast()
			{
				Id = Guid.NewGuid(),
				Date = DateTime.Now,
				TemperatureC = 32,
				Summary = "Hot"
			};

			// Arrange: render the Counter.razor component
			using var ctx = new TestContext();
			var mock = ctx.Services.AddMockHttpClient();
			
			ctx.Services.AddSingleton<IForecastService>(new MockForecastService());
			MockForecastService.Forecasts = new WeatherForecast[] { weather };		

			var cut = ctx.RenderComponent<FetchData>();
			
			//There should only be 1 row
			cut.FindAll("table > tbody > tr").Count().Should().Be(1);

			//Now check property values
			cut.Find("table > tbody > tr:nth-child(1) > td:nth-child(1)").TextContent.Should().Be(weather.Date.ToString("yyyy/MM/dd"));
			cut.Find("table > tbody > tr:nth-child(1) > td:nth-child(2)").TextContent.Should().Be(weather.TemperatureC.ToString());
			cut.Find("table > tbody > tr:nth-child(1) > td:nth-child(3)").TextContent.Should().Be(weather.TemperatureF.ToString());
			cut.Find("table > tbody > tr:nth-child(1) > td:nth-child(4)").TextContent.Should().Be(weather.Summary);

		}

		[Fact]
		public void TableIsEmptyAfterRemove()
		{
			var weather = new WeatherForecast()
			{
				Id = Guid.NewGuid(),
				Date = DateTime.Now,
				TemperatureC = 32,
				Summary = "Hot"
			};

			

			// Arrange: render the Counter.razor component
			using var ctx = new TestContext();
			var mock = ctx.Services.AddMockHttpClient();

			ctx.Services.AddSingleton<IForecastService>(new MockForecastService());
			MockForecastService.Forecasts = new List<WeatherForecast> { weather };

			var cut = ctx.RenderComponent<FetchData>();

			//There should only be 1 row
			cut.FindAll("table > tbody > tr").Count().Should().Be(1);

			//Now remove the item
			cut.Find("button[id=\"remove\"]").Click();
			cut.FindAll("table > tbody > tr").Count().Should().Be(0);
		}

		[Fact]
		public void AbleToAddItemsToTheTable()
		{
			var weather = new WeatherForecast()
			{
				Id = Guid.NewGuid(),
				Date = DateTime.Now,
				TemperatureC = 32,
				Summary = "Hot"
			};

			var weather2 = new WeatherForecast()
			{
				Date = DateTime.Now.AddDays(1),
				TemperatureC = 13,
				Summary = "Cool"
			};

			// Arrange: render the Counter.razor component
			using var ctx = new TestContext();
			var mock = ctx.Services.AddMockHttpClient();

			ctx.Services.AddSingleton<IForecastService>(new MockForecastService());
			MockForecastService.Forecasts = new List<WeatherForecast> { weather };

			var cut = ctx.RenderComponent<FetchData>();

			//There should only be 1 row
			cut.FindAll("table > tbody > tr").Count().Should().Be(1);

			var ele = cut.Find("input[id=\"Date\"]");
			ele.Change(weather2.Date.ToString("yyyy/MM/dd"));

			cut.Find("input[id=\"Temp\"]").Change(weather2.TemperatureC);
			cut.Find("input[id=\"Summary\"]").Change(weather2.Summary);
            cut.Find("form").Submit();

            cut.FindAll("table > tbody > tr").Count().Should().Be(2);

        }


	}
}
