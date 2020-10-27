using Microsoft.AspNetCore.Http;
#pragma warning disable 8618

namespace Sunday.CMS.Core.Models.Media
{
    public class FileUploadInput
    {
        public string Directory { get; set; }
        public IFormFile FileUpload { get; set; }
    }
}
