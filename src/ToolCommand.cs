using Spectre.Console;
using Spectre.Console.Cli;

namespace LoadTestToolbox;

public abstract class ToolCommand<T> : AsyncCommand<T> where T : CommandSettings
{
	protected readonly HttpClient _httpClient;
	protected readonly Func<string, Stream> _fileWriter;
	protected readonly IAnsiConsole _console;

	protected ToolCommand(HttpClient httpClient, Func<string, Stream> fileWriter, IAnsiConsole console)
	{
		_httpClient = httpClient;
		_fileWriter = fileWriter;
		_console = console;
	}

	protected async Task SaveChart(SkiaChart chart, string filename)
	{
		await using var output = _fileWriter(filename);
		await chart.Save(output);
	}
}