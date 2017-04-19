using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using LoadTestToolbox.Common;

namespace LoadTestToolbox.Hammer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Usage: hammer {site} {min hammers} {max hammers} {graph output filename}");
                return;
            }

            var url = new Uri(args[0], UriKind.Absolute);
            var min = Convert.ToInt32(args[1]);
            var max = Convert.ToInt32(args[2]);
            var outputFileName = args[3];

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
    }
}