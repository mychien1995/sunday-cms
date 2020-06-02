using Sunday.Core;
using Sunday.FeatureAccess.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Models.FeatureAccess
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

    [MappedTo(typeof(ApplicationModule))]
    public class FeatureItem
    {
        public int ID { get; set; }
        public string FeatureCode { get; set; }
        public string FeatureName { get; set; }
    }
}
