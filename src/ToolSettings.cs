using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace LoadTestToolbox;

public abstract class ToolSettings : CommandSettings
{
	[CommandOption("-u|--url")]
	[Description("the URL to send requests to")]
	public Uri URL { get; init; } = null!;

	[CommandOption("-f|--filename")]
	[Description("the file to write the chart to")]
	public string Filename { get; init; } = null!;

	public override ValidationResult Validate()
	{
		if (URL == null)
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