using System.Threading.Tasks;
using Sunday.CMS.Core.Models.Organizations;
using Sunday.Core;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Models;

namespace Sunday.CMS.Core.Application
{
    public interface IApplicationOrganizationManager
    {
        Task<OrganizationListJsonResult> SearchOrganizations(OrganizationQuery criteria);

        Task<CreateOrganizationJsonResult> CreateOrganization(OrganizationMutationModel data);

        Task<BaseApiResponse> UpdateOrganization(OrganizationMutationModel data);

        Task<OrganizationDetailJsonResult> GetOrganizationById(int orgId);

        Task<BaseApiResponse> DeleteOrganization(int orgId);

        Task<BaseApiResponse> ActivateOrganization(int orgId);

        Task<BaseApiResponse> DeactivateOrganization(int orgId);

        Task<ListApiResponse<OrganizationItem>> GetOrganizationLookup();
    }
}
