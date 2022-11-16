using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;

namespace LoadTestToolbox;

public sealed class MultilineChart : LineChart<Stats>
{
	public MultilineChart(IDictionary<uint, Stats> results) : base(results)
	{
	}

	protected override IReadOnlyCollection<LineSeries<ObservablePoint>> Series
		=> new[]
		{
			LineSeries(nameof(Stats.Max), _results.OrderBy(r => r.Key).Select(r => new ObservablePoint(r.Key, r.Value.Max)).ToArray(), SKColors.DarkRed),
			LineSeries(nameof(Stats.Median), _results.OrderBy(r => r.Key).Select(r => new ObservablePoint(r.Key, r.Value.Median)).ToArray(), SKColors.DarkOrange),
			LineSeries(nameof(Stats.Mean), _results.OrderBy(r => r.Key).Select(r => new ObservablePoint(r.Key, r.Value.Mean)).ToArray(), SKColors.Green),
			LineSeries(nameof(Stats.Min), _results.OrderBy(r => r.Key).Select(r => new ObservablePoint(r.Key, r.Value.Min)).ToArray(), SKColors.DodgerBlue)
		};

	protected override double YAxisMax
		=> GetYAxisMax(_results.Max(r => r.Value.Max));
}