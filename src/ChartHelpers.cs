using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LoadTestToolbox
{
    public static class ChartHelpers
    {
        public static void SaveChartFor(this Visualizer visualizer, IDictionary<int, double> results, string outputFileName)
        {
            var imageData = visualizer.GetImageDataFor(results);
            var input = new MemoryStream(imageData);
            var output = new FileStream(outputFileName, FileMode.OpenOrCreate, FileAccess.Write);
            input.CopyTo(output);
        }

        public static byte[] GetImageDataFor(this Visualizer visualizer, IDictionary<int, double> results)
        {
            var dataUrl = visualizer.GetChart(results).GetAwaiter().GetResult();
            return Convert.FromBase64String(dataUrl.Substring(22));
        }

        public static double GetYAxisMax(this IDictionary<int, double> results)
        {
            var max = results.Max(r => r.Value);
            return GetYAxisMax(max);
        }

        public static double GetYAxisMax(this double max)
        {
            var interval = GetYStepSize(max);
            return Math.Ceiling(max / interval) * interval;
        }

        public static double GetYStepSize(this double max)
        {
            var magnitude = GetMagnitude(max);
            var ratio = max / magnitude;
            if (ratio < 2) return magnitude / 5;
            if (ratio < 4) return magnitude / 2;
            if (ratio > 8) return magnitude * 2;
            return magnitude;
        }

        public static double GetMagnitude(this double max)
        {
            return Math.Pow(10, Math.Floor(Math.Log10(max)));
        }

        public static double GetMagnitude(this int max)
        {
            return Math.Pow(10, Math.Floor(Math.Log10(max)));
        }

        public static double GetXStepSize(this int max)
        {
            var magnitude = max.GetMagnitude();
            var step = magnitude / 10;
            var ratio = max / step;
            return ratio < 50
                ? step
                : step * 2;
        }
    }
}