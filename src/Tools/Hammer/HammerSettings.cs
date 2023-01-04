using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace LoadTestToolbox.Tools.Hammer;

public sealed class HammerSettings : ToolSettings
{
	[CommandOption("--min")]
	[Description("<required> The minimum number of simultaneous requests to send")]
	public ushort Min { get; init; }

	[CommandOption("--max")]
	[Description("<required> The maximum number of simultaneous requests to send")]
	public ushort Max { get; init; }

	public override ValidationResult Validate()
	{
		if (Min == 0)
		{
			return ValidationResult.Error("Minimum value is required");
		}

		if (Max == 0)
		{
			return ValidationResult.Error("Maximum value is required");
		}

		return base.Validate();
	}
}