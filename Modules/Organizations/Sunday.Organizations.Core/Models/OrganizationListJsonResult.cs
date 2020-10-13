using Sunday.Core;
using System.Collections.Generic;
using Sunday.Core.Models.Base;

namespace Sunday.Organizations.Core.Models
{
    public class OrganizationListJsonResult : BaseApiResponse
    {
        public OrganizationListJsonResult() : base()
        {
            Organizations = new List<OrganizationItem>();
        }
        public int Total { get; set; }
        public IEnumerable<OrganizationItem> Organizations { get; set; }
    }

    [MappedTo(typeof(ApplicationOrganization))]
    public class OrganizationItem : IOrganizationProperties
    {
        public int ID { get; set; }
        public string LogoLink { get; set; }
        public string OrganizationName { get; set; }
        public string CreatedBy { get; set; }
        public long CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public long UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public string ColorScheme { get; set; }

        public OrganizationItem()
        {
        }

    }
}
