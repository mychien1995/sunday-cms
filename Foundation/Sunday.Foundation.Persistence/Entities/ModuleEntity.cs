using System;
using Sunday.Core;
using Sunday.Foundation.Domain;

namespace Sunday.Foundation.Persistence.Entities
{
    [MappedTo(typeof(ApplicationModule))]
    public class ModuleEntity
    {
        public Guid Id { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public string ModuleCode { get; set; } = string.Empty;
    }
}
