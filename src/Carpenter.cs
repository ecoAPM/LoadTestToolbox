using Spectre.Console;

namespace LoadTestToolbox;

public sealed class Carpenter : Wielder<Hammer, Stats>
{
	public Carpenter(HttpClient http, IAnsiConsole console, HammerSettings settings) : base(console)
	{
		var strengths = GetStrengths(settings.Min, settings.Max);
		_tool = new Hammer(http, () => Factory.Message(settings), strengths);
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

	protected override async Task OutputProgress()
	{
		uint previewed = 0;
		while (!_tool.Complete() || previewed < _tool.Results.Max(r => r.Key))
		{
			var (key, value) = _tool.Results.OrderBy(r => r.Key).FirstOrDefault(r => r.Key > previewed);
			if (_tool.Results.ContainsKey(key))
			{
				_console.WriteLine($"{key}: {FormatTime(value.Min)}/{FormatTime(value.Mean)}/{FormatTime(value.Median)}/{FormatTime(value.Max)}");
				previewed = key;
			}

			await Task.Delay(1);
		}
	}
}