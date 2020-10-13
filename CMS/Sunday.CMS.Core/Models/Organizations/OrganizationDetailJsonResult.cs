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
        public OrganizationDetailJsonResult(Guid id, string organizationName, string logoLink, string description,
            string logoBlobUri, bool isActive, string colorScheme, List<string> hostNames)
        {
            Id = id;
            OrganizationName = organizationName;
            LogoLink = logoLink;
            Description = description;
            LogoBlobUri = logoBlobUri;
            IsActive = isActive;
            ColorScheme = colorScheme;
            HostNames = hostNames;
        }

        public Guid Id { get; set; }
        public string OrganizationName { get; set; }
        public string LogoLink { get; set; }
        public string Description { get; set; }
        public string LogoBlobUri { get; set; }
        public bool IsActive { get; set; }
        public string ColorScheme { get; set; }
        public List<string> HostNames { get; set; }
        public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
        public List<int> ModuleIds { get; set; } = new List<int>();
    }
}
