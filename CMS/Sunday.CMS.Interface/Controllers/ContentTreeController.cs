using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Models.Contents;
using Sunday.Foundation.Context;

namespace Sunday.CMS.Interface.Controllers
{
    public class ContentTreeController : BaseController
    {
        private readonly IContentTreeManager _contentTreeManager;
        public ContentTreeController(ISundayContext context, IContentTreeManager contentTreeManager)
            : base(context)
        {
            _contentTreeManager = contentTreeManager;
        }

        [HttpGet("getRoots")]
        public async Task<IActionResult> GetRoots()
        {
            var result = await _contentTreeManager.GetRoots();
            return Ok(result);
        }

        [HttpPost("getChilds")]
        public IActionResult GetLayout([FromBody] ContentTreeItem current)
        {
            var result = _contentTreeManager.GetChilds(current);
            return Ok(result);
        }
    }
}
