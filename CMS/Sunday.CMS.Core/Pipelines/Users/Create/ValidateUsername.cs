using Sunday.CMS.Core.Pipelines.Arguments;
using Sunday.Users.Application;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Pipelines.Users
{
    public class ValidateUsername
    {
        private readonly IUserRepository _userRepository;
        public ValidateUsername(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task ProcessAsync(BeforeCreateUserArg arg)
        {
            var username = arg.User.UserName;
            var exists = await _userRepository.FindUserByNameAsync(username);
            if (exists != null)
            {
                arg.Aborted = true;
                arg.AddMessage("Username already in use");
            }
        }
    }
}
