using Microsoft.AspNetCore.Http;
using Sunday.Core.Media.Models;
using System.Threading.Tasks;

namespace Sunday.Core.Media.Application
{
    public interface IApplicationMediaManager
    {
        Task<UploadBlobJsonResult> UploadBlob(string directory, IFormFile file);
    }
}
