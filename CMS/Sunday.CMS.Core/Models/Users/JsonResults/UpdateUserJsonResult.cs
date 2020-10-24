using System;
using Sunday.Core.Models.Base;

namespace Sunday.CMS.Core.Models.Users.JsonResults
{
    public class UpdateUserJsonResult : BaseApiResponse
    {
        public Guid UserId { get; set; }
        public UpdateUserJsonResult(Guid userId)
        {
            this.UserId = userId;
        }
    }
}
