using System;
using System.Collections.Generic;
using Sunday.Core;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Domain;

namespace Sunday.CMS.Core.Models.FeatureAccess
{
    public class ModuleListJsonResult : BaseApiResponse
    {
        public int Total { get; set; }
        public List<ModuleItem> Modules { get; set; } = new List<ModuleItem>();
    }

    [MappedTo(typeof(ApplicationModule))]
    public class ModuleItem
    {
        public Guid Id { get; }
        public string ModuleName { get; }
        public string ModuleCode { get; }

        public List<FeatureItem> Features { get; set; } = new List<FeatureItem>();

        public ModuleItem(Guid id, string moduleName, string moduleCode)
        {
            Id = id;
            ModuleName = moduleName;
            ModuleCode = moduleCode;
        }
    }
}
