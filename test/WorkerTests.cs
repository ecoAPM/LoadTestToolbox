using Xunit;

namespace LoadTestToolbox.Tests;

public sealed class WorkerTests
{
	[Fact]
	public void ReportsDurationFromTimer()
	{
		//arrange
		var http = new HttpClient(new MockHttpMessageHandler());
		HttpRequestMessage NewMessage()
		{
			Thread.Sleep(1);
			return new HttpRequestMessage(HttpMethod.Get, new Uri("http://localhost"));
		}

		double result = 0;
		var worker = new Worker(http, NewMessage, (_, r) => result = r.Duration, _ => {});

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
		HttpRequestMessage NewMessage() => throw new Exception();

		double result = 0;
		var warning = string.Empty;

		var worker = new Worker(http, NewMessage, (_, r) => result = r.Duration,s => warning = s);

		//act
		worker.Run(0);

		//assert
		Assert.Equal(0, result);
		Assert.NotEmpty(warning);
	}
}