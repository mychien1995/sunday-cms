using Sunday.CMS.Core.Models;
using Sunday.CMS.Core.Models.VirtualRoles;
using Sunday.VirtualRoles.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Application.VirtualRoles
{
    public interface IOrganizationRolesManager
    {
        Task<OrganizationRolesListJsonResult> GetRolesList(OrganizationRoleQuery query);
        Task<OrganizationRoleDetailJsonResult> GetRoleById(int roleId);

        Task<BaseApiResponse> CreateRole(OrganizationRoleMutationModel mutationData);

        Task<BaseApiResponse> UpdateRole(OrganizationRoleMutationModel mutationData);

        Task<BaseApiResponse> DeleteRole(int roleId);

        Task<BaseApiResponse> BulkUpdate(OrganizationRoleBulkUpdateModel model);
    }
}
