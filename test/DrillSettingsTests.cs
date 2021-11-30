using Xunit;

namespace LoadTestToolbox.Tests;

public sealed class DrillSettingsTests
{
	[Fact]
	public void URLIsRequired()
	{
		//arrange
		var settings = new DrillSettings
		{
			RPS = 1,
			Duration = 1,
			Filename = "test.png"
		};

		//act
		var result = settings.Validate();

		//assert
		Assert.False(result.Successful);
	}

	[Fact]
	public void FilenameIsRequired()
	{
		//arrange
		var settings = new DrillSettings
		{
			URL = new Uri("http://localhost"),
			RPS = 1,
			Duration = 1
		};

		//act
		var result = settings.Validate();

		//assert
		Assert.False(result.Successful);
	}

	[Fact]
	public void RPSIsRequired()
	{
		//arrange
		var settings = new DrillSettings
		{
			URL = new Uri("http://localhost"),
			Duration = 1,
			Filename = "test.png"
		};

		//act
		var result = settings.Validate();

		//assert
		Assert.False(result.Successful);
	}

	[Fact]
	public void DurationIsRequired()
	{
		//arrange
		var settings = new DrillSettings
		{
			URL = new Uri("http://localhost"),
			RPS = 1,
			Filename = "test.png"
		};

		//act
		var result = settings.Validate();

		//assert
		Assert.False(result.Successful);
	}

	[Fact]
	public void SuccessWhenAllExist()
	{
		//arrange
		var settings = new DrillSettings
		{
			URL = new Uri("http://localhost"),
			RPS = 1,
			Duration = 1,
			Filename = "test.png"
		};

		//act
		var result = settings.Validate();

		//assert
		Assert.True(result.Successful);
	}
}