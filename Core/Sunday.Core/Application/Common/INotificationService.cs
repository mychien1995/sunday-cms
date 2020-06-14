using Sunday.Core.Domain.Users;
using System.Threading.Tasks;

namespace Sunday.Core.Application.Common
{
    public interface INotificationService
    {
        Task NotifyPasswordReset(IApplicationUser user, string newPassword);
    }
}
