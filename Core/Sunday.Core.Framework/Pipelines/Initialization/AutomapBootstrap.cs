using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Sunday.Core.Framework.Automap;
using Sunday.Core.Framework.Helpers;
using Sunday.Core.Pipelines.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sunday.Core.Framework.Pipelines.Initialization
{
    public class AutomapBootstrap
    {
        public void Process(PipelineArg arg)
        {
            var services = (arg as InitializationArg).ServiceCollection;
            var assemblies = AssemblyHelper.GetAllAssemblies(x => !x.StartsWith("api-") && !x.StartsWith("Microsoft") && !x.StartsWith("System") &&
            (x.Contains("Sunday") || x.Contains("Plugin"))).ToArray();
            var types = AssemblyHelper.GetClassesWithAttribute(assemblies, typeof(MappedToAttribute));
            var mappingProfile = new MappingProfile();
            foreach (var type in types)
            {
                var mappedTypeAttr = type.GetCustomAttribute<MappedToAttribute>();
                if (mappedTypeAttr?.MappedType != null && mappedTypeAttr.MappedType.Any())
                {
                    foreach (var mappedType in mappedTypeAttr.MappedType)
                    {
                        var mapExp = mappingProfile.CreateMap(type, mappedType);
                        if (mappedTypeAttr.TwoWay)
                        {
                            var reversMapExp = mappingProfile.CreateMap(mappedType, type);
                        }
                    }
                }
            }
            mappingProfile.CreateMap<long, DateTime>().ConvertUsing<TicksToDateTimeConverter>();
            mappingProfile.CreateMap<DateTime, long>().ConvertUsing<DatetimeToTicksConverter>();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(mappingProfile);
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            var serviceProvider = services.BuildServiceProvider();
            ServiceActivator.Configure(serviceProvider);
        }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

        }
    }
}
