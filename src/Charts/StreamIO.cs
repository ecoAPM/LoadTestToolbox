using SkiaSharp;

namespace LoadTestToolbox.Charts;

public class StreamIO : ChartIO
{
	private readonly Func<string, Stream> _fileWriter;

	public StreamIO(Func<string, Stream> fileWriter)
		=> _fileWriter = fileWriter;

	public async Task SaveChart(SkiaChart chart, string filename)
	{
		var chartData = chart.GetChart();
		using var image = chartData.GetImage();
		using var imageData = image.Encode(SKEncodedImageFormat.Png, 100);
		var imageArray = imageData.ToArray();
		using var stream = new MemoryStream(imageArray);
		await using var output = _fileWriter(filename);
		await stream.CopyToAsync(output);
	}
}