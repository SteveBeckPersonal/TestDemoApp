using Bunit;
using Demo_Solution.Components;
using FluentAssertions;

namespace Demo_Solution_Tests
{
    public class CounterTests
    {
        [Fact]
        public void CounterShouldIncrementWhenClicked_FailureExample()
        {
            // Arrange: render the Counter.razor component
            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<Counter>(parameters => parameters
            .Add(p => p.CurrentCount, 0));

            // Act: find and click the <button> element to increment
            // the counter in the <p> element
            cut.Find("button[test-id=\"increment\"]").Click();

            // Assert: first find the <p> element, then verify its content
            cut.Find("p").MarkupMatches("<p>Current count: 1</p>");
        }

        [Fact]
        public void CounterShouldIncrementWhenClicked()
        {
            // Arrange: render the Counter.razor component
            using var ctx = new TestContext();
			var cut = ctx.RenderComponent<Counter>(parameters => parameters
			.Add(p => p.CurrentCount, 0));

			// Act: find and click the <button> element to increment
			// the counter in the <p> element
			cut.Find("button[test-id=\"increment\"]").Click();

			// Assert: first find the <p> element, then verify its content
			cut.Find("p").TextContent.Should().Be("Current count: 1");
		}

        [Fact]
        public void CounterShouldAbateWhenClicked()
        {
            // Arrange: render the Counter.razor component
            using var ctx = new TestContext();
			var cut = ctx.RenderComponent<Counter>(parameters => parameters
			.Add(p => p.CurrentCount, 1));

			// Act: find and click the <button> element to abate
			// the counter in the <p> element
			cut.Find("button[test-id=\"abatement\"]").Click();

            // Assert: first find the <p> element, then verify its content
            cut.Find("p").MarkupMatches("<p role=\"status\">Current count: 0</p>");


        }

        [Fact]
        public void CounterShouldNotBeNegative()
        {
            // Arrange: render the Counter.razor component
            using var ctx = new TestContext();
			var cut = ctx.RenderComponent<Counter>(parameters => parameters
			.Add(p => p.CurrentCount, 0));

			// Act: find and click the <button> element to abate
			// the counter in the <p> element
			cut.Find("button[test-id=\"abatement\"]").Click();

            // Assert: first find the <p> element, then verify its content
            cut.Find("p").TextContent.Should().Be("Current count: 0");


        }
    }
}