using LoadTestToolbox.Charts;
using Spectre.Console;
using Spectre.Console.Cli;

namespace LoadTestToolbox.Tools;

public abstract class ToolCommand<T> : AsyncCommand<T> where T : ToolSettings
{
	protected readonly HttpClient HttpClient;
	private readonly IAnsiConsole _console;
	private readonly ChartIO _io;

	protected ToolCommand(HttpClient httpClient, ChartIO io, IAnsiConsole console)
	{
		HttpClient = httpClient;
		_io = io;
		_console = console;
	}

	public async Task<int> ExecuteAsync(CommandContext context, T settings)
		=> await ExecuteAsync(context, settings, CancellationToken.None);

	public override async Task<int> ExecuteAsync(CommandContext context, T settings, CancellationToken cancelToken)
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

	private void Prime(Uri url)
		=> HttpClient.Send(new HttpRequestMessage(HttpMethod.Head, url));

	private async Task<int> Run(ProgressContext context, T settings)
	{
		try
		{
			Prime(settings.URL!);
			var task = context.AddTask("Sending/receiving requests");
			var chart = await WieldTool(task, settings);
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

	protected abstract Task<SkiaChart> WieldTool(ProgressTask task, T settings);

	protected static async Task ProgressBarCompletion(ProgressTask task)
	{
		while (!task.IsFinished)
		{
			await Task.Delay(0);
		}
	}
}