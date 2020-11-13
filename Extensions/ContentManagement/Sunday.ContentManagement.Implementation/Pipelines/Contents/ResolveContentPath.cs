using System;
using System.Threading.Tasks;
using Sunday.ContentManagement.Implementation.Pipelines.Arguments;
using Sunday.ContentManagement.Services;
using Sunday.Core.Pipelines;

namespace Sunday.ContentManagement.Implementation.Pipelines.Contents
{
    public class ResolveContentPath : IAsyncPipelineProcessor
    {
        private readonly IContentPathResolver _contentPathResolver;

        public ResolveContentPath(IContentPathResolver contentPathResolver)
        {
            _contentPathResolver = contentPathResolver;
        }

        public async Task ProcessAsync(PipelineArg pipelineArg)
        {
            var content = pipelineArg switch
            {
                BeforeCreateContentArg createContentArg => createContentArg.Content,
                BeforeUpdateContentArg updateContentArg => updateContentArg.Content,
                _ => throw new ArgumentException()
            };
            var address = await _contentPathResolver.GetAncestors(content);
            content.ContentAddress = address;
            content.Path = content.ContentAddress.IdPaths;
        }
    }
}
