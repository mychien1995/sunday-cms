using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Models.Contents;
using Sunday.ContentManagement.Models;
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

        [HttpGet("getTreeByPath")]
        public async Task<IActionResult> GetRoots([FromQuery] string path)
        {
            var result = await _contentTreeManager.GetTreeByPath(path);
            return Ok(result);
        }

        [HttpPost("getTreeByQuery")]
        public async Task<IActionResult> GetTreeByQuery([FromBody] ContentTreeQuery query)
        {
            var result = await _contentTreeManager.GetTreeByQuery(query);
            return Ok(result);
        }

        [HttpPost("getChilds")]
        public async Task<IActionResult> GetChilds([FromBody] ContentTreeItem current)
        {
            var result = await _contentTreeManager.GetChilds(current);
            return Ok(result);
        }

        [HttpPost("getContextMenu")]
        public async Task<IActionResult> GetContextMenu([FromBody] ContentTreeItem current)
        {
            var result = await _contentTreeManager.GetContextMenu(current);
            return Ok(result);
        }
    }
}
