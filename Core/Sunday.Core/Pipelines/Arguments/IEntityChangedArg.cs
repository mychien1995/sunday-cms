using Sunday.Core.Domain;

namespace Sunday.Core.Pipelines.Arguments
{
    public interface IEntityChangedArg
    {
        public IEntity EntityChange { get; set; }
    }
}
