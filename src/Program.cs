using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace LoadTestToolbox
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 5)
            {
                showUsage();
                return;
            }

            var tool = args[0].ToLower();
            var outputFileName = args[4];

            var results = getResults(tool, args);

            if (results == null)
            {
                showUsage();
                return;
            }

            var visualizer = new Visualizer(Environment.GetEnvironmentVariable("VISUALIZER_FILES") ?? ".");
            var output = new FileStream(outputFileName, FileMode.OpenOrCreate, FileAccess.Write);
            visualizer.SaveChart(results, output);
        }

        private static IDictionary<int, double> getResults(string tool, string[] args)
        {
            var url = new Uri(args[1], UriKind.Absolute);
            switch (tool)
            {
                case "hammer":
                    {
                        var min = Convert.ToInt32(args[2]);
                        var max = Convert.ToInt32(args[3]);

                        return getHammerResults(min, max, url);
                    }
                case "drill":
                    {
                        var requestsPerSecond = Convert.ToInt32(args[2]);
                        var duration = Convert.ToInt32(args[3]);

                        return getDrillResults(requestsPerSecond, duration, url);
                    }
                default:
                    return null;
            }
        }

        private static void showUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("dotnet run drill {site} {req/sec} {duration} {graph output filename}");
            Console.WriteLine("or");
            Console.WriteLine("Usage: dotnet run hammer {site} {min hammers} {max hammers} {graph output filename}");
        }

        private static IDictionary<int, double> getHammerResults(int min, int max, Uri url)
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
                Console.WriteLine(x + ": " + Math.Round(hammer.Average, 2) + " ms");
            }
            return results;
        }

        private static IDictionary<int, double> getDrillResults(int requestsPerSecond, int duration, Uri url)
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
                    Console.WriteLine(++previewed + ": " + Math.Round(average, 2) + " ms");
                }
                Thread.Sleep(100);
            }

            var index = 0;
            var results = drill.Results.ToDictionary(r => ++index, r => r);
            return results;
        }
    }
}