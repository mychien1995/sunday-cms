using System.Threading.Tasks;
using Sunday.CMS.Core.Models.VirtualRoles;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Models;

namespace Sunday.CMS.Core.Application
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
