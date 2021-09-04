using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LoadTestToolbox
{
	public class Hammer : Tool
	{
		private readonly IEnumerable<uint> _strengths;
		private readonly IDictionary<uint, double> _singleResults = new ConcurrentDictionary<uint, double>();

		public Hammer(HttpClient http, Uri url, IEnumerable<uint> strengths) : base(http, url)
			=> _strengths = strengths;

		public override Task Run()
		{
			var totals = new ConcurrentDictionary<uint, double>();
			foreach (var x in _strengths)
			{
				var results = RunOnce(x).GetAwaiter().GetResult();
				totals[x] = results.Average(r => r.Value);
				_results.Add(x, totals[x]);
			}

			SpinWait.SpinUntil(Complete);
			return Task.CompletedTask;
		}

		private Task<IDictionary<uint, double>> RunOnce(uint requests)
		{
			_singleResults.Clear();
			for (uint request = 0; request < requests; request++)
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

			SpinWait.SpinUntil(() => _singleResults.Count == requests);
			return Task.FromResult(_singleResults);
		}

		public override bool Complete() => _results.Count == _strengths.Count();

		protected override void addResult(uint request, double ms)
			=> _singleResults.Add(request, ms);

	}
}