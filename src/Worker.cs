using System.Diagnostics;

namespace LoadTestToolbox;

public sealed class Worker(HttpClient httpClient, Func<HttpRequestMessage> newMessage, Action<uint, Result> report, Action<string> warn)
{
	public void Run(uint request)
	{
		try
		{
			using var message = newMessage();
			var timer = Stopwatch.StartNew();
			var response = httpClient.Send(message);
			timer.Stop();

			var result = new Result { ResponseCode = (int)response.StatusCode, Duration = timer.Elapsed.TotalMilliseconds };
			report(request, result);
		}
		catch(Exception e)
		{
			warn(e.Message);
			report(request, new Result());
		}
	}
}