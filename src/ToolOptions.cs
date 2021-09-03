using System;

namespace LoadTestToolbox
{
	public abstract class ToolOptions<T> : IToolOptions<T> where T : ITool
	{
		public Uri URL { get; init; }
	}
}