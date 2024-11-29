using System.Collections.Concurrent;

namespace LoadTestToolbox;

public sealed record Stats
{
	public Stats(ConcurrentDictionary<uint, double> results)
	{
		if (results.IsEmpty)
		{
			return;
		}

		var ordered = results.Select(r => r.Value)
			.OrderBy(v => v).ToArray();

		Min = ordered[0];
		Mean = ordered.Average();
		Median = GetMedian(ordered);
		Max = ordered[^1];
	}

	private static double GetMedian(double[] values)
	{
		var halfIndex = values.Length / 2;
		return values.Length % 2 == 0
			? values[(halfIndex - 1)..(halfIndex + 1)].Average()
			: values[halfIndex];
	}

	public double Min { get; }
	public double Mean { get; }
	public double Median { get; }
	public double Max { get; }
}