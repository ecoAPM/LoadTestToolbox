using LoadTestToolbox.Charts;
using LoadTestToolbox.Tools.Hammer;
using NSubstitute;
using Spectre.Console.Testing;
using Xunit;

namespace LoadTestToolbox.Tests.Tools.Hammer;

public sealed class HammerCommandTests
{
	[Fact]
	public async Task WritesFileOnSuccess()
	{
		//arrange
		using var http = new HttpClient(new MockHttpMessageHandler());
		var io = Substitute.For<ChartIO>();
		using var console = new TestConsole();
		var command = new HammerCommand(http, io, console);
		var settings = new HammerSettings
		{
			URL = new Uri("http://localhost"),
			Min = 1,
			Max = 1,
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
		var command = new HammerCommand(http, null!, console);
		var settings = new HammerSettings();

		//act
		var result = await command.ExecuteAsync(null!, settings);

		//assert
		Assert.Equal(1, result);
		Assert.Contains("exception", console.Output, StringComparison.InvariantCultureIgnoreCase);
	}

	[Fact]
	public async Task CannotHammerInReverse()
	{
		//arrange
		using var http = new HttpClient(new MockHttpMessageHandler());
		using var console = new TestConsole();
		var settings = new HammerSettings
		{
			URL = new Uri("http://localhost"),
			Min = 2,
			Max = 1
		};

		//act
		var command = new HammerCommand(http, null!, console);
		var result = await command.ExecuteAsync(null!, settings);


		//assert
		Assert.Equal(1, result);
	}
}