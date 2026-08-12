using ScottPlot;

namespace LoadTestToolbox.Charts;

public class StreamIO(Func<string, Stream> fileWriter) : ChartIO
{
	public async Task SaveChart(PlotChart chart, string filename)
	{
		var plot = chart.GetChart();
		var data = plot.GetImageBytes(PlotChart.Width, PlotChart.Height, ImageFormat.Png);
		using var stream = new MemoryStream(data);
		await using var output = fileWriter(filename);
		await stream.CopyToAsync(output);
	}
}