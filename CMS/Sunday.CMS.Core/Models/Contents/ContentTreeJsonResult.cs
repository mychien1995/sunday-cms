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
    [MappedTo(typeof(ContentTreeNode))]
    public class ContentTreeItem
    {
        public string Id { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string? ParentId { get; set; }
    }
}
