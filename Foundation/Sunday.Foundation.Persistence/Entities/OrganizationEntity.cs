using System;
using System.Collections.Generic;
using System.Linq;
using Sunday.Core;
using Sunday.DataAccess.SqlServer.Attributes;
using Sunday.Foundation.Domain;
using Sunday.Foundation.Persistence.Extensions;

namespace Sunday.Foundation.Persistence.Entities
{
    [MappedTo(typeof(ApplicationOrganization))]
    public class OrganizationEntity
    {
        public Guid Id { get; set; }
        public string OrganizationName { get; set; } = string.Empty;
        public string? Description { get; set; }
        [DapperParam("Properties")]
        public string? ExtraProperties { get; set; }
        public string? LogoBlobUri { get; set; }
        [DapperIgnoreParam(DbOperation.Update)]
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        [DapperIgnoreParam(DbOperation.Update)]
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        [DapperIgnoreParam(DbOperation.Update)]
        public bool IsDeleted { get; set; }
        public string? Hosts { get; set; }
        [DapperIgnoreParam]
        public List<ModuleEntity> Modules { get; set; } = new List<ModuleEntity>();

        public string ModuleIds => Modules.Select(m => m.Id).ToDatabaseList();
    }
}
