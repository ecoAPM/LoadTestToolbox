using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LoadTestToolbox
{
	public class Drill : Tool
	{
		private readonly uint _totalRequests;
		private readonly long _delay;

		public Drill(HttpClient http, Uri url, uint requests, long delay) : base(http, url)
		{
			_totalRequests = requests;
			_delay = delay;
		}

		public override async Task Run()
		{
			uint started = 0;
			var timer = Stopwatch.StartNew();

			while (started < _totalRequests)
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

			await True(Complete);
		}

		public override bool Complete()
			=> _results.Count == _totalRequests;
	}
}