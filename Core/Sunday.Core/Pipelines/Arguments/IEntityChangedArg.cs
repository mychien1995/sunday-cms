using Sunday.Core.Domain;
using Sunday.Core.Domain.Interfaces;

namespace Sunday.Core.Pipelines.Arguments
{
    public interface IEntityChangedArg
    {
        public IEntity EntityChange { get; set; }
    }
}
