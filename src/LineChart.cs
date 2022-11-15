using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;

namespace LoadTestToolbox;

public sealed class LineChart : SkiaChart
{
	private readonly IDictionary<uint, double> _results;

	public LineChart(IDictionary<uint, double> results)
		=> _results = results;

	protected override IEnumerable<LineSeries<ObservablePoint>> Series
		=> new[] { SingleLine };

	private LineSeries<ObservablePoint> SingleLine
		=> LineSeries("Response Time (ms)", _results.OrderBy(r => r.Key).Select(r => new ObservablePoint(r.Key, r.Value)), SKColors.DodgerBlue);

	protected override uint MinXAxis
		=> _results.Count > 1
			? _results.Min(r => r.Key)
			: 0;

	protected override uint MaxXAxis
		=> _results.Count > 1
			? _results.Max(r => r.Key)
			: _results.Max(r => r.Key) + 1;

	protected override double YAxisMax
		=> GetYAxisMax(_results.Max(r => r.Value));
}