using Sunday.CMS.Core.Application.VirtualRoles;
using Sunday.CMS.Core.Models;
using Sunday.CMS.Core.Models.VirtualRoles;
using Sunday.Core;
using Sunday.Core.Pipelines.Arguments;
using Sunday.VirtualRoles.Application;
using Sunday.VirtualRoles.Core;
using Sunday.VirtualRoles.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Implementation.VirtualRoles
{
    public class DefaultOrganizationRolesManager : IOrganizationRolesManager
    {
        private readonly IOrganizationRoleRepository _organizationRoleRepository;
        public DefaultOrganizationRolesManager(IOrganizationRoleRepository organizationRoleRepository)
        {
            _organizationRoleRepository = organizationRoleRepository;
        }
        public virtual async Task<OrganizationRolesListJsonResult> GetRolesList(OrganizationRoleQuery query)
        {
            var result = new OrganizationRolesListJsonResult();
            var searchResult = await _organizationRoleRepository.GetRoles(query);
            result.Total = searchResult.Total;
            result.Roles = searchResult.Result.Select(x => x.MapTo<OrganizationRoleItem>()).ToList();
            return result;
        }
        public virtual async Task<OrganizationRoleDetailJsonResult> GetRoleById(int roleId)
        {
            var result = new OrganizationRoleDetailJsonResult();
            var organizationRole = await _organizationRoleRepository.GetById(roleId);
            result = organizationRole.MapTo<OrganizationRoleDetailJsonResult>();
            var arg = new EntityModelExchangeArg(organizationRole, result);
            await ApplicationPipelines.RunAsync("cms.organizationRoles.translateToModel", arg);
            return result;
        }
        public virtual async Task<BaseApiResponse> CreateRole(OrganizationRoleMutationModel mutationData)
        {
            var result = new BaseApiResponse();
            var organization = mutationData.MapTo<OrganizationRole>();
            var arg = new BeforeUpdateEntityArg(mutationData, organization);
            await ApplicationPipelines.RunAsync("cms.organizationRoles.beforeCreate", arg);
            if (arg.Aborted)
            {
                result.AddErrors(arg.Messages);
                return result;
            }
            await _organizationRoleRepository.Create(organization);
            return result;
        }

        public virtual async Task<BaseApiResponse> UpdateRole(OrganizationRoleMutationModel mutationData)
        {
            var result = new BaseApiResponse();
            var organization = mutationData.MapTo<OrganizationRole>();
            var arg = new BeforeUpdateEntityArg(mutationData, organization);
            await ApplicationPipelines.RunAsync("cms.organizationRoles.beforeUpdate", arg);
            if (arg.Aborted)
            {
                result.AddErrors(arg.Messages);
                return result;
            }
            await _organizationRoleRepository.Update(organization);
            return result;
        }

        public virtual async Task<BaseApiResponse> DeleteRole(int roleId)
        {
            var result = new BaseApiResponse();
            await ApplicationPipelines.RunAsync("cms.organizationRoles.beforeDelete", new BeforeDeleteEntityArg(roleId));
            await _organizationRoleRepository.Delete(roleId);
            return result;
        }
    }
}
