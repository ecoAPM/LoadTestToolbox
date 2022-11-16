using Spectre.Console;

namespace LoadTestToolbox;

public sealed class DrillCommand : ToolCommand<DrillSettings>
{
	public DrillCommand(HttpClient httpClient, Func<string, Stream> fileWriter, IAnsiConsole console) : base(httpClient, fileWriter, console)
	{
	}

	protected override SkiaChart WieldTool(ProgressTask task, DrillSettings settings)
	{
		var driller = new Driller(_httpClient, task, settings);
		var results = driller.Run();

		return new SingleLineChart(results);
	}
}