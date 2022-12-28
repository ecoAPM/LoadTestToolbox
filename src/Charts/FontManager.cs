using SkiaSharp;

namespace LoadTestToolbox.Charts;

public static class FontManager
{
	private static readonly IReadOnlyCollection<string> _fonts = SKFontManager.Default.FontFamilies.ToArray();
	private static readonly IReadOnlyCollection<string> _defaultOrder = new[] { "Noto Sans", "Open Sans", "Roboto", "Segoe UI", "Arial", "San Francisco", "Helvetica Neue", "Helvetica" };

	public static readonly string DefaultFont
		= _defaultOrder.FirstOrDefault(name => _fonts.Any(f => f == name))
		  ?? _fonts.FirstOrDefault(name => name.Contains("Sans") && !name.Contains("Fallback"))
		  ?? _fonts.First();
}