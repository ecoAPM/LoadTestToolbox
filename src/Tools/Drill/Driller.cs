using System.Diagnostics;
using Spectre.Console;

namespace LoadTestToolbox.Tools.Drill;

public sealed class Driller : Wielder<Drill, Result>
{
	public Driller(HttpClient http, ProgressTask task, DrillSettings settings)
	{
		var requests = (uint)settings.RPS * settings.Duration;
		var delay = Stopwatch.Frequency / settings.RPS;
		task.MaxValue(requests);
		Tool = new Drill(http, () => Factory.Message(settings), () => task.Increment(1), requests, delay);
	}
}