using Spectre.Console;
using Spectre.Console.Rendering;

namespace LoadTestToolbox;

public sealed class LabelProgressColumn : ProgressColumn
{
	private readonly string _text;

	public LabelProgressColumn(string text)
		=> _text = text;

	public override IRenderable Render(RenderOptions options, ProgressTask task, TimeSpan deltaTime)
		=> new Markup(_text);
}