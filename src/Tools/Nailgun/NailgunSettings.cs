using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace LoadTestToolbox.Tools.Nailgun;

public sealed class NailgunSettings : ToolSettings
{
	[CommandOption("-r|--requests")]
	[Description("<required> The number of requests to send")]
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