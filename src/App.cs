using System.CommandLine;
using System.CommandLine.Invocation;

namespace LoadTestToolbox;

public class App
{
	private readonly HttpClient _http;
	private readonly IConsole _console;
	private readonly Func<string, Stream> _fileWriter;

	public App(HttpClient http, IConsole console, Func<string, Stream> fileWriter)
	{
		_http = http;
		_console = console;
		_fileWriter = fileWriter;
	}

	public async Task Run(string[] args)
		=> await Command.InvokeAsync(args, _console);

	private Command Command
	{
		get
		{
			var cmd = new Command("LoadTestToolbox", "Lightweight tools for load testing web applications");
			cmd.AddCommand(Drill);
			cmd.AddCommand(Hammer);

			return cmd;
		}
	}

	private Command Drill
	{
		get
		{
			var drill = new Command("drill", "Sends requests at a consistent rate")
				{
					AppOptions.URL,
					AppOptions.RPS,
					AppOptions.Duration,
					AppOptions.Filename
				};
			drill.Handler = CommandHandler.Create<string, uint, byte, string>(RunDrill);

			return drill;
		}
	}

	private Command Hammer
	{
		get
		{
			var hammer = new Command("hammer", "Sends increasing numbers of simultaneous requests")
				{
					AppOptions.URL,
					AppOptions.Min,
					AppOptions.Max,
					AppOptions.Filename
				};
			hammer.Handler = CommandHandler.Create<string, uint, uint, string>(RunHammer);

			return hammer;
		}
	}

	private async Task RunDrill(string url, uint rps, byte duration, string filename)
	{
		var options = new DrillOptions
		{
			URL = new Uri(url),
			RPS = rps,
			Duration = duration
		};
		var driller = new Driller(_http, _console, options);
		var results = await driller.Run();
		await SaveChart(results, filename);
	}

	private async Task RunHammer(string url, uint min, uint max, string filename)
	{
		var options = new HammerOptions
		{
			URL = new Uri(url),
			Min = min,
			Max = max
		};
		var carpenter = new Carpenter(_http, _console, options);
		var results = await carpenter.Run();
		await SaveChart(results, filename);
	}

	private async Task SaveChart(IDictionary<uint, double> results, string filename)
	{
		var chart = new SkiaChart(results);
		var output = _fileWriter(filename);
		await chart.Save(output);
	}
}