using Microsoft.AspNetCore.Mvc;
using Sunday.Core.Media.Application;
using Sunday.Foundation.Context;

namespace Sunday.CMS.Interface.Controllers
{
    public class FileController : BaseController
    {
        private readonly IBlobLinkManager _blobLinkManager;
        public FileController(ISundayContext context, IBlobLinkManager blobLinkManager) : base(context)
        {
            _blobLinkManager = blobLinkManager;
        }

        [HttpGet("preview")]
        public IActionResult Preview(string id)
        {
            return Ok(new { link = _blobLinkManager.GetPreviewLink(id) });
        }
    }
}
