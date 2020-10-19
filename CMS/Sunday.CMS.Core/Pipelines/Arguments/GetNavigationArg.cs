using Sunday.CMS.Core.Models.Layout;
using Sunday.Core;
using System.Collections.Generic;
using Sunday.Foundation.Domain;

namespace Sunday.CMS.Core.Pipelines.Arguments
{
    public class GetNavigationArg : PipelineArg
    {
        public ApplicationUser User { get; set; }
        public List<NavigationItem> NavigationItems { get; set; }
        public GetNavigationArg(ApplicationUser user)
        {
            this.User = user;
            this.NavigationItems = new List<NavigationItem>();
        }
    }
}
