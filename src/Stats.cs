using System.Collections.Concurrent;

namespace LoadTestToolbox;

public sealed record Stats
{
	public Stats(ConcurrentDictionary<uint, Result> results)
	{
		if (results.IsEmpty)
		{
			return;
		}

		var ordered = results.Select(r => r.Value)
			.OrderBy(v => v.Duration).ToArray();

		Min = ordered[0].Duration;
		Mean = ordered.Average(o => o.Duration);
		Median = GetMedian(ordered);
		Max = ordered[^1].Duration;
		ResponseCodes = ordered.GroupBy(o => o.ResponseCode).ToDictionary(o => o.Key, o => o.Count());
	}

	private static double GetMedian(Result[] results)
	{
		var halfIndex = results.Length / 2;
		return results.Length % 2 == 0
			? results[(halfIndex - 1)..(halfIndex + 1)].Average(r => r.Duration)
			: results[halfIndex].Duration;
	}

	public double Min { get; }
	public double Mean { get; }
	public double Median { get; }
	public double Max { get; }
	public Dictionary<int, int> ResponseCodes { get; } = null!;
}