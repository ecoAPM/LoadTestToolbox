using System;
using System.Collections.Generic;
using System.Threading;
using LoadTestToolbox.Common;

namespace LoadTestToolbox.Drill
{
    static class Program
    {
        private static Uri url;
        private static IDictionary<int, double> results;
        private static int done;

        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Usage: LoadTest {site} {min hammers} {max hammers} {graph output filename}");
                return;
            }

            url = new Uri(args[0], UriKind.Absolute);
            var requestsPerSecond = Convert.ToInt32(args[1]);
            var duration = Convert.ToInt32(args[2]);

            var delay = 1000 / requestsPerSecond;
            var totalRequests = requestsPerSecond * duration;

            results = new Dictionary<int, double>();
            for (var x = 0; x < totalRequests; x++)
            {
                var w = new Worker(url);
                w.OnComplete += addResult;
                new Thread(w.Run).Start();
                Thread.Sleep(delay);
            }

            while (done < totalRequests)
                Thread.Sleep(1);

            Visualizer.SaveChart(results, args[3]);
        }

        private static void addResult(object sender, EventArgs e)
        {
            var length = (double)sender;
            results.Add(results.Count + 1, length);
            //Console.WriteLine(length);
            done++;
        }
    }
}
