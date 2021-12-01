using System.Text;
using Spectre.Console.Testing;
using Xunit;

namespace LoadTestToolbox.Tests;

public sealed class DrillCommandTests
{
	[Fact]
	public async Task WritesFileOnSuccess()
	{
		//arrange
		var http = new HttpClient(new MockHttpMessageHandler());
		var console = new TestConsole();
		var file = new byte[ushort.MaxValue];
		Stream writer(string s) => new MemoryStream(file);
		var command = new DrillCommand(http, writer, console);
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
		var contents = Encoding.UTF8.GetString(file);
		Assert.Equal(0, result);
		Assert.NotEmpty(contents);
	}

	[Fact]
	public async Task ShowsExceptionOnFailure()
	{
		//arrange
		var http = new HttpClient(new MockHttpMessageHandler());
		var console = new TestConsole();
		var file = new byte[ushort.MaxValue];
		Stream writer(string s) => new MemoryStream(file);
		var command = new DrillCommand(http, writer, console);
		var settings = new DrillSettings();

		//act
		var result = await command.ExecuteAsync(null!, settings);

		//assert
		Assert.Equal(1, result);
		Assert.Contains("exception", console.Output, StringComparison.InvariantCultureIgnoreCase);
	}
}