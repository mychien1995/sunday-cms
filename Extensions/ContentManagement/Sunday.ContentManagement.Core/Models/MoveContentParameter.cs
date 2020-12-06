using System;

namespace Sunday.ContentManagement.Models
{
    public class MoveContentParameter
    {
        public Guid ContentId { get; set; }
        public Guid TargetId { get; set; }
        public int TargetType { get; set; }
        public MovePosition Position { get; set; }
    }

    public enum MovePosition
    {
        Inside = 0,
        Above = -1,
        Below = 1
    }
}
