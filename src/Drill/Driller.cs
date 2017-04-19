using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LoadTestToolbox.Common;

namespace LoadTestToolbox.Drill
{
    public class Driller
    {
        public readonly IList<double> Results = new List<double>();

        private readonly Uri _url;
        private readonly int _totalRequests;
        private readonly long _delay;
        public int WorkersStarted;
        private int done;
        private readonly HttpClient _httpClient;

        public Driller(Uri url, int totalRequests, long delay, HttpClient httpClient)
        {
            _url = url;
            _totalRequests = totalRequests;
            _delay = delay;
            _httpClient = httpClient;
        }

        public async Task Run()
        {
            //warmup
            await _httpClient.GetAsync(_url);

            var start = DateTime.UtcNow;

            while (WorkersStarted < _totalRequests)
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

        private void addResult(object sender, EventArgs e)
        {
            var length = (double)sender;
            Results.Add(length);
            Interlocked.Increment(ref done);
        }

        public bool Complete()
        {
            return done == _totalRequests;
        }
    }
}
