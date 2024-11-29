using SkiaSharp;

namespace LoadTestToolbox.Charts;

public static class FontManager
{
	private static readonly string[] Fonts
		= SKFontManager.Default.FontFamilies.ToArray();

	private static readonly string[] DefaultOrder =
	[
		"Noto Sans",
		"Open Sans",
		"Roboto",
		"Segoe UI",
		"Arial",
		"San Francisco",
		"Helvetica Neue",
		"Helvetica"
	];

	public static readonly string DefaultFont
		= DefaultOrder.FirstOrDefault(name => Fonts.Any(f => f == name))
		  ?? Fonts.FirstOrDefault(name => name.Contains("Sans") && !name.Contains("Fallback"))
		  ?? Fonts.FirstOrDefault()
		  ?? throw new FileNotFoundException("No fonts installed");
}