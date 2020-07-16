using System.Collections.Generic;
using System.Threading.Tasks;
using Sunday.Foundation.Domain;
using Sunday.Foundation.Entities;

namespace Sunday.Foundation.Application.Repositories
{
    public interface IFeatureRepository
    {
        Task<List<FeatureEntity>> GetFeaturesByModules(List<int> moduleIds);
    }
}
