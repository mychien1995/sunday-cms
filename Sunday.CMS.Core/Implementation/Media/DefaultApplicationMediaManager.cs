using Microsoft.AspNetCore.Http;
using Sunday.CMS.Core.Application.Media;
using Sunday.CMS.Core.Models.Media;
using Sunday.Core;
using Sunday.Core.Media.Application;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Implementation.Media
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
