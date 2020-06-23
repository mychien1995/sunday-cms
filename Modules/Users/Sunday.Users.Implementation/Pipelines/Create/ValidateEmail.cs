using Sunday.Users.Application;
using Sunday.Users.Implementation.Pipelines.Arguments;
using System.Threading.Tasks;

namespace Sunday.Users.Implementation.Pipelines.Create
{
    public class ValidateEmail
    {
        private readonly IUserRepository _userRepository;
        public ValidateEmail(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task ProcessAsync(BeforeCreateUserArg arg)
        {
            var email = arg.User.Email;
            var exists = await _userRepository.FindUserByEmailAsync(email);
            if (exists != null)
            {
                if (arg is BeforeUpdateUserArg)
                {
                    if (arg.User.ID == exists.ID) return;
                }

                arg.Aborted = true;
                arg.AddMessage("Email already in use");
            }
        }
    }
}