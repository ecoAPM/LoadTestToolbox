using System;
using System.Threading;

namespace LoadTest.Hammer
{
    public class Runner
    {
        private readonly Uri url;
        private readonly int hammers;
        private int done;
        private double total;

        public double Average => total / done;

        public Runner(Uri url, int hammers)
        {
            this.url = url;
            this.hammers = hammers;
        }

        public void Run()
        {
            for (var x = 0; x < hammers; x++)
            {
                var w = new Worker(url);
                w.OnComplete += doMath;
                var t = new Thread(w.Run);
                t.Start();
            }
        }

        private void doMath(object returned, EventArgs e)
        {
            var length = (double)returned;
            total += length;
            Interlocked.Increment(ref done);
        }

        public bool Complete()
        {
            return done == hammers;
        }
    }
}