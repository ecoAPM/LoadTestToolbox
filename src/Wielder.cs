using Spectre.Console;

namespace LoadTestToolbox;

public abstract class Wielder<T, R> where T : Tool<R>
{
	protected readonly IAnsiConsole _console;
	protected T _tool = null!;

	protected Wielder(IAnsiConsole console)
		=> _console = console;

	public async Task<IDictionary<uint, R>> Run()
	{
#pragma warning disable 4014
		var thread = new Thread(() => _tool.Run())
#pragma warning restore 4014
		{
			Priority = ThreadPriority.Highest
		};

		thread.Start();
		await OutputProgress();
		thread.Join();

		return _tool.Results;
	}

	protected abstract Task OutputProgress();

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