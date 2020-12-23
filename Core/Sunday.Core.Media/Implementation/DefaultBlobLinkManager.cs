using Sunday.Core.Media.Application;
using Sunday.Core.Media.Domain;

namespace Sunday.Core.Media.Implementation
{
    [ServiceTypeOf(typeof(IBlobLinkManager))]
    public class DefaultBlobLinkManager : IBlobLinkManager
    {
        public virtual string GetPreviewLink(ApplicationBlob blob, bool absolute = true)
            => GetPreviewLink(blob.Identifier, absolute);

        public string GetPreviewLink(string blobIdentifier, bool absolute = true)
        {
            if (string.IsNullOrEmpty(blobIdentifier)) return string.Empty;
            var baseUrl = absolute ? ApplicationSettings.Get("Sunday.BaseMediaUrl") : "";
            var url = $"{baseUrl}/{RoutePaths.BlobPreviewRoute.Trim('/')}/{blobIdentifier.Replace('\\', '/').Trim('/')}";
            return url;
        }
    }
}
