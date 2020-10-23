using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Sunday.Core.Framework.Automap;
using Sunday.Core.Framework.Helpers;
using Sunday.Core.Pipelines.Arguments;
using System;
using System.Linq;
using System.Reflection;
using Sunday.Core.Pipelines;

namespace Sunday.Core.Framework.Pipelines.ConfigureServices
{
    public class AutomapBootstrap : IPipelineProcessor
    {
        public void Process(PipelineArg pipelineArg)
        {
            var arg = (ConfigureServicesArg) pipelineArg;
            var services = arg.ServicesCollection;
            services.AddSingleton(_ =>
            {
                var assemblies = AssemblyHelper.GetAllAssemblies(x => !x.StartsWith("api-") && !x.StartsWith("Microsoft") && !x.StartsWith("System") &&
                                                                      (x.Contains("Sunday") || x.Contains("Plugin"))).ToArray();
                var types = AssemblyHelper.GetClassesWithAttribute(assemblies, typeof(MappedToAttribute));
                var mappingProfile = new MappingProfile();
                foreach (var type in types)
                {
                    var mappedTypeAttr = type.GetCustomAttribute<MappedToAttribute>();
                    if (mappedTypeAttr?.MappedType == null || !mappedTypeAttr.MappedType.Any()) continue;
                    foreach (var mappedType in mappedTypeAttr.MappedType)
                    {
                        mappingProfile.CreateMap(type, mappedType);
                        if (mappedTypeAttr.TwoWay)
                        {
                            mappingProfile.CreateMap(mappedType, type);
                        }
                    }
                }
                mappingProfile.CreateMap<long, DateTime>().ConvertUsing<TicksToDateTimeConverter>();
                mappingProfile.CreateMap<DateTime, long>().ConvertUsing<DatetimeToTicksConverter>();
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(mappingProfile);
                });
                var mapper = mappingConfig.CreateMapper();
                return mapper;
            });
        }
    }

    public class MappingProfile : Profile
    {
    }
}
