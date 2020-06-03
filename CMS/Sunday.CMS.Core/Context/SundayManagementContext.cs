using Microsoft.AspNetCore.Http;
using Sunday.Core;
using Sunday.Core.Domain.Identity;
using Sunday.Core.Domain.Organizations;
using Sunday.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Context
{
    [ServiceTypeOf(typeof(ISundayContext), LifetimeScope.PerRequest)]
    public class SundayManagementContext : ISundayContext
    {
        private readonly IManagementContextHelper _contextHelper;


        private Lazy<IApplicationOrganization> _currentOrganization;
        private Lazy<IApplicationUser> _currentUser;

        public SundayManagementContext(IManagementContextHelper contextHelper)
        {
            _contextHelper = contextHelper;
            _currentOrganization = new Lazy<IApplicationOrganization>(() =>
            {
                return _contextHelper.GetCurrentOrganization();
            }, true);
            _currentUser = new Lazy<IApplicationUser>(() =>
            {
                return _contextHelper.GetCurrentUser();
            }, true);
        }
        public IApplicationOrganization CurrentOrganization
        {
            get
            {
                return this._currentOrganization?.Value;
            }
        }

        public IApplicationUser CurrentUser
        {
            get
            {
                return this._currentUser?.Value;
            }
        }
    }
}
