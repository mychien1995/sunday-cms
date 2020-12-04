using System;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Persistence.Entities;

namespace Sunday.ContentManagement.Persistence.Application
{
    public interface IContentRepository
    {
        Task<ContentEntity[]> GetByParentAsync(Guid parentId, int parentType);

        Task<Option<ContentEntity>> GetByIdAsync(Guid contentId, GetContentOptions? options = null);

        Task CreateAsync(ContentEntity content);

        Task UpdateAsync(ContentEntity content);

        Task DeleteAsync(Guid contentId);

        Task CreateNewVersionAsync(Guid contentId, Guid workVersionId, string? updatedBy = null,
            DateTime? updatedDate = null);

        Task PublishAsync(Guid contentId, string publishBy, DateTime? publishDate = null);
        Task<ContentEntity[]> GetMultiples(Guid[] ids);
        Task UpdateContentExplicit(ContentEntity content);
    }
}
