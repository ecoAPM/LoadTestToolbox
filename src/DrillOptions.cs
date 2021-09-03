namespace LoadTestToolbox
{
	public class DrillOptions : ToolOptions<Drill>
	{
		public uint RPS { get; init; }
		public byte Duration { get; init; }
	}
}