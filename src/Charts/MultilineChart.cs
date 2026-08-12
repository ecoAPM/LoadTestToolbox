using System.Collections.Concurrent;
using ScottPlot;
using ScottPlot.Plottables;
using SkiaSharp;

namespace LoadTestToolbox.Charts;

public sealed class MultilineChart(ConcurrentDictionary<uint, Stats> results, string description) : LineChart<Stats>(results, description)
{
	protected override Scatter[] Series
		=>
		[
			LineSeries(nameof(Stats.Max), [.. _results.OrderBy(r => r.Key).Select(r => new Coordinates(r.Key, r.Value.Max))], SKColors.DarkRed),
			LineSeries(nameof(Stats.Median), [.. _results.OrderBy(r => r.Key).Select(r => new Coordinates(r.Key, r.Value.Median))], SKColors.DarkOrange),
			LineSeries(nameof(Stats.Mean), [.. _results.OrderBy(r => r.Key).Select(r => new Coordinates(r.Key, r.Value.Mean))], SKColors.Green),
			LineSeries(nameof(Stats.Min), [.. _results.OrderBy(r => r.Key).Select(r => new Coordinates(r.Key, r.Value.Min))], SKColors.DodgerBlue)
		];

	protected override double YAxisMax
		=> GetYAxisMax(_results.Max(r => r.Value.Max));
}