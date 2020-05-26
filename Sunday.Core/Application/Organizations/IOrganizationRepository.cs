using Sunday.Core.Domain.Organizations;
using Sunday.Core.Models;
using Sunday.Core.Models.Organizations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.Core.Application.Organizations
{
    public interface IOrganizationRepository
    {
        Task<SearchResult<ApplicationOrganization>> Query(OrganizationQuery query);

        ApplicationOrganization GetById(int organizationId);


        Task<ApplicationOrganization> Create(ApplicationOrganization organization);

        Task<ApplicationOrganization> Update(ApplicationOrganization organization);

        Task<bool> Delete(int organizationId);

        Task<bool> Activate(int organizationId);

        Task<bool> Deactivate(int organizationId);
    }
}
