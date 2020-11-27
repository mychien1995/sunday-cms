using System;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentManagement.Models;

namespace Sunday.ContentManagement.Services
{
    public interface IContentPathResolver
    {
        Task<ContentAddress> GetAncestors(Content content);
        Task<Option<ContentAddress>> GetAddressByPath(string idPath);
        Task<Option<Content>> GetContentByNamePath(Guid websiteId, string path);
    }
}
