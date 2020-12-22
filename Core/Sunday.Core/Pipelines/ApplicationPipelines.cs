﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Sunday.Core.Pipelines.Arguments;

namespace Sunday.Core.Pipelines
{
    public class ApplicationPipelines
    {
        private static readonly ILogger Logger = Log.Logger;
        private static readonly ConcurrentDictionary<string, List<Type>> PipelineTypes = new ConcurrentDictionary<string, List<Type>>();
        public static void Initialize(XmlDocument configFile)
        {
            var pipelinesNode = configFile.SelectSingleNode("/configuration/pipelines");
            if (pipelinesNode == null || !pipelinesNode.HasChildNodes)
            {
                Logger.Warning("Start Loading CMS Pipelines. No pipelines found");
                return;
            }
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
                if (PipelineTypes.ContainsKey(pipelineName))
                {
                    PipelineTypes.TryRemove(pipelineName, out _);
                }
                var executorTypes = processorList.Select(p => Type.GetType(p, true, true)!).ToList();
                PipelineTypes.TryAdd(pipelineName, executorTypes);
                Logger.Information($"Pipeline {pipelineName} loaded with {executorTypes.Count} processors");
            }
        }

        public static async Task RunAsync(string pipelineName, PipelineArg arg)
        {
            using var scope = ServiceActivator.HasScope() ? ServiceActivator.GetScope() : null;
            if (PipelineTypes.TryGetValue(pipelineName, out var types))
            {
                Logger.Debug($"Executing pipeline {pipelineName}");
                foreach (var type in types)
                {
                    var isAsync = false;
                    MethodInfo processMethod;
                    if (typeof(IPipelineProcessor).IsAssignableFrom(type))
                    {
                        processMethod = type.GetMethod(nameof(IPipelineProcessor.Process))!;
                    }
                    else if (typeof(IAsyncPipelineProcessor).IsAssignableFrom(type))
                    {
                        processMethod = type.GetMethod(nameof(IAsyncPipelineProcessor.ProcessAsync))!;
                        isAsync = true;
                    }
                    else throw new InvalidOperationException($"Processor {type.Name} must be inherited from IPipelineProcessor or IAsyncPipelineProcessor");
                    var executor = scope != null ? ActivatorUtilities.CreateInstance(scope.ServiceProvider, type) : Activator.CreateInstance(type);
                    if (!isAsync)
                        processMethod.Invoke(executor, new object?[] { arg });
                    else
                        await (Task)processMethod.Invoke(executor, new object?[] { arg })!;
                    if (arg != null && arg.Aborted) break;
                }
            }
        }
    }
}
