using Sunday.Core.Domain.FeatureAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.FeatureAccess.Core
{
    public class ApplicationModule : IApplicationModule
    {
        public int ID { get; set; }
        public string ModuleName { get; set; }
        public string ModuleCode { get; set; }
    }
}
