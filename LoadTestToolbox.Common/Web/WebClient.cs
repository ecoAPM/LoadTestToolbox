using System;

namespace LoadTestToolbox.Common.Web
{
	public class WebClient : IWebClient
	{
		private System.Net.WebClient m_WebClient;

		public WebClient() 
		{
			m_WebClient = new System.Net.WebClient();
		}

		public void Dispose()
		{
			m_WebClient.Dispose();
		}

		/// <summary>
		/// Downloads a string from a specified address
		/// </summary>
		/// <param name="address">Url to download from</param>
		/// <returns></returns>
		public string DownloadString(Uri address)
		{
			return m_WebClient.DownloadString(address);
		}
	}
}
