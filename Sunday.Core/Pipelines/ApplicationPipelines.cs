using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Sunday.Core.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sunday.Core
{
    public class ApplicationPipelines
    {
        private static ConcurrentDictionary<string, List<object>> _pipelineExecutors = new ConcurrentDictionary<string, List<object>>();
        private static ConcurrentDictionary<string, List<string>> _pipelineDefinitions = new ConcurrentDictionary<string, List<string>>();

        public static void Initialize(ConfigurationNode configurationNode)
        {
            if (configurationNode == null || configurationNode.Pipelines == null || !configurationNode.Pipelines.Any()) return;
            foreach (var pipeline in configurationNode.Pipelines.Where(x => !string.IsNullOrEmpty(x.Name)))
            {
                if (_pipelineDefinitions.ContainsKey(pipeline.Name))
                {
                    _pipelineDefinitions.TryRemove(pipeline.Name, out List<string> tmp);
                }
                _pipelineDefinitions.TryAdd(pipeline.Name, pipeline.Processors.Where(x => !string.IsNullOrEmpty(x?.Type)).Select(x => x.Type).ToList());
            }
        }
        public static void Run(string pipelineName, PipelineArg arg)
        {
            using (var scope = ServiceActivator.GetScope())
            {
                if (_pipelineExecutors.TryGetValue(pipelineName, out List<object> executors))
                {
                    foreach (var executor in executors)
                    {
                        var processMethod = executor.GetType().GetMethod("Process");
                        if (processMethod == null) continue;
                        else processMethod.Invoke(executor, new object[1] { arg });
                        if (arg != null && arg.Aborted) break;
                    }
                }
                else if (_pipelineDefinitions.TryGetValue(pipelineName, out List<string> definitions))
                {
                    var executorObjs = definitions.Select(x => Type.GetType(x, true, true)).Select(x => ActivatorUtilities.CreateInstance(scope.ServiceProvider, x)).ToList();
                    _pipelineExecutors.TryAdd(pipelineName, executorObjs);
                    Run(pipelineName, arg);
                }
            }
        }

        public static async Task RunAsync(string pipelineName, PipelineArg arg)
        {
            using (var scope = ServiceActivator.GetScope())
            {
                if (_pipelineExecutors.TryGetValue(pipelineName, out List<object> executors))
                {
                    foreach (var executor in executors)
                    {
                        var processMethod = executor.GetType().GetMethod("ProcessAsync");
                        if (processMethod == null) continue;
                        var task = (Task)processMethod.Invoke(executor, new object[1] { arg });
                        await task;
                        if (arg != null && arg.Aborted) break;
                    }
                }
                else if (_pipelineDefinitions.TryGetValue(pipelineName, out List<string> definitions))
                {
                    var executorObjs = definitions.Select(x => Type.GetType(x, true, true)).Select(x => ActivatorUtilities.CreateInstance(scope.ServiceProvider, x)).ToList();
                    _pipelineExecutors.TryAdd(pipelineName, executorObjs);
                    await RunAsync(pipelineName, arg);
                }
            }
        }
    }
}
