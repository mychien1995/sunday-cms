using System.Threading.Tasks;
using Sunday.Core.Domain;

namespace Sunday.Core.Application.Common
{
    public interface INotificationService
    {
        Task NotifyPasswordReset(IApplicationUser user, string newPassword);
    }
}
