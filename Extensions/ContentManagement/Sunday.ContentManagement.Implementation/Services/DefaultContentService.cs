using System;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentManagement.Domain;
using Sunday.ContentManagement.Implementation.Pipelines.Arguments;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Persistence.Application;
using Sunday.ContentManagement.Persistence.Entities;
using Sunday.ContentManagement.Services;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.Core.Pipelines;
using Sunday.Core.Pipelines.Arguments;
using Sunday.Foundation.Context;

namespace Sunday.ContentManagement.Implementation.Services
{
    [ServiceTypeOf(typeof(IContentService))]
    public class DefaultContentService : IContentService
    {
        private readonly IContentRepository _contentRepository;
        private readonly ISundayContext _sundayContext;

        public DefaultContentService(ISundayContext sundayContext, IContentRepository contentRepository)
        {
            _sundayContext = sundayContext;
            _contentRepository = contentRepository;
        }

        public Task<Content[]> GetChildsAsync(Guid contentId, ContentType contentType)
           => _contentRepository.GetByParentAsync(contentId, (int)contentType)
                .MapResultTo(rs => rs.Select(ToModel).ToArray());

        public Task<Option<Content>> GetByIdAsync(Guid contentId, GetContentOptions? options = null)
            => _contentRepository.GetByIdAsync(contentId, options).MapResultTo(opt => opt.Map(ToModel));

        public async Task CreateAsync(Content content)
        {
            await ApplicationPipelines.RunAsync("cms.entity.beforeCreate", new BeforeCreateEntityArg(content));
            await ApplicationPipelines.RunAsync("cms.content.beforeCreate", new BeforeCreateContentArg(content));
            await _contentRepository.CreateAsync(ToEntity(content));
        }

        public async Task UpdateAsync(Content content)
        {
            await ApplicationPipelines.RunAsync("cms.entity.beforeUpdate", new BeforeUpdateEntityArg(content));
            await ApplicationPipelines.RunAsync("cms.content.beforeUpdate", new BeforeUpdateContentArg(content));
            await _contentRepository.UpdateAsync(ToEntity(content));
        }

        public async Task DeleteAsync(Guid contentId)
        {
            await ApplicationPipelines.RunAsync("cms.content.beforeDelete", new BeforeDeleteContentArg(contentId));
            await _contentRepository.DeleteAsync(contentId);
        }

        public Task NewVersionAsync(Guid contentId, Guid fromVersionId)
            => _contentRepository.CreateNewVersionAsync(contentId, fromVersionId, _sundayContext.CurrentUser!.UserName,
                DateTime.Now);

        public Task PublishAsync(Guid contentId)
            => _contentRepository.PublishAsync(contentId, _sundayContext.CurrentUser!.UserName, DateTime.Now);

        private Content ToModel(ContentEntity entity)
        {
            var model = entity.MapTo<Content>();
            model.Template = entity.Template.MapTo<Template>();
            return entity.MapTo<Content>();
        }

        private ContentEntity ToEntity(Content content)
        {
            return content.MapTo<ContentEntity>();
        }
    }
}
