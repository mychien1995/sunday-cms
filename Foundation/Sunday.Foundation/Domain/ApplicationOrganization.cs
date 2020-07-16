using System;
using System.Collections.Generic;

namespace Sunday.Foundation.Domain
{
    public class ApplicationOrganization
    {
        public ApplicationOrganization()
        {
            Properties = new Dictionary<string, object>();
            HostNames = new List<string>();
            Modules = new List<ApplicationModule>();
        }
        public int ID { get; set; }
        public string OrganizationName { get; set; }
        public string Description { get; set; }
        public Dictionary<string, object> Properties { get; set; }
        public List<string> HostNames { get; set; }
        public string LogoBlobUri { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public List<ApplicationModule> Modules { get; set; }
    }
}
