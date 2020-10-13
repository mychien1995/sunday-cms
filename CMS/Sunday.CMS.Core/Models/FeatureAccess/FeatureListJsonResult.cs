using System;
using System.Collections.Generic;
using Sunday.Core;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Domain;

namespace Sunday.CMS.Core.Models.FeatureAccess
{
    public class FeatureListJsonResult : BaseApiResponse
    {
        public int Total { get; set; }
        public List<FeatureItem> Features { get; set; } = new List<FeatureItem>();
    }

    [MappedTo(typeof(ApplicationFeature))]
    public class FeatureItem
    {
        public Guid Id { get; }
        public string FeatureCode { get; }
        public string FeatureName { get; }

        public FeatureItem(Guid id, string featureCode, string featureName)
        {
            Id = id;
            FeatureCode = featureCode;
            FeatureName = featureName;
        }
    }
}
