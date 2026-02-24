using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.SKCharts;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;

namespace LoadTestToolbox.Charts;

public abstract class SkiaChart
{
	protected abstract string Description { get; }
	protected abstract LineSeries<ObservablePoint>[] Series { get; }
	protected abstract uint MinXAxis { get; }
	protected abstract uint MaxXAxis { get; }
	protected abstract double YAxisMax { get; }

	private const int Width = 1280;
	private const int Height = 720;

	private static readonly SKTypeface DefaultTypeface = SKTypeface.FromFamilyName(FontManager.DefaultFont);
	private static readonly SolidColorPaint DefaultText = new(SKColors.Black) { SKTypeface = DefaultTypeface };
	private static readonly SolidColorPaint PaleGreyLine = new(SKColors.Black.WithAlpha(24), 1);

	private static readonly Lock ChartLock = new();


	public SKCartesianChart GetChart()
	{
		lock (ChartLock)
		{
			var chart = CreateChart();
			chart.Sections = chart.Sections.Append(new DrawnLabelVisual(Subtitle(Title)));
			return chart;
		}
	}

	private SKCartesianChart CreateChart()
		=> new()
		{
			Title = new DrawnLabelVisual(Title),
			Width = Width,
			Height = Height,
			Background = SKColors.White,
			XAxes = [XAxis],
			YAxes = [YAxis],
			Series = Series,
			LegendPosition = Series.Length > 1 ? LegendPosition.Bottom : LegendPosition.Hidden,
			LegendTextPaint = DefaultText,
			LegendTextSize = 16
		};

	private static readonly Padding TitlePadding = new(0, 8, 0, 32);

	private static readonly LabelGeometry Title = new()
	{
		Text = "LoadTestToolbox by ecoAPM",
		TextSize = 24,
		Paint = DefaultText,
		HorizontalAlign = Align.Start,
		VerticalAlign = Align.Start,
		Padding = TitlePadding
	};

	private LabelGeometry Subtitle(LabelGeometry title)
		=> new()
		{
			Text = Description,
			TextSize = 18,
			Paint = DefaultText,
			HorizontalAlign = Align.Middle,
			VerticalAlign = Align.Middle,
			Padding = TitlePadding,
			X = Width / 2f,
			Y = title.Y + title.Padding.Top + title.TextSize + title.Padding.Bottom + 2
		};


	private Axis XAxis
		=> new()
		{
			Name = "Requests",
			Position = AxisPosition.Start,
			NamePaint = DefaultText,
			LabelsPaint = DefaultText,
			NameTextSize = 18,
			TextSize = 16,
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
			NameTextSize = 18,
			TextSize = 16,
			SeparatorsPaint = PaleGreyLine,
			MinLimit = 0,
			MaxLimit = YAxisMax
		};

	protected static LineSeries<ObservablePoint> LineSeries(string name, ObservablePoint[] values, SKColor color)
		=> new()
		{
			Name = name,
			Values = values,
			Stroke = new SolidColorPaint(color, 2),
			Fill = new LinearGradientPaint(color.WithAlpha(24), color.WithAlpha(32), new SKPoint(0, 0), new SKPoint(0, 1)),
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