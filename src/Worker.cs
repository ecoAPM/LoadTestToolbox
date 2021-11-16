using System.Diagnostics;

namespace LoadTestToolbox;

public class Worker
{
	private readonly HttpClient _httpClient;
	private readonly Action<uint, double> _report;
	private readonly Uri _url;

	public Worker(HttpClient httpClient, Uri url, Action<uint, double> report)
	{
		_httpClient = httpClient;
		_url = url;
		_report = report;
	}

	public async Task Run(uint request)
	{
		var timer = Stopwatch.StartNew();
		await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, _url));
		timer.Stop();
		_report(request, timer.Elapsed.TotalMilliseconds);
	}
}