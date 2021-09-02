using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace LoadTestToolbox.Tests
{
	public class HammerTests
	{
		[Fact]
		public async Task NumberOfResultsMatchRequests()
		{
			//arrange
			var http = new HttpClient(new MockHttpMessageHandler());
			var url = new Uri("http://localhost");
			var hammer = new Hammer(http, url, 5);

			//act
			await hammer.Run();

			//assert
			Assert.Equal(5, hammer.Results.Count);
		}
	}
}