using System;
using System.Linq;
using System.Threading.Tasks;
using Sunday.ContentManagement.Implementation.Pipelines.Arguments;
using Sunday.ContentManagement.Services;
using Sunday.Core.Extensions;
using Sunday.Core.Pipelines;

namespace Sunday.ContentManagement.Implementation.Pipelines.Contents
{
    public class ValidateAndFormalizeContentName : IAsyncPipelineProcessor
    {
        private readonly IContentService _contentService;

        public ValidateAndFormalizeContentName(IContentService contentService)
        {
            _contentService = contentService;
        }

        public async Task ProcessAsync(PipelineArg pipelineArg)
        {
            var content = pipelineArg switch
            {
                BeforeCreateContentArg createContentArg => createContentArg.Content,
                BeforeUpdateContentArg updateContentArg => updateContentArg.Content,
                _ => throw new ArgumentException()
            };
            var siblings = await _contentService.GetChildsAsync(content.ParentId, (ContentType) content.ParentType)
                .MapResultTo(rs => rs.Where(c => c.Id != content.Id).ToArray());
            pipelineArg.AddProperty("siblings", siblings);
            var formalizedName = ContentUtils.FormalizeName(content.Name);
            if (siblings.Any(c => c.Name == formalizedName))
                throw new ArgumentException($"There is already a content named {formalizedName} under this path");
            content.Name = formalizedName;
            content.DisplayName = ContentUtils.FormalizeDisplayName(content.DisplayName);

        }
    }
}
