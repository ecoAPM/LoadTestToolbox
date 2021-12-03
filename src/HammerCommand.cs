using Spectre.Console;

namespace LoadTestToolbox;

public sealed class HammerCommand : ToolCommand<HammerSettings>
{
	public HammerCommand(HttpClient httpClient, Func<string, Stream> fileWriter, IAnsiConsole console) : base(httpClient, fileWriter, console)
	{
	}

	protected override SkiaChart WieldTool(ProgressTask task, HammerSettings settings)
	{
		var carpenter = new Carpenter(_httpClient, task, settings);
		var results = carpenter.Run();

		return new MultilineChart(results);
	}
}