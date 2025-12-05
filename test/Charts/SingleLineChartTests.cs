using LiveChartsCore.Defaults;
using LoadTestToolbox.Charts;
using Xunit;

namespace LoadTestToolbox.Tests.Charts;

public sealed class SingleLineChartTests
{
	[Fact]
	public void ChartContainsAllResults()
	{
		//arrange
		var results = new Dictionary<uint, Result>
		{
			{ 1, new Result(200, 1.23) },
			{ 2, new Result(200, 2.34) },
			{ 3, new Result(200, 3.45) }
		}.AsConcurrent();

		var skia = new SingleLineChart(results, string.Empty);

		//act
		var chart = skia.GetChart();

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
		var results = new Dictionary<uint, Result>
		{
			{ 1, new Result(200, max) }
		}.AsConcurrent();

		var skia = new SingleLineChart(results, string.Empty);

		//act
		var chart = skia.GetChart();

		//assert
		Assert.Equal(expected, chart.YAxes.First().MaxLimit);
	}

	[Fact]
	public void XAxisIsSortedAndBoundByValues()
	{
		//arrange
		var results = new Dictionary<uint, Result>
		{
			{ 3, new Result(200, 2.34) },
			{ 5, new Result(200, 3.45) },
			{ 2, new Result(200, 1.23) }
		}.AsConcurrent();

		var skia = new SingleLineChart(results, string.Empty);

		//act
		var chart = skia.GetChart();

		//assert
		Assert.Equal(2, chart.XAxes.First().MinLimit);
		Assert.Equal(5, chart.XAxes.First().MaxLimit);
	}

	[Fact]
	public void XAxisCanHandleSingleValues()
	{
		//arrange
		var results = new Dictionary<uint, Result>
		{
			{ 3, new Result(200, 2.34) }
		}.AsConcurrent();

		var skia = new SingleLineChart(results, string.Empty);

		//act
		var chart = skia.GetChart();

		//assert
		Assert.Equal(0, chart.XAxes.First().MinLimit);
		Assert.Equal(4, chart.XAxes.First().MaxLimit);
	}
}