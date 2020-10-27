using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Models.VirtualRoles;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Application.Services;
using Sunday.Foundation.Domain;
using Sunday.Foundation.Models;

namespace Sunday.CMS.Core.Implementation
{
    [ServiceTypeOf(typeof(IOrganizationRolesManager))]
    public class DefaultOrganizationRoleManager : IOrganizationRolesManager
    {
        private readonly IOrganizationRoleService _organizationRoleService;

        public DefaultOrganizationRoleManager(IOrganizationRoleService organizationRoleService)
        {
            _organizationRoleService = organizationRoleService;
        }

        public Task<OrganizationRolesListJsonResult> GetRolesList(OrganizationRoleQuery query)
            => _organizationRoleService.QueryAsync(query).MapResultTo(list => new OrganizationRolesListJsonResult
            {
                Roles = list.Result.Select(ToJsonItem).ToList(),
                Total = list.Total
            });
        public async Task<OrganizationRoleDetailJsonResult> GetRoleById(Guid roleId)
        {
            var role = await _organizationRoleService.GetRoleByIdAsync(roleId);
            return role.Some(ToJsonResult)
                .None(() => BaseApiResponse.ErrorResult<OrganizationRoleDetailJsonResult>("Role not found"));
        }

        public async Task<BaseApiResponse> CreateRole(OrganizationRoleMutationModel mutationData)
        {
            var role = ToOrganizationRole(mutationData);
            await _organizationRoleService.CreateAsync(role);
            return BaseApiResponse.SuccessResult;
        }

        public async Task<BaseApiResponse> UpdateRole(OrganizationRoleMutationModel mutationData)
        {
            var role = ToOrganizationRole(mutationData);
            await _organizationRoleService.UpdateAsync(role);
            return BaseApiResponse.SuccessResult;
        }

        public async Task<BaseApiResponse> DeleteRole(Guid roleId)
        {
            await _organizationRoleService.DeleteAsync(roleId);
            return BaseApiResponse.SuccessResult;
        }

        public async Task<BaseApiResponse> BulkUpdate(OrganizationRoleBulkUpdateModel model)
        {
            await _organizationRoleService.BulkUpdateAsync(ToBulkUpdateData(model));
            return BaseApiResponse.SuccessResult;
        }

        private OrganizationRoleItem ToJsonItem(ApplicationOrganizationRole role)
        {
            var item = role.MapTo<OrganizationRoleItem>();
            item.FeatureIds = role.Features.Select(f => f.Id).ToList();
            return item;
        }

        private OrganizationRoleDetailJsonResult ToJsonResult(ApplicationOrganizationRole role)
        {
            var model = role.MapTo<OrganizationRoleDetailJsonResult>();
            model.FeatureIds = role.Features.Select(f => f.Id).ToList();
            return model;
        }

        private static ApplicationOrganizationRole ToOrganizationRole(OrganizationRoleMutationModel role)
        {
            var model = role.MapTo<ApplicationOrganizationRole>();
            model.Features = role.FeatureIds
                .Select(f => new ApplicationFeature(f, string.Empty, string.Empty, Guid.Empty, null!))
                .ToList();
            return model;
        }

        private static IEnumerable<ApplicationOrganizationRole> ToBulkUpdateData(OrganizationRoleBulkUpdateModel data)
            => data.Roles.Select(ToOrganizationRole);
    }
}
