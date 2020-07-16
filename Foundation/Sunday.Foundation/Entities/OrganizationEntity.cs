using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sunday.Core;
using Sunday.Foundation.Domain;

namespace Sunday.Foundation.Implementation.Repositories.Entities
{
    [MappedTo(typeof(ApplicationOrganization))]
    public class OrganizationEntity
    {
        public int ID { get; set; }
        public string OrganizationName { get; set; }
        public string Description { get; set; }
        public string Properties { get; set; }
        public string LogoBlobUri { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string ExtraProperties { get; set; }
        public string HostNames { get; set; }
        public List<ModuleEntity> Modules { get; set; }
    }
}
