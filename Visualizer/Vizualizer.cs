using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace LoadTest.Visualizer
{
    public static class Vizualizer
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
                ChartAreas =
                {
                    new ChartArea
                    {
                        Name = "ResponseTimes",
                        AxisX = new Axis
                        {
                            Minimum = 1,
                            Maximum = results.Max(r => r.Key),
                            IsLogarithmic = true
                        }
                    }
                }
            };

            chart.SaveImage(filename, ChartImageFormat.Png);
        }
    }
}