using LoadTestToolbox.Charts;
using LoadTestToolbox.Tools.Drill;
using NSubstitute;
using Spectre.Console.Testing;
using Xunit;

namespace LoadTestToolbox.Tests.Tools.Drill;

public sealed class DrillCommandTests
{
	[Fact]
	public async Task WritesFileOnSuccess()
	{
		//arrange
		using var http = new HttpClient(new MockHttpMessageHandler());
		var io = Substitute.For<ChartIO>();
		using var console = new TestConsole();
		var command = new DrillCommand(http, io, console);
		var settings = new DrillSettings
		{
			URL = new Uri("http://localhost"),
			RPS = 1,
			Duration = 1,
			Filename = "test.png"
		};

		//act
		var result = await command.ExecuteAsync(null!, settings);

		//assert
		Assert.Empty(console.Output);
		Assert.Equal(0, result);
		await io.Received().SaveChart(Arg.Any<SkiaChart>(), "test.png");
	}

	[Fact]
	public async Task ShowsExceptionOnFailure()
	{
		//arrange
		using var http = new HttpClient(new MockHttpMessageHandler());
		using var console = new TestConsole();
		var command = new DrillCommand(http, null!, console);
		var settings = new DrillSettings();

		//act
		var result = await command.ExecuteAsync(null!, settings);

		//assert
		Assert.Equal(1, result);
		Assert.Contains("exception", console.Output, StringComparison.InvariantCultureIgnoreCase);
	}
}