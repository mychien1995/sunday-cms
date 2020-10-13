using Sunday.Core;
using Sunday.VirtualRoles.Core.Models;
using System.Threading.Tasks;
using Sunday.Core.Models.Base;

namespace Sunday.VirtualRoles.Application
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
