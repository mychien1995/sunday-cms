using System;
using System.Collections.Generic;
using Sunday.Core;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Domain;

namespace Sunday.CMS.Core.Models.Organizations
{
    [MappedTo(typeof(ApplicationOrganization))]
    public class OrganizationDetailJsonResult : BaseApiResponse
    {
        public Guid Id { get; set; }
        public string OrganizationName { get; set; } = string.Empty;
        public string LogoLink { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string LogoBlobUri { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string ColorScheme { get; set; } = string.Empty;
        public List<string> HostNames { get; set; } = new List<string>();
        public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
        public List<Guid> ModuleIds { get; set; } = new List<Guid>();
    }
}
