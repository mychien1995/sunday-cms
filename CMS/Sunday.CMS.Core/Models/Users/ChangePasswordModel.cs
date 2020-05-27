using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Models.Users
{
    public class ChangePasswordModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
