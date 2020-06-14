using Sunday.Core;

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
