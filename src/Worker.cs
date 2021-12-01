using System.Diagnostics;

namespace LoadTestToolbox;

public sealed class Worker
{
	private readonly HttpClient _httpClient;
	private readonly Action<uint, double> _report;
	private readonly Func<HttpRequestMessage> _newMessage;

	public Worker(HttpClient httpClient, Func<HttpRequestMessage> newMessage, Action<uint, double> report)
	{
		_httpClient = httpClient;
		_newMessage = newMessage;
		_report = report;
	}

	public async Task Run(uint request)
	{
		var message = _newMessage();

		var timer = Stopwatch.StartNew();
		await _httpClient.SendAsync(message);
		timer.Stop();

		_report(request, timer.Elapsed.TotalMilliseconds);
	}
}