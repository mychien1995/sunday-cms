using Sunday.Core;
using System.Collections.Generic;
using Sunday.Core.Models.Base;

namespace Sunday.FeatureAccess.Core.Models
{
    public class FeatureListJsonResult : BaseApiResponse
    {
        public FeatureListJsonResult() : base()
        {
            Features = new List<FeatureItem>();
        }
        public int Total { get; set; }
        public IEnumerable<FeatureItem> Features { get; set; }
    }

    [MappedTo(typeof(ApplicationFeature))]
    public class FeatureItem
    {
        public int ID { get; set; }
        public string FeatureCode { get; set; }
        public string FeatureName { get; set; }
    }
}
