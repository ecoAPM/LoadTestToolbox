using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace LoadTestToolbox;

public sealed class NailgunSettings : ToolSettings
{
	[CommandOption("-r|--requests")]
	[Description("the number of requests to send")]
	public ushort Requests { get; init; }

	public override ValidationResult Validate()
	{
		if (Requests == 0)
		{
			return ValidationResult.Error("Value for number of requests is required");
		}

		return base.Validate();
	}
}