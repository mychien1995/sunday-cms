using System;
using System.Linq;
using System.Threading.Tasks;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Models.Organizations;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.Core.Media.Application;
using Sunday.Core.Models.Base;
using Sunday.Core.Pipelines;
using Sunday.Core.Pipelines.Arguments;
using Sunday.Foundation.Application.Services;
using Sunday.Foundation.Domain;
using Sunday.Foundation.Models;

namespace Sunday.CMS.Core.Implementation
{
    [ServiceTypeOf(typeof(IApplicationOrganizationManager))]
    public class DefaultApplicationOrganizationManager : IApplicationOrganizationManager
    {
        private readonly IBlobLinkManager _blobLinkManager;
        private readonly IOrganizationService _organizationService;

        public DefaultApplicationOrganizationManager(IOrganizationService organizationService, IBlobLinkManager blobLinkManager)
        {
            _organizationService = organizationService;
            _blobLinkManager = blobLinkManager;
        }

        public Task<OrganizationListJsonResult> SearchOrganizations(OrganizationQuery criteria)
            => _organizationService.QueryAsync(criteria).MapResultTo(list => new OrganizationListJsonResult()
            {
                Organizations = list.Result.Select(ToJsonResult).ToList(),
                Total = list.Total
            });

        public async Task<CreateOrganizationJsonResult> CreateOrganization(OrganizationMutationModel data)
        {
            var organization = ToOrganization(data);
            await ApplicationPipelines.RunAsync("cms.entity.beforeCreate", new BeforeCreateEntityArg(organization));
            return new CreateOrganizationJsonResult(await _organizationService.CreateAsync(organization));
        }

        public async Task<BaseApiResponse> UpdateOrganization(OrganizationMutationModel data)
        {
            var organization = ToOrganization(data);
            await ApplicationPipelines.RunAsync("cms.entity.beforeUpdate", new BeforeUpdateEntityArg(organization));
            await _organizationService.UpdateAsync(organization);
            return BaseApiResponse.SuccessResult;
        }

        public async Task<OrganizationDetailJsonResult> GetOrganizationById(Guid orgId)
        {
            var organization = await _organizationService.GetOrganizationByIdAsync(orgId);
            return organization.Some(ToDetailJsonResult)
                .None(() => BaseApiResponse.ErrorResult<OrganizationDetailJsonResult>("Organization not found"));
        }

        public async Task<BaseApiResponse> DeleteOrganization(Guid orgId)
        {
            await _organizationService.DeleteAsync(orgId);
            return BaseApiResponse.SuccessResult;
        }

        public async Task<BaseApiResponse> ActivateOrganization(Guid orgId)
        {
            await _organizationService.ActivateAsync(orgId);
            return BaseApiResponse.SuccessResult;
        }

        public async Task<BaseApiResponse> DeactivateOrganization(Guid orgId)
        {
            await _organizationService.DeactivateAsync(orgId);
            return BaseApiResponse.SuccessResult;
        }

        public async Task<ListApiResponse<OrganizationItem>> GetOrganizationLookup()
        {
            var organizations = await _organizationService.QueryAsync(new OrganizationQuery());
            return new ListApiResponse<OrganizationItem>()
            {
                Total = organizations.Total,
                List = organizations.Result.Select(o => new OrganizationItem
                {
                    OrganizationName = o.OrganizationName,
                    Id = o.Id
                }).ToList()
            };
        }
        private OrganizationDetailJsonResult ToDetailJsonResult(ApplicationOrganization organization)
        {
            var model = organization.MapTo<OrganizationDetailJsonResult>();
            model.LogoLink = _blobLinkManager.GetPreviewLink(organization.LogoBlobUri ?? string.Empty);
            model.ColorScheme = organization.Properties.Get("color").Map(c => c.ToString()!).IfNone(string.Empty);
            model.ModuleIds = organization.Modules.Select(m => m.Id).ToList();
            return model;
        }
        private OrganizationItem ToJsonResult(ApplicationOrganization organization)
        {
            var model = organization.MapTo<OrganizationItem>();
            model.LogoLink = _blobLinkManager.GetPreviewLink(organization.LogoBlobUri ?? string.Empty);
            model.ColorScheme = organization.Properties.Get("color").Map(c => c.ToString()!).IfNone(string.Empty);
            return model;
        }
        private ApplicationOrganization ToOrganization(OrganizationMutationModel mutationData)
        {
            var result = mutationData.MapTo<ApplicationOrganization>();
            result.Properties.Add("color", mutationData.ColorScheme);
            result.Modules = mutationData.ModuleIds.Select(m => new ApplicationModule(m, string.Empty, string.Empty))
                .ToList();
            return result;
        }
    }
}
