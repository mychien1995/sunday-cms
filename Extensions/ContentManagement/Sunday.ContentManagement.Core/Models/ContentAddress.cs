using System.Collections.Generic;
using System.Linq;
using Sunday.ContentManagement.Domain;
using Sunday.Foundation.Domain;

namespace Sunday.ContentManagement.Models
{
    public class ContentAddress
    {
        public ApplicationOrganization? Organization { get; set; }
        public ApplicationWebsite? Website { get; set; }
        public List<Content> Ancestors { get; set; } = new List<Content>();

        public string IdPaths => $"{Organization!.Id}/{Website!.Id}/{string.Join('/', Ancestors.Select(a => a.Id))}";
        public string NamePaths => $"{Organization!.OrganizationName}/{Website!.WebsiteName}/{string.Join('/', Ancestors.Select(a => a.Name))}";
    }
}
