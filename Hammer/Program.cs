using System;
using System.Collections.Generic;
using System.Threading;
using LoadTest.Visualizer;

namespace LoadTest.Hammer
{
    class Program
    {
        private static Uri url;

        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Usage: LoadTest {site} {min hammers} {max hammers} {graph output filename}");
                return;
            }

            url = new Uri(args[0], UriKind.Absolute);
            var min = Convert.ToInt32(args[1]);
            var max = Convert.ToInt32(args[2]);
            var hammers = HardwareStore.GetHammers(min, max);

            var results = new Dictionary<int, double>();
            foreach (var x in hammers)
            {
                var r = new Runner(url, Convert.ToInt32(x));
                r.Run();
                while (!r.Complete())
                    Thread.Sleep(1);

                results.Add(x, r.Average);
                Console.WriteLine(x + ": " + Math.Round(r.Average, 2) + " ms");
            }

            Vizualizer.SaveChart(results, args[3]);
        }
    }
}