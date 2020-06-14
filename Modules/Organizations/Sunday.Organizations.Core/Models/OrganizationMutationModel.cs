using Sunday.Core;
using System.Collections.Generic;

namespace Sunday.Organizations.Core.Models
{
    [MappedTo(typeof(ApplicationOrganization))]
    public class OrganizationMutationModel
    {
        public OrganizationMutationModel()
        {
            Properties = new Dictionary<string, object>();
            HostNames = new List<string>();
            ModuleIds = new List<int>();
        }
        public int ID { get; set; }
        public string OrganizationName { get; set; }
        public string Description { get; set; }
        public string LogoBlobUri { get; set; }
        public string ColorScheme { get; set; }
        public bool IsActive { get; set; }
        public List<string> HostNames { get; set; }
        public List<int> ModuleIds { get; set; }
        public Dictionary<string, object> Properties { get; set; }
    }
}
