using System.Collections.Concurrent;

namespace LoadTestToolbox.Tools;

public abstract class Tool<T>
{
	protected readonly Action Notify;
	protected readonly ConcurrentDictionary<uint, T> Results = new();

	private readonly Worker _worker;

	protected Tool(HttpClient http, Func<HttpRequestMessage> newMessage, Action notify)
	{
		Notify = notify;
		_worker = new Worker(http, newMessage, AddResult, Console.WriteLine);
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
		=> new(() => _worker.Run(request))
		{
			Priority = ThreadPriority.Highest
		};

	protected abstract void AddResult(uint request, Result result);

	protected static void WaitFor(IEnumerable<Thread> threads)
	{
		foreach (var thread in threads)
		{
			thread.Join();
		}
	}
}