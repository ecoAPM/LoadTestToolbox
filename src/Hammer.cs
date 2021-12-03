using System.Collections.Concurrent;

namespace LoadTestToolbox;

public sealed class Hammer : Tool<Stats>
{
	private readonly IEnumerable<uint> _strengths;
	private readonly IDictionary<uint, double> _singleResults = new ConcurrentDictionary<uint, double>();

	public Hammer(HttpClient http, Func<HttpRequestMessage> newMessage, Action notify, IEnumerable<uint> strengths) : base(http, newMessage, notify)
		=> _strengths = strengths;

	public override IDictionary<uint, Stats> Run()
	{
		var totals = new ConcurrentDictionary<uint, Stats>();
		foreach (var x in _strengths)
		{
			var results = RunOnce(x);
			totals[x] = new Stats(results);
			_results.Add(x, totals[x]);
		}

		return _results;
	}

	private IDictionary<uint, double> RunOnce(uint requests)
	{
		var threads = CreateThreads(requests);

		_singleResults.Clear();
		foreach (var thread in threads)
		{
			thread.Start();
		}

		foreach (var thread in threads)
		{
			thread.Join();
		}

		return _singleResults;
	}

	protected override void addResult(uint request, double ms)
	{
		_singleResults.Add(request, ms);
		_notify();
	}
}