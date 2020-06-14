using Sunday.Core.Media.Domain;

namespace Sunday.Core.Media.Application
{
    public interface IBlobLinkManager
    {
        string GetPreviewLink(string blobIdentifier, bool absolute = true);
        string GetPreviewLink(ApplicationBlob blob, bool absolute = true);
    }
}
