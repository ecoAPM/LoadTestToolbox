using System.Collections.Concurrent;

namespace LoadTestToolbox.Tools.Hammer;

public sealed class Hammer(HttpClient http, Func<HttpRequestMessage> newMessage, Action notify, uint[] strengths)
	: Tool<Stats>(http, newMessage, notify)
{
	private readonly ConcurrentDictionary<uint, Result> _singleResults = new();

	public override ConcurrentDictionary<uint, Stats> Run()
	{
		var totals = new ConcurrentDictionary<uint, Stats>();
		foreach (var x in strengths)
		{
			var results = RunOnce(x);
			totals[x] = new Stats(results);
			Results.TryAdd(x, totals[x]);
		}

		return Results;
	}

	private ConcurrentDictionary<uint, Result> RunOnce(uint requests)
	{
		var threads = CreateThreads(requests);

		_singleResults.Clear();
		foreach (var thread in threads)
		{
			thread.Start();
		}

		WaitFor(threads);
		return _singleResults;
	}

	protected override void AddResult(uint request, Result result)
	{
		_singleResults.TryAdd(request, result);
		Notify();
	}
}