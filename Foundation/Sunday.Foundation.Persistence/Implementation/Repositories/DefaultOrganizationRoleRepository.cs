using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.Core;
using Sunday.Core.Models.Base;
using Sunday.DataAccess.SqlServer.Attributes;
using Sunday.DataAccess.SqlServer.Database;
using Sunday.DataAccess.SqlServer.Extensions;
using Sunday.Foundation.Implementation;
using Sunday.Foundation.Models;
using Sunday.Foundation.Persistence.Application.Repositories;
using Sunday.Foundation.Persistence.Entities;

namespace Sunday.Foundation.Persistence.Implementation.Repositories
{
    [ServiceTypeOf(typeof(IOrganizationRoleRepository))]
    internal class DefaultOrganizationRoleRepository : IOrganizationRoleRepository
    {
        private readonly StoredProcedureRunner _dbRunner;
        public DefaultOrganizationRoleRepository(StoredProcedureRunner dbRunner)
        {
            _dbRunner = dbRunner;
        }
        public async Task<SearchResult<OrganizationRoleEntity>> QueryAsync(OrganizationRoleQuery query)
        {
            var result = new SearchResult<OrganizationRoleEntity>();
            var queryResult = await _dbRunner.ExecuteMultipleAsync<int, OrganizationRoleEntity, OrganizationRoleMappingEntity>(ProcedureNames.OrganizationRoles.Search, query);
            result.Total = queryResult.Item1.Single();
            var roles = queryResult.Item2.ToArray();
            var features = queryResult.Item3.ToList();
            roles.Iter(role =>
            {
                role.Features = features.Where(f => f.OrganizationRoleId == role.Id)
                    .Select(f => new FeatureEntity { Id = f.FeatureId }).ToList();
            });
            result.Result = roles;
            return result;
        }

        public async Task<Option<OrganizationRoleEntity>> GetRoleByIdAsync(Guid organizationRoleId)
        {
            var queryResult = await _dbRunner.ExecuteMultipleAsync<OrganizationRoleEntity, FeatureEntity>
            (ProcedureNames.OrganizationRoles.GetById, new
            {
                OrganizationRoleId = organizationRoleId
            });
            var organizationRole = queryResult.Item1.Single();
            if (organizationRole == null) return Option<OrganizationRoleEntity>.None;
            organizationRole.Features = queryResult.Item2.ToList();
            return organizationRole;
        }

        public async Task<Guid> CreateAsync(OrganizationRoleEntity role)
        {
            if (role.Id == Guid.Empty) role.Id = Guid.NewGuid();
            await _dbRunner.ExecuteAsync(ProcedureNames.OrganizationRoles.Create, role.ToDapperParameters());
            return role.Id;
        }

        public Task UpdateAsync(OrganizationRoleEntity role)
        => _dbRunner.ExecuteAsync(ProcedureNames.OrganizationRoles.Update, role.ToDapperParameters(DbOperation.Update));

        public Task DeleteAsync(Guid roleId)
        => _dbRunner.ExecuteAsync(ProcedureNames.OrganizationRoles.Delete, new { RoleId = roleId });

        public async Task BulkUpdateAsync(IEnumerable<OrganizationRoleEntity> roles)
        {
            var dbRoleType = new DataTable("OrganizationRoleType");
            dbRoleType.Columns.Add("OrganizationRoleId", typeof(Guid));
            dbRoleType.Columns.Add("Features", typeof(string));
            foreach (var role in roles)
            {
                var row = dbRoleType.NewRow();
                row["OrganizationRoleId"] = role.Id;
                row["Features"] = role.Features.Select(f => f.Id).ToDatabaseList();
                dbRoleType.Rows.Add(row);
            }
            var param = new SqlParameter
            {
                ParameterName = "@Roles",
                SqlDbType = SqlDbType.Structured,
                Value = dbRoleType,
                TypeName = "dbo.OrganizationRoleType",
                Direction = ParameterDirection.Input
            };
            await Task.Run(() => _dbRunner.Execute(ProcedureNames.OrganizationRoles.BulkUpdate, param));
        }
    }
}
