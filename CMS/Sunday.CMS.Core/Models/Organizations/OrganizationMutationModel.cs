using System;
using System.Collections.Generic;
using Sunday.Core;
using Sunday.Foundation.Domain;

namespace Sunday.CMS.Core.Models.Organizations
{
    [MappedTo(typeof(ApplicationOrganization))]
    public class OrganizationMutationModel
    {
        public OrganizationMutationModel(Guid id, string organizationName, 
            string description, string logoBlobUri, string colorScheme, bool isActive)
        {
            Id = id;
            OrganizationName = organizationName;
            Description = description;
            LogoBlobUri = logoBlobUri;
            ColorScheme = colorScheme;
            IsActive = isActive;
        }

        public Guid Id { get; set; }
        public string OrganizationName { get; set; }
        public string Description { get; set; }
        public string LogoBlobUri { get; set; }
        public string ColorScheme { get; set; }
        public bool IsActive { get; set; }
        public List<string> HostNames { get; set; } = new List<string>();
        public List<int> ModuleIds { get; set; } = new List<int>();
        public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
    }
}
