namespace LoadTestToolbox;

public abstract class Wielder<T, R> where T : Tool<R>
{
	protected T _tool = null!;

	public IDictionary<uint, R> Run()
		=> _tool.Run();
}