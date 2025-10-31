using LoadTestToolbox.Charts;
using LoadTestToolbox.Tools;
using LoadTestToolbox.Tools.Drill;
using LoadTestToolbox.Tools.Hammer;
using LoadTestToolbox.Tools.Nailgun;
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

	private static TypeRegistrar DI()
	{
		var services = new ServiceCollection();
		services.AddSingleton<HttpClient>();
		services.AddSingleton<ChartIO, StreamIO>();
		services.AddSingleton<Func<string, Stream>>(filename => new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write));
		return new TypeRegistrar(services);
	}

	private static void AddCommands(IConfigurator config)
	{
		config.AddCommand<DrillCommand>("drill").WithDescription("Send requests at a consistent rate");
		config.AddCommand<HammerCommand>("hammer").WithDescription("Send increasing numbers of simultaneous requests");
		config.AddCommand<NailgunCommand>("nailgun").WithDescription("Send a large number of requests all at once");
	}

	public static HttpRequestMessage Message(ToolSettings settings)
	{
		var message = new HttpRequestMessage(HttpMethod(settings.Method), settings.URL)
		{
			Content = new StringContent(settings.Body)
		};

		var contentHeaders = settings.Headers.Where(h => h == "Allow" || h.StartsWith("Content")).ToArray();
		foreach (var header in contentHeaders)
		{
			var split = header.Split(':');
			var name = split[0].Trim();
			var value = split[1].Trim();

			if (message.Content.Headers.Any(h => h.Key == name))
				message.Content.Headers.Remove(name);
			message.Content.Headers.Add(name, value);
		}

		var requestHeaders = settings.Headers.Except(contentHeaders).ToArray();
		foreach (var header in requestHeaders)
		{
			var split = header.Split(':');
			var name = split[0].Trim();
			var value = split[1].Trim();

			if (message.Headers.Any(h => h.Key == name))
				message.Headers.Remove(name);
			message.Headers.Add(name, value);
		}

		return message;
	}

	private static HttpMethod HttpMethod(string method)
		=> new(method.ToUpperInvariant());
}