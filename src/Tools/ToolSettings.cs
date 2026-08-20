using System.ComponentModel;
using LoadTestToolbox.Charts;
using Spectre.Console;
using Spectre.Console.Cli;
using Dimensions = (ushort X, ushort Y);

namespace LoadTestToolbox.Tools;

public abstract class ToolSettings : CommandSettings
{
	[CommandOption("-u|--url")]
	[Description("<required> The URL to send requests to")]
	public Uri? URL { get; init; }

	[CommandOption("-f|--filename")]
	[Description("<required> The PNG file to write the chart to")]
	public string Filename { get; init; } = null!;

	[CommandOption("-i|--image-size")]
	[Description($"The dimensions of the chart image (default: {PlotChart.DefaultSize}")]
	public string ImageSize { get; init; } = PlotChart.DefaultSize;

	[CommandOption("-m|--method")]
	[Description("The HTTP method to use (default: GET)")]
	public string Method { get; init; } = HttpMethod.Get.Method;

	[CommandOption("-H|--header")]
	[Description("The HTTP header(s) to add to the request")]
	public string[] Headers { get; init; } = [];

	[CommandOption("-b|--body")]
	[Description("The body of the HTTP request")]
	public string Body { get; init; } = string.Empty;

	public override ValidationResult Validate()
	{
		if (URL == null || !URL.IsAbsoluteUri)
		{
			return ValidationResult.Error("URL is required");
		}

		if (string.IsNullOrWhiteSpace(Filename))
		{
			return ValidationResult.Error("Filename is required");
		}

		if (!ImageSize.Contains('x') || GetDimensions() is (0, 0))
		{
			return ValidationResult.Error("Image dimensions must be in the format WxH");
		}

		return base.Validate();
	}

	public Dimensions GetDimensions()
	{
		var split = ImageSize.Split('x');
		if (!ushort.TryParse(split[0], out var width)
		    || !ushort.TryParse(split[1], out var height))
			return (0, 0);

		return (width, height);
	}
}