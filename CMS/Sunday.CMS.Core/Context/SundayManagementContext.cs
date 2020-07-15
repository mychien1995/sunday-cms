using Sunday.Core;
using Sunday.Core.Domain.Organizations;
using Sunday.Core.Domain.Users;
using System;

namespace Sunday.CMS.Core.Context
{
    [ServiceTypeOf(typeof(ISundayContext), LifetimeScope.PerRequest)]
    public class SundayManagementContext : ISundayContext
    {
        private readonly Lazy<IApplicationOrganization> _currentOrganization;
        private readonly Lazy<IApplicationUser> _currentUser;

        public SundayManagementContext(IManagementContextHelper contextHelper)
        {
            _currentOrganization = new Lazy<IApplicationOrganization>(contextHelper.GetCurrentOrganization, true);
            _currentUser = new Lazy<IApplicationUser>(contextHelper.GetCurrentUser, true);
        }
        public IApplicationOrganization CurrentOrganization => this._currentOrganization?.Value;

        public IApplicationUser CurrentUser => this._currentUser?.Value;
    }
}
