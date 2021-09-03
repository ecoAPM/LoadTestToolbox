using System.Threading.Tasks;

namespace LoadTestToolbox
{
	public interface ITool
	{
		Task Run();
		bool Complete();
	}
}