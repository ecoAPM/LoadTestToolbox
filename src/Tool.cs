using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace LoadTestToolbox;

public abstract class Tool<T>
{
	public IDictionary<uint, T> Results => _results.ToImmutableDictionary();
	protected readonly IDictionary<uint, T> _results = new ConcurrentDictionary<uint, T>();

	protected readonly Worker _worker;

	protected Tool(HttpClient http, Uri url)
	{
		_worker = new Worker(http, url, addResult);
		Prime(http, url).GetAwaiter().GetResult();
	}

	public abstract Task Run();

	public abstract bool Complete();

	private static async Task Prime(HttpClient httpClient, Uri url)
		=> await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));

	protected abstract void addResult(uint request, double ms);

	protected static async Task True(Func<bool> expression)
	{
		while (!expression())
		{
			await Task.Delay(1);
		}
	}
}