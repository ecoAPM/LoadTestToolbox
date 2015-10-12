using System;
using System.Threading;

namespace LoadTest
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
            var minOrder = Convert.ToInt32(args[1]);
            var maxOrder = Convert.ToInt32(args[2]);

            for (var n = minOrder; n < maxOrder; n++)
            {
                var pow = Math.Pow(10, n);
                for (var x = (n == minOrder ? 1 : 2) * pow; x <= Math.Pow(10, n + 1); x += pow)
                {
                    var r = new Runner(url, Convert.ToInt32(x));
                    r.Run();
                    while (!r.Complete())
                        Thread.Sleep(100);

                    Console.WriteLine(x + ": " + Math.Round(r.Average, 2) + " ms");
                }
            }
        }
    }
}
