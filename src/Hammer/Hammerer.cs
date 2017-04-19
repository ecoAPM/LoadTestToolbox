using System;
using System.Net.Http;
using System.Threading;
using LoadTestToolbox.Common;

namespace LoadTestToolbox.Hammer
{
    public class Hammerer
    {
        private readonly Uri url;
        private readonly int hammers;
        private readonly HttpClient httpClient;
        private int done;
        private double total;

        public double Average => total / done;

        public Hammerer(Uri url, int hammers, HttpClient httpClient)
        {
            this.url = url;
            this.hammers = hammers;
            this.httpClient = httpClient;
        }

        public void Run()
        {
            for (var x = 0; x < hammers; x++)
            {
                var w = new Worker(httpClient, url);
                w.OnComplete += doMath;
                new Thread(async() => await w.Run()).Start();
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