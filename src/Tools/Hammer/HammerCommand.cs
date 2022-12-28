using LoadTestToolbox.Charts;
using Spectre.Console;

namespace LoadTestToolbox.Tools.Hammer;

public sealed class HammerCommand : ToolCommand<HammerSettings>
{
	public HammerCommand(HttpClient httpClient, ChartIO io, IAnsiConsole console) : base(httpClient, io, console)
	{
	}

	protected override SkiaChart WieldTool(ProgressTask task, HammerSettings settings)
	{
		if (settings.Min > settings.Max)
		{
			throw new ArgumentException("Minimum cannot be greater than maximum", nameof(settings));
		}

		var carpenter = new Carpenter(_httpClient, task, settings);
		var results = carpenter.Run();
		WaitForProgressBarToCatchUp(task);

		var description = $"Hammer {settings.URL} from {settings.Min} to {settings.Max} simultaneous requests";
		return new MultilineChart(results, description);
	}
}