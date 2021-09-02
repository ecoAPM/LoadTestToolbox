using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LoadTestToolbox
{
	public abstract class ITool
	{
		public readonly IDictionary<uint, double> Results = new ConcurrentDictionary<uint, double>();

		protected readonly Worker _worker;
		protected readonly HttpClient _httpClient;
		protected readonly Uri _url;
		protected readonly uint _requests;

		private uint done;

		protected ITool(HttpClient httpClient, Uri url, uint requests)
		{
			_httpClient = httpClient;
			_url = url;
			_requests = requests;
			_worker = new Worker(_httpClient, _url, addResult);

			PrimeHttpClient().GetAwaiter().GetResult();
		}

		private async Task PrimeHttpClient()
			=> await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, _url));

		public abstract Task Run();

		private void addResult(uint request, double ms)
		{
			Results.Add(request, ms);
			Interlocked.Increment(ref done);
		}

		public bool Complete() => done > 0 && done == _requests;
	}
}