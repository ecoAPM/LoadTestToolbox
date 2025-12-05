using System.Collections.Concurrent;
using Xunit;

namespace LoadTestToolbox.Tests;

public class StatsTests
{
	[Fact]
	public void CanGetStatsForOddNumberedList()
	{
		//arrange
		var results = new Dictionary<uint, Result>
		{
			{ 1, new Result(200, 0.1) },
			{ 2, new Result(200, 0.5) },
			{ 3, new Result(200, 0.9) },
			{ 4, new Result(200, 0.7) },
			{ 5, new Result(200, 0.8) }
		}.AsConcurrent();

		//act
		var stats = new Stats(results);

		//assert
		Assert.Equal(0.1, stats.Min, 15);
		Assert.Equal(0.6, stats.Mean, 15);
		Assert.Equal(0.7, stats.Median, 15);
		Assert.Equal(0.9, stats.Max, 15);
	}

	[Fact]
	public void CanGetStatsForEvenNumberedList()
	{
		//arrange
		var results = new Dictionary<uint, Result>
		{
			{ 1, new Result(200, 0.1) },
			{ 2, new Result(200, 0.5) },
			{ 3, new Result(200, 0.9) },
			{ 4, new Result(200, 0.7) },
			{ 5, new Result(200, 0.6) },
			{ 6, new Result(200, 0.2) }
		}.AsConcurrent();

		//act
		var stats = new Stats(results);

		//assert
		Assert.Equal(0.1, stats.Min, 15);
		Assert.Equal(0.5, stats.Mean, 15);
		Assert.Equal(0.55, stats.Median, 15);
		Assert.Equal(0.9, stats.Max, 15);
	}

	[Fact]
	public void CanHandleEmptyList()
	{
		//arrange
		var results = new ConcurrentDictionary<uint, Result>();

		//act
		var stats = new Stats(results);

		//assert
		Assert.Equal(0, stats.Min);
		Assert.Equal(0, stats.Mean);
		Assert.Equal(0, stats.Median);
		Assert.Equal(0, stats.Max);
	}
}