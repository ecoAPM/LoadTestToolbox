using System.Collections.Concurrent;

namespace LoadTestToolbox.Tools;

public abstract class Wielder<T, R> where T : Tool<R>
{
	protected T Tool = null!;

	public ConcurrentDictionary<uint, R> Run()
		=> Tool.Run();
}