using System;

namespace Sunday.Foundation.Persistence.Entities
{
    public class FeatureEntity
    {
        public Guid Id { get; set; }
        public string FeatureName { get; set; }
        public string FeatureCode { get; set; }
        public int ModuleId { get; set; }

        public FeatureEntity(Guid id, string featureName, string featureCode, int moduleId)
        {
            Id = id;
            FeatureName = featureName;
            FeatureCode = featureCode;
            ModuleId = moduleId;
        }
    }
}
