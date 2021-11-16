using Xunit;

namespace LoadTestToolbox.Tests;

public class WorkerTests
{
	[Fact]
	public async Task ReportsDurationFromTimer()
	{
		//arrange
		var http = new HttpClient(new MockHttpMessageHandler());
		var url = new Uri("http://localhost");

		double result = 0;
		var worker = new Worker(http, url, (_, ms) => result = ms);

		//act
		await worker.Run(0);

		//assert
		Assert.True(result > 0);
	}
}