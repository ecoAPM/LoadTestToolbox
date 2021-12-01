using System.Collections.Concurrent;

namespace LoadTestToolbox;

public sealed class Hammer : Tool<Stats>
{
	private readonly IEnumerable<uint> _strengths;
	private readonly IDictionary<uint, double> _singleResults = new ConcurrentDictionary<uint, double>();

	public Hammer(HttpClient http, Func<HttpRequestMessage> newMessage, IEnumerable<uint> strengths) : base(http, newMessage)
		=> _strengths = strengths;

	public override async Task Run()
	{
		var totals = new ConcurrentDictionary<uint, Stats>();
		foreach (var x in _strengths)
		{
			var results = await RunOnce(x);
			totals[x] = new Stats
			{
				Min = results.Min(r => r.Value),
				Mean = results.Average(r => r.Value),
				Median = results.OrderBy(r => r.Key).Skip(results.Count/2).First().Value,
				Max = results.Max(r => r.Value)
			};
			_results.Add(x, totals[x]);
		}

		await True(Complete);
	}

	private async Task<IDictionary<uint, double>> RunOnce(uint requests)
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

		await True(() => _singleResults.Count == requests);
		return _singleResults;
	}

	public override bool Complete()
		=> _results.Count == _strengths.Count();

	protected override void addResult(uint request, double ms)
		=> _singleResults.Add(request, ms);
}