using LoadTestToolbox.Common.Web;
using System;
using System.Diagnostics;
using System.Net;

namespace LoadTestToolbox.Common
{
	public class WebInvoker : IDisposable
    {
        private readonly Uri m_Url;
		private readonly IWebClient m_WebClient;
        public event EventHandler OnComplete;

		public WebInvoker(Uri url) 
			: this(url, new Web.WebClient())
		{

		}

        public WebInvoker(Uri url, IWebClient webClient)
        {
			m_Url = url;
			m_WebClient = webClient;
        }

        public void Invoke()
        {
			var timer = new Stopwatch();

            timer.Start();
			CallWebUrl();
            timer.Stop();

            OnComplete?.Invoke(timer.Elapsed.TotalMilliseconds, null);
        }

		private void CallWebUrl() 
		{
			try
			{
				m_WebClient.DownloadString(m_Url);
			}
			catch (WebException)
			{
				// ignored - should we log this?
			}
		}

		public void Dispose()
		{
			m_WebClient.Dispose();
		}
	}
}