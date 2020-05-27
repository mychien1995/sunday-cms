using Sunday.CMS.Core.Pipelines.Arguments;
using Sunday.Users.Application;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Pipelines.Users
{
    public class KeepUnchangedData
    {
        private readonly IUserRepository _userRepository;
        public KeepUnchangedData(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }
        public async Task ProcessAsync(BeforeUpdateUserArg arg)
        {
            var existingUser = _userRepository.GetUserById(arg.Input.ID);
            var applicationUser = arg.User;
            if (string.IsNullOrEmpty(arg.Input.AvatarBlobUri))
            {
                applicationUser.AvatarBlobUri = existingUser.AvatarBlobUri;
            }
        }
    }
}
