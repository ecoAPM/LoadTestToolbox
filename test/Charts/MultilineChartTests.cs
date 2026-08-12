using System.Collections.Concurrent;
using LoadTestToolbox.Charts;
using ScottPlot.Plottables;
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

		var chart = new MultilineChart(results, string.Empty);

		//act
		var plot = chart.GetChart();

		//assert
		Assert.Equal(3, plot.GetPlottables().Cast<Scatter>().First().Data.GetScatterPoints().Count);
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

		var chart = new MultilineChart(results, string.Empty);

		//act
		var plot = chart.GetChart();

		//assert
		Assert.Equal(expected, plot.Axes.Left.Max);
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

		var chart = new MultilineChart(results, string.Empty);

		//act
		var plot = chart.GetChart();

		//assert
		Assert.Equal(2, plot.Axes.Bottom.Min);
		Assert.Equal(5, plot.Axes.Bottom.Max);
	}

	[Fact]
	public void XAxisCanHandleSingleValue()
	{
		//arrange
		var results = new Dictionary<uint, Stats>
		{
			{ 3, new Stats(new ConcurrentDictionary<uint, Result>()) }
		}.AsConcurrent();

		var chart = new MultilineChart(results, string.Empty);

		//act
		var plot = chart.GetChart();

		//assert
		Assert.Equal(0, plot.Axes.Bottom.Min);
		Assert.Equal(4, plot.Axes.Bottom.Max);
	}
}