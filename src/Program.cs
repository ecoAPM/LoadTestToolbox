namespace LoadTestToolbox;

public static class Program
{
	static Program() => Thread.CurrentThread.Priority = ThreadPriority.Highest;

	public static async Task<int> Main(string[] args)
		=> await Factory.App().RunAsync(args);
}