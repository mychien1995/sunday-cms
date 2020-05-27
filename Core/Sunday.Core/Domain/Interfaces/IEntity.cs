using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core.Domain
{
    public interface IEntity : IChangeTrackable, IUniqueEntity
    {
    }
}
