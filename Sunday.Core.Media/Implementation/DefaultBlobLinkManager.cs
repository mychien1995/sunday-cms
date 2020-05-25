using Sunday.Core.Media.Application;
using Sunday.Core.Media.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core.Media.Implementation
{
    [ServiceTypeOf(typeof(IBlobLinkManager))]
    public class DefaultBlobLinkManager : IBlobLinkManager
    {
        private readonly IBlobProvider _blobProvider;
        public DefaultBlobLinkManager(IBlobProvider blobProvider)
        {
            _blobProvider = blobProvider;
        }
        public virtual string GetPreviewLink(ApplicationBlob blob, bool absolute = true)
        {
            var baseUrl = absolute ? ApplicationSettings.Get("Sunday.BaseMediaUrl") : "";
            var url = $"{baseUrl}/{RoutePaths.BlobPreviewRoute.Trim('/')}/{blob.Identifier.Replace('\\', '/').Trim('/')}";
            return url;
        }

        public string GetPreviewLink(string blobIdentifier, bool absolute = true)
        {
            if (string.IsNullOrEmpty(blobIdentifier)) return string.Empty;
            var blob = _blobProvider.GetBlob(blobIdentifier);
            if (blob == null) return string.Empty;
            return GetPreviewLink(blob, absolute);
        }
    }
}
