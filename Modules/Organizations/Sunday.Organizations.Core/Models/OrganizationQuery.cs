﻿namespace Sunday.Organizations.Core.Models
{
    public class OrganizationQuery
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string Text { get; set; }
        public string SortBy { get; set; }
        public bool? IsActive { get; set; }
    }
}
