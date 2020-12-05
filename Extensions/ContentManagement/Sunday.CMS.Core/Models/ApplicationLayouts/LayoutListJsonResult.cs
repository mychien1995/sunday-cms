using System;
using System.Collections.Generic;
using Sunday.ContentManagement.Domain;
using Sunday.Core;
using Sunday.Core.Models.Base;

namespace Sunday.CMS.Core.Models.ApplicationLayouts
{
    public class LayoutListJsonResult : BaseApiResponse
    {
        public int Total { get; set; }
        public List<LayoutItem> Layouts { get; set; } = new List<LayoutItem>();
    }

    [MappedTo(typeof(ApplicationLayout))]
    public class LayoutItem
    {
        public Guid Id { get; set; }
        public string LayoutName { get; set; } = string.Empty;
        public string LayoutPath { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
    }
}
