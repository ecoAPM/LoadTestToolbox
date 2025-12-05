using System.Collections.Concurrent;
using LiveChartsCore.Defaults;
using LoadTestToolbox.Charts;
using Xunit;

namespace LoadTestToolbox.Tests.Charts;

public sealed class MultilineChartTests
{
	[Fact]
	public void ChartContainsAllResults()
	{
		//arrange
		var results = new Dictionary<uint, Stats>
		{
			{ 1, new Stats(new ConcurrentDictionary<uint, Result>()) },
			{ 2, new Stats(new ConcurrentDictionary<uint, Result>()) },
			{ 3, new Stats(new ConcurrentDictionary<uint, Result>()) }
		}.AsConcurrent();

		var skia = new MultilineChart(results, string.Empty);

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
		var results = new Dictionary<uint, Stats>
		{
			{ 1, new Stats(new Dictionary<uint, Result> { { 1, new Result(200, max) } }.AsConcurrent()) }
		}.AsConcurrent();

		var skia = new MultilineChart(results, string.Empty);

		//act
		var chart = skia.GetChart();

		//assert
		Assert.Equal(expected, chart.YAxes.First().MaxLimit);
	}

	[Fact]
	public void XAxisIsSortedAndBoundByValues()
	{
		//arrange
		var results = new Dictionary<uint, Stats>
		{
			{ 3, new Stats(new ConcurrentDictionary<uint, Result>()) },
			{ 5, new Stats(new ConcurrentDictionary<uint, Result>()) },
			{ 2, new Stats(new ConcurrentDictionary<uint, Result>()) }
		}.AsConcurrent();

		var skia = new MultilineChart(results, string.Empty);

		//act
		var chart = skia.GetChart();

		//assert
		Assert.Equal(2, chart.XAxes.First().MinLimit);
		Assert.Equal(5, chart.XAxes.First().MaxLimit);
	}

	[Fact]
	public void XAxisCanHandleSingleValue()
	{
		//arrange
		var results = new Dictionary<uint, Stats>
		{
			{ 3, new Stats(new ConcurrentDictionary<uint, Result>()) }
		}.AsConcurrent();

		var skia = new MultilineChart(results, string.Empty);

		//act
		var chart = skia.GetChart();

		//assert
		Assert.Equal(0, chart.XAxes.First().MinLimit);
		Assert.Equal(4, chart.XAxes.First().MaxLimit);
	}
}