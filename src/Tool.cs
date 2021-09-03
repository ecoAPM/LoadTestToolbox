using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace LoadTestToolbox
{
	public abstract class Tool : ITool
	{
		public readonly IDictionary<uint, double> Results = new ConcurrentDictionary<uint, double>();

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
			=> Results.Add(request, ms);
	}
}