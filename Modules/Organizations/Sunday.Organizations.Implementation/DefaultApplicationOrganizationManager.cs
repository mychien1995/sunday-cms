using Sunday.Core;
using Sunday.Organizations.Application;
using Sunday.Organizations.Core;
using Sunday.Organizations.Core.Models;
using System.Linq;
using System.Threading.Tasks;
using Sunday.Core.Extensions;
using Sunday.Core.Models.Base;
using Sunday.Core.Pipelines;

namespace Sunday.Organizations.Implementation
{
    [ServiceTypeOf(typeof(IApplicationOrganizationManager))]
    public class DefaultApplicationOrganizationManager : IApplicationOrganizationManager
    {
        private readonly IOrganizationRepository _organizationRepository;

        public DefaultApplicationOrganizationManager(IOrganizationRepository organizationRepository)
        {
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
                var orgItem = x.MapTo<OrganizationItem>();
                var pipelineArg = new PipelineArg();
                pipelineArg["Source"] = x;
                pipelineArg["Target"] = orgItem;
                await ApplicationPipelines.RunAsync("cms.organizations.translateToModel", pipelineArg);
                return orgItem;
            }));
            return apiResult;
        }
        public virtual async Task<OrganizationDetailJsonResult> GetOrganizationById(int orgId)
        {
            var organization = _organizationRepository.GetById(orgId);
            var result = organization.MapTo<OrganizationDetailJsonResult>();
            var pipelineArg = new PipelineArg();
            pipelineArg["Source"] = organization;
            pipelineArg["Target"] = result;
            await ApplicationPipelines.RunAsync("cms.organizations.translateToModel", pipelineArg);
            result.Success = true;
            return await Task.FromResult(result);
        }
        public virtual async Task<BaseApiResponse> UpdateOrganization(OrganizationMutationModel data)
        {
            var result = new BaseApiResponse();
            var organization = data.MapTo<ApplicationOrganization>();
            var pipelineArg = new PipelineArg();
            pipelineArg["Source"] = data;
            pipelineArg["Target"] = organization;
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
            pipelineArg["Source"] = data;
            pipelineArg["Target"] = organization;
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

        public virtual async Task<ListApiResponse<OrganizationItem>> GetOrganizationLookup()
        {
            var query = new OrganizationQuery()
            {
                PageIndex = 0,
                PageSize = int.MaxValue,
                IsActive = true
            };
            var searchResult = (await _organizationRepository.Query(query)).Result;
            var result = new ListApiResponse<OrganizationItem>();
            result.List = searchResult.Select(x => new OrganizationItem()
            {
                ID = x.ID,
                OrganizationName = x.OrganizationName
            }).ToList();
            return result;
        }
    }
}
