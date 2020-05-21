using AutoMapper;
using Sunday.CMS.Core.Application.Users;
using Sunday.CMS.Core.Models;
using Sunday.CMS.Core.Models.Users;
using Sunday.CMS.Core.Pipelines.Arguments;
using Sunday.Core;
using Sunday.Core.Domain.Users;
using Sunday.Core.Models;
using Sunday.Core.Models.Users;
using Sunday.Core.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Implementation.Users
{
    [ServiceTypeOf(typeof(IApplicationUserManager))]
    public class DefaultApplicationUserManager : IApplicationUserManager
    {
        private readonly IUserRepository _userRepository;
        public DefaultApplicationUserManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserListJsonResult> SearchUsers(SearchUserCriteria criteria)
        {
            var query = criteria.MapTo<UserQuery>();
            var searchResult = await _userRepository.QueryUsers(query);
            var apiResult = new UserListJsonResult();
            apiResult.Total = searchResult.Total;
            apiResult.Users = searchResult.Result.Select(x =>
            {
                var user = x.MapTo<UserItem>();
                return user;
            });
            return apiResult;
        }

        public async Task<CreateUserJsonResult> CreateUser(UserMutationModel userData)
        {
            var applicationUser = userData.MapTo<ApplicationUser>();
            await ApplicationPipelines.RunAsync("cms.users.beforeCreate", new BeforeCreateUserArg(applicationUser, userData));
            applicationUser.EmailConfirmed = true;
            var createResult = await _userRepository.CreateUser(applicationUser);
            var result = new CreateUserJsonResult(createResult.ID);
            return result;
        }
    }
}
