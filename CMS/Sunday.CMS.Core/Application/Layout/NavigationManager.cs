using Sunday.CMS.Core.Models.Layout;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Application.Layout
{
    public interface INavigationManager
    {
        Task<NavigationTreeResponse> GetUserNavigation();
    }
}
