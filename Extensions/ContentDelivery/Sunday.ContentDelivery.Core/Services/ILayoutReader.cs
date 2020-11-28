using System.Threading.Tasks;
using Sunday.ContentManagement.Domain;

namespace Sunday.ContentDelivery.Core.Services
{
    public interface ILayoutReader
    {
        Task<string?> GetLayout(ApplicationWebsite website);
    }
}
