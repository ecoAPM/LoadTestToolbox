using System.Diagnostics;

namespace LoadTestToolbox;

public sealed class Drill : Tool<double>
{
	private readonly uint _totalRequests;
	private readonly long _delay;

	public Drill(HttpClient http, Func<HttpRequestMessage> newMessage, uint requests, long delay) : base(http, newMessage)
	{
		_totalRequests = requests;
		_delay = delay;
	}

	public override async Task Run()
	{
		var threads = CreateThreads(_totalRequests);

		uint started = 0;
		var timer = Stopwatch.StartNew();

		while (started < _totalRequests)
		{
			threads[started].Start();
			var nextStart = ++started * _delay;

			if (started < _totalRequests)
			{
				SpinWait.SpinUntil(() => timer.ElapsedTicks > nextStart);
			}
		}

		foreach (var thread in threads)
		{
			thread.Join();
		}

		await True(Complete);
	}

	public override bool Complete()
		=> _results.Count == _totalRequests;

	protected override void addResult(uint request, double ms)
		=> _results.Add(request, ms);
}