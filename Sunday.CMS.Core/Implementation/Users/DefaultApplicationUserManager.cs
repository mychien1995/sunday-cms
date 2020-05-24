using AutoMapper;
using Sunday.CMS.Core.Application.Users;
using Sunday.CMS.Core.Models;
using Sunday.CMS.Core.Models.Users;
using Sunday.CMS.Core.Pipelines.Arguments;
using Sunday.Core;
using Sunday.Core.Application.Common;
using Sunday.Core.Application.Identity;
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
        private readonly IIdentityService _identityService;
        private readonly INotificationService _notificationService;
        public DefaultApplicationUserManager(IUserRepository userRepository, IIdentityService identityService, INotificationService notificationService)
        {
            _userRepository = userRepository;
            _identityService = identityService;
            _notificationService = notificationService;
        }
        public async Task<UserListJsonResult> SearchUsers(SearchUserCriteria criteria)
        {
            var query = criteria.MapTo<UserQuery>();
            await ApplicationPipelines.RunAsync("cms.users.beforeSearch", new BeforeSearchUserArg(criteria, query));
            var searchResult = await _userRepository.QueryUsers(query);
            var apiResult = new UserListJsonResult();
            apiResult.Total = searchResult.Total;
            apiResult.Users = searchResult.Result.Select(x =>
            {
                var user = x.MapTo<UserItem>();
                return user;
            });
            await ApplicationPipelines.RunAsync("cms.users.afterSearch", new AfterSearchUserArg(searchResult, apiResult));
            return apiResult;
        }

        public async Task<CreateUserJsonResult> CreateUser(UserMutationModel userData)
        {
            var result = new CreateUserJsonResult();
            var applicationUser = userData.MapTo<ApplicationUser>();
            var createUserArg = new BeforeCreateUserArg(applicationUser, userData);
            await ApplicationPipelines.RunAsync("cms.users.beforeCreate", createUserArg);
            if (createUserArg.Aborted)
            {
                result.AddErrors(createUserArg.Messages);
                return result;
            }
            applicationUser.EmailConfirmed = true;
            var createResult = await _userRepository.CreateUser(applicationUser);
            result = new CreateUserJsonResult(createResult.ID);
            return result;
        }

        public async Task<UpdateUserJsonResult> UpdateUser(UserMutationModel userData)
        {
            var result = new UpdateUserJsonResult();
            var applicationUser = userData.MapTo<ApplicationUser>();
            var updateArg = new BeforeUpdateUserArg(applicationUser, userData);
            await ApplicationPipelines.RunAsync("cms.users.beforeUpdate", updateArg);
            if (updateArg.Aborted)
            {
                result.AddErrors(updateArg.Messages);
                return result;
            }
            var createResult = await _userRepository.UpdateUser(applicationUser);
            result = new UpdateUserJsonResult(createResult.ID);
            return result;
        }

        public async Task<UserDetailJsonResult> GetUserById(int userId)
        {
            var user = _userRepository.GetUserWithOptions(userId, new GetUserOptions() { FetchRoles = true, FetchOrganizations = true });
            var result = user.MapTo<UserDetailJsonResult>();
            result.RoleIds = user.Roles.Select(x => x.ID).ToList();
            result.Success = true;
            return await Task.FromResult(result);
        }

        public async Task<BaseApiResponse> DeleteUser(int userId)
        {
            var result = await _userRepository.DeleteUser(userId);
            var respone = new BaseApiResponse();
            respone.Success = result;
            return respone;
        }

        public async Task<BaseApiResponse> ActivateUser(int userId)
        {
            var result = await _userRepository.ActivateUser(userId);
            var respone = new BaseApiResponse();
            respone.Success = result;
            return respone;
        }
        public async Task<BaseApiResponse> DeactivateUser(int userId)
        {
            var result = await _userRepository.DeactivateUser(userId);
            var respone = new BaseApiResponse();
            respone.Success = result;
            return respone;
        }

        public async Task<BaseApiResponse> ResetUserPassword(int userId)
        {
            var newPassword = await _identityService.ResetPasswordAsync(userId);
            var user = _userRepository.GetUserById(userId);
            Task.Run(async () => await _notificationService.NotifyPasswordReset(user, newPassword));
            var respone = new BaseApiResponse();
            return respone;
        }
    }
}
