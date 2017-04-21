using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LoadTestToolbox
{
    public static class ChartHelpers
    {
        public static void SaveChartImage(this IDictionary<int, double> results, string outputFileName)
        {
            var v = new Visualizer(Environment.GetEnvironmentVariable("VISUALIZER_FILES") ?? ".");
            var data = v.GetChart(results).GetAwaiter().GetResult();
            var imageData = Convert.FromBase64String(data.Substring(22));
            var input = new MemoryStream(imageData);
            var output = new FileStream(outputFileName, FileMode.OpenOrCreate, FileAccess.Write);
            input.CopyTo(output);
        }

        public static double GetYAxisMax(this IDictionary<int, double> results)
        {
            var max = results.Max(r => r.Value);
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

        public static double getXStepSize(this int max)
        {
            var magnitude = max.GetMagnitude();
            var baseStep = magnitude / 10;
            return max / baseStep < 50 ? baseStep : baseStep * 2;
        }
    }
}