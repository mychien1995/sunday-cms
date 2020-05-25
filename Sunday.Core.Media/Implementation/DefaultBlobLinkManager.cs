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
        public virtual string GetPreviewLink(ApplicationBlob blob, bool absolute = true)
        {
            var baseUrl = absolute ? ApplicationSettings.Get("Sunday.BaseMediaUrl") : "";
            var url = $"{baseUrl}/{RoutePaths.BlobPreviewRoute.Trim('/')}/{blob.Identifier.Replace('\\', '/').Trim('/')}";
            return url;
        }
    }
}
