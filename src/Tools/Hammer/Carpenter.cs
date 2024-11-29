using Spectre.Console;

namespace LoadTestToolbox.Tools.Hammer;

public sealed class Carpenter : Wielder<Hammer, Stats>
{
	public Carpenter(HttpClient http, ProgressTask task, HammerSettings settings)
	{
		var strengths = GetStrengths(settings.Min, settings.Max);
		task.MaxValue(strengths.Sum(s => s));
		_tool = new Hammer(http, () => Factory.Message(settings), () => task.Increment(1), strengths);
	}

	public static uint[] GetStrengths(uint min, uint max)
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

		if (list[^1] < max)
			list.Add(max);

		return list.ToArray();
	}
}