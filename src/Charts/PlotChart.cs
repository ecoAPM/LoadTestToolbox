using ScottPlot;
using ScottPlot.DataSources;
using ScottPlot.Plottables;
using ScottPlot.TickGenerators;
using SkiaSharp;
using Color = ScottPlot.Color;

namespace LoadTestToolbox.Charts;

public abstract class PlotChart
{
	protected abstract string Description { get; }
	protected abstract uint MinXAxis { get; }
	protected abstract uint MaxXAxis { get; }
	protected abstract double YAxisMax { get; }

	protected abstract Scatter[] Series { get; }

	public const int Width = 1280;
	public const int Height = 720;

	private const int TitleSize = 24;
	private const int SubtitleSize = TitleSize * 3 / 4;
	private const int HeadingSize = TitleSize * 2 / 3;

	private static readonly SKTypeface DefaultTypeface = SKTypeface.FromFamilyName(FontManager.DefaultFont);
	private static readonly Color PaleGreyLine = Color.FromSKColor(SKColors.Black.WithAlpha(24));

	public Plot GetChart()
	{
		var plot = CreateDefaultPlot();
		plot.Title("LoadTestToolbox by ecoAPM" + Environment.NewLine, TitleSize);
		SetSubtitle(plot.Axes.Top.Label, Description);

		SetAxis(plot.Axes.Bottom, "Requests", MinXAxis, MaxXAxis);
		SetAxis(plot.Axes.Left, "Response Time (ms)", 0, YAxisMax);

		DrawLines(plot);

		if (Series.Length > 1)
			SetLegend(plot);
		else
			plot.Legend.IsVisible = false;

		return plot;
	}

	private static Plot CreateDefaultPlot()
	{
		var plot = new Plot();
		plot.Font.Set(DefaultTypeface.FamilyName);
		plot.FigureBackground.Color = Colors.White;
		plot.DataBackground.Color = Colors.White;
		plot.Axes.AntiAlias(true);
		plot.Axes.Top.FrameLineStyle.IsVisible = false;
		plot.Axes.Right.FrameLineStyle.IsVisible = false;
		plot.Axes.Right.MinimumSize = HeadingSize * 2;
		return plot;
	}

	private static void SetSubtitle(LabelStyle label, string subtitle)
	{
		label.Text = subtitle;
		label.Font = DefaultTypeface;
		label.FontSize = SubtitleSize;
	}

	private static void SetAxis(IAxis axis, string label, double min, double max)
	{
		axis.Label.Text = label;
		axis.Label.Font = DefaultTypeface;
		axis.Label.FontSize = SubtitleSize;
		axis.MinorTickStyle.Color = PaleGreyLine;
		axis.TickLabelStyle.Font = DefaultTypeface;
		axis.TickLabelStyle.FontSize = HeadingSize;
		axis.TickGenerator = new NumericAutomatic { IntegerTicksOnly = true };
		axis.Min = min;
		axis.Max = max;
	}

	private void DrawLines(Plot plot)
	{
		foreach (var series in Series)
			DrawLine(plot, series);
	}

	private static void DrawLine(Plot plot, Scatter series)
	{
		var line = plot.Add.ScatterLine(series.Data, series.Color);
		line.LegendText = series.LegendText;
		line.LineWidth = 2;
		line.FillY = true;
		line.FillYValue = 0;
		line.FillYColor = series.Color.WithAlpha(24);
	}

	private static void SetLegend(Plot plot)
	{
		plot.Legend.FontSize = HeadingSize;
		plot.Legend.Padding = PixelPadding.Zero;
		plot.Legend.Margin = PixelPadding.Zero;
		plot.Legend.InterItemPadding = new PixelPadding(HeadingSize * 2);
		plot.Legend.SymbolPadding = HeadingSize / 2f;
		plot.Legend.OutlineStyle.IsVisible = false;
		plot.Legend.ShadowFillStyle.IsVisible = false;
		plot.ShowLegend(Edge.Bottom);
	}

	protected static Scatter LineSeries(string name, Coordinates[] coords, SKColor color)
	{
		var data = new ScatterSourceCoordinatesArray(coords);
		var line = new Scatter(data)
		{
			LegendText = name,
			Color = Color.FromSKColor(color)
		};
		return line;
	}

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