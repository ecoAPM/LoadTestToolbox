using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace LoadTestToolbox;

public abstract class ToolSettings : CommandSettings
{
	[CommandOption("-u|--url")]
	[Description("the URL to send requests to")]
	public Uri? URL { get; init; }

	[CommandOption("-f|--filename")]
	[Description("the file to write the chart to")]
	public string Filename { get; init; } = null!;

	[CommandOption("-m|--method")]
	[Description("the HTTP method to use")]
	public string Method { get; init; } = HttpMethod.Get.Method;

	[CommandOption("-H|--header")]
	[Description("the HTTP header(s) to add to the request")]
	public string[] Headers { get; init; } = Array.Empty<string>();

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