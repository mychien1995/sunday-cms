using System;
using Sunday.Core;
using Sunday.Foundation.Domain;

namespace Sunday.Foundation.Persistence.Entities
{
    [MappedTo(typeof(ApplicationFeature))]
    public class FeatureEntity
    {
        public Guid Id { get; set; }
        public string FeatureName { get; set; } = string.Empty;
        public string FeatureCode { get; set; } = string.Empty;
        public Guid ModuleId { get; set; }
    }
}
