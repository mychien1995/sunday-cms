using Sunday.Core.Ultilities;
using Sunday.Users.Implementation.Pipelines.Arguments;
using System;
using System.Threading.Tasks;

namespace Sunday.Users.Implementation.Pipelines.Create
{
    public class EncryptPassword
    {
        public async Task ProcessAsync(BeforeCreateUserArg arg)
        {
            var user = arg.User;
            var password = arg.Input.Password;
            var securityHash = Guid.NewGuid().ToString("N");
            var passwordHash = EncryptUltis.SHA256Encrypt(password, securityHash);
            user.PasswordHash = passwordHash;
            user.SecurityStamp = securityHash;
        }
    }
}
