using System.Text;
using LoadTestToolbox.Tools.Nailgun;
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
		var console = new TestConsole();
		var stream = new MemoryStream();
		Stream writer(string s) => stream;
		var command = new NailgunCommand(http, writer, console);
		var settings = new NailgunSettings
		{
			URL = new Uri("http://localhost"),
			Requests = 1,
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