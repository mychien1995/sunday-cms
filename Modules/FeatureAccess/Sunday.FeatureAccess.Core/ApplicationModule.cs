using Sunday.Core.Domain.FeatureAccess;

namespace Sunday.FeatureAccess.Core
{
    public class ApplicationModule : IApplicationModule
    {
        public int ID { get; set; }
        public string ModuleName { get; set; }
        public string ModuleCode { get; set; }
    }
}
