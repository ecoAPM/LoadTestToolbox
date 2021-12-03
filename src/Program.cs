namespace LoadTestToolbox;

public static class Program
{
	public static async Task<int> Main(string[] args)
		=> await Factory.App().RunAsync(args);
}