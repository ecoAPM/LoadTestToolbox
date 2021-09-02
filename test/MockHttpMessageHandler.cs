using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LoadTestToolbox.Tests
{
	internal class MockHttpMessageHandler : HttpMessageHandler
	{
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			return await Task.FromResult(new HttpResponseMessage());
		}
	}
}