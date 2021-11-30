using Spectre.Console.Cli;
using Xunit;

namespace LoadTestToolbox.Tests;

public sealed class FactoryTests
{
	[Fact]
	public void CanCreateApp()
	{
		//arrange/act
		var app = Factory.App();

		//assert
		Assert.IsType<CommandApp>(app);
	}
}