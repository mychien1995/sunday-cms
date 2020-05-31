using Sunday.FeatureAccess.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.FeatureAccess.Application
{
    public interface IModuleRepository
    {
        Task<List<ApplicationModule>> GetAllModules();
    }
}
