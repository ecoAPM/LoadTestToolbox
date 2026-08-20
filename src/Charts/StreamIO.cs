using LoadTestToolbox.Tools;
using ScottPlot;

namespace LoadTestToolbox.Charts;

public class StreamIO(Func<string, Stream> fileWriter) : ChartIO
{
	public async Task SaveChart(PlotChart chart, ToolSettings settings)
	{
		var plot = chart.GetChart();
		var size = settings.GetDimensions();
		var data = plot.GetImageBytes(size.X, size.Y, ImageFormat.Png);
		using var stream = new MemoryStream(data);
		await using var output = fileWriter(settings.Filename);
		await stream.CopyToAsync(output);
	}
}