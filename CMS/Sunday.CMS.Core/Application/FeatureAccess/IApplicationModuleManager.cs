using Sunday.CMS.Core.Models.FeatureAccess;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Application.FeatureAccess
{
    public interface IApplicationModuleManager
    {
        Task<ModuleListJsonResult> GetModules();
    }
}
