using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoadTestToolbox
{
    public interface IVisualizer
    {
        Task<string> GetChart(IDictionary<int, double> results);
    }
}