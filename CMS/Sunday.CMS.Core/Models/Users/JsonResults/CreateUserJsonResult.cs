﻿using Sunday.Core;
using Sunday.Core.Models.Base;

namespace Sunday.Users.Core.Models
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
