using System.Collections.Concurrent;

namespace LoadTestToolbox.Tools.Nailgun;

public sealed class Nailgun(HttpClient http, Func<HttpRequestMessage> newMessage, Action notify, uint totalRequests)
	: Tool<Result>(http, newMessage, notify)
{
	public override ConcurrentDictionary<uint, Result> Run()
	{
		var threads = CreateThreads(totalRequests);

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