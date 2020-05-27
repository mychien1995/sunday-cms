using Sunday.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.Core.Application.Common
{
    public interface INotificationService
    {
        Task NotifyPasswordReset(IApplicationUser user, string newPassword);
    }
}
