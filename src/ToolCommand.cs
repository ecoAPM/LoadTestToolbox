using SkiaSharp;
using Spectre.Console;
using Spectre.Console.Cli;

namespace LoadTestToolbox;

public abstract class ToolCommand<T> : AsyncCommand<T> where T : ToolSettings
{
	protected readonly HttpClient _httpClient;
	private readonly Func<string, Stream> _fileWriter;
	private readonly IAnsiConsole _console;

	protected ToolCommand(HttpClient httpClient, Func<string, Stream> fileWriter, IAnsiConsole console)
	{
		_httpClient = httpClient;
		_fileWriter = fileWriter;
		_console = console;
	}

	public override async Task<int> ExecuteAsync(CommandContext context, T settings)
		=> await _console.Progress()
			.Columns(Columns)
			.StartAsync(async ctx => await Run(ctx, settings));

	private readonly ProgressColumn[] Columns =
	{
		new SpinnerColumn(),
		new TaskDescriptionColumn(),
		new ProgressBarColumn(),
		new PercentageColumn(),
		new LabelProgressColumn("[["),
		new ElapsedTimeColumn(),
		new LabelProgressColumn("elapsed /"),
		new RemainingTimeColumn(),
		new LabelProgressColumn("remaining ]]")
	};

	private async Task<int> Run(ProgressContext context, T settings)
	{
		try
		{
			var task = context.AddTask("Sending/receiving requests");
			var chart = WieldTool(task, settings);
			await SaveChart(chart, settings.Filename);

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

	private async Task SaveChart(SkiaChart chart, string filename)
	{
		var chartData = chart.GetChart();
		using var image = chartData.GetImage();
		using var imageData = image.Encode(SKEncodedImageFormat.Png, 100);
		var imageArray = imageData.ToArray();
		using var stream = new MemoryStream(imageArray);
		await using var output = _fileWriter(filename);
		await stream.CopyToAsync(output);
	}
}