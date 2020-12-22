using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentManagement.Domain;

namespace Sunday.ContentDelivery.Core.Services
{
    public interface IWebsiteLoader
    {
        Task<Option<ApplicationWebsite>> GetWebsiteByHostName(string hostname);
    }
}
