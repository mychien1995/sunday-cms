using Sunday.CMS.Core.Models.Layout;
using Sunday.Core;
using Sunday.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Pipelines.Arguments
{
    public class GetNavigationArg : PipelineArg
    {
        public IApplicationUser User { get; set; }
        public List<NavigationItem> NavigationItems { get; set; }
        public GetNavigationArg(IApplicationUser user)
        {
            this.User = user;
            this.NavigationItems = new List<NavigationItem>();
        }
    }
}
