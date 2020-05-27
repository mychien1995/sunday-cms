using Sunday.Core.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Organizations.Core
{
    public class ApplicationOrganization : IApplicationOrganization
    {
        public ApplicationOrganization()
        {
            Properties = new Dictionary<string, object>();
            HostNames = new List<string>();
        }
        public virtual int ID { get; set; }
        public virtual string OrganizationName { get; set; }
        public virtual string Description { get; set; }
        public virtual Dictionary<string,object> Properties { get; set; }
        public virtual List<string> HostNames { get; set; }
        public virtual string LogoBlobUri { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime UpdatedDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string UpdatedBy { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual bool IsDeleted { get; set; }
    }
}
