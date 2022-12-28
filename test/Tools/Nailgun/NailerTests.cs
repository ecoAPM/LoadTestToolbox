using LoadTestToolbox.Tools.Nailgun;
using Spectre.Console;
using Xunit;

namespace LoadTestToolbox.Tests.Tools.Nailgun;

public sealed class NailerTests
{
	[Fact]
	public void CanNail()
	{
		//arrange
		var http = new HttpClient(new MockHttpMessageHandler());
		var task = new ProgressTask(123, "test", 0, false);
		var options = new NailgunSettings()
		{
			URL = new Uri("http://localhost"),
			Requests = 5
		};
		var driller = new Nailer(http, task, options);

		//act
		var results = driller.Run();

		//assert
		Assert.Equal(5, results.Count);
	}
}