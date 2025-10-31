using System.Text.Json;
using LoadTestToolbox.Tools.Drill;
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
	public async Task CanCreateComplexMessage()
	{
		//arrange
		var settings = new DrillSettings
		{
			URL = new Uri("http://localhost"),
			Method = HttpMethod.Post.Method,
			Headers = ["Authorization: Basic a1b2c3d4e5f6", "Content-Type: application/json"],
			Body = """{"key123":"value456"}"""
		};

		//act
		var message = Factory.Message(settings);

		//assert
		Assert.Equal(HttpMethod.Post, message.Method);
		Assert.Contains("Authorization", message.Headers.Select(h => h.Key));
		Assert.Contains("Basic a1b2c3d4e5f6", message.Headers.Select(h => h.Value.FirstOrDefault()));
		Assert.Contains("Content-Type", message.Content!.Headers.Select(h => h.Key));
		Assert.Contains("application/json", message.Content!.Headers.Select(h => h.Value.FirstOrDefault()));

		var json = await JsonSerializer.DeserializeAsync<JsonElement>(await message.Content!.ReadAsStreamAsync());
		Assert.Equal("value456", json.GetProperty("key123").GetString());
	}
}