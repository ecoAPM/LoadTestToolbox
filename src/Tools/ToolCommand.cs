using LoadTestToolbox.Charts;
using Spectre.Console;
using Spectre.Console.Cli;

namespace LoadTestToolbox.Tools;

public abstract class ToolCommand<T> : AsyncCommand<T> where T : ToolSettings
{
	protected readonly HttpClient _httpClient;
	private readonly IAnsiConsole _console;
	private readonly ChartIO _io;

	protected ToolCommand(HttpClient httpClient, ChartIO io, IAnsiConsole console)
	{
		_httpClient = httpClient;
		_io = io;
		_console = console;
	}

	public override async Task<int> ExecuteAsync(CommandContext context, T settings)
		=> await _console.Progress()
			.Columns(_columns)
			.StartAsync(async ctx => await Run(ctx, settings));

	private readonly ProgressColumn[] _columns =
	[
		new SpinnerColumn(),
		new TaskDescriptionColumn(),
		new ProgressBarColumn(),
		new PercentageColumn(),
		new LabelProgressColumn("[["),
		new ElapsedTimeColumn(),
		new LabelProgressColumn("elapsed /"),
		new RemainingTimeColumn(),
		new LabelProgressColumn("remaining ]]")
	];

	private async Task Prime(Uri url)
		=> await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));

	private async Task<int> Run(ProgressContext context, T settings)
	{
		try
		{
			await Prime(settings.URL!);
			var task = context.AddTask("Sending/receiving requests");
			var chart = WieldTool(task, settings);
			await _io.SaveChart(chart, settings.Filename);

			context.Refresh();
			return 0;
		}
		catch (Exception e)
		{
			_console.WriteException(e, ExceptionFormats.ShortenEverything);
			return 1;
		}
	}

	protected abstract SkiaChart WieldTool(ProgressTask task, T settings);

	protected static void WaitForProgressBarToCatchUp(ProgressTask task)
	{
		while (!task.IsFinished)
		{
			Thread.Sleep(1);
		}
	}
}