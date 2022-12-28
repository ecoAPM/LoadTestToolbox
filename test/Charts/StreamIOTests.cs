using System.Text;
using LoadTestToolbox.Charts;
using Xunit;

namespace LoadTestToolbox.Tests.Charts;

public class StreamIOTests
{
	[Fact]
	public async Task CanSaveChartAsPNG()
	{
		//arrange
		var stream = new MemoryStream();
		Stream writer(string filename) => stream;
		var io = new StreamIO(writer);
		var data = new Dictionary<uint, double>
		{
			{ 1, 2 },
			{ 2, 4 },
			{ 3, 3 }
		}.AsConcurrent();
		var chart = new SingleLineChart(data, "test");

		//act
		await io.SaveChart(chart, "test.png");

		//assert
		var image = Encoding.UTF8.GetString(stream.GetBuffer());
		Assert.StartsWith("PNG", image[1..]);
	}
}