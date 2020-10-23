using Sunday.Core.Domain.Interfaces;

namespace Sunday.Core.Pipelines.Arguments
{
    public class BeforeCreateEntityArg : PipelineArg
    {
        public BeforeCreateEntityArg(IEntity entity)
        {
            Entity = entity;
        }

        public IEntity Entity { get; set; }
    }
}
