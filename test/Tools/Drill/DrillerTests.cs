using LoadTestToolbox.Tools.Drill;
using Spectre.Console;
using Xunit;

namespace LoadTestToolbox.Tests.Tools.Drill;

public sealed class DrillerTests
{
	[Fact]
	public void CanDrill()
	{
		//arrange
		using var http = new HttpClient(new MockHttpMessageHandler());
		var task = new ProgressTask(123, "test", 0, false);
		var options = new DrillSettings
		{
			URL = new Uri("http://localhost"),
			RPS = 1,
			Duration = 1
		};
		var driller = new Driller(http, task, options);

		//act
		var results = driller.Run();

		//assert
		Assert.Single(results);
	}
}