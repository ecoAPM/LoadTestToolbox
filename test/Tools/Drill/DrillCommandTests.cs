using System.Text;
using LoadTestToolbox.Tools.Drill;
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
		using var console = new TestConsole();
		using var stream = new MemoryStream();
		Stream writer(string s) => stream;
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
		var contents = Encoding.UTF8.GetString(stream.GetBuffer());
		Assert.Empty(console.Output);
		Assert.Equal(0, result);
		Assert.NotEmpty(contents);
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