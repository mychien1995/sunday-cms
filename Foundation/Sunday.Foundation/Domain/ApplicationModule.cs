using System;

namespace Sunday.Foundation.Domain
{
    public class ApplicationModule
    {
        public Guid Id { get; set; }
        public string ModuleName { get; set; }
        public string ModuleCode { get; set; }

        public ApplicationModule(Guid id, string moduleName, string moduleCode)
        {
            Id = id;
            ModuleName = moduleName;
            ModuleCode = moduleCode;
        }
    }
}
