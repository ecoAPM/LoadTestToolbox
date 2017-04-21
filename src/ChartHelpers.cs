using System;
using System.Collections.Generic;
using System.IO;

namespace LoadTestToolbox
{
    public static class ChartHelpers
    {
        public static void SaveChartImage(this IDictionary<int, double> results, string outputFileName)
        {
            var v = new NodeVisualizer(Environment.GetEnvironmentVariable("VISUALIZER_FILES") ?? ".");
            var data = v.GetChart(results).GetAwaiter().GetResult();
            var imageData = Convert.FromBase64String(data.Substring(22));
            var input = new MemoryStream(imageData);
            var output = new FileStream(outputFileName, FileMode.OpenOrCreate, FileAccess.Write);
            input.CopyTo(output);
        }
    }
}