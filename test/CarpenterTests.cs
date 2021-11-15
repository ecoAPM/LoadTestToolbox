using System;
using System.Collections.Generic;
using System.CommandLine.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace LoadTestToolbox.Tests;

public class CarpenterTests
{
	[Theory]
	[InlineData(1, 1, new uint[] { 1 })]
	[InlineData(1, 10, new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
	[InlineData(10, 10, new uint[] { 10 })]
	[InlineData(10, 100, new uint[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 })]
	[InlineData(10, 95, new uint[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 95 })]
	[InlineData(5, 500, new uint[] { 5, 6, 7, 8, 9, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 200, 300, 400, 500 })]
	public void GetsCorrectStrengths(uint min, uint max, IEnumerable<uint> expected)
	{
		//act
		var strengths = Carpenter.GetStrengths(min, max);

		//assert
		Assert.Equal(expected, strengths);
	}

	[Fact]
	public async Task OutputsCorrectStrengths()
	{
		//arrange
		var http = new HttpClient(new MockHttpMessageHandler());
		var console = new TestConsole();
		var options = new HammerOptions
		{
			URL = new Uri("http://localhost"),
			Min = 1,
			Max = 5
		};
		var carpenter = new Carpenter(http, console, options);

		//act
		await carpenter.Run();

		//assert
		var output = console.Out.ToString() ?? "";
		Assert.Contains("1: ", output);
		Assert.Contains("2: ", output);
		Assert.Contains("3: ", output);
		Assert.Contains("4: ", output);
		Assert.Contains("5: ", output);
	}
}
