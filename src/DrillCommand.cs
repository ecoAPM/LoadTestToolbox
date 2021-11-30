using Spectre.Console;
using Spectre.Console.Cli;

namespace LoadTestToolbox;

public sealed class DrillCommand : ToolCommand<DrillSettings>
{
	public DrillCommand(HttpClient httpClient, Func<string, Stream> fileWriter, IAnsiConsole console) : base(httpClient, fileWriter, console)
	{
	}

	public override async Task<int> ExecuteAsync(CommandContext context, DrillSettings settings)
		=> await _console.Status().StartAsync("Running...", _ => Run(settings));

	private async Task<int> Run(DrillSettings settings)
	{
		try
		{
			var driller = new Driller(_httpClient, _console, settings);
			var results = await driller.Run();
			await SaveChart(results, settings.Filename);
			return 0;
		}
		catch (Exception e)
		{
			_console.WriteException(e, ExceptionFormats.ShortenEverything);
			return 1;
		}
	}
}