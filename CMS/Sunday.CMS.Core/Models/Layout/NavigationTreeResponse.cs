using System.Collections.Generic;
using Sunday.Core.Models.Base;

namespace Sunday.CMS.Core.Models.Layout
{
    public class NavigationTreeResponse : BaseApiResponse
    {
        public NavigationTreeResponse()
        {
            Sections = new List<NavigationTreeSection>();
        }
        public List<NavigationTreeSection> Sections { get; set; }
    }

    public class NavigationTreeSection
    {
        public NavigationTreeSection()
        {
            Items = new List<NavigationItem>();
        }

        public string Section { get; set; } = string.Empty;
        public List<NavigationItem> Items { get; set; }
    }
}
