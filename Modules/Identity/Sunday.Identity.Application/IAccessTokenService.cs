using Sunday.Core.Domain.Users;

namespace Sunday.Identity.Application
{
    public interface IAccessTokenService
    {
        string GenerateToken(IApplicationUser user);

        bool ValidToken(string token, out int? userId);
    }
}
