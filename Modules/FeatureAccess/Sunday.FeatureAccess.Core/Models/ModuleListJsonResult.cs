using Sunday.Core;
using System.Collections.Generic;

namespace Sunday.FeatureAccess.Core.Models
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
        public ModuleItem()
        {
            Features = new List<FeatureItem>();
        }
        public int ID { get; set; }
        public string ModuleName { get; set; }
        public string ModuleCode { get; set; }

        public List<FeatureItem> Features { get; set; }
    }
}
