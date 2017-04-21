using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace LoadTestToolbox
{
    public class NodeVisualizer
    {
        private readonly string _baseDir;
        private readonly INodeServices _node;

        public NodeVisualizer(string baseDir)
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
            var config = JObject.Parse(File.ReadAllText(Path.Combine(_baseDir, "default.json")));
            config["data"]["labels"] = JArray.FromObject(results.Keys);
            config["data"]["datasets"][0]["data"] = JArray.FromObject(results.Select(r => new {x = r.Key, y = r.Value}));
            var result = await _node.InvokeAsync<string>(Path.Combine(_baseDir, "get-chart"), config);
            return result;
        }
    }
}
