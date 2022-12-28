using Xunit;

namespace LoadTestToolbox.Tests;

public sealed class WorkerTests
{
	[Fact]
	public async Task ReportsDurationFromTimer()
	{
		//arrange
		var http = new HttpClient(new MockHttpMessageHandler());
		HttpRequestMessage NewMessage() => new(HttpMethod.Get, new Uri("http://localhost"));

		double result = 0;
		var worker = new Worker(http, NewMessage, (_, ms) => result = ms, _ => {});

		//act
		await worker.Run(0);

		//assert
		Assert.True(result > 0);
	}

	[Fact]
	public async Task ReportsAndWarnsWhenFailed()
	{
		//arrange
		var http = new HttpClient(new MockHttpMessageHandler());
		HttpRequestMessage NewMessage() => throw new Exception();

		double result = 0;
		var warning = string.Empty;

		var worker = new Worker(http, NewMessage, (_, ms) => result = ms,s => warning = s);

		//act
		await worker.Run(0);

		//assert
		Assert.True(result > 0);
		Assert.NotEmpty(warning);
	}
}