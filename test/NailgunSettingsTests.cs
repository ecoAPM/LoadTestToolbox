using Xunit;

namespace LoadTestToolbox.Tests;

public sealed class NailgunSettingsTests
{
	[Fact]
	public void URLIsRequired()
	{
		//arrange
		var settings = new NailgunSettings
		{
			Requests = 1,
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
		var settings = new NailgunSettings
		{
			URL = new Uri("http://localhost"),
			Requests = 1
		};

		//act
		var result = settings.Validate();

		//assert
		Assert.False(result.Successful);
	}

	[Fact]
	public void NumberOfRequestsIsRequired()
	{
		//arrange
		var settings = new NailgunSettings
		{
			URL = new Uri("http://localhost"),
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
		var settings = new NailgunSettings
		{
			URL = new Uri("http://localhost"),
			Requests = 1,
			Filename = "test.png"
		};

		//act
		var result = settings.Validate();

		//assert
		Assert.True(result.Successful);
	}
}