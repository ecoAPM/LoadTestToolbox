using Xunit;

namespace LoadTestToolbox.Tests;

public sealed class WorkerTests
{
	private static HttpRequestMessage GoodResponse()
	{
		Task.Delay(1).GetAwaiter().GetResult();
		return new HttpRequestMessage(HttpMethod.Get, new Uri("http://localhost"));
	}

	private static HttpRequestMessage BadResponse() => throw new Exception();


	[Fact]
	public void ReportsDurationFromTimer()
	{
		//arrange
		var http = new HttpClient(new MockHttpMessageHandler());

		double result = 0;
		var worker = new Worker(http, GoodResponse, (_, r) => result = r.Duration, _ => { });

		//act
		worker.Run(0);

		//assert
		Assert.True(result > 0);
	}

	[Fact]
	public void ReportsAndWarnsWhenFailed()
	{
		//arrange
		var http = new HttpClient(new MockHttpMessageHandler());

		double result = 0;
		var warning = string.Empty;

		var worker = new Worker(http, BadResponse, (_, r) => result = r.Duration, s => warning = s);

		//act
		worker.Run(0);

		//assert
		Assert.Equal(0, result);
		Assert.NotEmpty(warning);
	}
}