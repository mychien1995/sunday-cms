using Sunday.Core;
using Sunday.FeatureAccess.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Models.FeatureAccess
{
    public class ModuleListJsonResult : BaseApiResponse
    {
        public ModuleListJsonResult() : base()
        {
            Modules = new List<ModuleItem>();
        }
        public int Total { get; set; }
        public IEnumerable<ModuleItem> Modules { get; set; }
    }

    [MappedTo(typeof(ApplicationModule))]
    public class ModuleItem
    {
        public int ID { get; set; }
        public string ModuleName { get; set; }
        public string ModuleCode { get; set; }
    }
}
