using Microsoft.AspNetCore.Http;
using Sunday.CMS.Core.Models.Media;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Application.Media
{
    public interface IApplicationMediaManager
    {
        Task<UploadBlobJsonResult> UploadBlob(string directory, IFormFile file);
    }
}
