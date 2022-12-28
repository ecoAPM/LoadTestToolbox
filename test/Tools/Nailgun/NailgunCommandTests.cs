using LoadTestToolbox.Charts;
using LoadTestToolbox.Tools.Nailgun;
using NSubstitute;
using Spectre.Console.Testing;
using Xunit;

namespace LoadTestToolbox.Tests.Tools.Nailgun;

public sealed class NailgunCommandTests
{
	[Fact]
	public async Task WritesFileOnSuccess()
	{
		//arrange
		var http = new HttpClient(new MockHttpMessageHandler());
		var io = Substitute.For<ChartIO>();
		var console = new TestConsole();
		var command = new NailgunCommand(http, io, console);
		var settings = new NailgunSettings
		{
			URL = new Uri("http://localhost"),
			Requests = 1,
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
		var http = new HttpClient(new MockHttpMessageHandler());
		var console = new TestConsole();
		var command = new NailgunCommand(http, null!, console);
		var settings = new NailgunSettings();

		//act
		var result = await command.ExecuteAsync(null!, settings);

		//assert
		Assert.Equal(1, result);
		Assert.Contains("exception", console.Output, StringComparison.InvariantCultureIgnoreCase);
	}
}