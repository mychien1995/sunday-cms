using Sunday.Core.Models.Base;

namespace Sunday.Core.Media.Models
{
    public class UploadBlobJsonResult : BaseApiResponse
    {
        public string BlobIdentifier { get; set; } = string.Empty;
        public string PreviewLink { get; set; } = string.Empty;
    }
}
