using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace LoadTestToolbox;

public abstract class Tool<T>
{
	public IDictionary<uint, T> Results => _results.ToImmutableDictionary();

	protected readonly IDictionary<uint, T> _results = new ConcurrentDictionary<uint, T>();

	private readonly Worker _worker;

	protected Tool(HttpClient http, Func<HttpRequestMessage> newMessage)
	{
		_worker = new Worker(http, newMessage, addResult);
		Prime(http, newMessage().RequestUri!).GetAwaiter().GetResult();
	}

	public abstract Task Run();

	public abstract bool Complete();

	private static async Task Prime(HttpClient httpClient, Uri url)
		=> await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));

	protected Thread[] CreateThreads(uint total)
	{
		var threads = new Thread[total];

		for (uint x = 0; x < total; x++)
		{
			var request = x;
#pragma warning disable 4014
			threads[x] = new Thread(() => _worker.Run(request))
#pragma warning restore 4014
			{
				Priority = ThreadPriority.Highest
			};
		}

		return threads;
	}

	protected abstract void addResult(uint request, double ms);

	protected static async Task True(Func<bool> expression)
	{
		while (!expression())
		{
			await Task.Delay(1);
		}
	}
}