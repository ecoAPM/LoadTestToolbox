using System.IO;

namespace LoadTestToolbox
{
    public interface IChart
    {
        public void Save(Stream output);
    }
}