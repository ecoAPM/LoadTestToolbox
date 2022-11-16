using Spectre.Console;

namespace LoadTestToolbox;

public sealed class NailgunCommand : ToolCommand<NailgunSettings>
{
	public NailgunCommand(HttpClient httpClient, Func<string, Stream> fileWriter, IAnsiConsole console) : base(httpClient, fileWriter, console)
	{
	}

	protected override SkiaChart WieldTool(ProgressTask task, NailgunSettings settings)
	{
		var nailer = new Nailer(_httpClient, task, settings);
		var results = nailer.Run();

		return new SingleLineChart(results);
	}
}