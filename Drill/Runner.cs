using System;
using System.Collections.Generic;
using System.Threading;
using LoadTestToolbox.Common;

namespace LoadTestToolbox.Drill
{
    class Runner
    {
        public readonly IList<double> Results = new List<double>();

        private readonly Uri url;
        private readonly int totalRequests;
        private readonly long delay;
        public int WorkersStarted;
        private int done;

        public Runner(Uri url, int totalRequests, long delay)
        {
            this.url = url;
            this.totalRequests = totalRequests;
            this.delay = delay;
        }

        public void Run()
        {
            var start = DateTime.Now;

            while (WorkersStarted < totalRequests)
            {
                if (start.Add(new TimeSpan((WorkersStarted + 1)*delay)) < DateTime.Now)
                    createWorker();
                else
                    Thread.Sleep(1);
            }
        }


        private void createWorker()
        {
            var w = new Worker(url);
            w.OnComplete += addResult;
            new Thread(w.Run).Start();
            Interlocked.Increment(ref WorkersStarted);
        }

        private void addResult(object sender, EventArgs e)
        {
            var length = (double)sender;
            Results.Add(length);
            Interlocked.Increment(ref done);
        }

        public bool Complete()
        {
            return done == totalRequests;
        }
    }
}
