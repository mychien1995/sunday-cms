using Sunday.Users.Application;
using Sunday.Users.Implementation.Pipelines.Arguments;
using System.Threading.Tasks;

namespace Sunday.Users.Implementation.Pipelines.Create
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
