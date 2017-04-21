using System;
using System.Net.Http;

namespace LoadTestToolbox
{
    public abstract class Tool
    {
        protected Uri _url;
        protected int _requests;
        protected int done;
        protected HttpClient _httpClient;

        protected Tool(HttpClient httpClient, Uri url, int requests)
        {
            _httpClient = httpClient;
            _url = url;
            _requests = requests;

            //warmup
            _httpClient.GetAsync(_url).GetAwaiter().GetResult();
        }

        public bool Complete()
        {
            return done == _requests;
        }

        public abstract void Run();
        protected abstract void addResult(object ms, EventArgs e);

    }
}