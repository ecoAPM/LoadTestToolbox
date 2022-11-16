namespace LoadTestToolbox;

public abstract class LineChart<T> : SkiaChart
{
	protected readonly IDictionary<uint, T> _results;

	protected LineChart(IDictionary<uint, T> results)
		=> _results = results;

	protected override uint MinXAxis
		=> _results.Count > 1
			? _results.Min(r => r.Key)
			: 0;

	protected override uint MaxXAxis
		=> _results.Count > 1
			? _results.Max(r => r.Key)
			: _results.Max(r => r.Key) + 1;
}