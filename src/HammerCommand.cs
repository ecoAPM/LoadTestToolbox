using Spectre.Console;
using Spectre.Console.Cli;

namespace LoadTestToolbox;

public sealed class HammerCommand : ToolCommand<HammerSettings>
{
	public HammerCommand(HttpClient httpClient, Func<string, Stream> fileWriter, IAnsiConsole console) : base(httpClient, fileWriter, console)
	{
	}

	public override async Task<int> ExecuteAsync(CommandContext context, HammerSettings settings)
		=> await _console.Status().StartAsync("Running...", _ => Run(settings));

	private async Task<int> Run(HammerSettings settings)
	{
		try
		{
			var carpenter = new Carpenter(_httpClient, _console, settings);
			var results = await carpenter.Run();
			var chart = new LineChart(results.ToDictionary(r => r.Key, r => r.Value.Mean));
			await SaveChart(chart, settings.Filename);
			return 0;
		}
		catch (Exception e)
		{
			_console.WriteException(e, ExceptionFormats.ShortenEverything);
			return 1;
		}
	}
}