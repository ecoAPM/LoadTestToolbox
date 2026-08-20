using LoadTestToolbox.Tools;

namespace LoadTestToolbox.Charts;

public interface ChartIO
{
	Task SaveChart(PlotChart chart, ToolSettings settings);
}