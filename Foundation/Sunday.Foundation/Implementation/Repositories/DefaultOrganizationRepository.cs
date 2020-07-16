using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sunday.Core;
using Sunday.Core.Domain.FeatureAccess;
using Sunday.Core.Exceptions;
using Sunday.Core.Models;
using Sunday.DataAccess.SqlServer;
using Sunday.Foundation.Application.Repositories;
using Sunday.Foundation.Domain;
using Sunday.Foundation.Implementation.Repositories.Entities;
using Sunday.Foundation.Models;

namespace Sunday.Foundation.Implementation.Repositories
{
    [ServiceTypeOf(typeof(IOrganizationRepository))]
    internal class DefaultOrganizationRepository : IOrganizationRepository
    {
        private readonly StoredProcedureRunner _dbRunner;
        public DefaultOrganizationRepository(StoredProcedureRunner dbRunner)
        {
            _dbRunner = dbRunner;
        }

        public async Task<SearchResult<OrganizationEntity>> QueryAsync(OrganizationQuery query)
        {
            var result = new SearchResult<OrganizationEntity>();
            var searchResult = await _dbRunner.ExecuteMultipleAsync(ProcedureNames.Organizations.Search, new[] { typeof(int), typeof(OrganizationEntity) }, query);
            result.Total = searchResult[0].Select(x => (int)x).FirstOrDefault();
            result.Result = searchResult[1].Select(x => (OrganizationEntity)x);
            return result;
        }
         
        public async Task<OrganizationEntity> GetOrganizationByIdAsync(int organizationId)
        {
            var queryResult = await _dbRunner.ExecuteMultipleAsync(ProcedureNames.Organizations.GetById, new[] { typeof(OrganizationEntity), typeof(ApplicationModule) },
                new { OrganizationId = organizationId });
            if (!(queryResult[0].FirstOrDefault() is OrganizationEntity organization)) return null;
            organization.Modules = queryResult[1].Select(x => (ModuleEntity)x).ToList();
            return organization;
        }

        public async Task<OrganizationEntity> CreateAsync(OrganizationEntity organization)
        {
            var result = await _dbRunner.ExecuteAsync<int>(ProcedureNames.Organizations.Insert, organization);
            organization.ID = result.FirstOrDefault();
            return organization;
        }

        public async Task<OrganizationEntity> UpdateAsync(OrganizationEntity organization)
        {
            _ = await _dbRunner.ExecuteAsync<object>(ProcedureNames.Organizations.Update, organization);
            return organization;
        }

        public async Task<bool> DeleteAsync(int organizationId)
        {
            await _dbRunner.ExecuteAsync<int>(ProcedureNames.Organizations.Delete, new { OrganizationId = organizationId });
            return true;
        }

        public async Task<bool> ActivateAsync(int organizationId)
        {
            _ = await _dbRunner.ExecuteAsync<object>(ProcedureNames.Organizations.Activate, new { OrganizationId = organizationId });
            return true;
        }

        public async Task<bool> DeactivateAsync(int organizationId)
        {
            _ = await _dbRunner.ExecuteAsync<object>(ProcedureNames.Organizations.Deactivate, new { OrganizationId = organizationId });
            return true;
        }
    }
}
