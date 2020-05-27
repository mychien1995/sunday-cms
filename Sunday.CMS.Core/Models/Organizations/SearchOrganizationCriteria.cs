using Sunday.Core;
using Sunday.Organizations.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Models.Organizations
{
    [MappedTo(typeof(OrganizationQuery))]
    public class SearchOrganizationCriteria
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string Text { get; set; }
        public string SortBy { get; set; }
    }
}
