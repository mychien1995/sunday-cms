using Sunday.Core;
using System.Collections.Generic;
using Sunday.Core.Models.Base;

namespace Sunday.Organizations.Core.Models
{
    [MappedTo(typeof(ApplicationOrganization))]
    public class OrganizationDetailJsonResult : BaseApiResponse, IOrganizationProperties
    {
        public OrganizationDetailJsonResult()
        {
            Properties = new Dictionary<string, object>();
            ModuleIds = new List<int>();
        }
        public int ID { get; set; }
        public string OrganizationName { get; set; }
        public string LogoLink { get; set; }
        public string Description { get; set; }
        public string LogoBlobUri { get; set; }
        public bool IsActive { get; set; }
        public string ColorScheme { get; set; }
        public List<string> HostNames { get; set; }
        public Dictionary<string, object> Properties { get; set; }
        public List<int> ModuleIds { get; set; }
    }
}
