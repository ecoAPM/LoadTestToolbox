using LoadTestToolbox.Charts;
using Spectre.Console;

namespace LoadTestToolbox.Tools.Nailgun;

public sealed class NailgunCommand : ToolCommand<NailgunSettings>
{
	public NailgunCommand(HttpClient httpClient, ChartIO io, IAnsiConsole console) : base(httpClient, io, console)
	{
	}

	protected override async Task<SkiaChart> WieldTool(ProgressTask task, NailgunSettings settings)
	{
		var nailer = new Nailer(HttpClient, task, settings);
		var results = nailer.Run();
		await ProgressBarCompletion(task);

		var description = $"Nailgun {settings.URL} with {settings.Requests} request{(settings.Requests != 1 ? "s" : string.Empty)}";
		return new SingleLineChart(results, description);
	}
}