using System;

namespace Sunday.ContentManagement.Models
{
    public class ContentTreeQuery
    {
        public ContentTreeQuery(string? location, Guid websiteId, bool expandLastNode = false)
        {
            Location = location;
            WebsiteId = websiteId;
            ExpandLastNode = expandLastNode;
        }

        public string? Location { get;  }
        public Guid WebsiteId { get;  }
        public bool ExpandLastNode { get;  }

    }
}
