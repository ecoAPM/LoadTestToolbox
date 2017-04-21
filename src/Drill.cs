using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;

namespace LoadTestToolbox
{
    public class Drill : Tool
    {
        public readonly IList<double> Results = new List<double>();

        private readonly long _delay;
        public int WorkersStarted;

        public Drill(HttpClient httpClient, Uri url, int requests, long delay) : base(httpClient, url, requests)
        {
            _delay = delay;
        }

        public override void Run()
        {
            var start = DateTime.UtcNow;

            while (WorkersStarted < _requests)
            {
                if (start.Add(new TimeSpan((WorkersStarted + 1)*_delay)) < DateTime.UtcNow)
                    createWorker();
                else
                    Thread.Sleep(1);
            }
        }

        private void createWorker()
        {
            var w = new Worker(_httpClient, _url);
            w.OnComplete += addResult;
            new Thread(async () => await w.Run()).Start();
            Interlocked.Increment(ref WorkersStarted);
        }

        protected override void addResult(object ms, EventArgs e)
        {
            var length = (double)ms;
            Results.Add(length);
            Interlocked.Increment(ref done);
        }
    }
}
