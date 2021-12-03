using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace LoadTestToolbox;

public sealed class DrillSettings : ToolSettings
{
	[CommandOption("-r|--rps")]
	[Description("The number of requests per second to send")]
	public ushort RPS { get; init; }

	[CommandOption("-d|--duration")]
	[Description("The duration (in seconds) to send requests for")]
	public byte Duration { get; init; }

	public override ValidationResult Validate()
	{
		if (RPS == 0)
		{
			return ValidationResult.Error("Value for requests per second is required");
		}

		if (Duration == 0)
		{
			return ValidationResult.Error("Duration is required");
		}

		return base.Validate();
	}
}