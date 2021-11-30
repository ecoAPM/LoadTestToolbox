using Spectre.Console;

namespace LoadTestToolbox;

public sealed class Carpenter : Wielder<Hammer>
{
	public Carpenter(HttpClient http, IAnsiConsole console, HammerSettings settings) : base(console)
	{
		var strengths = GetStrengths(settings.Min, settings.Max);
		_tool = new Hammer(http, settings.URL, strengths);
	}

	public static IEnumerable<uint> GetStrengths(uint min, uint max)
	{
		var list = new List<uint>();
		var minOrder = (uint)Math.Log10(min);
		var maxOrder = (uint)Math.Log10(max);

		for (var n = minOrder; n <= maxOrder; n++)
		{
			var pow = (uint)Math.Pow(10, n);
			var xMin = n == minOrder ? min : 2 * pow;
			var xMax = n == maxOrder ? max : Math.Pow(10, n + 1);

			for (var x = xMin; x <= xMax; x += pow)
				list.Add(x);
		}

		if (list.Last() < max)
			list.Add(max);

		return list;
	}

	public override async Task<IDictionary<uint, double>> Run()
	{
#pragma warning disable 4014
		var thread = new Thread(() => _tool.Run())
#pragma warning restore 4014
		{
			Priority = ThreadPriority.Highest
		};
		thread.Start();

		await OutputProgress();

		return _tool.Results;
	}

	private async Task OutputProgress()
	{
		uint previewed = 0;
		while (!_tool.Complete() || previewed < _tool.Results.Max(r => r.Key))
		{
			var (key, value) = _tool.Results.OrderBy(r => r.Key).FirstOrDefault(r => r.Key > previewed);
			if (_tool.Results.ContainsKey(key))
			{
				_console.WriteLine(key + ": " + FormatTime(value));
				previewed = key;
			}

			await Task.Delay(1);
		}
	}
}