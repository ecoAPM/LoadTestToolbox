using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LoadTestToolbox
{
	public class Drill : ITool
	{
		private readonly long _delay;

		public Drill(HttpClient httpClient, Uri url, uint requests, long delay) : base(httpClient, url, requests)
			=> _delay = delay;

		public override Task Run()
		{
			uint started = 0;
			var timer = Stopwatch.StartNew();

			while (started < _requests)
			{
				var request = started;
#pragma warning disable 4014
				var thread = new Thread(() => _worker.Run(request))
#pragma warning restore 4014
				{
					Priority = ThreadPriority.Highest
				};
				thread.Start();

				var nextStart = ++started * _delay;
				SpinWait.SpinUntil(() => timer.ElapsedTicks > nextStart);
			}

			SpinWait.SpinUntil(Complete);
			return Task.CompletedTask;
		}
	}
}