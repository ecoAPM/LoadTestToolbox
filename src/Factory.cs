using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace LoadTestToolbox;

public static class Factory
{
	public static CommandApp App()
	{
		var app = new CommandApp(DI());
		app.Configure(AddCommands);
		return app;
	}

	private static ITypeRegistrar DI()
	{
		var services = new ServiceCollection();
		services.AddSingleton<HttpClient>();
		services.AddSingleton<Func<string, Stream>>(filename => new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write));
		return new TypeRegistrar(services);
	}

	private static void AddCommands(IConfigurator config)
	{
		config.AddCommand<DrillCommand>("drill").WithDescription("Sends requests at a consistent rate");
		config.AddCommand<HammerCommand>("hammer").WithDescription("Sends increasing numbers of simultaneous requests");
	}

	public static HttpRequestMessage Message(ToolSettings settings)
	{
		var message = new HttpRequestMessage(HttpMethod(settings.Method), settings.URL)
		{
			Content = new StringContent(settings.Body)
		};

		foreach (var header in settings.Headers)
		{
			var split = header.Split(':');
			var name = split[0].Trim();
			var value = split[1].Trim();
			message.Headers.Add(name, value);
		}
		return message;
	}

	private static HttpMethod HttpMethod(string method)
		=> new(method.ToUpperInvariant());
}