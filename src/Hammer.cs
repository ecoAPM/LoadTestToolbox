using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LoadTestToolbox
{
	public class Hammer : ITool
	{
		public Hammer(HttpClient httpClient, Uri url, uint requests) : base(httpClient, url, requests)
		{
		}

		public override Task Run()
		{
			for (uint request = 0; request < _requests; request++)
			{
				var r = request;
#pragma warning disable 4014
				var thread = new Thread(() => _worker.Run(r))
#pragma warning restore 4014
				{
					Priority = ThreadPriority.Highest
				};
				thread.Start();
			}

			SpinWait.SpinUntil(Complete);
			return Task.CompletedTask;
		}
	}
}