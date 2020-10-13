using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.Foundation.Application.Services;
using Sunday.Foundation.Domain;
using Sunday.Foundation.Persistence.Application.Repositories;

namespace Sunday.Foundation.Implementation.Services
{
    [ServiceTypeOf(typeof(IFeatureService))]
    public class DefaultFeatureService : IFeatureService
    {
        private readonly IFeatureRepository _featureRepository;

        public DefaultFeatureService(IFeatureRepository featureRepository)
        {
            this._featureRepository = featureRepository;
        }

        public async Task<List<ApplicationFeature>> GetFeaturesByModules(List<Guid> moduleIds)
            => (await _featureRepository.GetFeaturesByModules(moduleIds)).CastListTo<ApplicationFeature>().ToList();
    }
}
