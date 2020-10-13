using Sunday.Core.Models.Base;

namespace Sunday.Core.Media.Models
{
    public class UploadBlobJsonResult : BaseApiResponse
    {
        public string BlobIdentifier { get; set; }
        public string PreviewLink { get; set; }
    }
}
