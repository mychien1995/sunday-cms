using Sunday.CMS.Core.Pipelines.Arguments;
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
        public FetchUserRoles(IUserRepository userRepository)
        {
            _userRepo = userRepository;
        }

        public async Task ProcessAsync(AfterSearchUserArg arg)
        {
            if (arg.SearchResult != null)
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
