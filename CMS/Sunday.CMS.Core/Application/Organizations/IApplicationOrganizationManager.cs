using Sunday.CMS.Core.Models;
using Sunday.CMS.Core.Models.FeatureAccess;
using Sunday.CMS.Core.Models.Organizations;
using Sunday.Core.Domain.Organizations;
using Sunday.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Application.Organizations
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

        Task<ModuleListJsonResult> GetOrganizationModules(int organizationId);
    }
}
