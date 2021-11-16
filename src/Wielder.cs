using System.CommandLine;

namespace LoadTestToolbox;

public abstract class Wielder<T> where T : Tool
{
	protected readonly IConsole _console;
	protected T _tool = null!;

	protected Wielder(IConsole console)
	{
		_console = console;
	}

	public abstract Task<IDictionary<uint, double>> Run();

	public static string FormatTime(double ms)
	{
		return ms switch
		{
			< 1 => Math.Round(ms * 1000) + " μs",
			< 10 => Math.Round(ms, 2) + " ms",
			< 100 => Math.Round(ms, 1) + " ms",
			_ => Math.Round(ms) + " ms"
		};
	}
}