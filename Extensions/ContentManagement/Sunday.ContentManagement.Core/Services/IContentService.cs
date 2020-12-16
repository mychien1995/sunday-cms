using System;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentManagement.Models;

namespace Sunday.ContentManagement.Services
{
    public interface IContentService
    {
        Task<Content[]> GetChildsAsync(Guid contentId, ContentType contentType);

        Task<Option<Content>> GetByIdAsync(Guid contentId, GetContentOptions? options = null);

        Task CreateAsync(Content content);

        Task UpdateAsync(Content content);

        Task DeleteAsync(Guid contentId);

        Task NewVersionAsync(Guid contentId, Guid fromVersionId);

        Task PublishAsync(Guid contentId);

        Task<Content[]> GetMultiples(Guid[] contentIds);

        Task MoveContent(MoveContentParameter moveContentParameter);
        Task UpdateExplicitAsync(Content content);
        Task<Option<Content>> GetFullContent(Guid contentId);
    }
}
