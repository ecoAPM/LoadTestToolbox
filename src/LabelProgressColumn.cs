using Spectre.Console;
using Spectre.Console.Rendering;

namespace LoadTestToolbox;

public sealed class LabelProgressColumn(string text) : ProgressColumn
{
	public override IRenderable Render(RenderOptions options, ProgressTask task, TimeSpan deltaTime)
		=> new Markup(text);
}