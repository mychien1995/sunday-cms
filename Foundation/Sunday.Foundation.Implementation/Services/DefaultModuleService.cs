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
    [ServiceTypeOf(typeof(IModuleService))]
    public class DefaultModuleService : IModuleService
    {
        private readonly IModuleRepository _moduleRepository;

        public DefaultModuleService(IModuleRepository moduleRepository)
        {
            _moduleRepository = moduleRepository;
        }

        public Task<List<ApplicationModule>> GetAllAsync()
            => _moduleRepository.GetAllAsync().MapResultTo(list => list.CastListTo<ApplicationModule>().ToList());
    }
}
