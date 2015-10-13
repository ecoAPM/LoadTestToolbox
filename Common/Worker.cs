using System;
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
            var start = DateTime.Now;
            wc.DownloadString(url);
            var end = DateTime.Now;

            var length = (end - start);
            OnComplete?.Invoke(length.TotalMilliseconds, null);
        }
    }
}