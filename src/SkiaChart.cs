using LiveChartsCore.Defaults;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.SKCharts;
using SkiaSharp;

namespace LoadTestToolbox;

public abstract class SkiaChart
{
	protected abstract string Description { get; }

	public async Task Save(Stream output)
	{
		var imageData = GetChart().GetImage().Encode(SKEncodedImageFormat.Png, 100).ToArray();
		var stream = new MemoryStream(imageData);
		await stream.CopyToAsync(output);
	}

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
				LegendTextPaint = new SolidColorPaint { FontFamily = FontManager.DefaultFont, Color = SKColors.Black },
			};
		}
	}

	protected abstract IReadOnlyCollection<LineSeries<ObservablePoint>> Series { get; }

	private Axis XAxis
		=> new()
		{
			Name = "Requests",
			Position = AxisPosition.Start,
			NamePaint = new SolidColorPaint(SKColors.Black) { FontFamily = FontManager.DefaultFont },
			LabelsPaint = new SolidColorPaint(SKColors.Black) { FontFamily = FontManager.DefaultFont },
			SeparatorsPaint = new SolidColorPaint(new SKColor(0, 0, 0, 24), 1),
			MinLimit = MinXAxis,
			MaxLimit = MaxXAxis
		};

	private Axis YAxis
		=> new()
		{
			Name = "Response Time (ms)",
			Position = AxisPosition.Start,
			NamePaint = new SolidColorPaint(SKColors.Black) { FontFamily = FontManager.DefaultFont },
			LabelsPaint = new SolidColorPaint(SKColors.Black) { FontFamily = FontManager.DefaultFont },
			SeparatorsPaint = new SolidColorPaint(new SKColor(0, 0, 0, 24), 1),
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

	protected abstract uint MinXAxis { get; }

	protected abstract uint MaxXAxis { get; }

	protected abstract double YAxisMax { get; }

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