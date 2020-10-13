using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sunday.Foundation.Persistence.Entities;

namespace Sunday.Foundation.Persistence.Application.Repositories
{
    public interface IFeatureRepository
    {
        Task<List<FeatureEntity>> GetFeaturesByModules(List<Guid> moduleIds);
    }
}
