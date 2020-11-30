using System;

namespace Sunday.ContentManagement.Models
{
    public class ContentTreeQuery
    {
        public ContentTreeQuery(string location, Guid websiteId)
        {
            Location = location;
            WebsiteId = websiteId;
        }

        public string Location { get; set; }
        public Guid WebsiteId { get; set; }

    }
}
