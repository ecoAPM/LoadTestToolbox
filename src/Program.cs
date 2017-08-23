using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace LoadTestToolbox
{
    public static class Program
    {
        static Program()
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
        }

        public static void Main(string[] args)
        {
            if (args.Length != 5)
            {
                ShowUsage();
                return;
            }

            var tool = args[0].ToLower();
            var outputFileName = args[4];

            var results = GetResults(tool, args);

            if (results == null)
            {
                ShowUsage();
                return;
            }

            var visualizerDir = Environment.GetEnvironmentVariable("VISUALIZER_FILES") ?? ".";
            var visualizer = new Visualizer(visualizerDir);
            var output = new FileStream(outputFileName, FileMode.OpenOrCreate, FileAccess.Write);
            visualizer.SaveChart(results, output);
        }

        private static IDictionary<int, double> GetResults(string tool, IReadOnlyList<string> args)
        {
            var url = new Uri(args[1], UriKind.Absolute);
            switch (tool)
            {
                case "hammer":
                    {
                        var min = Convert.ToInt32(args[2]);
                        var max = Convert.ToInt32(args[3]);

                        return GetHammerResults(min, max, url);
                    }
                case "drill":
                    {
                        var requestsPerSecond = Convert.ToInt32(args[2]);
                        var duration = Convert.ToInt32(args[3]);

                        return GetDrillResults(requestsPerSecond, duration, url);
                    }
                default:
                    return null;
            }
        }

        private static void ShowUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("dotnet run drill {site} {req/sec} {duration} {graph output filename}");
            Console.WriteLine("or");
            Console.WriteLine("Usage: dotnet run hammer {site} {min hammers} {max hammers} {graph output filename}");
        }

        private static IDictionary<int, double> GetHammerResults(int min, int max, Uri url)
        {
            var hammers = HardwareStore.GetHammers(min, max);

            var results = new Dictionary<int, double>();
            foreach (var x in hammers)
            {
                var hammer = new Hammer(new HttpClient(), url, x);
                new Thread(hammer.Run).Start();
                while (!hammer.Complete())
                    Thread.Sleep(100);

                results.Add(x, hammer.Average);
                Console.WriteLine(x + ": " + FormatTime(hammer.Average));
            }
            return results;
        }

        private static string FormatTime(double ms)
        {
            return ms < 1
                ? Math.Round(ms * 1000) + " μs"
                : ms < 10
                    ? Math.Round(ms, 2) + " ms"
                    : ms < 100
                        ? Math.Round(ms, 1) + " ms"
                        : Math.Round(ms) + " ms";
        }

        private static IDictionary<int, double> GetDrillResults(int requestsPerSecond, int duration, Uri url)
        {
            var delay = TimeSpan.TicksPerSecond / requestsPerSecond;
            var totalRequests = requestsPerSecond * duration;

            var started = DateTime.UtcNow;
            var previewed = 0;

            var drill = new Drill(new HttpClient(), url, totalRequests, delay);
            new Thread(drill.Run).Start();

            while (!drill.Complete())
            {
                if (DateTime.UtcNow.Subtract(started).Seconds > previewed && drill.Results.Any())
                {
                    var lastSecondOfResults = drill.Results.Reverse().Take(requestsPerSecond);
                    var average = lastSecondOfResults.Average();
                    Console.WriteLine(++previewed + ": " + FormatTime(average));
                }
                Thread.Sleep(100);
            }

            var index = 0;
            var results = drill.Results.ToDictionary(r => ++index, r => r);
            return results;
        }
    }
}