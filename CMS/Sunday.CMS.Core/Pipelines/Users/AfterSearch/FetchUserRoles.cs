using Sunday.CMS.Core.Pipelines.Arguments;
using Sunday.Core;
using Sunday.Core.Domain.Users;
using Sunday.Users.Application;
using Sunday.Users.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Pipelines.Users.Search
{
    public class FetchUserRoles
    {
        private readonly IUserRepository _userRepo;
        private readonly ISundayContext _sundayContext;

        public FetchUserRoles(IUserRepository userRepository, ISundayContext sundayContext)
        {
            _userRepo = userRepository;
            _sundayContext = sundayContext;
        }

        public async Task ProcessAsync(AfterSearchUserArg arg)
        {
            if (arg.SearchResult != null && _sundayContext.CurrentUser.IsInOneOfRoles(SystemRoleCodes.SystemAdmin, SystemRoleCodes.Developer))
            {
                var searchResult = arg.SearchResult.Result.ToList();
                await _userRepo.FetchUserRoles(searchResult);
                if (arg.DisplayResult != null)
                {
                    foreach (var item in arg.DisplayResult.Users)
                    {
                        item.Roles = searchResult.FirstOrDefault(x => x.ID == item.ID)?.Roles.Cast<ApplicationRole>().ToList() ?? new List<ApplicationRole>();
                    }
                }
            }
        }
    }
}
