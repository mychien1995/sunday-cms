using System;
using System.Threading.Tasks;
using Sunday.CMS.Core.Models.Roles;

namespace Sunday.CMS.Core.Application
{
    public interface IApplicationRoleManager
    {
        Task<RoleListJsonResult> GetAvailableRoles();
        Task<RoleDetailJsonResult> GetRoleById(int id);
    }
}
