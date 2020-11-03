using System;

namespace Sunday.ContentManagement.Models
{
    public class ContextMenu
    {
        public ContextMenuItem[] Items { get; set; } = Array.Empty<ContextMenuItem>();
    }

    public class ContextMenuItem
    {
        public string Icon { get; set; } = string.Empty;
        public string Command { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Hint { get; set; } = string.Empty;
    }
}
