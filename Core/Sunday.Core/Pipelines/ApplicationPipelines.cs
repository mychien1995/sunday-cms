using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.DependencyInjection;

namespace Sunday.Core.Pipelines
{
    public class ApplicationPipelines
    {
        private static readonly ConcurrentDictionary<string, List<Type>> PipelineTypes = new ConcurrentDictionary<string, List<Type>>();
        private static readonly ConcurrentDictionary<string, List<string>> PipelineDefinitions = new ConcurrentDictionary<string, List<string>>();
        public static void Initialize(XmlDocument configFile)
        {
            var pipelinesNode = configFile.SelectSingleNode("/configuration/pipelines");
            if (pipelinesNode == null || !pipelinesNode.HasChildNodes) return;
            var childNodes = pipelinesNode.ChildNodes;
            foreach (var node in childNodes)
            {
                var pipelineNode = node as XmlNode;
                if (pipelineNode == null) continue;
                var pipelineName = pipelineNode.Name;
                var processorList = new List<string>();
                if (pipelineNode.HasChildNodes)
                {
                    foreach (var childNode in pipelineNode.ChildNodes)
                    {
                        if (childNode is XmlNode processorNode &&
                            processorNode.Name == "processor" && !string.IsNullOrEmpty(processorNode.Attributes["type"]?.Value))
                        {
                            processorList.Add(processorNode.Attributes["type"].Value);
                        }
                    }
                }

                if (!processorList.Any()) continue;
                if (PipelineDefinitions.ContainsKey(pipelineName))
                {
                    PipelineDefinitions.TryRemove(pipelineName, out _);
                }
                PipelineDefinitions.TryAdd(pipelineName, processorList);
            }
        }

        public static void Run(string pipelineName, PipelineArg arg)
        {
            using var scope = ServiceActivator.GetScope();
            if (PipelineTypes.TryGetValue(pipelineName, out var types))
            {
                foreach (var type in types)
                {
                    var processMethod = type.GetMethod("Process");
                    if (processMethod == null) continue;
                    var executor = ActivatorUtilities.CreateInstance(scope.ServiceProvider, type);
                    processMethod.Invoke(executor, new object?[] { arg });
                    if (arg != null && arg.Aborted) break;
                }
            }
            else if (PipelineDefinitions.TryGetValue(pipelineName, out var definitions))
            {
                var executorTypes = definitions.Select(x => Type.GetType(x, true, true)!).ToList();
                PipelineTypes.TryAdd(pipelineName, executorTypes);
                Run(pipelineName, arg);
            }
        }

        public static async Task RunAsync(string pipelineName, PipelineArg arg)
        {
            using var scope = ServiceActivator.GetScope();
            if (PipelineTypes.TryGetValue(pipelineName, out var types))
            {
                foreach (var type in types)
                {
                    var processMethod = type.GetMethod("ProcessAsync");
                    if (processMethod == null) continue;
                    var executor = ActivatorUtilities.CreateInstance(scope.ServiceProvider, type);
                    var task = (Task)processMethod.Invoke(executor, new object?[] { arg })!;
                    await task;
                    if (arg != null && arg.Aborted) break;
                }
            }
            else if (PipelineDefinitions.TryGetValue(pipelineName, out var definitions))
            {
                var executorTypes = definitions.Select(x => Type.GetType(x, true, true)!).ToList();
                PipelineTypes.TryAdd(pipelineName, executorTypes);
                await RunAsync(pipelineName, arg);
            }
        }
    }
}
