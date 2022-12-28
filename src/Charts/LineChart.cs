using System.Collections.Concurrent;

namespace LoadTestToolbox.Charts;

public abstract class LineChart<T> : SkiaChart
{
	protected override string Description { get; }
	protected readonly ConcurrentDictionary<uint, T> _results;

	protected LineChart(ConcurrentDictionary<uint, T> results, string description)
	{
		Description = description;
		_results = results;
	}

	protected override uint MinXAxis
		=> _results.Count > 1
			? _results.Min(r => r.Key)
			: 0;

	protected override uint MaxXAxis
		=> _results.Count > 1
			? _results.Max(r => r.Key)
			: _results.Max(r => r.Key) + 1;
}