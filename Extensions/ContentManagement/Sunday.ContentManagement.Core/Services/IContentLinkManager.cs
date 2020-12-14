using System;

namespace Sunday.ContentManagement.Services
{
    public interface IContentLinkManager
    {
        Guid[] GetReferencesTo(Guid contentId);
        Guid[] GetReferencesFrom(Guid contentId);
    }
}
