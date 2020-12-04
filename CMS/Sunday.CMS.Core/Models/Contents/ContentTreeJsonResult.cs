using System;
using Sunday.ContentManagement.Models;
using Sunday.Core;
using Sunday.Core.Models.Base;

namespace Sunday.CMS.Core.Models.Contents
{
    public class ContentTreeJsonResult : BaseApiResponse
    {
        public ContentTreeItem[] Roots { get; set; } = Array.Empty<ContentTreeItem>();
    }

    public class ContentTreeListJsonResult : BaseApiResponse
    {
        public ContentTreeItem[] Nodes { get; set; } = Array.Empty<ContentTreeItem>();
    }
    [MappedTo(typeof(ContentTreeNode))]
    public class ContentTreeItem
    {
        public string Id { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool Open { get; set; }
        public string? ParentId { get; set; }
        public int? SortOrder { get; set; }
        public ContentTreeItem[] ChildNodes { get; set; } = Array.Empty<ContentTreeItem>();
    }
}
