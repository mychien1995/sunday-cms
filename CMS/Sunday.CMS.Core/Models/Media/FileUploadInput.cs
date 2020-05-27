using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Models.Media
{
    public class FileUploadInput
    {
        public string Directory { get; set; }
        public IFormFile FileUpload { get; set; }
    }
}
