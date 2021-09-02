using System;
using System.Collections.Generic;
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
		{
			if (args.Length != 5)
			{
				ShowUsage();
				return;
			}

			var tool = HardwareStore.FindTool(args[0]);
			var outputFileName = args[4];

			var results = await GetResults(tool, args);

			if (results == null)
			{
				ShowUsage();
				return;
			}

			var chart = new SkiaChart(results);
			var output = new FileStream(outputFileName, FileMode.OpenOrCreate, FileAccess.Write);
			await chart.Save(output);
		}

		private static async Task<IDictionary<uint, double>> GetResults(Tool tool, IReadOnlyList<string> args)
		{
			var url = new Uri(args[1], UriKind.Absolute);
			switch (tool)
			{
				case Tool.Hammer:
				{
					var min = Convert.ToUInt32(args[2]);
					var max = Convert.ToUInt32(args[3]);

					return await GetHammerResults(min, max, url);
				}
				case Tool.Drill:
				{
					var requestsPerSecond = Convert.ToUInt32(args[2]);
					var duration = Convert.ToByte(args[3]);

					return await GetDrillResults(requestsPerSecond, duration, url);
				}
				default:
					return null;
			}
		}

		private static void ShowUsage()
		{
			Console.WriteLine("Usage:");
			Console.WriteLine("ltt drill {site} {req/sec} {duration} {chart output filename}");
			Console.WriteLine("ltt hammer {site} {min hammers} {max hammers} {chart output filename}");
		}

		private static async Task<IDictionary<uint, double>> GetHammerResults(uint min, uint max, Uri url)
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

		private static async Task<IDictionary<uint, double>> GetDrillResults(uint requestsPerSecond, byte duration, Uri url)
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