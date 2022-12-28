using LoadTestToolbox.Tools.Hammer;
using Spectre.Console;
using Xunit;

namespace LoadTestToolbox.Tests.Tools.Hammer;

public sealed class CarpenterTests
{
	[Theory]
	[InlineData(1, 1, new uint[] { 1 })]
	[InlineData(1, 10, new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
	[InlineData(10, 10, new uint[] { 10 })]
	[InlineData(10, 100, new uint[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 })]
	[InlineData(10, 95, new uint[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 95 })]
	[InlineData(5, 500, new uint[] { 5, 6, 7, 8, 9, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 200, 300, 400, 500 })]
	public void GetsCorrectStrengths(uint min, uint max, IReadOnlyCollection<uint> expected)
	{
		//act
		var strengths = Carpenter.GetStrengths(min, max);

		//assert
		Assert.Equal(expected, strengths);
	}

	[Fact]
	public void CanHammer()
	{
		//arrange
		using var http = new HttpClient(new MockHttpMessageHandler());
		var task = new ProgressTask(123, "test", 0, false);
		var options = new HammerSettings
		{
			URL = new Uri("http://localhost"),
			Min = 1,
			Max = 100
		};
		var carpenter = new Carpenter(http, task, options);

		//act
		var results = carpenter.Run();

		//assert
		Assert.Equal(19, results.Count);
	}
}