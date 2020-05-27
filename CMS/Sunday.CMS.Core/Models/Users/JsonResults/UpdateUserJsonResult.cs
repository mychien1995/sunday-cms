using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Models.Users
{
    public class UpdateUserJsonResult : BaseApiResponse
    {
        public int UserId { get; set; }
        public UpdateUserJsonResult()
        {

        }
        public UpdateUserJsonResult(int userId)
        {
            this.UserId = userId;
        }
    }
}
