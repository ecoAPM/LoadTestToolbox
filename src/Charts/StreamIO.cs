using SkiaSharp;

namespace LoadTestToolbox.Charts;

public class StreamIO(Func<string, Stream> fileWriter) : ChartIO
{
	public async Task SaveChart(SkiaChart chart, string filename)
	{
		var chartData = chart.GetChart();
		using var image = chartData.GetImage();
		using var imageData = image.Encode(SKEncodedImageFormat.Png, 100);
		var imageArray = imageData.ToArray();
		using var stream = new MemoryStream(imageArray);
		await using var output = fileWriter(filename);
		await stream.CopyToAsync(output);
	}
}