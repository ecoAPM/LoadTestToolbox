using LoadTestToolbox.Tools.Hammer;
using Xunit;

namespace LoadTestToolbox.Tests.Tools.Hammer;

public sealed class HammerSettingsTests
{
	[Fact]
	public void URLIsRequired()
	{
		//arrange
		var settings = new HammerSettings
		{
			Min = 1,
			Max = 1,
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
		var settings = new HammerSettings
		{
			URL = new Uri("http://localhost"),
			Min = 1,
			Max = 1
		};

		//act
		var result = settings.Validate();

		//assert
		Assert.False(result.Successful);
	}

	[Fact]
	public void MinIsRequired()
	{
		//arrange
		var settings = new HammerSettings
		{
			URL = new Uri("http://localhost"),
			Max = 1,
			Filename = "test.png"
		};

		//act
		var result = settings.Validate();

		//assert
		Assert.False(result.Successful);
	}

	[Fact]
	public void MaxIsRequired()
	{
		//arrange
		var settings = new HammerSettings
		{
			URL = new Uri("http://localhost"),
			Min = 1,
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
		var settings = new HammerSettings
		{
			URL = new Uri("http://localhost"),
			Min = 1,
			Max = 1,
			Filename = "test.png"
		};

		//act
		var result = settings.Validate();

		//assert
		Assert.True(result.Successful);
	}
}