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
            using (var wc = new WebClient())
            {
                var timer = new Stopwatch();

                timer.Start();
                try
                {
                    wc.DownloadString(url);
                }
                catch (WebException)
                {
                    // ignored
                }
                timer.Stop();

                OnComplete?.Invoke(timer.Elapsed.TotalMilliseconds, null);
            }
        }
    }
}