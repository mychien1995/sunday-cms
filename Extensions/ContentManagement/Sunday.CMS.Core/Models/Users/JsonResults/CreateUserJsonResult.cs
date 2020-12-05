using System;
using Sunday.Core.Models.Base;

namespace Sunday.CMS.Core.Models.Users.JsonResults
{
    public class CreateUserJsonResult : BaseApiResponse
    {
        public Guid UserId { get; set; }
        public CreateUserJsonResult(Guid userId)
        {
            this.UserId = userId;
        }
    }
}
