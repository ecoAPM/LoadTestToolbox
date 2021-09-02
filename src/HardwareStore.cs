using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace LoadTestToolbox
{
	public static class HardwareStore
	{
		public static IEnumerable<uint> GetHammers(uint min, uint max)
		{
			var list = new List<uint>();
			var minOrder = (uint)Math.Log10(min);
			var maxOrder = (uint)Math.Log10(max);

			for (var n = minOrder; n <= maxOrder; n++)
			{
				var pow = (uint)Math.Pow(10, n);
				for (var x = (n == minOrder ? min : 2 * pow); x <= (n == maxOrder ? max : Math.Pow(10, n + 1)); x += pow)
					list.Add(x);
			}

			if (list.Last() < max)
				list.Add(max);

			return list;
		}

		public static Tool FindTool(string tool)
			=> Enum.Parse<Tool>(CultureInfo.InvariantCulture.TextInfo.ToTitleCase(tool));
	}
}