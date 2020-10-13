using System;

namespace Sunday.Foundation.Persistence.Entities
{
    public class ModuleEntity
    {
        public Guid Id { get; set; }
        public string ModuleName { get; set; }
        public string ModuleCode { get; set; }

        public ModuleEntity(Guid id, string moduleName, string moduleCode)
        {
            Id = id;
            ModuleName = moduleName;
            ModuleCode = moduleCode;
        }
    }
}
