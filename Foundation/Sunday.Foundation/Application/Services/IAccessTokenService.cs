using Sunday.Core.Domain.Users;

namespace Sunday.Foundation.Application.Services
{
    public interface IAccessTokenService
    {
        string GenerateToken(IApplicationUser user);

        bool ValidToken(string token, out int? userId);
    }
}
