using System;
using System.Linq;
using System.Threading.Tasks;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Models.Users;
using Sunday.CMS.Core.Models.Users.JsonResults;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Application.Services;
using Sunday.Foundation.Domain;
using Sunday.Foundation.Models;

namespace Sunday.CMS.Core.Implementation
{
    [ServiceTypeOf(typeof(IApplicationUserManager))]
    public class DefaultApplicationUserManager : IApplicationUserManager
    {
        private readonly IUserService _userService;
        private readonly IIdentityService _identityService;
        private readonly INotificationService _notificationService;

        public DefaultApplicationUserManager(IUserService userService, INotificationService notificationService, IIdentityService identityService)
        {
            _userService = userService;
            _notificationService = notificationService;
            _identityService = identityService;
        }

        public Task<UserListJsonResult> SearchUsers(UserQuery criteria)
            => _userService.QueryAsync(criteria).MapResultTo(list => new UserListJsonResult()
            {
                Users = list.Result.Select(ToJsonResult).ToList(),
                Total = list.Total
            });

        public async Task<CreateUserJsonResult> CreateUser(UserMutationModel userData)
        {
            var user = ToUser(userData);
            user.EmailConfirmed = false;
            return new CreateUserJsonResult(await _userService.CreateAsync(user));
        }

        public async Task<UpdateUserJsonResult> UpdateUser(UserMutationModel userData)
        {
            var user = ToUser(userData);
            await _userService.UpdateAsync(user);
            return new UpdateUserJsonResult(user.Id);
        }

        public async Task<UserDetailJsonResult> GetUserById(Guid userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            return user.Some(ToDetailJsonResult)
                .None(() => BaseApiResponse.ErrorResult<UserDetailJsonResult>("User not found"));
        }

        public async Task<BaseApiResponse> DeleteUser(Guid userId)
        {
            await _userService.DeleteAsync(userId);
            return BaseApiResponse.SuccessResult;
        }

        public async Task<BaseApiResponse> ActivateUser(Guid userId)
        {
            await _userService.ActivateAsync(userId);
            return BaseApiResponse.SuccessResult;
        }

        public async Task<BaseApiResponse> DeactivateUser(Guid userId)
        {
            await _userService.DeactivateAsync(userId);
            return BaseApiResponse.SuccessResult;
        }

        public async Task<BaseApiResponse> ResetUserPassword(Guid userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if(user.IsNone) return BaseApiResponse.ErrorResult<BaseApiResponse>("User not found");
            var newPassword = await _identityService.ResetPasswordAsync(userId);
            _ = Task.Run(async () => await _notificationService.NotifyPasswordReset(user.Get(), newPassword));
            return BaseApiResponse.SuccessResult;
        }

        private static UserItem ToJsonResult(ApplicationUser user)
        {
            return user.MapTo<UserItem>();
        }

        private static ApplicationUser ToUser(UserMutationModel data)
        {
            return data.MapTo<ApplicationUser>();
        }

        private static UserDetailJsonResult ToDetailJsonResult(ApplicationUser user)
        {
            return user.MapTo<UserDetailJsonResult>();
        }
    }
}
