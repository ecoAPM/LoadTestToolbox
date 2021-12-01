using System.Diagnostics;
using Spectre.Console;

namespace LoadTestToolbox;

public sealed class Driller : Wielder<Drill, double>
{
	private readonly uint _rps;

	public Driller(HttpClient http, IAnsiConsole console, DrillSettings settings) : base(console)
	{
		var requests = settings.RPS * settings.Duration;
		var delay = Stopwatch.Frequency / settings.RPS;
		_tool = new Drill(http, () => Factory.Message(settings), requests, delay);
		_rps = settings.RPS;
	}

	protected override async Task OutputProgress()
	{
		var started = DateTime.UtcNow;
		var previewed = 0;

		while (!_tool.Complete())
		{
			await Task.Delay(1);

			var secondsSinceStart = DateTime.UtcNow.Subtract(started).Seconds;
			if (!_tool.Results.Any() || secondsSinceStart <= previewed)
			{
				continue;
			}

			var alreadyPreviewed = (secondsSinceStart - 1) * _rps;
			var lastSecondOfResults = _tool.Results.Skip((int)alreadyPreviewed).ToArray();
			if (!lastSecondOfResults.Any())
			{
				continue;
			}

			var average = lastSecondOfResults.Average(r => r.Value);
			_console.WriteLine(secondsSinceStart + ": " + FormatTime(average));
			previewed = secondsSinceStart;
		}
	}
}