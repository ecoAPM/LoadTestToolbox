namespace LoadTestToolbox.Tests;

internal class MockHttpMessageHandler : HttpMessageHandler
{
	protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
		=> new();

	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		=> throw new NotImplementedException();
}