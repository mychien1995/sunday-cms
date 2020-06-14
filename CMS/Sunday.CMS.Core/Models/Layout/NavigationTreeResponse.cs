using Sunday.Core;
using System.Collections.Generic;

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
        public string Section { get; set; }
        public List<NavigationItem> Items { get; set; }
    }
}
