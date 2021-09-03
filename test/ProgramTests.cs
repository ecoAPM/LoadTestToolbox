using Xunit;

namespace LoadTestToolbox.Tests
{
	public class ProgramTests
	{
		[Fact]
		public void CanCreateApp()
		{
			//act
			var app = Program.AppFactory;

			//assert
			Assert.IsType<App>(app);
		}
	}
}