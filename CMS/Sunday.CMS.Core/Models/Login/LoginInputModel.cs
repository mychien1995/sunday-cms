using System.ComponentModel.DataAnnotations;

namespace Sunday.CMS.Core.Models.Login
{
    public class LoginInputModel
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
