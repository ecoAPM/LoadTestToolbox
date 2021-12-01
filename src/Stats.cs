namespace LoadTestToolbox;

public sealed record Stats
{
	public double Min { get; init; }
	public double Mean { get; init; }
	public double Median { get; init; }
	public double Max{ get; init; }
}