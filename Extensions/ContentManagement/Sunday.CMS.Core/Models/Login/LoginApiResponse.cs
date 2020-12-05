using Sunday.Core;
using Sunday.Core.Models.Base;

namespace Sunday.CMS.Core.Models.Login
{
    public class LoginApiResponse : BaseApiResponse
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string Fullname { get; set; } = null!;
        public string Token { get; set; } = null!;

        public string? AvatarLink { get; set; }


    }
}
