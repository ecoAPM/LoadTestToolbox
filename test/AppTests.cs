using System.CommandLine.IO;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LoadTestToolbox.Tests;

public class AppTests
{
	[Fact]
	public async Task ShowsHelpByDefault()
	{
		//arrange
		var http = new HttpClient(new MockHttpMessageHandler());
		var console = new TestConsole();
		var file = new byte[ushort.MaxValue];
		Stream writer(string s) => new MemoryStream(file);
		var app = new App(http, console, writer);

		//act
		await app.Run(System.Array.Empty<string>());

		//assert
		var output = console.Out.ToString() ?? "";
		Assert.Contains("Usage", output);
	}

	[Fact]
	public async Task DrillWritesFile()
	{
		//arrange
		var http = new HttpClient(new MockHttpMessageHandler());
		var console = new TestConsole();
		var file = new byte[ushort.MaxValue];
		Stream writer(string s) => new MemoryStream(file);
		var app = new App(http, console, writer);

		//act
		await app.Run(new[] { "drill", "-u", "http://localhost", "-r", "1", "-d", "1", "-f", "test.png" });

		//assert
		var contents = Encoding.UTF8.GetString(file);
		Assert.NotEmpty(contents);
	}

	[Fact]
	public async Task HammerWritesFile()
	{
		//arrange
		var http = new HttpClient(new MockHttpMessageHandler());
		var console = new TestConsole();
		var file = new byte[ushort.MaxValue];
		Stream writer(string s) => new MemoryStream(file);
		var app = new App(http, console, writer);

		//act
		await app.Run(new[] { "hammer", "-u", "http://localhost", "--min", "1", "--max", "1", "-f", "test.png" });

		//assert
		var contents = Encoding.UTF8.GetString(file);
		Assert.NotEmpty(contents);
	}
}
