using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Models.Contents;
using Sunday.Foundation.Context;

namespace Sunday.CMS.Interface.Controllers
{
    public class ContentsController : BaseController
    {
        private readonly IContentManager _contentManager;
        public ContentsController(ISundayContext context, IContentManager contentManager) : base(context)
        {
            _contentManager = contentManager;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContent([FromRoute]Guid id, [FromQuery]Guid? version)
        {
            return Ok(await _contentManager.GetContentByIdAsync(id, version));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ContentJsonResult content)
        {
            return Ok(await _contentManager.CreateContentAsync(content));
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] ContentJsonResult content)
        {
            return Ok(await _contentManager.UpdateContentAsync(content));
        }

        [HttpPost("{id}/{versionId}")]
        public async Task<IActionResult> NewVersion([FromRoute] Guid id, [FromRoute] Guid versionId)
        {
            return Ok(await _contentManager.NewContentVersionAsync(id, versionId));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Publish([FromRoute] Guid id)
        {
            return Ok(await _contentManager.PublishContentAsync(id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            return Ok(await _contentManager.DeleteContentAsync(id));
        }
    }
}
