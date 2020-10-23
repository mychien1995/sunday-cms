using System;

namespace Sunday.Core.Domain.Interfaces
{
    public interface IUniqueEntity
    {
        public Guid Id { get; set; }
    }
}
