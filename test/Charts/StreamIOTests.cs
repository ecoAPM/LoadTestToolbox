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
		Stream Writer(string filename) => stream;
		var io = new StreamIO(Writer);
		var data = new Dictionary<uint, Result>
		{
			{ 1, new Result(200, 2) },
			{ 2, new Result(200, 4) },
			{ 3, new Result(200, 3) }
		}.AsConcurrent();
		var chart = new SingleLineChart(data, "test");

		//act
		await io.SaveChart(chart, "test.png");

		//assert
		var image = Encoding.UTF8.GetString(stream.GetBuffer());
		Assert.StartsWith("PNG", image[1..]);
	}
}