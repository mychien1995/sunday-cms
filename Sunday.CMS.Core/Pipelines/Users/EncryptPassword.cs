using Sunday.CMS.Core.Pipelines.Arguments;
using Sunday.Core.Ultilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Pipelines.Users
{
    public class EncryptPassword
    {
        public async Task ProcessAsync(BeforeCreateUserArg arg)
        {
            var user = arg.User;
            var password = arg.Input.Password;
            var securityHash = Guid.NewGuid().ToString("N");
            var passwordHash = EncryptUtils.SHA256Encrypt(password, securityHash);
            user.PasswordHash = passwordHash;
            user.SecurityStamp = securityHash;
        }
    }
}
