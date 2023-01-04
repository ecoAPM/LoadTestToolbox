using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace LoadTestToolbox.Tools;

public abstract class ToolSettings : CommandSettings
{
	[CommandOption("-u|--url")]
	[Description("<required> The URL to send requests to")]
	public Uri? URL { get; init; }

	[CommandOption("-f|--filename")]
	[Description("<required> The file to write the chart to")]
	public string Filename { get; init; } = null!;

	[CommandOption("-m|--method")]
	[Description("The HTTP method to use")]
	public string Method { get; init; } = HttpMethod.Get.Method;

	[CommandOption("-H|--header")]
	[Description("The HTTP header(s) to add to the request")]
	public string[] Headers { get; init; } = Array.Empty<string>();

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

		return base.Validate();
	}
}