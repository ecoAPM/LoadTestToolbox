using System.Collections.Concurrent;
using System.Diagnostics;

namespace LoadTestToolbox.Tools.Drill;

public sealed class Drill(HttpClient http, Func<HttpRequestMessage> newMessage, Action notify, uint requests, long delay)
	: Tool<Result>(http, newMessage, notify)
{
	public override ConcurrentDictionary<uint, Result> Run()
	{
		var threads = CreateThreads(requests);

		uint started = 0;
		var timer = Stopwatch.StartNew();

		while (started < requests)
		{
			threads[started].Start();
			var nextStart = ++started * delay;

			if (started < requests)
			{
				SpinWait.SpinUntil(() => timer.ElapsedTicks > nextStart);
			}
		}

		WaitFor(threads);
		return Results;
	}

	protected override void AddResult(uint request, Result result)
	{
		Results.TryAdd(request, result);
		Notify();
	}
}