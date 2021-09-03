using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LoadTestToolbox
{
	public static class Program
	{
		static Program()
		{
			Console.OutputEncoding = System.Text.Encoding.Unicode;
			Thread.CurrentThread.Priority = ThreadPriority.Highest;
		}

		public static async Task Main(string[] args)
			=> await Command.InvokeAsync(args);

		private static Command Command
		{
			get
			{
				var cmd = new Command("LoadTestToolbox", "lightweight tools for load testing web applications");
				cmd.AddCommand(Drill);
				cmd.AddCommand(Hammer);

				return cmd;
			}
		}

        private static Command Drill
		{
			get
			{
				var drill = new Command("drill", "sends requests at a consistent rate")
				{
					Options.URL,
					Options.RPS,
					Options.Duration,
					Options.Filename
				};
				drill.Handler = CommandHandler.Create<string, uint, byte, string>(RunDrill);

				return drill;
			}
		}

		private static Command Hammer
		{
			get
			{
				var hammer = new Command("hammer", "sends increasing numbers of simultaneous requests")
				{
					Options.URL,
					Options.Min,
					Options.Max,
					Options.Filename
				};
				hammer.Handler = CommandHandler.Create<string, uint, uint, string>(RunHammer);

				return hammer;
			}
		}

        private static async Task RunDrill(string url, uint rps, byte duration, string filename)
		{
			var uri = new Uri(url, UriKind.Absolute);
			var results = await GetDrillResults(uri, rps, duration);
			await SaveChart(results, filename);
		}

		private static async Task RunHammer(string url, uint min, uint max, string filename)
		{
			var uri = new Uri(url, UriKind.Absolute);
			var results = await GetHammerResults(uri, min, max);
			await SaveChart(results, filename);
		}

		private static async Task SaveChart(IDictionary<uint, double> results, string filename)
		{
			var chart = new SkiaChart(results);
			var output = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
			await chart.Save(output);
		}

		private static async Task<IDictionary<uint, double>> GetHammerResults(Uri url, uint min, uint max)
		{
			var http = new HttpClient();
			var hammers = HardwareStore.GetHammers(min, max);

			var results = new Dictionary<uint, double>();
			foreach (var x in hammers)
			{
				var hammer = new Hammer(http, url, x);
				await hammer.Run();

				var avg = hammer.Results.Average(r => r.Value);
				results.Add(x, avg);
				Console.WriteLine(x + ": " + FormatTime(avg));
			}

			return results;
		}

		private static string FormatTime(double ms)
		{
			return ms < 1
				? Math.Round(ms * 1000) + " μs"
				: ms < 10
					? Math.Round(ms, 2) + " ms"
					: ms < 100
						? Math.Round(ms, 1) + " ms"
						: Math.Round(ms) + " ms";
		}

		private static async Task<IDictionary<uint, double>> GetDrillResults(Uri url, uint requestsPerSecond, byte duration)
		{
			var http = new HttpClient();

			var delay = TimeSpan.TicksPerSecond / requestsPerSecond;
			var totalRequests = requestsPerSecond * duration;

			var started = DateTime.UtcNow;
			var previewed = 0;

			var drill = new Drill(http, url, totalRequests, delay);
			new Thread(() => drill.Run()).Start();

			while (!drill.Complete())
			{
				var lastRun = DateTime.UtcNow.Subtract(started).Seconds;
				if (drill.Results.Any() && lastRun > previewed)
				{
					var lastSecondOfResults = drill.Results.Skip((int)((lastRun - 1) * requestsPerSecond));
					var average = lastSecondOfResults.Average(r => r.Value);
					Console.WriteLine(++previewed + ": " + FormatTime(average));
				}

				await Task.Delay(0);
			}

			return drill.Results;
		}
	}
}