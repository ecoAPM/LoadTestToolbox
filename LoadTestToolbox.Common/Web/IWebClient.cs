using System;

namespace LoadTestToolbox.Common.Web
{
	public interface IWebClient
    {
		string DownloadString(Uri address);
		void Dispose();
	}
}
