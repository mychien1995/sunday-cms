using System;
using System.Threading.Tasks;
using Sunday.ContentManagement.Models;

namespace Sunday.ContentManagement.Services
{
    public interface IContentLinkService
    {
        Task Save(Content content);

        Task<Guid[]> GetReferencesTo(Guid contentId);

        Task<Guid[]> GetReferencesFrom(Guid contentId);
    }
}
