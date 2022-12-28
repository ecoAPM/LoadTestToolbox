using Xunit;

namespace LoadTestToolbox.Tests.Tools.Nailgun;

public sealed class NailgunTests
{
	[Fact]
	public void NumberOfResultsMatchRequests()
	{
		//arrange
		var http = new HttpClient(new MockHttpMessageHandler());
		HttpRequestMessage newMessage() => new(HttpMethod.Get, new Uri("http://localhost"));
		var nailgun = new LoadTestToolbox.Tools.Nailgun.Nailgun(http, newMessage, () => { }, 5);

		//act
		var results = nailgun.Run();

		//assert
		Assert.Equal(5, results.Count);
	}
}