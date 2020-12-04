using System;

namespace Sunday.ContentManagement.Models
{
    public class ContentOrder
    {
        public Guid ContentId { get; set; }
        public int Order { get; set; }

        public ContentOrder()
        {
            
        }

        public ContentOrder(Guid contentId, int order)
        {
            ContentId = contentId;
            Order = order;
        }
    }
}
