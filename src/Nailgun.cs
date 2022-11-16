using System.Collections.Concurrent;

namespace LoadTestToolbox;

public sealed class Nailgun : Tool<double>
{
	private readonly uint _totalRequests;

	public Nailgun(HttpClient http, Func<HttpRequestMessage> newMessage, Action notify, uint totalRequests) : base(http, newMessage, notify)
		=> _totalRequests = totalRequests;

	public override ConcurrentDictionary<uint, double> Run()
	{
		var threads = CreateThreads(_totalRequests);

		foreach (var thread in threads)
		{
			thread.Start();
		}

		WaitFor(threads);
		return _results;
	}

	protected override void addResult(uint request, double ms)
	{
		_results.TryAdd(request, ms);
		_notify();
	}
}