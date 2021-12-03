using Xunit;

namespace LoadTestToolbox.Tests;

public sealed class ProgramTests
{
	[Fact] public async Task CanInvokeDrill()
	{
		//arrange
		var args = new[] { "drill" };

		//act
		var task = Program.Main(args);

		//assert
		await task;
		Assert.True(task.IsCompletedSuccessfully);
	}

	[Fact]
	public async Task CanInvokeHammer()
	{
		//arrange
		var args = new[] { "hammer" };

		//act
		var task = Program.Main(args);

		//assert
		await task;
		Assert.True(task.IsCompletedSuccessfully);
	}

	[Fact]
	public async Task CanInvokeNailgun()
	{
		//arrange
		var args = new[] { "nailgun" };

		//act
		var task = Program.Main(args);

		//assert
		await task;
		Assert.True(task.IsCompletedSuccessfully);
	}
}