using System;
using System.Threading.Tasks;

namespace Sunday.ContentManagement.Persistence.Application
{
    public interface IContentLinkRepository
    {
        Task Save(Guid contentId, Guid[] references);

        Task<Guid[]> GetReferencesTo(Guid contentId);

        Task<Guid[]> GetReferencesFrom(Guid contentId);
    }
}
