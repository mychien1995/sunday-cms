using System;
using System.Collections.Generic;
using Sunday.ContentManagement.Domain;
using Sunday.Core;

namespace Sunday.CMS.Core.Models.ApplicationLayouts
{
    [MappedTo(typeof(ApplicationLayout))]
    public class LayoutMutationModel
    {
        public Guid? Id { get; set; }
        public string LayoutName { get; set; } = string.Empty;
        public string LayoutPath { get; set; } = string.Empty;
        public List<Guid> OrganizationIds { get; set; } = new List<Guid>();
    }
}
