using Sunday.ContentManagement.Implementation.Pipelines.Arguments;
using Sunday.ContentManagement.Services;
using Sunday.Core.Application;
using Sunday.Core.Extensions;
using Sunday.Core.Pipelines;

namespace Sunday.ContentManagement.Implementation.Pipelines.Contents
{
    public class UpdateContentCache : IPipelineProcessor
    {
        private readonly IEntityCacheManager _cacheManager;
        private readonly IContentService _contentService;

        public UpdateContentCache(IEntityCacheManager cacheManager, IContentService contentService)
        {
            _cacheManager = cacheManager;
            _contentService = contentService;
        }

        public void Process(PipelineArg pipelineArg)
        {
            switch (pipelineArg)
            {
                case AfterUpdateContentArg arg:
                    _ = _contentService.GetFullContent(arg.Content.Id).MapResultTo(rs => rs.IfSome(
                        content =>
                        {
                            _cacheManager.Set(content);
                        }));
                    break;
                case BeforeDeleteContentArg contentArg:
                    _ = _contentService.GetFullContent(contentArg.ContentId).MapResultTo(rs => rs.IfSome(
                        content =>
                        {
                            _cacheManager.Remove(content);
                        }));
                    break;
            }
        }
    }
}
