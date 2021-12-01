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
		var worker = new Worker(http, NewMessage, (_, ms) => result = ms);

		//act
		await worker.Run(0);

		//assert
		Assert.True(result > 0);
	}
}