using Sunday.Users.Application;
using Sunday.Users.Implementation.Pipelines.Arguments;
using System.Threading.Tasks;

namespace Sunday.Users.Implementation.Pipelines.Update
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
