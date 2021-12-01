using Spectre.Console;

namespace LoadTestToolbox;

public abstract class Wielder<T,R> where T : Tool<R>
{
	protected readonly IAnsiConsole _console;
	protected T _tool = null!;

	protected Wielder(IAnsiConsole console)
		=> _console = console;

	public abstract Task<IDictionary<uint, R>> Run();

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