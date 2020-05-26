using Sunday.CMS.Core.Application.Organizations;
using Sunday.CMS.Core.Models;
using Sunday.CMS.Core.Models.Organizations;
using Sunday.Core;
using Sunday.Core.Application.Organizations;
using Sunday.Core.Domain.Organizations;
using Sunday.Core.Media.Application;
using Sunday.Core.Models.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Implementation.Organizations
{
    [ServiceTypeOf(typeof(IApplicationOrganizationManager))]
    public class DefaultApplicationOrganizationManager : IApplicationOrganizationManager
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IBlobLinkManager _blobLinkManager;
        public DefaultApplicationOrganizationManager(IOrganizationRepository organizationRepository, IBlobLinkManager blobLinkManager)
        {
            _blobLinkManager = blobLinkManager;
            _organizationRepository = organizationRepository;
        }

        public virtual async Task<OrganizationListJsonResult> SearchOrganizations(SearchOrganizationCriteria criteria)
        {
            var query = criteria.MapTo<OrganizationQuery>();
            var searchResult = await _organizationRepository.Query(query);
            var apiResult = new OrganizationListJsonResult();
            apiResult.Total = searchResult.Total;
            apiResult.Organizations = await Task.WhenAll(searchResult.Result.Select(async x =>
            {
                var user = x.MapTo<OrganizationItem>();
                var pipelineArg = new PipelineArg();
                pipelineArg["source"] = x;
                pipelineArg["target"] = user;
                await ApplicationPipelines.RunAsync("cms.organizations.translateListItem", pipelineArg);
                user.LogoLink = _blobLinkManager.GetPreviewLink(x.LogoBlobUri);
                return user;
            }));
            return apiResult;
        }
        public virtual async Task<OrganizationDetailJsonResult> GetOrganizationById(int orgId)
        {
            var organization = _organizationRepository.GetById(orgId);
            var result = organization.MapTo<OrganizationDetailJsonResult>();
            result.Success = true;
            return await Task.FromResult(result);
        }
        public virtual async Task<BaseApiResponse> UpdateOrganization(OrganizationMutationModel data)
        {
            var result = new BaseApiResponse();
            var organization = data.MapTo<ApplicationOrganization>();
            var pipelineArg = new PipelineArg();
            pipelineArg["mutationData"] = data;
            pipelineArg["organization"] = organization;
            pipelineArg["EntityChanged"] = organization;
            await ApplicationPipelines.RunAsync("cms.organizations.beforeUpdate", pipelineArg);
            if (pipelineArg.Aborted)
            {
                result.AddErrors(pipelineArg.Messages);
                return result;
            }
            var createResult = await _organizationRepository.Update(organization);
            return result;
        }
        public virtual async Task<CreateOrganizationJsonResult> CreateOrganization(OrganizationMutationModel data)
        {
            var result = new CreateOrganizationJsonResult();
            var organization = data.MapTo<ApplicationOrganization>();
            var pipelineArg = new PipelineArg();
            pipelineArg["mutationData"] = data;
            pipelineArg["organization"] = organization;
            pipelineArg["EntityChanged"] = organization;
            await ApplicationPipelines.RunAsync("cms.organizations.beforeCreate", pipelineArg);
            if (pipelineArg.Aborted)
            {
                result.AddErrors(pipelineArg.Messages);
                return result;
            }
            var createResult = await _organizationRepository.Create(organization);
            result = new CreateOrganizationJsonResult(createResult.ID);
            return result;
        }

        public virtual async Task<BaseApiResponse> DeactivateOrganization(int orgId)
        {
            var result = new BaseApiResponse();
            var actionResult = await _organizationRepository.Deactivate(orgId);
            result.Success = actionResult;
            return result;
        }
        public virtual async Task<BaseApiResponse> ActivateOrganization(int orgId)
        {
            var result = new BaseApiResponse();
            var actionResult = await _organizationRepository.Activate(orgId);
            result.Success = actionResult;
            return result;
        }

        public virtual async Task<BaseApiResponse> DeleleOrganization(int orgId)
        {
            var result = await _organizationRepository.Delete(orgId);
            var response = new BaseApiResponse();
            response.Success = result;
            return response;
        }
    }
}
