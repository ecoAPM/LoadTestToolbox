using System.IO;
using System.Threading.Tasks;

namespace LoadTestToolbox
{
    public interface IChart
    {
        public Task Save(Stream output);
    }
}