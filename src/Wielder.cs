namespace LoadTestToolbox;

public abstract class Wielder<T, R> where T : Tool<R>
{
	protected T _tool = null!;

	public IDictionary<uint, R> Run()
		=> _tool.Run();

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