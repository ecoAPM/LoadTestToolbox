using Spectre.Console;
using Spectre.Console.Rendering;
using Spectre.Console.Testing;
using Xunit;

namespace LoadTestToolbox.Tests;

public class LabelProgressColumnTests
{
	[Fact]
	public void CanRenderColumn()
	{
		//arrange
		var column = new LabelProgressColumn("[[ test ]]");

		var console = new TestConsole();
		var capabilities = new TestCapabilities();
		var context = new RenderOptions(capabilities, new Size());
		var task = new ProgressTask(123, "test", 0, false);

		//act
		var markup = column.Render(context, task, TimeSpan.Zero) as Markup;

		//assert
		var segments = markup!.GetSegments(console).Select(s => s.Text);
		var output = string.Join("", segments);
		Assert.Equal("[ test ]", output);
	}
}