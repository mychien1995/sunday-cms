using System;
using System.Collections.Generic;

namespace Sunday.ContentManagement.Models
{
    public class ContentTree
    {
        public ContentTreeNode[] Roots { get; set; } = Array.Empty<ContentTreeNode>();
    }

    public class ContentTreeNode
    {
        public string Id { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Type { get; set; }
        public string? ParentId { get; set; }
        public List<ContentTreeNode> ChildNodes { get; set; } = new List<ContentTreeNode>();
    }
}
