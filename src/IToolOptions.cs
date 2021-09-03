using System;

namespace LoadTestToolbox
{
	public interface IToolOptions<out T> where T : ITool
	{
		Uri URL { get; init; }
	}
}