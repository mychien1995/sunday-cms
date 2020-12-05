namespace Sunday.CMS.Core.Models.Users
{
    public class ChangePasswordModel
    {
        public ChangePasswordModel(string oldPassword, string newPassword)
        {
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }

        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
