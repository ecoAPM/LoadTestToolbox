using Spectre.Console.Testing;
using Xunit;

namespace LoadTestToolbox.Tests;

public sealed class DrillerTests
{
	[Fact]
	public async Task CanDrill()
	{
		//arrange
		var http = new HttpClient(new MockHttpMessageHandler());
		var console = new TestConsole();
		var options = new DrillSettings
		{
			URL = new Uri("http://localhost"),
			RPS = 1,
			Duration = 1
		};
		var driller = new Driller(http, console, options);

		//act
		var results = await driller.Run();

		//assert
		Assert.Equal(1, results.Count);
	}
}