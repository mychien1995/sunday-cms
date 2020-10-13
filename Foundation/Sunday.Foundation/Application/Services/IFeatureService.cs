using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sunday.Foundation.Domain;

namespace Sunday.Foundation.Application.Services
{
    public interface IFeatureService
    {
        Task<List<ApplicationFeature>> GetFeaturesByModules(List<Guid> moduleIds);
    }
}
