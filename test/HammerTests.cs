using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace LoadTestToolbox.Tests;

public class HammerTests
{
	[Fact]
	public async Task NumberOfResultsMatchRequests()
	{
		//arrange
		var http = new HttpClient(new MockHttpMessageHandler());
		var hammer = new Hammer(http, new Uri("http://localhost"), new uint[] { 1, 2, 3, 4, 5 });

		//act
		await hammer.Run();

		//assert
		Assert.Equal(5, hammer.Results.Count);
		Assert.Equal((uint)1, hammer.Results.Min(r => r.Key));
		Assert.Equal((uint)5, hammer.Results.Max(r => r.Key));
	}
}
