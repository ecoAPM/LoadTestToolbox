using System.Collections.Concurrent;

namespace LoadTestToolbox;

public abstract class Wielder<T, R> where T : Tool<R>
{
	protected T _tool = null!;

	public ConcurrentDictionary<uint, R> Run()
		=> _tool.Run();
}