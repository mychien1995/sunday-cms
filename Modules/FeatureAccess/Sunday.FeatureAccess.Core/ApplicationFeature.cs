using Sunday.Core.Domain.FeatureAccess;

namespace Sunday.FeatureAccess.Core
{
    public class ApplicationFeature : IApplicationFeature
    {
        public int ID { get; set; }
        public string FeatureName { get; set; }
        public string FeatureCode { get; set; }
        public int ModuleId { get; set; }
        public IApplicationModule Module { get; set; }
    }
}
