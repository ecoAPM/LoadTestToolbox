namespace LoadTestToolbox.Tests;

internal class MockHttpMessageHandler : HttpMessageHandler
{
	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		=> await Task.FromResult(new HttpResponseMessage());
}