namespace LoadTestToolbox.Charts;

public interface ChartIO
{
	Task SaveChart(SkiaChart chart, string filename);
}