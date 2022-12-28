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
		WaitForProgressBarToCatchUp(task);

		var description = $"Drill {settings.URL} @ {settings.RPS} request{(settings.RPS > 1 ? "s" : string.Empty)} per second for {settings.Duration} second{(settings.Duration != 1 ? "s" : string.Empty)}";
		return new SingleLineChart(results, description);
	}
}