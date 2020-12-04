﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Models.Contents;
using Sunday.ContentManagement.Models;
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
        public async Task<IActionResult> GetContent([FromRoute] Guid id, [FromQuery] Guid? version)
        {
            return Ok(await _contentManager.GetContentByIdAsync(id, version));
        }

        [HttpPost("getMultiples")]
        public async Task<IActionResult> GetMultiples(GetMultipleContentParameter param)
        {
            return Ok(await _contentManager.GetMultiples(param.Ids.Where(c => Guid.TryParse(c, out _)).Select(Guid.Parse).ToArray()));
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

        [HttpPost("updateExplicit")]
        public async Task<IActionResult> UpdateExplicit([FromBody] ContentJsonResult content)
        {
            return Ok(await _contentManager.UpdateExplicitAsync(content));
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


        [HttpPost("move")]
        public async Task<IActionResult> MoveContent([FromBody] MoveContentParameter parameter)
        {
            var result = await _contentManager.MoveContent(parameter);
            return Ok(result);
        }
        public class GetMultipleContentParameter
        {
            public string[] Ids { get; set; } = Array.Empty<string>();
        }
    }
}
