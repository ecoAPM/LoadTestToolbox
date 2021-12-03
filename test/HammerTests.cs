using Xunit;

namespace LoadTestToolbox.Tests;

public sealed class HammerTests
{
	[Fact]
	public void NumberOfResultsMatchRequests()
	{
		//arrange
		var http = new HttpClient(new MockHttpMessageHandler());
		HttpRequestMessage NewMessage() => new(HttpMethod.Get, new Uri("http://localhost"));
		var hammer = new Hammer(http, NewMessage, () => { }, new uint[] { 1, 2, 3, 4, 5 });

		//act
		var results = hammer.Run();

		//assert
		Assert.Equal(5, results.Count);
		Assert.Equal((uint)1, results.Min(r => r.Key));
		Assert.Equal((uint)5, results.Max(r => r.Key));
	}
}