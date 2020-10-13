using Sunday.Core;
using Sunday.Core.Models.Base;

namespace Sunday.Users.Core.Models
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
