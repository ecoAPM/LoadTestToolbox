using System;
using System.Collections.Generic;
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

            switch (args[0].ToLower())
            {
                case "hammer":
                    hammer(args);
                    return;
                case "drill":
                    drill(args);
                    return;
                default:
                    showUsage();
                    return;
            }
        }

        private static void showUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("dotnet run drill {site} {req/sec} {duration} {graph output filename}");
            Console.WriteLine("or");
            Console.WriteLine("Usage: dotnet run hammer {site} {min hammers} {max hammers} {graph output filename}");
        }

        private static void hammer(string[] args)
        {
            var url = new Uri(args[1], UriKind.Absolute);
            var min = Convert.ToInt32(args[2]);
            var max = Convert.ToInt32(args[3]);
            var outputFileName = args[4];

            var hammers = HardwareStore.GetHammers(min, max);

            var results = new Dictionary<int, double>();
            foreach (var x in hammers)
            {
                var runner = new Hammerer(url, x, new HttpClient());
                new Thread(runner.Run).Start();
                while (!runner.Complete())
                    Thread.Sleep(100);

                results.Add(x, runner.Average);
                Console.WriteLine(x + ": " + Math.Round(runner.Average, 2) + " ms");
            }

            results.SaveChartImage(outputFileName);
        }

        private static void drill(string[] args)
        {
            var url = new Uri(args[1], UriKind.Absolute);
            var requestsPerSecond = Convert.ToInt32(args[2]);
            var duration = Convert.ToInt32(args[3]);
            var outputFileName = args[4];

            var delay = TimeSpan.TicksPerSecond / requestsPerSecond;
            var totalRequests = requestsPerSecond * duration;

            var started = DateTime.UtcNow;
            var previewed = 0;

            var runner = new Driller(url, totalRequests, delay, new HttpClient());
            new Thread(runner.Run().GetAwaiter().GetResult).Start();

            while (!runner.Complete())
            {
                if (DateTime.UtcNow.Subtract(started).Seconds > previewed && runner.Results.Any())
                {
                    var lastSecondOfResults = runner.Results.Reverse().Take(requestsPerSecond);
                    var average = lastSecondOfResults.Average();
                    Console.WriteLine(++previewed + ": " + Math.Round(average, 2) + " ms");
                }
                Thread.Sleep(100);
            }

            var index = 0;
            var results = runner.Results.ToDictionary(r => ++index, r => r);
            results.SaveChartImage(outputFileName);
        }
    }
}