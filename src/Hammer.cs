using System.Collections.Concurrent;

namespace LoadTestToolbox;

public sealed class Hammer : Tool<Stats>
{
	private readonly IReadOnlyCollection<uint> _strengths;
	private readonly ConcurrentDictionary<uint, double> _singleResults = new ConcurrentDictionary<uint, double>();

	public Hammer(HttpClient http, Func<HttpRequestMessage> newMessage, Action notify, IReadOnlyCollection<uint> strengths) : base(http, newMessage, notify)
		=> _strengths = strengths;

	public override ConcurrentDictionary<uint, Stats> Run()
	{
		var totals = new ConcurrentDictionary<uint, Stats>();
		foreach (var x in _strengths)
		{
			var results = RunOnce(x);
			totals[x] = new Stats(results);
			_results.TryAdd(x, totals[x]);
		}

		return _results;
	}

	private ConcurrentDictionary<uint, double> RunOnce(uint requests)
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

	protected override void addResult(uint request, double ms)
	{
		_singleResults.TryAdd(request, ms);
		_notify();
	}
}