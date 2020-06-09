using Sunday.CMS.Core.Pipelines.Arguments;
using Sunday.Core;
using Sunday.Core.Domain.Users;
using Sunday.Users.Application;
using Sunday.Users.Core;
using Sunday.VirtualRoles.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Pipelines.Users.Search
{
    public class FetchOrganizationRoles
    {
        private readonly IUserRepository _userRepo;
        private readonly ISundayContext _sundayContext;

        public FetchOrganizationRoles(IUserRepository userRepository, ISundayContext sundayContext)
        {
            _userRepo = userRepository;
            _sundayContext = sundayContext;
        }

        public async Task ProcessAsync(AfterSearchUserArg arg)
        {
            if (arg.SearchResult != null && _sundayContext.CurrentUser.IsInOneOfRoles(SystemRoleCodes.OrganizationAdmin, SystemRoleCodes.OrganizationUser))
            {
                var searchResult = arg.SearchResult.Result.ToList();
                await _userRepo.FetchVirtualRoles(searchResult);
                if (arg.DisplayResult != null)
                {
                    foreach (var item in arg.DisplayResult.Users)
                    {
                        item.OrganizationRoles = searchResult.FirstOrDefault(x => x.ID == item.ID)?.VirtualRoles.Cast<OrganizationRole>().ToList() ?? new List<OrganizationRole>();
                    }
                }
            }
        }
    }
}
