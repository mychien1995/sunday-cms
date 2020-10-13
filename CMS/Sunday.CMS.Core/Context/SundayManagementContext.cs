using Sunday.Core;
using System;
using Sunday.Foundation.Context;
using Sunday.Foundation.Domain;

namespace Sunday.CMS.Core.Context
{
    [ServiceTypeOf(typeof(ISundayContext), LifetimeScope.PerRequest)]
    public class SundayManagementContext : ISundayContext
    {
        private readonly Lazy<ApplicationOrganization> _currentOrganization;
        private readonly Lazy<ApplicationUser> _currentUser;

        public SundayManagementContext(IManagementContextHelper contextHelper)
        {
            _currentOrganization = new Lazy<ApplicationOrganization>(contextHelper.GetCurrentOrganization, true);
            _currentUser = new Lazy<ApplicationUser>(contextHelper.GetCurrentUser, true);
        }
        public ApplicationOrganization CurrentOrganization => this._currentOrganization.Value;

        public ApplicationUser CurrentUser => this._currentUser.Value;
    }
}
