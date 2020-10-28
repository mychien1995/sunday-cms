using System;
using System.Collections.Generic;
using Sunday.ContentManagement.Domain;
using Sunday.Core;
using Sunday.DataAccess.SqlServer.Attributes;
using Sunday.DataAccess.SqlServer.Extensions;

namespace Sunday.ContentManagement.Persistence.Entities
{
    [MappedTo(typeof(ApplicationLayout))]
    public class LayoutEntity
    {
        public Guid Id { get; set; }
        public string LayoutName { get; set; } = string.Empty;
        public string LayoutPath { get; set; } = string.Empty;
        [DapperIgnoreParam(DbOperation.Update)]
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        [DapperIgnoreParam(DbOperation.Update)]
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        [DapperIgnoreParam]
        public List<Guid> OrganizationIds { get; set; } = new List<Guid>();
        public string Organizations => OrganizationIds.ToDatabaseList();
    }
}
