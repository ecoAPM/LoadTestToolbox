using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net.Http;
using System.Threading.Tasks;

namespace LoadTestToolbox
{
	public abstract class Tool : ITool
	{
		public IDictionary<uint, double> Results => _results.ToImmutableDictionary();
		protected readonly IDictionary<uint, double> _results = new ConcurrentDictionary<uint, double>();

		protected readonly Worker _worker;

		protected Tool(HttpClient http, Uri url)
		{
			_worker = new Worker(http, url, addResult);
			Prime(http, url).GetAwaiter().GetResult();
		}

		public abstract Task Run();

		public abstract bool Complete();

		private static async Task Prime(HttpClient httpClient, Uri url)
			=> await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));

		protected virtual void addResult(uint request, double ms)
			=> _results.Add(request, ms);
	}
}