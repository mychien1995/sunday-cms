using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Models
{
    public class LoginApiResponse : BaseApiResponse
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Fullname { get; set; }
        public string Token { get; set; }

        public string AvatarLink { get; set; }
    }
}
