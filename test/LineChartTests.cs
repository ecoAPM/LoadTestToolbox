using System.Text;
using LiveChartsCore.Defaults;
using Xunit;

namespace LoadTestToolbox.Tests;

public sealed class LineChartTests
{
	[Fact]
	public void ChartContainsAllResults()
	{
		//arrange
		var results = new Dictionary<uint, double>
			{
				{ 1, 1.23 },
				{ 2, 2.34 },
				{ 3, 3.45 }
			};
		var skia = new LineChart(results);

		//act
		var chart = skia.Chart;

		//assert
		Assert.Equal(3, chart.Series.First().Values?.Cast<ObservablePoint>().Count());
	}

	[Theory]
	[InlineData(750, 800)]
	[InlineData(900, 1000)]
	[InlineData(1001, 1200)]
	public void YAxisRangeIsRounded(double max, double expected)
	{
		//arrange
		var results = new Dictionary<uint, double>
			{
				{ 1, max }
			};
		var skia = new LineChart(results);

		//act
		var chart = skia.Chart;

		//assert
		Assert.Equal(expected, chart.YAxes.First().MaxLimit);
	}

	[Fact]
	public void XAxisIsSortedAndBoundByValues()
	{
		//arrange
		var results = new Dictionary<uint, double>
			{
				{ 3, 2.34 },
				{ 5, 3.45 },
				{ 2, 1.23 }
			};
		var skia = new LineChart(results);

		//act
		var chart = skia.Chart;

		//assert
		Assert.Equal(2, chart.XAxes.First().MinLimit);
		Assert.Equal(5, chart.XAxes.First().MaxLimit);
	}

	[Fact]
	public async Task CanSaveChartToStream()
	{
		//arrange
		var results = new Dictionary<uint, double>
			{
				{ 2, 1.23 },
				{ 3, 2.34 },
				{ 5, 3.45 }
			};
		var chart = new LineChart(results);
		var data = new byte[ushort.MaxValue];
		var stream = new MemoryStream(data);

		//act
		await chart.Save(stream);

		//assert
		Assert.NotEmpty(Encoding.UTF8.GetString(data));
	}
}