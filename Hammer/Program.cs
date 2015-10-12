using System;
using System.Threading;

namespace LoadTest.Hammer
{
    class Program
    {
        private static Uri url;

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: LoadTest {site} {min order hammers} {max order hammers}");
                return;
            }

            url = new Uri(args[0], UriKind.Absolute);
            var min = Convert.ToInt32(args[1]);
            var max = Convert.ToInt32(args[2]);
            var hammers = HardwareStore.GetHammers(min, max);

            foreach (var x in hammers)
            {
                var r = new Runner(url, Convert.ToInt32(x));
                r.Run();
                while (!r.Complete())
                    Thread.Sleep(10);

                Console.WriteLine(x + ": " + Math.Round(r.Average, 2) + " ms");
            }
        }
    }
}
