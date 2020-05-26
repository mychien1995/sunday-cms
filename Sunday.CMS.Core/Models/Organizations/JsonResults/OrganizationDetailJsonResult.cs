using Sunday.Core;
using Sunday.Core.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Models.Organizations
{
    [MappedTo(typeof(ApplicationOrganization))]
    public class OrganizationDetailJsonResult : BaseApiResponse, IOrganizationProperties
    {
        public OrganizationDetailJsonResult()
        {
            Properties = new Dictionary<string, object>();
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
    }
}
