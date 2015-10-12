using System;
using System.Collections.Generic;
using System.Linq;

namespace LoadTest.Hammer
{
    static class HardwareStore
    {
        public static IEnumerable<int> GetHammers(int min, int max)
        {
            var list = new List<int>();
            var minOrder = (int)Math.Log10(min);
            var maxOrder = (int)Math.Log10(max);

            for (var n = minOrder; n <= maxOrder; n++)
            {
                var pow = (int)Math.Pow(10, n);
                for (var x = (n == minOrder ? min : 2 * pow); x <= (n == maxOrder ? max : Math.Pow(10, n + 1)); x += pow)
                    list.Add(x);
            }

            if (list.Last() < max)
                list.Add(max);

            return list;
        }
    }
}