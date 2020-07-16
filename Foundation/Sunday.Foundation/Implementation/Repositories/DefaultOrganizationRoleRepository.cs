using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunday.Core;
using Sunday.Core.Models;
using Sunday.DataAccess.SqlServer;
using Sunday.Foundation.Application.Repositories;
using Sunday.Foundation.Entities;
using Sunday.Foundation.Models;

namespace Sunday.Foundation.Implementation.Repositories
{
    [ServiceTypeOf(typeof(IOrganizationRoleRepository))]
    internal class DefaultOrganizationRoleRepository: IOrganizationRoleRepository
    {
        private readonly StoredProcedureRunner _dbRunner;
        public DefaultOrganizationRoleRepository(StoredProcedureRunner dbRunner)
        {
            _dbRunner = dbRunner;
        }
        public async Task<ISearchResult<OrganizationRoleEntity>> QueryAsync(OrganizationRoleQuery query)
        {
            var result = new SearchResult<OrganizationRoleEntity>();
            var queryResult = await _dbRunner.ExecuteMultipleAsync(ProcedureNames.OrganizationRoles.GetByOrganization, new [] { typeof(int), typeof(OrganizationRoleEntity) }, query);
            result.Total = queryResult[0].Select(x => (int)x).FirstOrDefault();
            result.Result = queryResult[1].Select(x => x as OrganizationRoleEntity).ToList();
            return result;
        }

        public async Task<OrganizationRoleEntity> GetRoleByIdAsync(int organizationRoleId)
        {
            var queryResult = await _dbRunner.ExecuteMultipleAsync(ProcedureNames.OrganizationRoles.GetById, new [] { typeof(OrganizationRoleEntity),
                typeof(FeatureEntity) }, new
            {
                OrganizationRoleId = organizationRoleId
            });
            var organizationRole = queryResult[0].Select(x => x as OrganizationRoleEntity).FirstOrDefault();
            if (organizationRole == null) return null;
            organizationRole.Features = queryResult[1].Select(x => x as FeatureEntity).ToList();
            return organizationRole;
        }

        public async Task<int> CreateAsync(OrganizationRoleEntity role)
        {
            var result = await _dbRunner.ExecuteAsync<int>(ProcedureNames.OrganizationRoles.Create, role);
            return result.FirstOrDefault();
        }

        public async Task<OrganizationRoleEntity> UpdateAsync(OrganizationRoleEntity role)
        {
            _ = await _dbRunner.ExecuteAsync<object>(ProcedureNames.OrganizationRoles.Update, role);
            return role;
        }

        public async Task<bool> DeleteAsync(int roleId)
        {
            _ = await _dbRunner.ExecuteAsync<object>(ProcedureNames.OrganizationRoles.Delete, new
            {
                RoleId = roleId
            });
            return true;
        }

        public async Task<bool> BulkUpdateAsync(IEnumerable<OrganizationRoleEntity> roles)
        {
            var dbRoleType = new DataTable("OrganizationRoleType");
            dbRoleType.Columns.Add("OrganizationRoleId", typeof(int));
            dbRoleType.Columns.Add("Features", typeof(string));
            foreach (var role in roles)
            {
                var row = dbRoleType.NewRow();
                row["OrganizationRoleId"] = role.ID;
                row["Features"] = string.Join(',', role.Features.Select(x => x.ID));
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
            return true;
        }
    }
}
