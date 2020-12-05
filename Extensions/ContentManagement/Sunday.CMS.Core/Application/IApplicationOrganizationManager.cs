using System;
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

        Task<OrganizationDetailJsonResult> GetOrganizationById(Guid orgId);

        Task<BaseApiResponse> DeleteOrganization(Guid orgId);

        Task<BaseApiResponse> ActivateOrganization(Guid orgId);

        Task<BaseApiResponse> DeactivateOrganization(Guid orgId);

        Task<ListApiResponse<OrganizationItem>> GetOrganizationLookup();
    }
}
