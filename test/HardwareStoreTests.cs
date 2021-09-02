using System.Collections.Generic;
using Xunit;

namespace LoadTestToolbox.Tests
{
	public class HardwareStoreTests
	{
		[Theory]
		[InlineData("drill", Tool.Drill)]
		[InlineData("hammer", Tool.Hammer)]
		public void GetsCorrectTool(string name, Tool expected)
		{
			//act
			var tool = HardwareStore.FindTool(name);

			//assert
			Assert.Equal(expected, tool);
		}

		[Theory]
		[InlineData(1, 1, new uint[] { 1 })]
		[InlineData(1, 10, new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
		[InlineData(10, 10, new uint[] { 10 })]
		[InlineData(10, 100, new uint[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 })]
		[InlineData(10, 95, new uint[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 95 })]
		[InlineData(5, 500, new uint[] { 5, 6, 7, 8, 9, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 200, 300, 400, 500 })]
		public void GetsCorrectHammers(uint min, uint max, IEnumerable<uint> expected)
		{
			//act
			var hammers = HardwareStore.GetHammers(min, max);

			//assert
			Assert.Equal(expected, hammers);
		}
	}
}