using System;
using System.Collections.Generic;
using Sunday.ContentManagement.Domain;
using Sunday.Core;
using Sunday.Core.Models.Base;

namespace Sunday.CMS.Core.Models.ApplicationLayouts
{
    [MappedTo(typeof(ApplicationLayout))]
    public class LayoutDetailJsonResult : BaseApiResponse
    {
        public Guid Id { get; set; }
        public string LayoutName { get; set; } = string.Empty;
        public string LayoutPath { get; set; } = string.Empty;
        public List<Guid> OrganizationIds { get; set; } = new List<Guid>();
    }
}
