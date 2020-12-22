using System;

namespace Sunday.Core.Pipelines.Arguments
{
    public class BeforeDeleteEntityArg : PipelineArg
    {
        public Type EntityType { get; set; }
        public Guid EntityId { get; set; }
        public BeforeDeleteEntityArg(Guid id, Type entityType)
        {
            EntityId = id;
            EntityType = entityType;
            AddProperty("EntityId", id);
        }
    }
}
