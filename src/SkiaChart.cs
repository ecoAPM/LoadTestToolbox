using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiveChartsCore.Defaults;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView.SKCharts;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace LoadTestToolbox
{
    public class SkiaChart : IChart
    {
        private readonly IDictionary<int, double> _results;

        public SkiaChart(IDictionary<int, double> results)
            => _results = results;

        public void Save(Stream output)
        {
            var imageData = Chart.GetImage().Encode(SKEncodedImageFormat.Png, 100).ToArray();
            var stream = new MemoryStream(imageData);
            stream.CopyTo(output);
        }

        private SKCartesianChart Chart
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
                SeparatorsPaint = new SolidColorPaint(new SKColor(0, 0, 0, 24), 1),
                MinLimit = MinXAxis,
                MaxLimit = MaxXAxis
            };

        private Axis YAxis
            => new()
            {
                Name = "Response Time (ms)",
                Position = AxisPosition.Start,
                SeparatorsPaint = new SolidColorPaint(new SKColor(0, 0, 0, 24), 1),
                MinLimit = 0,
                MaxLimit = YAxisMax
            };

        private LineSeries<ObservablePoint> Series
            => new()
            {
                Values = _results.Select(r => new ObservablePoint(r.Key, r.Value)),
                Stroke = new SolidColorPaint(SKColors.DodgerBlue, 2),
                LineSmoothness = 0,
                GeometrySize = 0,
                GeometryStroke = null,
                GeometryFill = null
            };

        private int MinXAxis
            => _results.Min(r => r.Key);

        private int MaxXAxis
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