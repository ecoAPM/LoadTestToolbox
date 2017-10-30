namespace LoadTestToolbox
{
    public class ChartLabels
    {
        public string TitleLabel { get; set; } = "LoadTestToolbox";
        public string XAxisLabel { get; set; } = "Request(s)";
        public string YAxisLabel { get; set; } = "Response Time (ms)";

        public ChartLabels()
        {
            
        }

        public ChartLabels(string titleLabel, string xAxisLabel, string yAxisLabel)
        {
            TitleLabel = titleLabel;
            XAxisLabel = xAxisLabel;
            YAxisLabel = yAxisLabel;
        }
    }
}
