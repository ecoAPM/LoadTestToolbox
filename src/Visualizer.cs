using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace LoadTestToolbox
{
    public class Visualizer : IVisualizer
    {
        private readonly string _baseDir;
        private readonly INodeServices _node;

        public Visualizer(string baseDir)
        {
            _baseDir = baseDir;
            var services = new ServiceCollection();
            services.AddNodeServices();
            var options = new NodeServicesOptions(services.BuildServiceProvider())
            {
                ProjectPath = Directory.GetCurrentDirectory()

            };
            _node = NodeServicesFactory.CreateNodeServices(options);
        }

        public async Task<string> GetChart(IDictionary<int, double> results)
        {
            var config = getConfig(results);
            return await _node.InvokeAsync<string>(Path.Combine(_baseDir, "get-chart"), config);
        }

        private JObject getConfig(IDictionary<int, double> results)
        {
            var xMax = results.Max(r => r.Key);
            var xStep = xMax.GetXStepSize();

            var yMax = results.GetYAxisMax();
            var yStep = yMax.GetYStepSize();

            var config = JObject.Parse(File.ReadAllText(Path.Combine(_baseDir, "default.json")));
            config["data"]["labels"] = JArray.FromObject(results.Keys);
            config["data"]["datasets"][0]["data"] = JArray.FromObject(results.Select(r => new {x = r.Key, y = r.Value}));
            config["options"]["scales"]["xAxes"][0]["ticks"]["fixedStepSize"] = xStep;
            config["options"]["scales"]["xAxes"][0]["ticks"]["stepSize"] = xStep;
            config["options"]["scales"]["xAxes"][0]["ticks"]["max"] = xMax;
            config["options"]["scales"]["yAxes"][0]["ticks"]["fixedStepSize"] = yStep;
            config["options"]["scales"]["yAxes"][0]["ticks"]["max"] = yMax;

            return config;
        }
    }
}
