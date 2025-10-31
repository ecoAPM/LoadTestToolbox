﻿using System.Collections.Concurrent;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;

namespace LoadTestToolbox.Charts;

public sealed class SingleLineChart : LineChart<double>
{
	public SingleLineChart(ConcurrentDictionary<uint, double> results, string description)
		: base(results, description)
	{
	}

	protected override LineSeries<ObservablePoint>[] Series
		=> [SingleLine];

	private LineSeries<ObservablePoint> SingleLine
		=> LineSeries("Response Time (ms)", _results.OrderBy(r => r.Key).Select(r => new ObservablePoint(r.Key, r.Value)).ToArray(), SKColors.DodgerBlue);

	protected override double YAxisMax
		=> GetYAxisMax(_results.Max(r => r.Value));
}