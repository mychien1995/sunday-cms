using Sunday.Core.Domain.FeatureAccess;
using System.Collections.Generic;

namespace Sunday.Core.Domain.Organizations
{
    public interface IApplicationOrganization : IEntity
    {
        string OrganizationName { get; set; }
        string Description { get; set; }
        Dictionary<string, object> Properties { get; set; }
        List<string> HostNames { get; set; }
        string LogoBlobUri { get; set; }
        bool IsActive { get; set; }
        bool IsDeleted { get; set; }

        List<IApplicationModule> Modules { get; set; }
    }
}
