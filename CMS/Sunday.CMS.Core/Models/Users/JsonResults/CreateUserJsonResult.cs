using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Models.Users
{
    public class CreateUserJsonResult : BaseApiResponse
    {
        public int UserId { get; set; }
        public CreateUserJsonResult()
        {

        }
        public CreateUserJsonResult(int userId)
        {
            this.UserId = userId;
        }
    }
}
