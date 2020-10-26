using System;

namespace Sunday.Foundation.Domain
{
    public class ApplicationFeature
    {
        public Guid Id { get; set; }
        public string FeatureName { get; set; } = string.Empty;
        public string FeatureCode { get; set; } = string.Empty;
        public Guid ModuleId { get; set; }
        public ApplicationModule? Module { get; set; }

        public ApplicationFeature()
        {
            
        }

        public ApplicationFeature(Guid id, string featureName, string featureCode, Guid moduleId, ApplicationModule module)
        {
            Id = id;
            FeatureName = featureName;
            FeatureCode = featureCode;
            ModuleId = moduleId;
            Module = module;
        }
    }
}
