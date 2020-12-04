using System;

namespace Sunday.ContentManagement.Models
{
    public class MoveContentParameter
    {
        public Guid ContentId { get; set; }
        public Guid? ParentId { get; set; }
        public int? ParentType { get; set; }
        public int? SortOrder { get; set; }
    }
}
