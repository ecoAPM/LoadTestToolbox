using System;
using System.Diagnostics;
using System.Net;

namespace LoadTestToolbox.Common
{
    public class Worker
    {
        private readonly Uri url;
        public event EventHandler OnComplete;

        public Worker(Uri url)
        {
            this.url = url;
        }

        public void Run()
        {
            var wc = new WebClient();
            var timer = new Stopwatch();

            timer.Start();
            wc.DownloadString(url);
            timer.Stop();

            var ms = (double)timer.ElapsedTicks / TimeSpan.TicksPerMillisecond;
            OnComplete?.Invoke(ms, null);
        }
    }
}