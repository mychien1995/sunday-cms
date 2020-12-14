using System.Collections.Generic;
using Sunday.Core.Domain.Interfaces;
using Sunday.Core.Pipelines;

namespace Sunday.ContentManagement.Implementation.Pipelines.Arguments
{
    public class GetEntityMastersArg : PipelineArg
    {
        public IEntity Entity { get; }
        public List<string> Masters { get; } = new List<string>();

        public GetEntityMastersArg(IEntity entity)
        {
            Entity = entity;
        }
    }
}