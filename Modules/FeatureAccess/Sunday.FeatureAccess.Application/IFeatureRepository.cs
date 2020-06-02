using Sunday.FeatureAccess.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.FeatureAccess.Application
{
    public interface IFeatureRepository
    {
        Task<List<ApplicationFeature>> GetFeaturesByModules(List<int> moduleIds);
    }
}
