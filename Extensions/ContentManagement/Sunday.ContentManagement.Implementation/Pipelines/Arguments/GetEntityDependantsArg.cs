using System.Collections.Generic;
using Sunday.Core.Domain.Interfaces;
using Sunday.Core.Pipelines;

namespace Sunday.ContentManagement.Implementation.Pipelines.Arguments
{
    public class GetEntityDependantsArg : PipelineArg
    {
        public IEntity Entity { get; }
        public List<string> Dependants { get; } = new List<string>();

        public GetEntityDependantsArg(IEntity entity)
        {
            Entity = entity;
        }
    }
}
