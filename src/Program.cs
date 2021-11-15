using System.CommandLine.IO;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LoadTestToolbox;

public static class Program
{
	static Program() => Thread.CurrentThread.Priority = ThreadPriority.Highest;

	public static async Task Main(string[] args)
		=> await AppFactory.Run(args);

	public static App AppFactory
		=> new(new HttpClient(), new SystemConsole(), filename => new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write));
}
