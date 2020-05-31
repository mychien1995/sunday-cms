using Newtonsoft.Json;
using Sunday.Core;
using Sunday.Core.Domain.FeatureAccess;
using Sunday.Core.Domain.Organizations;
using Sunday.Core.Exceptions;
using Sunday.Core.Models;
using Sunday.DataAccess.SqlServer;
using Sunday.FeatureAccess.Core;
using Sunday.Organizations.Application;
using Sunday.Organizations.Core;
using Sunday.Organizations.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.Organizations.Implementation
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
            if (query.IsActive.HasValue) (param as dynamic).IsActive = query.IsActive.Value;
            else (param as dynamic).IsActive = null;
            var searchResult = await _dbRunner.ExecuteMultipleAsync(ProcedureNames.Organizations.Search, new Type[] { typeof(int), typeof(OrganizationEntity) }, param as object);
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
            var param = GetInsertParameters(organization);
            var result = await _dbRunner.ExecuteAsync<int>(ProcedureNames.Organizations.Insert, param);
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
            var queryResult = _dbRunner.ExecuteMultiple(ProcedureNames.Organizations.GetById, new Type[] { typeof(OrganizationEntity), typeof(ApplicationModule) },
                new { OrganizationId = organizationId });
            OrganizationEntity organization = null;
            if (queryResult.Count > 0)
            {
                organization = queryResult[0].FirstOrDefault() as OrganizationEntity;
            }
            if (queryResult.Count > 1)
            {
                organization.Modules = queryResult[1].Select(x => x as IApplicationModule).ToList();
            }
            return organization;
        }

        public virtual async Task<ApplicationOrganization> Update(ApplicationOrganization organization)
        {
            var param = GetUpdateParameters(organization);
            var result = await _dbRunner.ExecuteAsync<ApplicationOrganization>(ProcedureNames.Organizations.Update, param);
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
        public virtual async Task<ApplicationOrganization> FindOrganizationByHostname(string hostName)
        {
            var result = await _dbRunner.ExecuteAsync<OrganizationEntity>(ProcedureNames.Organizations.FindByHostname, new { Hostname = hostName });
            var matchedOrg = result.FirstOrDefault(x => x.Hosts != null && x.HostNames.Contains(hostName));
            if (matchedOrg == null) return null;
            return matchedOrg;
        }
        protected virtual object GetInsertParameters(ApplicationOrganization org)
        {
            return new
            {
                org.OrganizationName,
                org.CreatedBy,
                org.CreatedDate,
                org.Description,
                org.UpdatedBy,
                org.UpdatedDate,
                org.IsActive,
                org.LogoBlobUri,
                HostNames = string.Join('|', org.HostNames.Where(x => !string.IsNullOrEmpty(x))),
                Properties = org.Properties != null ? JsonConvert.SerializeObject(org.Properties) : "",
                ModuleIds = string.Join(',', (org.Modules ?? new List<IApplicationModule>()).Select(x => x.ID))
            };
        }
        protected virtual object GetUpdateParameters(ApplicationOrganization org)
        {
            return new
            {
                org.ID,
                org.OrganizationName,
                org.Description,
                org.UpdatedBy,
                org.UpdatedDate,
                org.IsActive,
                org.LogoBlobUri,
                HostNames = string.Join('|', org.HostNames.Where(x => !string.IsNullOrEmpty(x))),
                Properties = org.Properties != null ? JsonConvert.SerializeObject(org.Properties) : "",
                ModuleIds = string.Join(',', (org.Modules ?? new List<IApplicationModule>()).Select(x => x.ID))
            };
        }
    }
}
