using Spectre.Console;

namespace LoadTestToolbox.Tools.Nailgun;

public sealed class Nailer : Wielder<Nailgun, double>
{
	public Nailer(HttpClient http, ProgressTask task, NailgunSettings settings)
	{
		task.MaxValue(settings.Requests);
		_tool = new Nailgun(http, () => Factory.Message(settings), () => task.Increment(1), settings.Requests);
	}
}