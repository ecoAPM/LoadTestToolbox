using System.Collections.Concurrent;

namespace LoadTestToolbox.Tools.Nailgun;

public sealed class Nailgun : Tool<Result>
{
	private readonly uint _totalRequests;

	public Nailgun(HttpClient http, Func<HttpRequestMessage> newMessage, Action notify, uint totalRequests) : base(http, newMessage, notify)
		=> _totalRequests = totalRequests;

	public override ConcurrentDictionary<uint, Result> Run()
	{
		var threads = CreateThreads(_totalRequests);

		foreach (var thread in threads)
		{
			thread.Start();
		}

		WaitFor(threads);
		return Results;
	}

	protected override void AddResult(uint request, Result result)
	{
		Results.TryAdd(request, result);
		Notify();
	}
}