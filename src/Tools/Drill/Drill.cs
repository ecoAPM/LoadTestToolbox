using System.Collections.Concurrent;
using System.Diagnostics;

namespace LoadTestToolbox.Tools.Drill;

public sealed class Drill : Tool<Result>
{
	private readonly uint _totalRequests;
	private readonly long _delay;

	public Drill(HttpClient http, Func<HttpRequestMessage> newMessage, Action notify, uint requests, long delay) : base(http, newMessage, notify)
	{
		_totalRequests = requests;
		_delay = delay;
	}

	public override ConcurrentDictionary<uint, Result> Run()
	{
		var threads = CreateThreads(_totalRequests);

		uint started = 0;
		var timer = Stopwatch.StartNew();

		while (started < _totalRequests)
		{
			threads[started].Start();
			var nextStart = ++started * _delay;

			if (started < _totalRequests)
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