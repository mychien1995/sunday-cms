using System.Threading.Tasks;
using Sunday.Core.Domain;
using Sunday.Foundation.Domain;

namespace Sunday.Foundation.Application.Services
{
    public interface INotificationService
    {
        Task NotifyPasswordReset(ApplicationUser user, string newPassword);
    }
}
