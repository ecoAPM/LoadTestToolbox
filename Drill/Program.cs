using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LoadTestToolbox.Common;

namespace LoadTestToolbox.Drill
{
    public static class Program
    {
        private static Uri url;

        public static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Usage: drill {site} {req/sec} {duration} {graph output filename}");
                return;
            }

            url = new Uri(args[0], UriKind.Absolute);
            var requestsPerSecond = Convert.ToInt32(args[1]);
            var duration = Convert.ToInt32(args[2]);

            var delay = TimeSpan.TicksPerSecond / requestsPerSecond;
            var totalRequests = requestsPerSecond * duration;

            var started = DateTime.UtcNow;
            var previewed = 0;

            var runner = new Runner(url, totalRequests, delay);
            new Thread(runner.Run).Start();

            while (!runner.Complete())
            {
                if (DateTime.UtcNow.Subtract(started).Seconds > previewed && runner.Results.Any())
                {
                    var lastSecondOfResults = runner.Results.Reverse().Take(requestsPerSecond);
                    var average = lastSecondOfResults.Average();
                    Console.WriteLine(++previewed + ": " + Math.Round(average, 2) + " ms");
                }
                Thread.Sleep(1);
            }

            var index = 0;
            var results = runner.Results.ToDictionary(r => ++index, r => r);
            results.SaveChart(args[3]);
        }
    }
}
