using LiveChartsCore.Defaults;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.SKCharts;
using SkiaSharp;

namespace LoadTestToolbox.Charts;

public abstract class SkiaChart
{
	protected abstract IReadOnlyCollection<LineSeries<ObservablePoint>> Series { get; }
	protected abstract uint MinXAxis { get; }
	protected abstract uint MaxXAxis { get; }
	protected abstract double YAxisMax { get; }

	protected abstract string Description { get; }

	private static readonly SolidColorPaint DefaultText = new(SKColors.Black) { FontFamily = FontManager.DefaultFont };
	private static readonly SolidColorPaint PaleGreyLine = new(SKColors.Black.WithAlpha(24), 1);

	private static readonly object chartLock = new();

	public SKCartesianChart GetChart()
	{
		lock (chartLock)
		{
			return new SKCartesianChart
			{
				Width = 1280,
				Height = 720,
				Background = SKColors.White,
				XAxes = new[] { XAxis },
				YAxes = new[] { YAxis },
				Series = Series,
				LegendPosition = Series.Count > 1 ? LegendPosition.Bottom : LegendPosition.Hidden,
				LegendTextPaint = DefaultText,
			};
		}
	}

	private Axis XAxis
		=> new()
		{
			Name = "Requests",
			Position = AxisPosition.Start,
			NamePaint = DefaultText,
			LabelsPaint = DefaultText,
			SeparatorsPaint = PaleGreyLine,
			MinLimit = MinXAxis,
			MaxLimit = MaxXAxis,
			MinStep = 1
		};

	private Axis YAxis
		=> new()
		{
			Name = "Response Time (ms)",
			Position = AxisPosition.Start,
			NamePaint = DefaultText,
			LabelsPaint = DefaultText,
			SeparatorsPaint = PaleGreyLine,
			MinLimit = 0,
			MaxLimit = YAxisMax
		};

	protected static LineSeries<ObservablePoint> LineSeries(string name, IReadOnlyCollection<ObservablePoint> values, SKColor color)
		=> new()
		{
			Name = name,
			Values = values,
			Stroke = new SolidColorPaint(color, 2),
			Fill = new SolidColorPaint(color.WithAlpha(32)),
			LineSmoothness = 0,
			GeometrySize = 0,
			GeometryStroke = null,
			GeometryFill = null
		};

	protected static double GetYAxisMax(double max)
	{
		var interval = GetYStepSize(max);
		return Math.Ceiling(max / interval) * interval;
	}

	private static double GetYStepSize(double max)
	{
		var magnitude = GetMagnitude(max);
		var ratio = max / magnitude;

		return ratio switch
		{
			< 2 => magnitude / 5,
			< 4 => magnitude / 2,
			> 8 => magnitude * 2,
			_ => magnitude
		};
	}

	private static double GetMagnitude(double max)
		=> Math.Pow(10, Math.Floor(Math.Log10(max)));
}