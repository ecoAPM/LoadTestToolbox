namespace LoadTestToolbox;

public sealed record Stats
{
	public Stats(IDictionary<uint, double> results)
	{
		if (!results.Any())
		{
			return;
		}

		var ordered = results.Select(r => r.Value)
			.OrderBy(v => v).ToArray();

		Min = ordered.First();

		Mean = ordered.Average();

		Median = GetMedian(ordered);

		Max = ordered.Last();
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