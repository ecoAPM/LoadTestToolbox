using System.Diagnostics;

namespace LoadTestToolbox;

public sealed class Worker
{
	private readonly HttpClient _httpClient;
	private readonly Func<HttpRequestMessage> _newMessage;
	private readonly Action<uint, Result> _report;
	private readonly Action<string> _warn;

	public Worker(HttpClient httpClient, Func<HttpRequestMessage> newMessage, Action<uint, Result> report, Action<string> warn)
	{
		_httpClient = httpClient;
		_newMessage = newMessage;
		_report = report;
		_warn = warn;
	}

	public void Run(uint request)
	{
		try
		{
			using var message = _newMessage();
			var timer = Stopwatch.StartNew();
			var response = _httpClient.Send(message);
			timer.Stop();

			var result = new Result { ResponseCode = (int)response.StatusCode, Duration = timer.Elapsed.TotalMilliseconds };
			_report(request, result);
		}
		catch(Exception e)
		{
			_warn(e.Message);
			_report(request, new Result());
		}
	}
}