using System.Diagnostics;

namespace LoadTestToolbox;

public sealed class Worker
{
	private readonly HttpClient _httpClient;
	private readonly Func<HttpRequestMessage> _newMessage;
	private readonly Action<uint, double> _report;
	private readonly Action<string> _warn;

	public Worker(HttpClient httpClient, Func<HttpRequestMessage> newMessage, Action<uint, double> report, Action<string> warn)
	{
		_httpClient = httpClient;
		_newMessage = newMessage;
		_report = report;
		_warn = warn;
	}

	public async Task Run(uint request)
	{
		var timer = Stopwatch.StartNew();
		try
		{
			using var message = _newMessage();
			await _httpClient.SendAsync(message);
		}
		catch(Exception e)
		{
			_warn(e.Message);
		}
		finally
		{
			timer.Stop();
			_report(request, timer.Elapsed.TotalMilliseconds);
		}
	}
}