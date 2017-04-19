using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LoadTestToolbox.Common;

namespace LoadTestToolbox.Drill
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Usage: drill {site} {req/sec} {duration} {graph output filename}");
                return;
            }

            var url = new Uri(args[0], UriKind.Absolute);
            var requestsPerSecond = Convert.ToInt32(args[1]);
            var duration = Convert.ToInt32(args[2]);
            var outputFileName = args[3];

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
