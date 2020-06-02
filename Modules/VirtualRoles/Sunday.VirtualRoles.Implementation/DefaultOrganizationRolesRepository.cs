using Sunday.Core;
using Sunday.Core.Domain.FeatureAccess;
using Sunday.Core.Models;
using Sunday.DataAccess.SqlServer;
using Sunday.FeatureAccess.Core;
using Sunday.VirtualRoles.Application;
using Sunday.VirtualRoles.Core;
using Sunday.VirtualRoles.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sunday.VirtualRoles.Implementation
{
    [ServiceTypeOf(typeof(IOrganizationRoleRepository))]
    public class DefaultOrganizationRolesRepository : IOrganizationRoleRepository
    {
        private readonly StoredProcedureRunner _dbRunner;
        public DefaultOrganizationRolesRepository(StoredProcedureRunner dbRunner)
        {
            _dbRunner = dbRunner;
        }
        public async Task<ISearchResult<OrganizationRole>> GetRoles(OrganizationRoleQuery query)
        {
            var result = new SearchResult<OrganizationRole>();
            var queryResult = await _dbRunner.ExecuteMultipleAsync(ProcedureNames.OrganizationRoles.GetByOrganization, new Type[] { typeof(OrganizationRole), typeof(int) }, new
            {
                OrganizationId = query.OrganizationId,
                PageIndex = query.PageIndex,
                PageSize = query.PageSize
            });
            result.Total = queryResult[0].Select(x => x as int?).FirstOrDefault().Value;
            result.Result = queryResult[1].Select(x => x as OrganizationRole).ToList();
            return result;
        }

        public async Task<OrganizationRole> GetById(int organizationRoleId)
        {
            var queryResult = await _dbRunner.ExecuteMultipleAsync(ProcedureNames.OrganizationRoles.GetById, new Type[] { typeof(OrganizationRole), typeof(ApplicationFeature) }, new
            {
                OrganizationRoleId = organizationRoleId
            });
            var organizationRole = queryResult[0].Select(x => x as OrganizationRole).FirstOrDefault();
            if (organizationRole != null)
            {
                organizationRole.Features = queryResult[1].Select(x => x as IApplicationFeature).ToList();
            }
            return organizationRole;
        }

        public async Task<int> Create(OrganizationRole role)
        {
            var result = await _dbRunner.ExecuteAsync<int>(ProcedureNames.OrganizationRoles.Create, new
            {
                role.RoleCode,
                role.RoleName,
                role.OrganizationId,
                role.Description,
                role.CreatedDate,
                role.CreatedBy,
                role.UpdatedDate,
                role.UpdatedBy,
                FeatureIds = string.Join(",", role.Features.Select(x => x.ID))
            });
            return result.FirstOrDefault();
        }

        public async Task<OrganizationRole> Update(OrganizationRole role)
        {
            var result = await _dbRunner.ExecuteAsync<OrganizationRole>(ProcedureNames.OrganizationRoles.Update, new
            {
                role.RoleName,
                role.Description,
                RoleId = role.ID,
                role.UpdatedDate,
                role.UpdatedBy,
                FeatureIds = string.Join(",", role.Features.Select(x => x.ID))
            });
            return result.FirstOrDefault();
        }

        public async Task<bool> Delete(int roleId)
        {
            _ = await _dbRunner.ExecuteAsync<OrganizationRole>(ProcedureNames.OrganizationRoles.Delete, new
            {
                RoleId = roleId
            });
            return true;
        }
    }
}
