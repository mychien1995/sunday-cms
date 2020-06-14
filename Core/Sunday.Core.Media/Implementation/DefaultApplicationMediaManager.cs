using Microsoft.AspNetCore.Http;
using Sunday.Core.Media.Application;
using Sunday.Core.Media.Models;
using System.IO;
using System.Threading.Tasks;

namespace Sunday.Core.Media.Implementation
{
    [ServiceTypeOf(typeof(IApplicationMediaManager))]
    public class DefaultApplicationMediaManager : IApplicationMediaManager
    {
        private readonly IBlobProvider _blobProvider;
        private readonly IBlobLinkManager _blobLinkManager;
        public DefaultApplicationMediaManager(IBlobProvider blobProvider, IBlobLinkManager blobLinkManager)
        {
            _blobProvider = blobProvider;
            _blobLinkManager = blobLinkManager;
        }
        public async Task<UploadBlobJsonResult> UploadBlob(string directory, IFormFile file)
        {
            var result = new UploadBlobJsonResult();
            var fileInfo = new FileInfo(file.FileName);
            var blob = this._blobProvider.CreateBlob(directory, fileInfo.Extension);
            using (var stream = file.OpenReadStream())
            {
                blob.Write(stream);
            }
            result.BlobIdentifier = blob.Identifier;
            result.PreviewLink = this._blobLinkManager.GetPreviewLink(blob, true);
            return result;
        }
    }
}
