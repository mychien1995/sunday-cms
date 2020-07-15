using Microsoft.Extensions.DependencyInjection;
using Sunday.Core.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace Sunday.Core
{
    public class ApplicationPipelines
    {
        private static ConcurrentDictionary<string, List<Type>> _pipelineTypes = new ConcurrentDictionary<string, List<Type>>();
        private static ConcurrentDictionary<string, List<string>> _pipelineDefinitions = new ConcurrentDictionary<string, List<string>>();
        public static void Initialize(XmlDocument configFile)
        {
            var piplinesNode = configFile.SelectSingleNode("/configuration/pipelines");
            if (piplinesNode == null || !piplinesNode.HasChildNodes) return;
            var childNodes = piplinesNode.ChildNodes;
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
                        var processorNode = childNode as XmlNode;
                        if (processorNode.Name == "processor" && !string.IsNullOrEmpty(processorNode.Attributes["type"]?.Value))
                        {
                            processorList.Add(processorNode.Attributes["type"]?.Value);
                        }
                    }
                }

                if (!processorList.Any()) continue;
                if (_pipelineDefinitions.ContainsKey(pipelineName))
                {
                    _pipelineDefinitions.TryRemove(pipelineName, out List<string> tmp);
                }
                _pipelineDefinitions.TryAdd(pipelineName, processorList);
            }
        }

        public static void Initialize(ConfigurationNode configurationNode)
        {
            if (configurationNode?.Pipelines == null || !configurationNode.Pipelines.Any()) return;
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
            using var scope = ServiceActivator.GetScope();
            if (_pipelineTypes.TryGetValue(pipelineName, out List<Type> types))
            {
                foreach (var type in types)
                {
                    var processMethod = type.GetMethod("Process");
                    if (processMethod == null) continue;
                    var executor = ActivatorUtilities.CreateInstance(scope.ServiceProvider, type);
                    processMethod.Invoke(executor, new object[1] { arg });
                    if (arg != null && arg.Aborted) break;
                }
            }
            else if (_pipelineDefinitions.TryGetValue(pipelineName, out List<string> definitions))
            {
                var executorObjs = definitions.Select(x => Type.GetType(x, true, true)).ToList();
                _pipelineTypes.TryAdd(pipelineName, executorObjs);
                Run(pipelineName, arg);
            }
        }

        public static async Task RunAsync(string pipelineName, PipelineArg arg)
        {
            using var scope = ServiceActivator.GetScope();
            if (_pipelineTypes.TryGetValue(pipelineName, out List<Type> types))
            {
                foreach (var type in types)
                {
                    var processMethod = type.GetMethod("ProcessAsync");
                    if (processMethod == null) continue;
                    var executor = ActivatorUtilities.CreateInstance(scope.ServiceProvider, type);
                    var task = (Task)processMethod.Invoke(executor, new object[1] { arg });
                    await task;
                    if (arg != null && arg.Aborted) break;
                }
            }
            else if (_pipelineDefinitions.TryGetValue(pipelineName, out List<string> definitions))
            {
                var executorObjs = definitions.Select(x => Type.GetType(x, true, true)).ToList();
                _pipelineTypes.TryAdd(pipelineName, executorObjs);
                await RunAsync(pipelineName, arg);
            }
        }
    }
}
