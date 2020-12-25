using System;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentManagement;
using Sunday.ContentManagement.Models;

namespace Sunday.ContentDelivery.Core.Services
{
    public interface IContentReader
    {
        Task<Option<Content>> GetHomePage(Guid websiteId);
        public Task<Option<Content>> GetPage(Guid websiteId, string path);
        public Task<Option<Content>> GetContent(Guid contentId);

        public Task<Content[]> GetChilds(Guid parentId, ContentType contentType);
    }
}
