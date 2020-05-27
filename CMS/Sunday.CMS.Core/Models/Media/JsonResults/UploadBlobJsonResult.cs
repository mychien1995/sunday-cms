using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Models.Media
{
    public class UploadBlobJsonResult : BaseApiResponse
    {
        public string BlobIdentifier { get; set; }
        public string PreviewLink { get; set; }
    }
}
