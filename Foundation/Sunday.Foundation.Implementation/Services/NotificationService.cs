using System.Collections.Generic;
using System.Threading.Tasks;
using Sunday.Core;
using Sunday.Core.Application.Common;
using Sunday.Core.Domain;
using Sunday.Foundation.Application.Services;
using Sunday.Foundation.Domain;

namespace Sunday.Foundation.Implementation.Services
{
    [ServiceTypeOf(typeof(INotificationService))]
    public class NotificationService : INotificationService
    {
        private readonly IMailService _mailService;
        public NotificationService(IMailService mailService)
        {
            _mailService = mailService;
        }
        public virtual async Task NotifyPasswordReset(ApplicationUser user, string newPassword)
        {
            var template = @"Hi {0}. <br/>
                            Your new password is {1}";
            await _mailService.SendEmail("Password Reset", template, new List<string>() { user.Email }, user.Fullname, newPassword);
        }
    }
}
