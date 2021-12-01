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
		=> new(HttpMethod(settings.Method), settings.URL);

	private static HttpMethod HttpMethod(string method)
		=> new(method.ToUpperInvariant());
}