using System.Collections.Concurrent;

namespace LoadTestToolbox.Charts;

public abstract class LineChart<T>(ConcurrentDictionary<uint, T> results, string description) : SkiaChart
{
	protected override string Description { get; } = description;
	protected readonly ConcurrentDictionary<uint, T> _results = results;

	protected override uint MinXAxis
		=> _results.Count > 1
			? _results.Min(r => r.Key)
			: 0;

	protected override uint MaxXAxis
		=> _results.Count > 1
			? _results.Max(r => r.Key)
			: _results.Max(r => r.Key) + 1;
}