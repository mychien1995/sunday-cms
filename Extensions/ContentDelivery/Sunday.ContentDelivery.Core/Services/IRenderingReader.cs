using System;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentManagement.Models;

namespace Sunday.ContentDelivery.Core.Services
{
    public interface IRenderingReader
    {
        Task<Option<Rendering>> GetRendering(Guid id);
    }
}
