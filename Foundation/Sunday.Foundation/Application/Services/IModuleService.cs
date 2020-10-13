using System.Collections.Generic;
using System.Threading.Tasks;
using Sunday.Foundation.Domain;

namespace Sunday.Foundation.Application.Services
{
    public interface IModuleService
    {
        Task<List<ApplicationModule>> GetAllAsync();
    }
}
