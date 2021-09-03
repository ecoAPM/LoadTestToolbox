namespace LoadTestToolbox
{
	public class HammerOptions : ToolOptions<Hammer>
	{
		public uint Min { get; init; }
		public uint Max { get; init; }
	}
}