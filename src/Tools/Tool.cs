using System.Collections.Concurrent;

namespace LoadTestToolbox.Tools;

public abstract class Tool<T>
{
	protected readonly Action _notify;
	protected readonly ConcurrentDictionary<uint, T> _results = new();

	private readonly Worker _worker;

	protected Tool(HttpClient http, Func<HttpRequestMessage> newMessage, Action notify)
	{
		_notify = notify;
		_worker = new Worker(http, newMessage, addResult, Console.WriteLine);
	}

	public abstract ConcurrentDictionary<uint, T> Run();

	protected Thread[] CreateThreads(uint total)
	{
		var threads = new Thread[total];

		for (uint x = 0; x < total; x++)
		{
			threads[x] = CreateThread(x);
		}

		return threads;
	}

	private Thread CreateThread(uint request)
	{
#pragma warning disable 4014
		return new Thread(() => _worker.Run(request))
#pragma warning restore 4014
		{
			Priority = ThreadPriority.Highest
		};
	}

	protected abstract void addResult(uint request, double ms);

	protected static void WaitFor(IEnumerable<Thread> threads)
	{
		foreach (var thread in threads)
		{
			thread.Join();
		}
	}
}