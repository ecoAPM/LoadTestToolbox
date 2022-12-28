using LoadTestToolbox.Charts;
using Spectre.Console;

namespace LoadTestToolbox.Tools.Drill;

public sealed class DrillCommand : ToolCommand<DrillSettings>
{
	public DrillCommand(HttpClient httpClient, ChartIO io, IAnsiConsole console) : base(httpClient, io, console)
	{
	}

	protected override SkiaChart WieldTool(ProgressTask task, DrillSettings settings)
	{
		var driller = new Driller(_httpClient, task, settings);
		var results = driller.Run();

		var description = $"Drill {settings.URL} @ {settings.RPS} req/sec for {settings.Duration} sec";
		return new SingleLineChart(results, description);
	}
}