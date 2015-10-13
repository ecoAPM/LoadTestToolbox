using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace LoadTestToolbox.Common
{
    public static class Visualizer
    {
        public static void SaveChart(IDictionary<int, double> results, string filename)
        {
            var series = new Series
            {
                Name = "ResponseTime",
                ChartType = SeriesChartType.Line,
                BorderWidth = 4
            };
            foreach (var result in results)
                series.Points.AddXY(result.Key, result.Value);

            var chart = new Chart
            {
                Size = new Size(800, 400),
                Series = { series },
                ChartAreas = { "ReponseTimes" }
            };

            chart.SaveImage(filename, ChartImageFormat.Png);
        }
    }
}