using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace LoadTestToolbox.Common
{
    public class Worker
    {
        private readonly Uri _url;
        private readonly HttpClient _httpClient;
        private readonly Stopwatch _timer;
        public event EventHandler OnComplete;

        public Worker(HttpClient httpClient, Uri url)
        {
            _url = url;
            _httpClient = httpClient;
            _timer = new Stopwatch();
        }

        public async Task Run()
        {
            _timer.Start();
            await _httpClient.GetAsync(_url);
            _timer.Stop();
            OnComplete?.Invoke(_timer.Elapsed.TotalMilliseconds, null);
        }
    }
}