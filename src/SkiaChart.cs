using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LiveChartsCore.Defaults;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.SKCharts;
using SkiaSharp;

namespace LoadTestToolbox
{
	public class SkiaChart
	{
		private readonly IDictionary<uint, double> _results;

		private static readonly IEnumerable<string> _fonts = SKFontManager.Default.FontFamilies;
		private static readonly IEnumerable<string> _defaultOrder = new[] { "Noto Sans", "Open Sans", "Roboto", "Segoe UI", "Arial", "San Francisco", "Helvetica Neue", "Helvetica" };
		private static readonly string DefaultFont = _defaultOrder.FirstOrDefault(name => _fonts.Any(f => f == name));

		public SkiaChart(IDictionary<uint, double> results)
			=> _results = results;

		public async Task Save(Stream output)
		{
			var imageData = Chart.GetImage().Encode(SKEncodedImageFormat.Png, 100).ToArray();
			var stream = new MemoryStream(imageData);
			await stream.CopyToAsync(output);
		}

		public SKCartesianChart Chart
			=> new()
			{
				Width = 1280,
				Height = 720,
				XAxes = new[] { XAxis },
				YAxes = new[] { YAxis },
				Series = new[] { Series }
			};

		private Axis XAxis
			=> new()
			{
				Name = "Requests",
				Position = AxisPosition.Start,
				NamePaint = new SolidColorPaint(SKColors.Black) { FontFamily = DefaultFont },
				LabelsPaint = new SolidColorPaint(SKColors.Black) { FontFamily = DefaultFont },
				SeparatorsPaint = new SolidColorPaint(new SKColor(0, 0, 0, 24), 1),
				MinLimit = MinXAxis,
				MaxLimit = MaxXAxis
			};

		private Axis YAxis
			=> new()
			{
				Name = "Response Time (ms)",
				Position = AxisPosition.Start,
				NamePaint = new SolidColorPaint(SKColors.Black) { FontFamily = DefaultFont },
				LabelsPaint = new SolidColorPaint(SKColors.Black) { FontFamily = DefaultFont },
				SeparatorsPaint = new SolidColorPaint(new SKColor(0, 0, 0, 24), 1),
				MinLimit = 0,
				MaxLimit = YAxisMax
			};

		private LineSeries<ObservablePoint> Series
			=> new()
			{
				Values = _results.OrderBy(r => r.Key).Select(r => new ObservablePoint(r.Key, r.Value)),
				Stroke = new SolidColorPaint(SKColors.DodgerBlue, 2),
				LineSmoothness = 0,
				GeometrySize = 0,
				GeometryStroke = null,
				GeometryFill = null
			};

		private uint MinXAxis
			=> _results.Min(r => r.Key);

		private uint MaxXAxis
			=> _results.Max(r => r.Key);

		private double YAxisMax
			=> GetYAxisMax(_results.Max(r => r.Value));

		private static double GetYAxisMax(double max)
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
}