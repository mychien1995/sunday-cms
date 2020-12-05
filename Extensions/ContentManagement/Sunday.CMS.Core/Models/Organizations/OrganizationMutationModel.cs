using System;
using System.Collections.Generic;
using Sunday.Core;
using Sunday.Foundation.Domain;

namespace Sunday.CMS.Core.Models.Organizations
{
    [MappedTo(typeof(ApplicationOrganization))]
    public class OrganizationMutationModel
    {

        public Guid? Id { get; set; }
        public string OrganizationName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string LogoBlobUri { get; set; } = string.Empty;
        public string ColorScheme { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public List<string> HostNames { get; set; } = new List<string>();
        public List<Guid> ModuleIds { get; set; } = new List<Guid>();
    }
}
