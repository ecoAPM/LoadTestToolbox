using System;
using System.Diagnostics;
using System.Net.Http;

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
            var timer = new Stopwatch();

            timer.Start();

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    httpClient.GetStringAsync(url).GetAwaiter().GetResult();
                }
                catch (Exception)
                {
                }
                timer.Stop();

                OnComplete?.Invoke(timer.Elapsed.TotalMilliseconds, null);
            }

        }
    }
}