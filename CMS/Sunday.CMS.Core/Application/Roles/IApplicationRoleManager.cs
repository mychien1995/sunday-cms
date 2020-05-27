using Sunday.CMS.Core.Models.Roles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Application.Roles
{
    public interface IApplicationRoleManager
    {
        Task<RoleListJsonResult> GetAvailableRoles();
        Task<RoleDetailJsonResult> GetRoleById(int id);
    }
}
