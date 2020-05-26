using Newtonsoft.Json;
using Sunday.Core.Application.Organizations;
using Sunday.Core.DataAccess.Database;
using Sunday.Core.DataAccess.Models.Organizations;
using Sunday.Core.Domain.Organizations;
using Sunday.Core.Exceptions;
using Sunday.Core.Models;
using Sunday.Core.Models.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.Core.DataAccess.Repositories.Organizations
{
    [ServiceTypeOf(typeof(IOrganizationRepository))]
    public class DefaultOrganizationRepository : IOrganizationRepository
    {
        private readonly StoredProcedureRunner _dbRunner;
        public DefaultOrganizationRepository(StoredProcedureRunner dbRunner)
        {
            _dbRunner = dbRunner;
        }
        public virtual async Task<SearchResult<ApplicationOrganization>> Query(OrganizationQuery query)
        {
            var result = new SearchResult<ApplicationOrganization>();
            var param = query.ToDapperParameters();
            var searchResult = await _dbRunner.ExecuteMultipleAsync(ProcedureNames.Organizations.Search, new Type[] { typeof(int), typeof(OrganizationEntity) }, param);
            result.Total = searchResult[0].Select(x => (int)x).FirstOrDefault();
            result.Result = searchResult[1].Select(x =>
            {
                var dao = (OrganizationEntity)x;
                var model = dao.MapTo<ApplicationOrganization>();
                return model;
            });
            return result;
        }

        public virtual async Task<ApplicationOrganization> Create(ApplicationOrganization organization)
        {
            var arg = new PipelineArg();
            arg["organization"] = organization;
            await ApplicationPipelines.RunAsync("cms.organizations.translateToInsert", arg);
            var result = await _dbRunner.ExecuteAsync<int>(ProcedureNames.Organizations.Insert, arg["input"]);
            organization.ID = result.FirstOrDefault();
            return organization;
        }

        public virtual async Task<bool> Delete(int organizationId)
        {
            await _dbRunner.ExecuteAsync<int>(ProcedureNames.Organizations.Delete, new { OrganizationId = organizationId });
            return true;
        }

        public virtual ApplicationOrganization GetById(int organizationId)
        {
            var result = _dbRunner.Execute<ApplicationOrganization>(ProcedureNames.Organizations.GetById, new { OrganizationId = organizationId });
            return result.FirstOrDefault();
        }

        public virtual async Task<ApplicationOrganization> Update(ApplicationOrganization organization)
        {
            var arg = new PipelineArg();
            arg["organization"] = organization;
            await ApplicationPipelines.RunAsync("cms.organizations.translateToUpdate", arg);
            var result = await _dbRunner.ExecuteAsync<ApplicationOrganization>(ProcedureNames.Organizations.Update, arg["input"]);
            return result.FirstOrDefault();
        }

        public virtual async Task<bool> Deactivate(int organizationId)
        {
            var organization = GetById(organizationId);
            if (organization == null) throw new EntityNotFoundException("Organization not found");
            if (!organization.IsActive) throw new EntityNotFoundException("Organization already deactivated");
            await _dbRunner.ExecuteAsync<object>(ProcedureNames.Organizations.Deactivate, new { OrganizationId = organizationId });
            return true;
        }
        public virtual async Task<bool> Activate(int organizationId)
        {
            var organization = GetById(organizationId);
            if (organization == null) throw new EntityNotFoundException("Organization not found");
            if (organization.IsActive) throw new EntityNotFoundException("Organization already activated");
            await _dbRunner.ExecuteAsync<object>(ProcedureNames.Organizations.Activate, new { OrganizationId = organizationId });
            return true;
        }
    }
}
