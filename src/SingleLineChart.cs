using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;

namespace LoadTestToolbox;

public sealed class SingleLineChart : LineChart<double>
{
	public SingleLineChart(IDictionary<uint, double> results) : base(results)
	{
	}

	protected override IReadOnlyCollection<LineSeries<ObservablePoint>> Series
		=> new[] { SingleLine };

	private LineSeries<ObservablePoint> SingleLine
		=> LineSeries("Response Time (ms)", _results.OrderBy(r => r.Key).Select(r => new ObservablePoint(r.Key, r.Value)).ToArray(), SKColors.DodgerBlue);

	protected override double YAxisMax
		=> GetYAxisMax(_results.Max(r => r.Value));
}