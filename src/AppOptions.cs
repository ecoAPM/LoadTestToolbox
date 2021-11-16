using System.CommandLine;

namespace LoadTestToolbox;

public static class AppOptions
{
	public static Option URL
		=> new(new[] { "--url", "-u" }, "the URL to send requests to", typeof(string))
		{
			ArgumentHelpName = "URL",
			IsRequired = true
		};

	public static Option RPS
		=> new(new[] { "--rps", "-r" }, "the number of requests per second to send", typeof(uint))
		{
			ArgumentHelpName = "requests per second",
			IsRequired = true
		};

	public static Option Duration
		=> new(new[] { "--duration", "-d" }, "the duration (in seconds) to send requests for", typeof(byte))
		{
			ArgumentHelpName = "seconds",
			IsRequired = true
		};

	public static Option Min
		=> new("--min", "the minimum number of simultaneous requests to send", typeof(uint))
		{
			ArgumentHelpName = "requests",
			IsRequired = true
		};

	public static Option Max
		=> new("--max", "the maximum number of simultaneous requests to send", typeof(uint))
		{
			ArgumentHelpName = "requests",
			IsRequired = true
		};

	public static Option Filename
		=> new(new[] { "--filename", "-f" }, "the file to write the chart to", typeof(string))
		{
			ArgumentHelpName = "path",
			IsRequired = true
		};
}