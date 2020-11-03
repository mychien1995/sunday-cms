using System;
using System.Collections.Generic;

namespace Sunday.ContentManagement.Models
{
    public class ContextMenu
    {
        public List<ContextMenuItem> Items { get; set; } = new List<ContextMenuItem>();
    }

    public class ContextMenuItem
    {
        public string Icon { get; set; } = string.Empty;
        public string Command { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Hint { get; set; } = string.Empty;
    }
}
