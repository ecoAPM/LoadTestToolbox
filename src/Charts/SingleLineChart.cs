using System.Collections.Concurrent;
using ScottPlot;
using ScottPlot.Plottables;
using SkiaSharp;

namespace LoadTestToolbox.Charts;

public sealed class SingleLineChart(ConcurrentDictionary<uint, Result> results, string description) : LineChart<Result>(results, description)
{
	protected override Scatter[] Series
		=> [SingleLine];

	private Scatter SingleLine
		=> LineSeries("Response Time (ms)", [.. _results.OrderBy(r => r.Key).Select(r => new Coordinates(r.Key, r.Value.Duration))], SKColors.DodgerBlue);

	protected override double YAxisMax
		=> GetYAxisMax(_results.Max(r => r.Value.Duration));
}