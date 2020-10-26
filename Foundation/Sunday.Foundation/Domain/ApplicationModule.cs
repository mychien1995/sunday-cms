using System;

namespace Sunday.Foundation.Domain
{
    public class ApplicationModule
    {
        public Guid Id { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public string ModuleCode { get; set; } = string.Empty;

        public ApplicationModule()
        {
            
        }

        public ApplicationModule(Guid id, string moduleName, string moduleCode)
        {
            Id = id;
            ModuleName = moduleName;
            ModuleCode = moduleCode;
        }
    }
}
