using Xunit;

namespace LoadTestToolbox.Tests;

public sealed class WielderTests
{
	[Theory]
	[InlineData(0.1234, "123 μs")]
	[InlineData(1.234, "1.23 ms")]
	[InlineData(12.34, "12.3 ms")]
	[InlineData(123.4, "123 ms")]
	[InlineData(1234, "1234 ms")]
	public void FormatsTimeCorrectly(double ms, string expected)
	{
		//act
		var output = Wielder<Tool>.FormatTime(ms);

		//assert
		Assert.Equal(expected, output);
	}
}