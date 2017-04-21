using System;
using System.Collections.Generic;
using System.Linq;

namespace LoadTestToolbox
{
    public static class Visualizer
    {
        private static Size defaultSize => new Size(1280, 720);
        private static Font defaultAxisFont => new Font("Arial", 12, FontStyle.Bold);
        private static Font defaultTitleFont => new Font("Arial", 18, FontStyle.Bold);
        private static Title defaultTitle => new Title("LoadTestToolbox", Docking.Top, defaultTitleFont, Color.Black);
        private static Color defaultColor => Color.LightGray;
        private static Grid defaultGrid => new Grid { LineColor = defaultColor };

        private static Series defaultSeries => new Series
        {
            BorderWidth = 4,
            ChartType = SeriesChartType.Line,
            Name = "ResponseTime",
            Points = new[] { new DataPoint(0, 0) }
        };

        private static Axis getXAxis(this IDictionary<int, double> results) => new Axis
        {
            MajorGrid = defaultGrid,
            MajorTickMark = new TickMark { LineColor = defaultColor },
            Minimum = 0,
            Maximum = results.Max(r => r.Key),
            Title = "Request(s)",
            TitleFont = defaultAxisFont
        };

        private static Axis getYAxis(this IDictionary<int, double> results) => new Axis
        {
            Interval = results.getYAxisMax().getInterval(),
            MajorGrid = defaultGrid,
            MajorTickMark = new TickMark { LineColor = defaultColor },
            Minimum = 0,
            Maximum = results.getYAxisMax(),
            Title = "Response Time (ms)",
            TitleFont = defaultAxisFont
        };

        private static double getYAxisMax(this IDictionary<int, double> results)
        {
            var max = results.Max(r => r.Value);
            var interval = max.getInterval();
            return Math.Ceiling(max / interval) * interval;
        }

        private static double getInterval(this double max)
        {
            var family = Math.Pow(10, Math.Floor(Math.Log10(max)));
            var ratio = max / family;
            if (ratio < 2) return family / 5;
            if (ratio < 4) return family / 2;
            if (ratio > 8) return family * 2;
            return family;
        }

        private static ChartArea getChartArea(this IDictionary<int, double> results) => new ChartArea("ReponseTimes")
        {
            AxisX = results.getXAxis(),
            AxisY = results.getYAxis()
        };

        private static Chart getChart(this IDictionary<int, double> results)
        {
            var chart = new Chart
            {
                ChartAreas = new[] { results.getChartArea() },
                Series = new[] { results.getSeries() },
                Size = defaultSize
            };
            chart.Titles.Add(defaultTitle);
            return chart;
        }

        private static Series getSeries(this IDictionary<int, double> results)
        {
            var series = defaultSeries;
            foreach (var result in results)
                series.Points.AddXY(result.Key, result.Value);

            return series;
        }

        public static void SaveChart(this IDictionary<int, double> results, string filename)
        {
            var chart = results.getChart();
            chart.SaveImage(filename, ChartImageFormat.Png);
        }
    }

    #region FullFXstubs
    public class ChartImageFormat
    {
        public static object Png { get; set; }
    }

    internal class Chart
    {
        public ChartArea[] ChartAreas { get; set; }
        public Series[] Series { get; set; }
        public Size Size { get; set; }
        public IList<Title> Titles { get; set; }

        public void SaveImage(string filename, object png)
        {
            throw new NotImplementedException();
        }
    }

    internal class ChartArea
    {
        public ChartArea(string reponsetimes)
        {
            throw new NotImplementedException();
        }

        public Axis AxisX { get; set; }
        public Axis AxisY { get; set; }
    }

    internal class TickMark
    {
        public Color LineColor { get; set; }
    }

    internal class Axis
    {
        public Grid MajorGrid { get; set; }
        public TickMark MajorTickMark { get; set; }
        public int Minimum { get; set; }
        public double Maximum { get; set; }
        public string Title { get; set; }
        public Font TitleFont { get; set; }
        public double Interval { get; set; }
    }

    internal class DataPoint
    {
        public DataPoint(int i, int i1)
        {
            throw new NotImplementedException();
        }
    }

    internal static class DataPointExtensions
    {
        internal static void AddXY(this DataPoint[] points, int x, double y)
        {

        }
    }

    internal class SeriesChartType
    {
        public static object Line { get; set; }
    }

    internal class Series
    {
        public int BorderWidth { get; set; }
        public object ChartType { get; set; }
        public string Name { get; set; }
        public DataPoint[] Points { get; set; }
    }

    internal class Grid
    {
        public Color LineColor { get; set; }
    }

    internal class Title
    {
        public Title(string loadtesttoolbox, object top, Font defaultTitleFont, object black)
        {
            throw new NotImplementedException();
        }
    }

    internal class Color
    {
        public static object Black { get; set; }
        public static Color LightGray { get; set; }
    }

    internal class Docking
    {
        public static object Top { get; set; }
    }

    internal class Font
    {
        public Font(string arial, int i, object bold)
        {
            throw new NotImplementedException();
        }
    }

    internal class FontStyle
    {
        public static object Bold { get; set; }
    }

    internal class Size
    {
        public Size(int i, int i1)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
}