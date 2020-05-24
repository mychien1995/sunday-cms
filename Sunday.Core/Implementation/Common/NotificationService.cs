using Sunday.Core.Application.Common;
using Sunday.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.Core.Implementation.Common
{
    [ServiceTypeOf(typeof(INotificationService))]
    public class NotificationService : INotificationService
    {
        private readonly IMailService _mailService;
        public NotificationService(IMailService mailService)
        {
            _mailService = mailService;
        }
        public async Task NotifyPasswordReset(ApplicationUser user, string newPassword)
        {
            var template = @"Hi {0}. <br/>
                            Your new password is {1}";
            await _mailService.SendEmail("Password Reset", template, new List<string>() { user.Email }, user.Fullname, newPassword);
        }
    }
}
