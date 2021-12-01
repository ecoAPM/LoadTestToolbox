using Spectre.Console.Cli;
using Xunit;

namespace LoadTestToolbox.Tests;

public sealed class FactoryTests
{
	[Fact]
	public void CanCreateApp()
	{
		//arrange/act
		var app = Factory.App();

		//assert
		Assert.IsType<CommandApp>(app);
	}

	[Fact]
	public void CanCreateMessage()
	{
		//arrange
		var settings = new DrillSettings
		{
			URL = new Uri("http://localhost"),
			Method = HttpMethod.Post.Method,
			Headers = new[] { "Authorization: Basic a1b2c3d4e5f6" }
		};

		//act
		var message = Factory.Message(settings);

		//assert
		Assert.Equal(HttpMethod.Post, message.Method);
		Assert.Contains("Authorization", message.Headers.Select(h => h.Key));
		Assert.Contains("Basic a1b2c3d4e5f6", message.Headers.Select(h => h.Value.FirstOrDefault()));
	}
}