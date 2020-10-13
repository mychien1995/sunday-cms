using Sunday.Core;
using Sunday.Core.Domain.Users;
using Sunday.Users.Application;
using Sunday.Users.Core;
using Sunday.Users.Implementation.Pipelines.Arguments;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sunday.Core.Constants;
using Sunday.Core.Context;

namespace Sunday.Users.Implementation.Pipelines.AfterSearch
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
