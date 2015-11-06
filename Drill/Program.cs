using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LoadTestToolbox.Common;

namespace LoadTestToolbox.Drill
{
    static class Program
    {
        private static Uri url;

        static void Main(string[] args)
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

            var started = DateTime.Now;
            var previewed = 0;

            var runner = new Runner(url, totalRequests, delay);
            new Thread(runner.Run).Start();

            while (!runner.Complete())
            {
                if (DateTime.Now.Subtract(started).Seconds > previewed)
                {
                    var lastSecondOfResults = runner.Results.OrderByDescending(r => r.Key).Take(requestsPerSecond);
                    var average = lastSecondOfResults.Average(r => r.Value);
                    Console.WriteLine(++previewed + ": " + Math.Round(average, 2) + " ms");
                }
                Thread.Sleep(1);
            }

            Visualizer.SaveChart(runner.Results, args[3]);
        }
    }
}
