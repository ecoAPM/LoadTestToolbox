﻿using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LoadTestToolbox
{
	public class Driller : Wielder<Drill>
	{
		private readonly uint _rps;

		public Driller(HttpClient http, IConsole console, DrillOptions options) : base(http, console)
		{
			var requests = options.RPS * options.Duration;
			var delay = TimeSpan.TicksPerSecond / options.RPS;
			_tool = new Drill(_http, options.URL, requests, delay);
			_rps = options.RPS;
		}

		public override async Task<IDictionary<uint, double>> Run()
		{
			var thread = new Thread(() => _tool.Run())
			{
				Priority = ThreadPriority.Highest
			};
			thread.Start();

			await OutputProgress();

			return _tool.Results;
		}

		private async Task OutputProgress()
		{
			var started = DateTime.UtcNow;
			var previewed = 0;

			while (!_tool.Complete())
			{
				var secondsSinceStart = DateTime.UtcNow.Subtract(started).Seconds;
				if (_tool.Results.Any() && secondsSinceStart > previewed)
				{
					var alreadyPreviewed = (secondsSinceStart - 1) * _rps;
					var lastSecondOfResults = _tool.Results.Skip((int)alreadyPreviewed);
					var average = lastSecondOfResults.Average(r => r.Value);
					_console.Out.WriteLine(secondsSinceStart + ": " + FormatTime(average));
					previewed = secondsSinceStart;
				}

				await Task.Delay(1);
			}
		}
	}
}