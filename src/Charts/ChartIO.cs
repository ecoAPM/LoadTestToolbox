namespace LoadTestToolbox.Charts;

public interface ChartIO
{
	Task SaveChart(PlotChart chart, string filename);
}