using System;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.Core;
using Sunday.Core.Models.Base;
using Sunday.DataAccess.SqlServer.Attributes;
using Sunday.DataAccess.SqlServer.Database;
using Sunday.Foundation.Implementation;
using Sunday.Foundation.Models;
using Sunday.Foundation.Persistence.Application.Repositories;
using Sunday.Foundation.Persistence.Entities;
using Sunday.Foundation.Persistence.Extensions;

namespace Sunday.Foundation.Persistence.Implementation.Repositories
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
            var searchResult = await _dbRunner.ExecuteMultipleAsync<int, OrganizationEntity>(ProcedureNames.Organizations.Search,
                GetOrganizationQuery(query));
            result.Total = searchResult.Item1.Single();
            result.Result = searchResult.Item2.ToList();
            return result;
        }

        public async Task<Option<OrganizationEntity>> GetOrganizationByIdAsync(Guid organizationId)
        {
            var queryResult = await _dbRunner.ExecuteMultipleAsync<OrganizationEntity, ModuleEntity>
                (ProcedureNames.Organizations.GetById, new { OrganizationId = organizationId });
            if (!queryResult.Item1.Any()) return Option<OrganizationEntity>.None;
            var organization = queryResult.Item1.Single();
            organization.Modules = queryResult.Item2.ToList();
            return organization;
        }

        public async Task<Guid> CreateAsync(OrganizationEntity organization)
        {
            if (organization.Id == Guid.Empty) organization.Id = Guid.NewGuid();
            await _dbRunner.ExecuteAsync(ProcedureNames.Organizations.Insert, organization);
            return organization.Id;
        }

        public Task UpdateAsync(OrganizationEntity organization)
        => _dbRunner.ExecuteAsync(ProcedureNames.Organizations.Update, organization);

        public Task DeleteAsync(Guid organizationId)
        => _dbRunner.ExecuteAsync(ProcedureNames.Organizations.Delete, new { OrganizationId = organizationId });

        public Task ActivateAsync(Guid organizationId)
        => _dbRunner.ExecuteAsync(ProcedureNames.Organizations.Activate, new { OrganizationId = organizationId });

        public Task DeactivateAsync(Guid organizationId)
        => _dbRunner.ExecuteAsync(ProcedureNames.Organizations.Deactivate, new { OrganizationId = organizationId });

        private static object GetOrganizationQuery(OrganizationQuery query)
            => new
            {
                query.SortBy,
                query.IsActive,
                query.Text,
                query.PageSize,
                query.PageIndex,
                query.HostName,
            };
    }
}
