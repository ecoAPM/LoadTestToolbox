using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoadTestToolbox;

public interface ITool
{
	IDictionary<uint, double> Results { get; }
	Task Run();
	bool Complete();
}
