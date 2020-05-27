using Sunday.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core.Pipelines.Arguments
{
    public interface IEntityChangedArg
    {
        public IEntity EntityChange { get; set; }
    }
}
