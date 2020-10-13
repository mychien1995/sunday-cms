using Sunday.Core;
using Sunday.Organizations.Core.Models;
using System.Threading.Tasks;
using Sunday.Core.Models.Base;

namespace Sunday.Organizations.Application
{
    public interface IApplicationOrganizationManager
    {
        Task<OrganizationListJsonResult> SearchOrganizations(SearchOrganizationCriteria criteria);

        Task<CreateOrganizationJsonResult> CreateOrganization(OrganizationMutationModel data);

        Task<BaseApiResponse> UpdateOrganization(OrganizationMutationModel data);

        Task<OrganizationDetailJsonResult> GetOrganizationById(int orgId);

        Task<BaseApiResponse> DeleleOrganization(int orgId);

        Task<BaseApiResponse> ActivateOrganization(int orgId);

        Task<BaseApiResponse> DeactivateOrganization(int orgId);

        Task<ListApiResponse<OrganizationItem>> GetOrganizationLookup();
    }
}
