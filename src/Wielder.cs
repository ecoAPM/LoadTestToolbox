using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace LoadTestToolbox;

public abstract class Wielder<T> where T : ITool
{
	protected readonly HttpClient _http;
	protected readonly IConsole _console;
	protected T _tool;

	protected Wielder(HttpClient http, IConsole console)
	{
		_http = http;
		_console = console;
	}

	public abstract Task<IDictionary<uint, double>> Run();

	public static string FormatTime(double ms)
	{
		return ms switch
		{
			< 1 => Math.Round(ms * 1000) + " μs",
			< 10 => Math.Round(ms, 2) + " ms",
			< 100 => Math.Round(ms, 1) + " ms",
			_ => Math.Round(ms) + " ms"
		};
	}
}
