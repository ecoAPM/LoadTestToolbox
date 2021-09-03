using System.CommandLine;

namespace LoadTestToolbox
{
	public static class Options
    {
        public static Option URL
			=> new Option(new[] { "-u", "--url" }, "the URL to send requests to", typeof(string))
			{
				ArgumentHelpName = "URL",
				IsRequired = true
			};

		public static Option RPS
			=> new Option(new[] { "-r", "--rps" }, "the number of requests per second to send", typeof(uint))
			{
				ArgumentHelpName = "requests per second",
				IsRequired = true
			};

		public static Option Duration
			=> new Option(new[] { "-d", "--duration" }, "the duration (in seconds) to send requests for", typeof(byte))
			{
				ArgumentHelpName = "seconds",
				IsRequired = true
			};

		public static Option Min
			=> new Option("--min", "the minimum number of simultaneous requests to send", typeof(uint))
			{
				ArgumentHelpName = "requests",
				IsRequired = true
			};

		public static Option Max
			=> new Option("--max", "the maximum number of simultaneous requests to send", typeof(uint))
			{
				ArgumentHelpName = "requests",
				IsRequired = true
			};

		public static Option Filename
			=> new Option(new[] { "-f", "--filename" }, "the file to write the chart to", typeof(string))
			{
				ArgumentHelpName = "path",
				IsRequired = true
			};
    }
}