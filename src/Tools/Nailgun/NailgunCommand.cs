using LoadTestToolbox.Charts;
using Spectre.Console;

namespace LoadTestToolbox.Tools.Nailgun;

public sealed class NailgunCommand : ToolCommand<NailgunSettings>
{
	public NailgunCommand(HttpClient httpClient, ChartIO io, IAnsiConsole console) : base(httpClient, io, console)
	{
	}

	protected override SkiaChart WieldTool(ProgressTask task, NailgunSettings settings)
	{
		var nailer = new Nailer(_httpClient, task, settings);
		var results = nailer.Run();
		WaitForProgressBarToCatchUp(task);

		var description = $"Nailgun {settings.URL} with {settings.Requests} request{(settings.Requests != 1 ? "s" : string.Empty)}";
		return new SingleLineChart(results, description);
	}
}